using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using MetaParser.Core;
using MetaParser.Builders;
using MetaParser.Builders.Parser.Functions;
using MetaParser.Builders.Core;
using MetaParser.Json.Definitions;
using MetaParser.Builders.TokenLogic.Consumer;
using JetBrains.Annotations;
using MetaParser.Exceptions;
using MetaParser.Json.JsonTypeConverters;
using Microsoft.CodeAnalysis.CSharp;
using MetaParser.Consumers;
using MetaParser.Tokens;
using MetaParser.Graphs;

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
                context.WorkingSet.Consumers = new TokenConsumer[1];

                foreach (var def in schema.Definitions)
                {
                    var tokenKey = CodeCommon.Format_Token_Key(def.Key);
                    context.Registry.TryGetToken(tokenKey, out var token);
                    context.WorkingSet.Tokens[0] = token;

                    foreach (var consumerDeclaration in def.Value)
                    {
                        var consumer = new TokenConsumer(context, consumerDeclaration);
                        context.Registry.AddConsumer(consumer);
                    }
                }

                DependencyGraph.Build(context);
                DependencyGraph.Resolve(context);
            }

            return context;
        });

        var constantTokens = ctxParserTokens.Select(static (MetaParserContext parser, CancellationToken cancellationToken) => (parser with { WorkingSet = parser.WorkingSet with { Consumers = parser.Registry.Consumers.Values.Where(static (o) => o.Type == EConsumerType.Data).ToArray() } }));
        var compoundTokens = ctxParserTokens.Select(static (MetaParserContext parser, CancellationToken cancellationToken) => (parser with { WorkingSet = parser.WorkingSet with { Consumers = parser.Registry.Consumers.Values.Where(static (o) => o.Type == EConsumerType.Token && o.DependencyInfo.MaxDepth <= 1).ToArray() } }));
        var complexTokens = ctxParserTokens.Select(static (MetaParserContext parser, CancellationToken cancellationToken) => (parser with { WorkingSet = parser.WorkingSet with { Consumers = parser.Registry.Consumers.Values.Where(static (o) => o.Type == EConsumerType.Token && o.DependencyInfo.MaxDepth > 1).ToArray() } }));
        #endregion

        // Parser Class
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

        // Constant-Type Tokens
        context.RegisterSourceOutput(constantTokens, static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            var consumer = CodeCommon.Get_Token_Processor_Function_Definition(context.Config, EConsumerType.Data, CodeCommon.ConstantTokenProcessorFunctionName);
            new ClassBuilder(CodeCommon.ParserClassModifiers, context.Config.ClassName!)
                .And(consumer)
                .WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.tokens.constant", writer.InnerWriter.ToString());
        });

        // Compound-Type Tokens
        context.RegisterSourceOutput(compoundTokens, static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            var consumer = CodeCommon.Get_Token_Processor_Function_Definition(context.Config, EConsumerType.Token, CodeCommon.CompoundTokenProcessorFunctionName);
            new ClassBuilder(CodeCommon.ParserClassModifiers, context.Config.ClassName!)
                .And(consumer)
                .WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.tokens.compound", writer.InnerWriter.ToString());
        });

        // Complex-Type Tokens
        context.RegisterSourceOutput(complexTokens, static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            var consumer = CodeCommon.Get_Token_Processor_Function_Definition(context.Config, EConsumerType.Token, CodeCommon.ComplexTokenProcessorFunctionName);
            new ClassBuilder(CodeCommon.ParserClassModifiers, context.Config.ClassName!)
                .And(consumer)
                .WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.tokens.complex", writer.InnerWriter.ToString());
        });

        // Token Structure
        context.RegisterSourceOutput(ctxParser, static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            context.Config.CodeFactory.Get_Token_Struct_Builder().WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.token.struct", writer.InnerWriter.ToString());
        });

        // Enums
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

        // Constants
        context.RegisterSourceOutput(ctxParserTokens, static (SourceProductionContext spc, [NotNull] MetaParserContext context) =>
        {
            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            new ClassBuilder(SyntaxFactory.ParseTokens("internal static"), CodeCommon.TokenConsts)
                .And(context.Config.CodeFactory.Get_Token_ID_Constants_Builder())
                .WriteTo(context);

            AddSource(spc, $"{context.Config.BaseFileName}.constants", writer.InnerWriter.ToString());
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
