using JetBrains.Annotations;

using MetaParser.Builders.Core;
using MetaParser.Builders.Parser;
using MetaParser.Builders.Parser.Functions;
using MetaParser.Compiler;
using MetaParser.Core;
using MetaParser.Exceptions;
using MetaParser.Graphs;
using MetaParser.Json.Definitions;
using MetaParser.Json.JsonTypeConverters;
using MetaParser.Mermaid;
using MetaParser.Parsing.Constructs;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using System;
using System.CodeDom.Compiler;
using System.IO;
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
            deserializerOptions.Converters.Add(new PatternDeclarationConverterFactory());

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

        IncrementalValuesProvider<ParserInterpreter> interpreterStart = ctxSchema.Select(static (ValueTuple<FileData, ParserDefinition> data, CancellationToken cancellationToken) =>
        {
            FileData file = data.Item1;
            var definition = data.Item2;

            return new ParserInterpreter(file.FileName, data.Item2);
        });

        #region Interpreter Steps

        var declared = interpreterStart.Select(static (ParserInterpreter interpreter, CancellationToken cancellationToken) =>
        {
            return interpreter.ExecuteNext();
        });

        var assigned = declared.Select(static (ParserInterpreter interpreter, CancellationToken cancellationToken) =>
        {
            return interpreter.ExecuteNext();
        });

        var specified = assigned.Select(static (ParserInterpreter interpreter, CancellationToken cancellationToken) =>
        {
            return interpreter.ExecuteNext();
        });

        var computed = specified.Select(static (ParserInterpreter interpreter, CancellationToken cancellationToken) =>
        {
            return interpreter.ExecuteNext();
        });

        var used = computed.Select(static (ParserInterpreter interpreter, CancellationToken cancellationToken) =>
        {
            return interpreter.ExecuteNext();
        });
        #endregion

        IncrementalValuesProvider<ParserContext> ctxParser = used.Select(static (ParserInterpreter interpreter, CancellationToken cancellationToken) =>
        {
            return interpreter.Compile();
        });

        IncrementalValuesProvider<ParserContext> ctxRecursiveTokens = ctxParser.Select(static (ParserContext context, CancellationToken cancellationToken) =>
        {
            var tokens = context.Registry.Tokens.Where(static (entity) => entity.DependencyInfo.NodeDepth.Max > 0);
            return (context with { State = context.State with { Targets = new WorkingSet(tokens) } });
        });
        #endregion

#if DEBUG
        context.RegisterSourceOutput(ctxParser, static (SourceProductionContext spc, [NotNull] ParserContext context) =>
        {
            context.Writer = new IndentedTextWriter(new StringWriter());
            var writer = context.Writer;
            var graph = new DirectedGraph(context.DepsGraph);
            DependencyGraph.Simplify_Graph(graph);

            writer.WriteLine("/*");
            writer.WriteLine("```mermaid");
            var mermaidFormatter = new MermaidFormatter(context.Registry, graph);
            mermaidFormatter.Write(writer, MermaidChartType.Graph);
            writer.WriteLine("```");
            writer.WriteLine("*/");

            spc.AddSource($"{context.Config.BaseFileName}.dependency_graph.md", writer.InnerWriter.ToString());
        });
#endif

#if DEBUG
        context.RegisterSourceOutput(ctxParser, static (SourceProductionContext spc, [NotNull] ParserContext context) =>
        {
            context = context with { Writer = new IndentedTextWriter(new StringWriter()) };
            var writer = context.Writer;

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
                writer.WriteLine(consumer.DependencyInfo);
            }
            writer.WriteLine();

            foreach (var pattern in context.Registry.Patterns)
            {
                writer.WriteLine(pattern.DependencyInfo);
            }
            writer.WriteLine("```");
            writer.WriteLine("*/");

            spc.AddSource($"{context.Config.BaseFileName}.registry.debug", writer.InnerWriter.ToString());
        });
#endif

        #region Parser Class
        context.RegisterSourceOutput(ctxParser, static (SourceProductionContext spc, [NotNull] ParserContext context) =>
        {
            context = context with { Writer = new IndentedTextWriter(new StringWriter()) };
            var writer = context.Writer;

            CodeBuilderFactory codeFactory = context.Config.CodeFactory;
            new ClassBuilder(CodeCommon.ParserClassModifiers, context.Config.ClassName!)
            .And(codeFactory.Get_Parsing_Logic())
            .And(new GenParsingTableExecutors())
                .WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.parser.class", context.Writer.InnerWriter.ToString());
        });
        #endregion

        #region Consumer Builders
        context.RegisterSourceOutput(ctxParser.Select(static (ParserContext context, CancellationToken cancellationToken) =>
        {
            ConsumerEntity[] consumers = context.Registry.Consumers.Where(static (o) => !o.IsConstant).ToArray();
            return context with 
            { 
                State = context.State with 
                { 
                    Targets = new WorkingSet(consumers) 
                },
                Writer = new IndentedTextWriter(new StringWriter())
            };
        }),
        static (SourceProductionContext spc, [NotNull] ParserContext context) =>
        {
            var writer = context.Writer!;

            new ClassBuilder(CodeCommon.ParserClassModifiers, context.Config.ClassName!)
                .And(new GenPatternConsumerFunctions())
                .WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.consumers", writer.InnerWriter.ToString());
        });
        #endregion

        #region Token Start Detection
        // Any token which has an incoming link must have a start detection function
        context.RegisterSourceOutput(ctxRecursiveTokens,
        static (SourceProductionContext spc, [NotNull] ParserContext context) =>
        {
            context = context with { Writer = new IndentedTextWriter(new StringWriter()) };
            var writer = context.Writer;

            new ClassBuilder(CodeCommon.ParserClassModifiers, context.Config.ClassName!)
                .And(new GenTokenStartDetectors())
                .WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.tokens.detection", context.Writer.InnerWriter.ToString());
        });
        #endregion

        #region Token Builders
        // Any token which has an incoming link must have a dedicated consumption function
        context.RegisterSourceOutput(ctxRecursiveTokens,
        static (SourceProductionContext spc, [NotNull] ParserContext context) =>
        {
            context = context with { Writer = new IndentedTextWriter(new StringWriter()) };
            var writer = context.Writer;

            new ClassBuilder(CodeCommon.ParserClassModifiers, context.Config.ClassName!)
                .And(new GenTokenConsumeFunctions())
                .WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.tokens.consumption", context.Writer.InnerWriter.ToString());
        });
        #endregion

        #region Parse Table Executors
        context.RegisterSourceOutput(ctxParser, static (SourceProductionContext spc, [NotNull] ParserContext context) =>
        {
            // for each stage in context, generate the parsing table function in a different file for the stage
            foreach (var stage in context.Stages)
            {
                context = context with 
                { 
                    Writer = new IndentedTextWriter(new StringWriter()),
                    State = context.State with
                    {
                        Stage = stage,
                        Targets = new WorkingSet(stage.Consumers)
                    }
                };
                new ClassBuilder(CodeCommon.ParserClassModifiers, context.Config.ClassName!)
                    .And(context.Config.CodeFactory.Get_Parsing_Table_Function())
                    .WriteTo(context);
                AddSource(spc, $"{context.Config.BaseFileName}.parsing_table.stage_{stage.Index}", context.Writer.InnerWriter.ToString());
            }
        });
        #endregion

        #region Result Structure
        context.RegisterSourceOutput(ctxParser, static (SourceProductionContext spc, [NotNull] ParserContext context) =>
        {
            context = context with { Writer = new IndentedTextWriter(new StringWriter()) };
            var writer = context.Writer;

            context.Config.CodeFactory.Get_Parsing_Struct_Builder().WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.parser.structs", context.Writer.InnerWriter.ToString());
        });
        #endregion

        #region Token Structure
        context.RegisterSourceOutput(ctxParser, static (SourceProductionContext spc, [NotNull] ParserContext context) =>
        {
            context = context with { Writer = new IndentedTextWriter(new StringWriter()) };
            var writer = context.Writer;

            context.Config.CodeFactory.Get_Token_Struct_Builder().WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.token.struct", context.Writer.InnerWriter.ToString());
        });
        #endregion

        #region Token-ID Enums
        context.RegisterSourceOutput(ctxParser, static (SourceProductionContext spc, [NotNull] ParserContext context) =>
        {
            context = context with { Writer = new IndentedTextWriter(new StringWriter()) };
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
        context.RegisterSourceOutput(ctxParser, static (SourceProductionContext spc, [NotNull] ParserContext context) =>
        {
            context = context with { Writer = new IndentedTextWriter(new StringWriter()) };
            var writer = context.Writer;

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
