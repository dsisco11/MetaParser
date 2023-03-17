using JetBrains.Annotations;

using MetaParser.Builders.Core;
using MetaParser.Builders.Parser;
using MetaParser.Builders.Parser.Functions;
using MetaParser.Core;
using MetaParser.Exceptions;
using MetaParser.Graphs;
using MetaParser.Json.Definitions;
using MetaParser.Json.JsonTypeConverters;
using MetaParser.Parsing.Constructs;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using System;
using System.Linq;
using System.Text.Json;
using System.Threading;

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

            if (schema is null || schema.Stages is null)
            {
                throw new MalformedSchemaException($"MetaParser schema ({file.Path}) is malformed!");
            }

            return (file, schema!);
        }));

        IncrementalValuesProvider<ValueTuple<MetaParserContext, ParserDefinition>> ctxFull = ctxSchema.Select(static (ValueTuple<FileData, ParserDefinition> data, CancellationToken cancellationToken) =>
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

            if (schema.Stages is not null)
            {
                var lexerTokenNames = schema.Stages.LexingStage?.Consumers?.Keys;
                var grammarTokenNames = schema.Stages.GrammarStage?.Consumers?.Keys;
                var allTokenNames = lexerTokenNames.Concat(grammarTokenNames).Distinct().ToList();

                config.IdType = Common.Get_Integer_Type(allTokenNames.Count);
            }

            return new ValueTuple<MetaParserContext, ParserDefinition>(new MetaParserContext() { Config = config }, schema);
        });

        IncrementalValuesProvider<MetaParserContext> ctxParserOnly = ctxFull.Select(static (ValueTuple<MetaParserContext, ParserDefinition> data, CancellationToken cancellationToken) =>
        {
            return data.Item1;
        });

        IncrementalValuesProvider<ValueTuple<MetaParserContext, IParsingStageDefinition?>> ctxLexingStage = ctxFull.Select(static (ValueTuple<MetaParserContext, ParserDefinition> data, CancellationToken cancellationToken) =>
        {
            return new ValueTuple<MetaParserContext, IParsingStageDefinition?>(data.Item1, data.Item2.Stages!.LexingStage);
        });

        IncrementalValuesProvider<ValueTuple<MetaParserContext, IParsingStageDefinition?>> ctxGrammarStage = ctxFull.Select(static (ValueTuple<MetaParserContext, ParserDefinition> data, CancellationToken cancellationToken) =>
        {
            return new ValueTuple<MetaParserContext, IParsingStageDefinition?>(data.Item1, data.Item2.Stages!.GrammarStage);
        });
        #endregion

        #region Resolving

        IncrementalValuesProvider<MetaParserContext> ctxTokens = ctxFull.Select(static (ValueTuple<MetaParserContext, ParserDefinition> data, CancellationToken cancellationToken) =>
        {
            var context = data.Item1 with { };// clone the context, so we don't end up mutating other providers
            var parserDefinition = data.Item2;
            ParsingStages stages = parserDefinition.Stages!;

            if (stages.LexingStage is not null)
            {
                Populate(context, stages.LexingStage);
            }

            if (stages.GrammarStage is not null)
            {
                Populate(context, stages.GrammarStage);
            }

            context.DepsGraph = DependencyGraph.Build(context.Registry);

            return context;

            static void Populate(MetaParserContext context, IParsingStageDefinition stage)
            {
                context.WorkingSet.Tokens = new TokenInfo[1];
                context.WorkingSet.Consumers = new Consumer[1];

                if (stage.Consumers is null)
                {
                    throw new ArgumentNullException(nameof(stage));
                }

                foreach (var definition in stage.Consumers)
                {
                    string tokenKey = CodeCommon.Format_Token_Key(definition.Key);
                    var tokenInfo = new TokenInfo(tokenKey, context);
                    context.Registry.AddToken(tokenInfo);

                    context.WorkingSet.Tokens[0] = tokenInfo;
                    foreach (var consumerDeclaration in definition.Value)
                    {
                        var consumer = new Consumer(context, consumerDeclaration);
                    }
                }

            }
        });

        #endregion

        //#if DEBUG
        //        context.RegisterSourceOutput(ctxParserTokens, static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        //        {
        //            var writer = context.Writer;
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
        context.RegisterSourceOutput(ctxTokens, static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            var writer = context.Writer;

            writer.WriteLine("/*");
            writer.WriteLine("```");
            // write all items in the registry
            foreach (var token in context.Registry.Tokens.Values)
            {
                writer.WriteLine(token);
            }
            writer.WriteLine();

            foreach (var consumer in context.Registry.Consumers.Values)
            {
                writer.WriteLine(consumer.DependencyInfo);
            }
            writer.WriteLine();

            foreach (var pattern in context.Registry.Patterns.Values)
            {
                writer.WriteLine(pattern.DependencyInfo);
            }
            writer.WriteLine("```");
            writer.WriteLine("*/");

            spc.AddSource($"{context.Config.BaseFileName}.registry.debug", writer.InnerWriter.ToString());
        });
#endif

        #region Parser Class
        context.RegisterSourceOutput(ctxParserOnly, static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            CodeBuilderFactory codeFactory = context.Config.CodeFactory;
            new ClassBuilder(CodeCommon.ParserClassModifiers, context.Config.ClassName!)
            .And(codeFactory.Get_Parsing_Logic())
            .And(new ProcessingLogicLexer())
            .And(new CompoundTokenStage())
            .And(new ComplexTokenStage())
                .WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.parser.class", context.Writer.InnerWriter.ToString());
        });
        #endregion

        #region Token Start Detection
        // Any token which has an incoming link must have a start detection function
        context.RegisterSourceOutput(ctxTokens.Select(static (MetaParserContext parser, CancellationToken cancellationToken) =>
        {
            var tokens = parser.Registry.Tokens.Values.Where(static (t) => t.DependencyInfo.NodeDepth.Max > 0);
            return (parser with { WorkingSet = new WorkingSet(tokens) });
        }),
        static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            new ClassBuilder(CodeCommon.ParserClassModifiers, context.Config.ClassName!)
                .And(new GenTokenStartDetectors())
                .WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.tokens.detection", context.Writer.InnerWriter.ToString());
        });
        #endregion

        #region Constant-Type Tokens
        context.RegisterSourceOutput(ctxTokens.Select(static (MetaParserContext parser, CancellationToken cancellationToken) =>
        {
            Consumer[] consumers = parser.Registry.Consumers.Values.Where(static (o) => o.Type == EConsumerType.Lexer).ToArray();
            return (parser with { WorkingSet = new WorkingSet(consumers) });
        }),
        static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            var consumer = CodeCommon.Get_Token_Processor_Function_Definition(context, EConsumerType.Lexer, CodeCommon.ConstantTokenProcessorFunctionName);
            new ClassBuilder(CodeCommon.ParserClassModifiers, context.Config.ClassName!)
                .And(consumer)
                .WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.tokens.constant", context.Writer.InnerWriter.ToString());
        });
        #endregion

        #region Compound-Type Tokens
        context.RegisterSourceOutput(ctxTokens.Select(static (MetaParserContext parser, CancellationToken cancellationToken) =>
        {
            Consumer[] consumers = parser.Registry.Consumers.Values.Where(static (o) => o.Type == EConsumerType.Syntax && !o.DependencyInfo!.IsRecursive).ToArray();
            return (parser with { WorkingSet = new WorkingSet(consumers) });
        }),
        static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            var consumer = CodeCommon.Get_Token_Processor_Function_Definition(context, EConsumerType.Syntax, CodeCommon.CompoundTokenProcessorFunctionName);
            new ClassBuilder(CodeCommon.ParserClassModifiers, context.Config.ClassName!)
                .And(consumer)
                .WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.tokens.compound", context.Writer.InnerWriter.ToString());
        });
        #endregion

        #region Complex-Type Tokens
        context.RegisterSourceOutput(ctxTokens.Select(static (MetaParserContext parser, CancellationToken cancellationToken) =>
        {
            Consumer[] consumers = parser.Registry.Consumers.Values.Where(static (o) => o.Type == EConsumerType.Syntax && o.DependencyInfo!.IsRecursive).ToArray();
            return (parser with { WorkingSet = new WorkingSet(consumers) });
        }),
        static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            var consumer = CodeCommon.Get_Token_Processor_Function_Definition(context, EConsumerType.Syntax, CodeCommon.ComplexTokenProcessorFunctionName);
            new ClassBuilder(CodeCommon.ParserClassModifiers, context.Config.ClassName!)
                .And(consumer)
                .WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.tokens.complex", context.Writer.InnerWriter.ToString());
        });
        #endregion

        #region Token Structure
        context.RegisterSourceOutput(ctxParserOnly, static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            context.Config.CodeFactory.Get_Token_Struct_Builder().WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.token.struct", context.Writer.InnerWriter.ToString());
        });
        #endregion

        #region Token-ID Enums
        context.RegisterSourceOutput(ctxTokens, static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            var writer = context.Writer;

            writer.WriteLine($"namespace {context.Config.Namespace};");
            writer.WriteLine($"public enum {CodeCommon.TokenEnum} : {context.Config.IdType}");
            writer.WriteLine("{");
            writer.Indent++;

            context.Config.CodeFactory.Get_Token_ID_Enum_Builder().WriteTo(context);

            writer.Indent--;
            writer.WriteLine("}");

            AddSource(spc, $"{context.Config.BaseFileName}.enum", context.Writer.InnerWriter.ToString());
        });
        #endregion

        #region Token-ID Constants
        context.RegisterSourceOutput(ctxTokens, static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            new ClassBuilder(SyntaxFactory.ParseTokens("internal static"), CodeCommon.TokenConsts)
                .And(context.Config.CodeFactory.Get_Token_ID_Constants_Builder())
                .WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.constants", context.Writer.InnerWriter.ToString());
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
