using Microsoft.CodeAnalysis;

using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.CodeDom.Compiler;
using MetaParser.Core;
using MetaParser.Builders.Parser.Functions;
using MetaParser.Builders.Core;
using MetaParser.Json.Definitions;
using JetBrains.Annotations;
using MetaParser.Exceptions;
using MetaParser.Json.JsonTypeConverters;
using Microsoft.CodeAnalysis.CSharp;
using MetaParser.Graphs;
using MetaParser.Parsing.Constructs;
using MetaParser.Builders.Parser;

namespace MetaParser;


[Generator(LanguageNames.CSharp)]
public partial class Generator : IIncrementalGenerator
{
    private static JsonSerializerOptions SerializerOptions
    {
        get
        {
            var deserializerOptions = new JsonSerializerOptions(JsonSerializerOptions.Default);
            deserializerOptions.Converters.Add(new JsonEnumerableConverter());
            deserializerOptions.Converters.Add(new ValuePatternDeclarationConverterFactory());
            deserializerOptions.Converters.Add(new TokenPatternDeclarationConverterFactory());

            deserializerOptions.AddContext<MetaParserJsonSerializer>();
            return deserializerOptions;
        }
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        #region Loading
        // find all additional files that end with .parser-meta.json
        IncrementalValuesProvider<AdditionalText> ctxFileNames = context.AdditionalTextsProvider.Where(static file => file.Path.EndsWith(Common.MetaParserFileExtension, StringComparison.InvariantCultureIgnoreCase));

        // read their contents and save their name
        IncrementalValuesProvider<FileData> ctxFiles = ctxFileNames.Select(static (text, cancellationToken) => new FileData(Common.Get_FileName(text.Path), text.Path, text.GetText(cancellationToken)!.ToString()));

        // Parse the file into a JSON structure
        IncrementalValuesProvider<(FileData, JsonDocument)> ctxParserJson = ctxFiles.Select((Func<FileData, CancellationToken, ValueTuple<FileData, JsonDocument>>)(static (FileData file, CancellationToken cancellationToken) =>
        {
            var doc = JsonDocument.Parse(file.Content, new JsonDocumentOptions() { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true });
            return (file, doc);
        }));

        // Create metaparser context from json
        IncrementalValuesProvider<ValueTuple<FileData, ParserDefinition>> ctxSchema = ctxParserJson.Select(selector: (Func<(FileData file, JsonDocument jsonDoc), CancellationToken, ValueTuple<FileData, ParserDefinition>>)(static (ValueTuple<FileData, JsonDocument> data, CancellationToken cancellationToken) =>
        {
            FileData file = data.Item1;
            JsonDocument jsonDoc = data.Item2;
            var schema = jsonDoc.Deserialize<ParserDefinition>(SerializerOptions);

            if (schema is null || schema.Definitions is null)
            {
                throw new MalformedSchemaException($"MetaParser schema ({file.Path}) is malformed!");
            }

            return (file, schema!);
        }));

        IncrementalValuesProvider <ValueTuple<MetaParserContext, ParserDefinition>> ctxFull = ctxSchema.Select(static (ValueTuple<FileData, ParserDefinition> data, CancellationToken cancellationToken) =>
        {
            FileData file = data.Item1;
            var schema = data.Item2;
            var config = new MetaParserConfig()
            {
                BaseFileName = file.FileName,
                Namespace = schema.Namespace!
            };

            if (schema.ClassName is not null)
            {
                config.ClassName = schema.ClassName;
            }

            if (schema.ParserType is not null)
            {
                config.ParserType = schema.ParserType;
            }

            if (schema.Definitions is not null)
            {
                config.IdType = Common.Get_Integer_Type(schema.Definitions.Count);
            }

            return new ValueTuple<MetaParserContext, ParserDefinition>(new MetaParserContext() { Config = config }, schema);
        });

        IncrementalValuesProvider<MetaParserContext> ctxParser = ctxFull.Select(static (ValueTuple<MetaParserContext, ParserDefinition> data, CancellationToken cancellationToken) =>
        {
            return data.Item1;
        });
        #endregion

        #region Resolving

        IncrementalValuesProvider<MetaParserContext> ctxParserTokens = ctxFull.Select(static (ValueTuple<MetaParserContext, ParserDefinition> data, CancellationToken cancellationToken) =>
        {
            var schema = data.Item2;
            var context = data.Item1;

            if (schema?.Definitions is not null)
            {
                foreach ( var definition in schema.Definitions )
                {
                    string tokenKey = CodeCommon.Format_Token_Key(definition.Key);
                    var tokenInfo = new TokenInfo(tokenKey, context);
                    context.Registry.AddToken(tokenInfo);
                }

                context.WorkingSet.Tokens = new TokenInfo[1];
                context.WorkingSet.Consumers = new Consumer[1];

                foreach (var def in schema.Definitions)
                {
                    var tokenKey = CodeCommon.Format_Token_Key(def.Key);
                    context.Registry.TryGetToken(tokenKey, out var token);
                    context.WorkingSet.Tokens[0] = token;

                    foreach (var consumerDeclaration in def.Value)
                    {
                        var consumer = new Consumer(context, consumerDeclaration);
                        context.Registry.AddConsumer(consumer);
                    }
                }

                context.DepsGraph = DependencyGraph.Build(context.Registry);
            }

            return context;
        });

        #endregion

        //#if DEBUG
        //        context.RegisterSourceOutput(ctxParserTokens, static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        //        {
        //            using IndentedTextWriter writer = new(new StringWriter());
        //            context = context with {  writer = writer  };

        //            var graph = new DirectedGraph(context.DepsGraph);
        //            // we only want to see a graph of our token relationships, so we'll remove everything else from the graph
        //            var trash = graph.Nodes.Keys.Where(static k => k.Type != NodeType.Token).ToList();
        //            foreach (var key in trash)
        //            {
        //                graph.TryRemove(key);
        //            }

        //            writer.WriteLine("/*");
        //            writer.WriteLine("```mermaid");
        //            var mermaidFormatter = new MermaidFormatter(context.Registry, graph);
        //            mermaidFormatter.Write(writer, MermaidChartType.Graph);
        //            writer.WriteLine("```");
        //            writer.WriteLine("*/");

        //            spc.AddSource($"{context.Config.BaseFileName}.dependency_graph.md", writer.InnerWriter.ToString());
        //        });
        //#endif

#if DEBUG
        context.RegisterSourceOutput(ctxParserTokens, static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            writer.WriteLine("/*");
            writer.WriteLine("```");
            // write all items in the registry
            foreach (var token in context.Registry.Tokens)
            {
                writer.WriteLine(token);
            }
            writer.WriteLine();

            foreach (var consumer in context.Registry.Consumers)
            {
                writer.WriteLine(consumer);
            }
            writer.WriteLine();

            foreach (var pattern in context.Registry.Patterns)
            {
                writer.WriteLine(pattern);
            }
            writer.WriteLine("```");
            writer.WriteLine("*/");

            spc.AddSource($"{context.Config.BaseFileName}.registry.debug", writer.InnerWriter.ToString());
        });
#endif

        #region Parser Class
        context.RegisterSourceOutput(ctxParser, static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            using IndentedTextWriter writer = new(new StringWriter());
            context = context with {  writer = writer  };

            CodeBuilderFactory codeFactory = context.Config.CodeFactory;
            new ClassBuilder(CodeCommon.ParserClassModifiers, context.Config.ClassName!)
            .And(codeFactory.Get_Parsing_Logic())
            .And(new ConstantTokenStage())
            .And(new CompoundTokenStage())
            .And(new ComplexTokenStage())
                .WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.parser.class", writer.InnerWriter.ToString());
        });
        #endregion

        #region Token Start Detection
        // Any token which has an incoming link must have a start detection function
        context.RegisterSourceOutput(ctxParserTokens.Select(static (MetaParserContext parser, CancellationToken cancellationToken) =>
        {
            //var tokens = parser.Registry.Tokens.Values.Where(static (t) => t.DependencyInfo!.Incoming.Any(static (x) => x.Key.Type == Graphs.NodeType.Token && x.Depth[(int)NodeType.Token].Max > 0));
            var tokens = parser.Registry.Tokens.Values.Where(static (t) => t.DependencyInfo.Depth[(int)NodeType.Token].Min > 0);
            return (parser with { WorkingSet = new WorkingSet(tokens) });
        }), 
        static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            new ClassBuilder(CodeCommon.ParserClassModifiers, context.Config.ClassName!)
                .And(new GenTokenStartDetectors())
                .WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.tokens.detection", writer.InnerWriter.ToString());
        });
        #endregion

        #region Constant-Type Tokens
        context.RegisterSourceOutput(ctxParserTokens.Select(static (MetaParserContext parser, CancellationToken cancellationToken) =>
        {
            Consumer[] consumers = parser.Registry.Consumers.Values.Where(static (o) => o.Type == EConsumerType.Data).ToArray();
            return (parser with { WorkingSet = new WorkingSet(consumers) });
        }), 
        static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            var consumer = CodeCommon.Get_Token_Processor_Function_Definition(context.Config, EConsumerType.Data, CodeCommon.ConstantTokenProcessorFunctionName);
            new ClassBuilder(CodeCommon.ParserClassModifiers, context.Config.ClassName!)
                .And(consumer)
                .WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.tokens.constant", writer.InnerWriter.ToString());
        });
        #endregion

        #region Compound-Type Tokens
        context.RegisterSourceOutput(ctxParserTokens.Select(static (MetaParserContext parser, CancellationToken cancellationToken) =>
        {
            Consumer[] consumers = parser.Registry.Consumers.Values.Where(static (o) => o.Type == EConsumerType.Token && !o.DependencyInfo!.IsRecursive).ToArray();
            return (parser with { WorkingSet = new WorkingSet(consumers) });
        }), 
        static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            var consumer = CodeCommon.Get_Token_Processor_Function_Definition(context.Config, EConsumerType.Token, CodeCommon.CompoundTokenProcessorFunctionName);
            new ClassBuilder(CodeCommon.ParserClassModifiers, context.Config.ClassName!)
                .And(consumer)
                .WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.tokens.compound", writer.InnerWriter.ToString());
        });
        #endregion

        #region Complex-Type Tokens
        context.RegisterSourceOutput(ctxParserTokens.Select(static (MetaParserContext parser, CancellationToken cancellationToken) =>
        {
            Consumer[] consumers = parser.Registry.Consumers.Values.Where(static (o) => o.Type == EConsumerType.Token && o.DependencyInfo!.IsRecursive).ToArray();
            return (parser with { WorkingSet = new WorkingSet(consumers) });
        }), 
        static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            var consumer = CodeCommon.Get_Token_Processor_Function_Definition(context.Config, EConsumerType.Token, CodeCommon.ComplexTokenProcessorFunctionName);
            new ClassBuilder(CodeCommon.ParserClassModifiers, context.Config.ClassName!)
                .And(consumer)
                .WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.tokens.complex", writer.InnerWriter.ToString());
        });
        #endregion

        #region Token Structure
        context.RegisterSourceOutput(ctxParser, static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            context.Config.CodeFactory.Get_Token_Struct_Builder().WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.token.struct", writer.InnerWriter.ToString());
        });
        #endregion

        #region Token-ID Enums
        context.RegisterSourceOutput(ctxParserTokens, static (SourceProductionContext spc, [NotNull] MetaParserContext context) => 
        {
            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            writer.WriteLine($"namespace {context.Config.Namespace};");
            writer.WriteLine($"public enum {CodeCommon.TokenEnum} : {context.Config.IdType}");
            writer.WriteLine("{");
            writer.Indent++;

            context.Config.CodeFactory.Get_Token_ID_Enum_Builder().WriteTo(context);

            writer.Indent--;
            writer.WriteLine("}");

            AddSource(spc, $"{context.Config.BaseFileName}.enum", writer.InnerWriter.ToString());
        });
        #endregion

        #region Token-ID Constants
        context.RegisterSourceOutput(ctxParserTokens, static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            new ClassBuilder(SyntaxFactory.ParseTokens("internal static"), CodeCommon.TokenConsts)
                .And(context.Config.CodeFactory.Get_Token_ID_Constants_Builder())
                .WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.constants", writer.InnerWriter.ToString());
        });
        #endregion

    }

    private static void AddSource(SourceProductionContext spc, string fileName, string content)
    {
        const string extension = "g.cs";
        fileName = $"{nameof(MetaParser)}.{fileName}";
        if (!fileName.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase))
        {
            fileName = $"{nameof(MetaParser)}.{fileName}.{extension}";
        }

        spc.AddSource(fileName, content);
        //spc.ReportDiagnostic(Diagnostic.Create(DIAGNOSTIC_DEFS.Info, Location.None, $"Generated: {fileName}"));
    }
}
