using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using MetaParser.Contexts;
using MetaParser.Builders;
using MetaParser.Builders.Parser.Functions;
using MetaParser.CodeGen.Base;
using MetaParser.Json.Definitions;
using MetaParser.Builders.TokenLogic.Consumer;
using JetBrains.Annotations;
using MetaParser.Exceptions;
using MetaParser.Json.JsonTypeConverters;
using Microsoft.CodeAnalysis.CSharp;
using MetaParser.Consumers;
using MetaParser.Tokens;
using MetaParser.DepsGraph;

namespace MetaParser;


[Generator(LanguageNames.CSharp)]
public partial class Generator : IIncrementalGenerator
{
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
            var deserializerOptions = new JsonSerializerOptions(JsonSerializerOptions.Default);
            deserializerOptions.Converters.Add(new JsonEnumerableConverter());
            deserializerOptions.Converters.Add(new ValuePatternDeclarationConverterFactory());
            deserializerOptions.Converters.Add(new TokenPatternDeclarationConverterFactory());

            deserializerOptions.AddContext<MetaParserJsonSerializer>();

            var schema = jsonDoc.Deserialize<ParserDefinition>(deserializerOptions);

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
            var result = new MetaParserContext()
            {
                BaseFileName = file.FileName,
                Namespace = schema.Namespace!
            };

            if (schema.ClassName is not null)
            {
                result.ClassName = schema.ClassName;
            }

            if (schema.ParserType is not null)
            {
                result.ParserType = schema.ParserType;
            }

            if (schema.Definitions is not null)
            {
                result.IdType = Common.Get_Integer_Type(schema.Definitions.Count);
            }

            return new ValueTuple<MetaParserContext, ParserDefinition>(result, schema);
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
                int tokenIndex = 0;
                int consumerIndex = 0;
                var Tokens = new List<TokenInfo>();
                var Consumers = new List<ConsumerInfo>();

                foreach (var def in schema.Definitions)
                {
                    var token = new TokenInfo(++tokenIndex, MetaParserContext.Format_Token_Key(def.Key));
                    Tokens.Add(token);

                    foreach (var consumerDeclaration in def.Value)
                    {
                        var consumerData = new ConsumerData(context, consumerDeclaration, ++consumerIndex);
                        var consumer = new ConsumerInfo(token, consumerData);

                        token.Consumers.Add(consumer);
                        Consumers.Add(consumer);
                    }
                }

                context.Tokens = Tokens.ToImmutableDictionary(static (x) => x.Name);
                context.TokenGraph = new DependencyGraph(Tokens);
                
                // We now have the full consumer dependency graph
                // So now we can go through and properly resolve the types of each consumer
                foreach (var consumer in Consumers)
                {
                    consumer.Resolve(context.TokenGraph);
                }

                context.Consumers = new PatternConsumerList() { CompleteSet = Consumers.ToImmutableArray() };
            }

            return context;
        });

        var constantTokens = ctxParserTokens.Select(static (MetaParserContext parser, CancellationToken cancellationToken) => (parser with { Consumers = parser.Consumers with { WorkingSet = parser.Consumers.CompleteSet.Where(o => o.Stage == 0).ToArray() } }));
        var compoundTokens = ctxParserTokens.Select(static (MetaParserContext parser, CancellationToken cancellationToken) => (parser with { Consumers = parser.Consumers with { WorkingSet = parser.Consumers.CompleteSet.Where(o => o.Stage == 1).ToArray() } }));
        var complexTokens = ctxParserTokens.Select(static (MetaParserContext parser, CancellationToken cancellationToken) => (parser with { Consumers = parser.Consumers with { WorkingSet = parser.Consumers.CompleteSet.Where(o => o.Stage >= 2).ToArray() } }));
        #endregion

        // Parser Class
        context.RegisterSourceOutput(ctxParser, static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            using IndentedTextWriter writer = new(new StringWriter());
            context = context with {  writer = writer  };

            new ClassBuilder(MetaParserContext.ParserClassModifiers, context.ClassName!, new ParsingLogic(), ConstantTokenStage.Instance, CompoundTokenStage.Instance, ComplexTokenStage.Instance)
                .WriteTo(context);

            AddSource(spc, $"{context.BaseFileName}.parser.class", writer.InnerWriter.ToString());
        });

        // Constant-Type Tokens
        context.RegisterSourceOutput(constantTokens, static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            var consumer = context.Get_Token_Processor_Function_Definition(EConsumerType.Data, MetaParserContext.ConstantTokenProcessorFunctionName, TokenProcessor.Instance);
            new ClassBuilder(MetaParserContext.ParserClassModifiers, context.ClassName!, consumer)
                .WriteTo(context);

            AddSource(spc, $"{context.BaseFileName}.tokens.constant", writer.InnerWriter.ToString());
        });

        // Compound-Type Tokens
        context.RegisterSourceOutput(compoundTokens, static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            var consumer = context.Get_Token_Processor_Function_Definition(EConsumerType.Token, MetaParserContext.CompoundTokenProcessorFunctionName, TokenProcessor.Instance);
            new ClassBuilder(MetaParserContext.ParserClassModifiers, context.ClassName!, consumer)
                .WriteTo(context);

            AddSource(spc, $"{context.BaseFileName}.tokens.compound", writer.InnerWriter.ToString());
        });

        // Complex-Type Tokens
        context.RegisterSourceOutput(complexTokens, static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            var consumer = context.Get_Token_Processor_Function_Definition(EConsumerType.Token, MetaParserContext.ComplexTokenProcessorFunctionName, RecursiveTokenProcessor.Instance);
            new ClassBuilder(MetaParserContext.ParserClassModifiers, context.ClassName!, consumer)
                .WriteTo(context);

            AddSource(spc, $"{context.BaseFileName}.tokens.complex", writer.InnerWriter.ToString());
        });

        // Token Structure
        context.RegisterSourceOutput(ctxParser, static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            new TokenStructBuilder().WriteTo(context);

            AddSource(spc, $"{context.BaseFileName}.token.struct", writer.InnerWriter.ToString());
        });

        // Enums
        context.RegisterSourceOutput(ctxParserTokens, static (SourceProductionContext spc, [NotNull] MetaParserContext context) => 
        {
            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            writer.WriteLine($"namespace {context.Namespace};");
            writer.WriteLine($"public enum {MetaParserContext.TokenEnum} : {context.IdType}");
            writer.WriteLine("{");
            writer.Indent++;

            TokenIDEnumBuilder.Instance.WriteTo(context);

            writer.Indent--;
            writer.WriteLine("}");

            AddSource(spc, $"{context.BaseFileName}.enum", writer.InnerWriter.ToString());
        });

        // Constants
        context.RegisterSourceOutput(ctxParserTokens, static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            new ClassBuilder(SyntaxFactory.ParseTokens("internal static"), MetaParserContext.TokenConsts, TokenIDConstBuilder.Instance)
                .WriteTo(context);

            AddSource(spc, $"{context.BaseFileName}.constants", writer.InnerWriter.ToString());
        });

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
