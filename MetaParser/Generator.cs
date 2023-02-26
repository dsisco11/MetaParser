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
using MetaParser.Structs;
using MetaParser.Patternization;
using MetaParser.Builders.TokenLogic.Consumer;

namespace MetaParser;


[Generator(LanguageNames.CSharp)]
public partial class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        #region Pipeline
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
            var schema = jsonDoc.Deserialize<ParserDefinition>(MetaParserJsonSerializer.Default.ParserDefinition);

            if (schema is null || schema.Definitions is null)
            {
                throw new Exception($"MetaParser schema ({file.Path}) is malformed!");
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

        IncrementalValuesProvider<MetaParserContext> ctxParserTokens = ctxFull.Select(static (ValueTuple<MetaParserContext, ParserDefinition> data, CancellationToken cancellationToken) =>
        {
            var schema = data.Item2;
            var context = data.Item1;

            if (schema?.Definitions is not null)
            {
                int tokenIndex = 0;
                int consumerIndex = 0;
                var Tokens = new List<PatternConsumer>();
                foreach (var def in schema.Definitions)
                {
                    var Name = def.Key;
                    tokenIndex++;

                    foreach (var consumer in def.Value)
                    {
                        consumerIndex++;

                        var startClause = consumer.Start.Length == 0 ? null : new PatternGroup(EPatternCondition.All, consumer.Start.Select(o => o.Resolve(context)).ToArray());
                        var consumeClause = consumer.Consume.Length == 0 ? null : new PatternGroup(EPatternCondition.Any, consumer.Consume.Select(o => o.Resolve(context)).ToArray());
                        var stopClause = consumer.Stop.Length == 0 ? null : new PatternGroup(EPatternCondition.All, consumer.Stop.Select(o => o.Resolve(context)).ToArray());
                        var escapeClause = consumer.Escape.Length == 0 ? null : new PatternGroup(EPatternCondition.All, consumer.Escape.Select(o => o.Resolve(context)).ToArray());

                        if (startClause is null && consumeClause is not null)
                        {
                            startClause = new PatternGroup(EPatternCondition.Any, consumer.Consume.Select(o => o.Resolve(context)).ToArray());
                        }

                        var token = new PatternConsumer(consumer.Type, tokenIndex, consumerIndex, def.Key, startClause, consumeClause, stopClause, escapeClause);
                        Tokens.Add(token);
                    }
                }

                context.Tokens = new TokenDeclarationsList() { CompleteSet = Tokens.ToImmutableArray() };
            }

            return context;
        });

        var constantTokens = ctxParserTokens.Select(static (MetaParserContext parser, CancellationToken cancellationToken) => (parser with { Tokens = parser.Tokens with { WorkingSet = parser.Tokens.CompleteSet.Where(o => o.Type == ETokenType.Constant).ToArray() } }));
        var compoundTokens = ctxParserTokens.Select(static (MetaParserContext parser, CancellationToken cancellationToken) => (parser with { Tokens = parser.Tokens with { WorkingSet = parser.Tokens.CompleteSet.Where(o => o.Type == ETokenType.Compound).ToArray() } }));
        var complexTokens = ctxParserTokens.Select(static (MetaParserContext parser, CancellationToken cancellationToken) => (parser with { Tokens = parser.Tokens with { WorkingSet = parser.Tokens.CompleteSet.Where(o => o.Type == ETokenType.Complex).ToArray() } }));
        #endregion

        // Parser Class
        context.RegisterSourceOutput(ctxParser, static (SourceProductionContext spc, MetaParserContext context) =>
        {
            using IndentedTextWriter writer = new(new StringWriter());
            context = context with {  writer = writer  };

            new ClassBuilder(context.ClassAccessKeywords, context.ClassName, new ParsingLogic(), new ConsumeNextToken())
                .WriteTo(context);

            AddSource(spc, $"{context.BaseFileName}.parser.class", writer.InnerWriter.ToString());
        });

        // Constant-Type Tokens
        context.RegisterSourceOutput(constantTokens, static (SourceProductionContext spc, MetaParserContext context) =>
        {
            if (context?.Tokens.WorkingSet.Length <= 0) return;

            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            var consumer = context.Get_ValueToken_Consumer(context.ConstantTokenConsumerFunctionName, DetectAndConsumeLogic.Instance);
            new ClassBuilder(context.ClassAccessKeywords, context.ClassName, consumer)
                .WriteTo(context);

            AddSource(spc, $"{context.BaseFileName}.tokens.constant", writer.InnerWriter.ToString());
        });

        // Compound-Type Tokens
        context.RegisterSourceOutput(compoundTokens, static (SourceProductionContext spc, MetaParserContext context) =>
        {
            if (context?.Tokens.WorkingSet.Length <= 0) return;

            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            var consumer = context.Get_ValueToken_Consumer(context.CompoundTokenConsumerFunctionName, DetectAndConsumeLogic.Instance);
            new ClassBuilder(context.ClassAccessKeywords, context.ClassName, consumer)
                .WriteTo(context);

            AddSource(spc, $"{context.BaseFileName}.tokens.compound", writer.InnerWriter.ToString());
        });

        // Complex-Type Tokens
        context.RegisterSourceOutput(complexTokens, static (SourceProductionContext spc, MetaParserContext context) =>
        {
            if (context?.Tokens.WorkingSet.Length <= 0) return;

            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            var consumer = context.Get_Token_Consumer(context.ComplexTokenConsumerFunctionName, DetectAndConsumeLogic.Instance);
            new ClassBuilder(context.ClassAccessKeywords, context.ClassName, consumer)
                .WriteTo(context);

            AddSource(spc, $"{context.BaseFileName}.tokens.complex", writer.InnerWriter.ToString());
        });

        // Token Structure
        context.RegisterSourceOutput(ctxParser, static (SourceProductionContext spc, MetaParserContext context) =>
        {
            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            new TokenStructBuilder().WriteTo(context);

            AddSource(spc, $"{context.BaseFileName}.token.struct", writer.InnerWriter.ToString());
        });

        // Enums
        context.RegisterSourceOutput(ctxParserTokens, static (SourceProductionContext spc, MetaParserContext context) => {
            if (context?.Tokens.WorkingSet.Length <= 0) return;

            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            writer.WriteLine($"namespace {context?.Namespace};");
            writer.WriteLine($"public enum {context.TokenEnum} : {context.IdTypeName}");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine($"Unknown = ({context.IdTypeName}) 0,");

            var distinct = context.Tokens.WorkingSet.ToImmutableSortedSet(new ConsumerComparer());
            foreach (var token in distinct)
            {
                var enumName = context.Format_TokenId(token.IdName);
                writer.WriteLine($"{enumName} = ({context.IdTypeName}) {token.TokenIndex},");
            }

            writer.Indent--;
            writer.WriteLine("}");

            AddSource(spc, $"{context.BaseFileName}.enum", writer.InnerWriter.ToString());
        });

        // Constants
        context.RegisterSourceOutput(ctxParserTokens, static (SourceProductionContext spc, MetaParserContext context) =>
        {
            if (context?.Tokens.WorkingSet.Length <= 0) return;

            using IndentedTextWriter writer = new(new StringWriter());
            context = context with { writer = writer };

            writer.WriteLine($"namespace {context?.Namespace};");
            writer.WriteLine($"internal static class {context.TokenConsts}");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine($"public const {context.IdTypeName} {context.Format_TokenId("unknown")} = 0;");

            var distinct = context.Tokens.WorkingSet.ToImmutableSortedSet(new ConsumerComparer());
            foreach (var token in distinct)
            {
                writer.WriteLine($"public const {context.IdTypeName} {context.Format_TokenId(token.IdName)} = {token.TokenIndex};");
            }

            writer.Indent--;
            writer.WriteLine("}");

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
