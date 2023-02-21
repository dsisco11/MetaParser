using MetaParser.Generators;
using MetaParser.Schemas.Structs;

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

namespace MetaParser;


[Generator(LanguageNames.CSharp)]
public partial class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext Context)
    {
        // define the execution pipeline here via a series of transformations:

        // find all additional files that end with .parser-meta.json
        IncrementalValuesProvider<AdditionalText> ctxFileNames = Context.AdditionalTextsProvider.Where(static file => file.Path.EndsWith(Common.MetaParserFileExtension));

        // read their contents and save their name
        IncrementalValuesProvider<FileData> ctxFiles = ctxFileNames.Select(static (text, cancellationToken) => new FileData(Common.Get_FileName(text.Path), text.Path, text.GetText(cancellationToken)!.ToString()));

        IncrementalValuesProvider<(FileData, JsonDocument)> ctxParserJson = ctxFiles.Select((Func<FileData, CancellationToken, ValueTuple<FileData, JsonDocument>>)(static (FileData file, CancellationToken cancellationToken) =>
        {
            var doc = JsonDocument.Parse(file.Content, new JsonDocumentOptions() { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true });
            return (file, doc);
        }));

        IncrementalValuesProvider<ValueTuple<FileData, ParserDefinitionSchema>> ctxSchema = ctxParserJson.Select(selector: (Func<(FileData file, JsonDocument jsonDoc), CancellationToken, ValueTuple<FileData, ParserDefinitionSchema>>)(static (ValueTuple<FileData, JsonDocument> data, CancellationToken cancellationToken) =>
        {
            FileData file = data.Item1;
            JsonDocument jsonDoc = data.Item2;
            var schema = jsonDoc.Deserialize(TokenJsonContext.Default.ParserDefinitionSchema)!;
            return (file, schema);
        }));

        IncrementalValuesProvider<ValueTuple<MetaParserContext, ParserDefinitionSchema>> ctxFull = ctxSchema.Select(static (ValueTuple<FileData, ParserDefinitionSchema> data, CancellationToken cancellationToken) =>
        {
            FileData file = data.Item1;
            var schema = data.Item2;
            var result = new MetaParserContext()
            {
                BaseFileName = file.FileName,
                Namespace = schema.Namespace!,
            };

            if (schema.Tokens is not null)
            {
                result.IdType = Common.Get_Integer_Type(schema.Tokens.Count);
            }

            return new ValueTuple<MetaParserContext, ParserDefinitionSchema>(result, schema);
        });

        IncrementalValuesProvider<MetaParserContext> ctxParser = ctxFull.Select(static (ValueTuple<MetaParserContext, ParserDefinitionSchema> data, CancellationToken cancellationToken) =>
        {
            return data.Item1;
        });

        IncrementalValuesProvider<MetaParserTokenContext> ctxParserTokens = ctxFull.Select(static (ValueTuple<MetaParserContext, ParserDefinitionSchema> data, CancellationToken cancellationToken) =>
        {
            var schema = data.Item2;
            var baseContext = data.Item1;
            var result = new MetaParserTokenContext(baseContext);

            if (schema?.Tokens is not null)
            {
                int idx = 0;
                // copy token definitions from dictionary to array
                var Tokens = new List<TokenDef>(schema.Tokens.Count);
                foreach (var tok in schema.Tokens)
                {
                    tok.Value.Name = tok.Key;
                    tok.Value.Index = ++idx;
                    Tokens.Add(tok.Value);
                }

                result.DefinedTokens = Tokens.ToImmutableArray();
            }

            return result;
        });

        var constantTokens = ctxParserTokens.Select(static (MetaParserTokenContext parser, CancellationToken cancellationToken) => (parser with { ConstantTokens = parser.DefinedTokens.OfType<TokenDefConstant>().ToImmutableArray() }));
        var compoundTokens = ctxParserTokens.Select(static (MetaParserTokenContext parser, CancellationToken cancellationToken) => (parser with { CompoundTokens = parser.DefinedTokens.OfType<TokenDefCompound>().ToImmutableArray() }));
        var complexTokens = ctxParserTokens.Select(static (MetaParserTokenContext parser, CancellationToken cancellationToken) => (parser with { ComplexTokens = parser.DefinedTokens.OfType<TokenDefComplex>().ToImmutableArray() }));

        // Parser Class
        Context.RegisterSourceOutput(ctxParser, static (SourceProductionContext spc, MetaParserContext context) =>
        {
            using IndentedTextWriter writer = new(new StringWriter());

            ParserClassGenerator.Generate(writer, context);

            AddSource(spc, $"{context.BaseFileName}.parser.class", writer.InnerWriter.ToString());
        });

        // Token Structure
        Context.RegisterSourceOutput(ctxParser, static (SourceProductionContext spc, MetaParserContext context) =>
        {
            using IndentedTextWriter writer = new(new StringWriter());

            TokenStructGenerator.Generate(writer, context);

            AddSource(spc, $"{context.BaseFileName}.token.struct", writer.InnerWriter.ToString());
        });

        // Constant-Type Tokens
        Context.RegisterSourceOutput(constantTokens, static (SourceProductionContext spc, MetaParserTokenContext context) =>
        {
            if (context.ConstantTokens.Length <= 0) return;

            using IndentedTextWriter writer = new(new StringWriter());

            writer.WriteLine($"namespace {context.Namespace};");
            writer.WriteLine($"{Common.ParserClassDeclaration}");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine($"private bool try_consume_token_constant ({CodeGen.FormatReadOnlyMemoryBuffer(context.InputType)} source, out {context.IdTypeName} id, out {typeof(int).FullName} length)");
            writer.WriteLine("{");
            writer.Indent++;
            context.ConstantTokenGen.Generate(writer, context);
            writer.Indent--;
            writer.WriteLine("}");// end function
            writer.Indent--;
            writer.WriteLine("}");// end class

            var strContent = writer.InnerWriter.ToString();
            if (string.IsNullOrWhiteSpace(strContent)) return;
            AddSource(spc, $"{context.BaseFileName}.tokens.constant", strContent);
        });

        // Compound-Type Tokens
        Context.RegisterSourceOutput(compoundTokens, static (SourceProductionContext spc, MetaParserTokenContext context) =>
        {
            if (context.CompoundTokens.Length <= 0) return;

            using IndentedTextWriter writer = new(new StringWriter());

            writer.WriteLine($"namespace {context.Namespace};");
            writer.WriteLine($"{Common.ParserClassDeclaration}");
            writer.WriteLine("{");
            writer.WriteLine($"private bool try_consume_token_compound ({CodeGen.FormatReadOnlyMemoryBuffer(context.InputType)} source, out {context.IdTypeName} id, out {typeof(int).FullName} length)");
            writer.WriteLine("{");
            writer.Indent++;
            context.CompoundTokenGen.Generate(writer, context);
            writer.Indent--;
            writer.WriteLine("}");// end function
            writer.Indent--;
            writer.WriteLine("}");// end class

            var strContent = writer.InnerWriter.ToString();
            if (string.IsNullOrWhiteSpace(strContent)) return;
            AddSource(spc, $"{context.BaseFileName}.tokens.compound", strContent);
        });

        // Complex-Type Tokens
        Context.RegisterSourceOutput(complexTokens, static (SourceProductionContext spc, MetaParserTokenContext context) =>
        {
            if (context.ComplexTokens.Length <= 0) return;

            using IndentedTextWriter writer = new(new StringWriter());

            writer.WriteLine($"namespace {context.Namespace};");
            writer.WriteLine($"{Common.ParserClassDeclaration}");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine($"private bool try_consume_token_complex ({CodeGen.FormatReadOnlyMemoryBuffer(context.IdType)} source, out {context.IdTypeName} id, out {typeof(int).FullName} length)");
            writer.WriteLine("{");
            writer.Indent++;
            context.ComplexTokenGen.Generate(writer, context);
            writer.Indent--;
            writer.WriteLine("}");// end function
            writer.Indent--;
            writer.WriteLine("}");// end class

            var strContent = writer.InnerWriter.ToString();
            if (string.IsNullOrWhiteSpace(strContent)) return;
            AddSource(spc, $"{context.BaseFileName}.tokens.complex", strContent);
        });

        // Enums
        Context.RegisterSourceOutput(ctxParserTokens, static (SourceProductionContext spc, MetaParserTokenContext context) => {
            if (context.DefinedTokens.Length <= 0) return;

            using IndentedTextWriter writer = new(new StringWriter());
            writer.WriteLine($"namespace {context.Namespace};");
            writer.WriteLine($"public enum {context.TokenEnum} : {context.IdTypeName}");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine($"Unknown = ({context.IdType.Name}) 0,");

            foreach (var token in context.DefinedTokens)
            {
                var enumName = context.Format_TokenId(token.Name!);
                writer.WriteLine($"{enumName} = ({context.IdType.Name}) {token.Index},");
            }

            writer.Indent--;
            writer.WriteLine("}");

            var strContent = writer.InnerWriter.ToString();
            if (string.IsNullOrWhiteSpace(strContent)) return;
            AddSource(spc, $"{context.BaseFileName}.enum", strContent);
        });

        // Constants
        Context.RegisterSourceOutput(ctxParserTokens, static (SourceProductionContext spc, MetaParserTokenContext context) =>
        {
            if (context.DefinedTokens.Length <= 0) return;

            using IndentedTextWriter writer = new(new StringWriter());
            writer.WriteLine($"namespace {context.Namespace};");
            writer.WriteLine($"internal static class {context.TokenConsts}");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine($"public const {context.IdTypeName} {context.Format_TokenId("unknown")} = 0;");

            foreach (var token in context.DefinedTokens)
            {
                writer.WriteLine($"public const {context.IdTypeName} {context.Format_TokenId(token.Name!)} = {token.Index};");
            }

            writer.Indent--;
            writer.WriteLine("}");

            var strContent = writer.InnerWriter.ToString();
            if (string.IsNullOrWhiteSpace(strContent)) return;
            AddSource(spc, $"{context.BaseFileName}.constants", strContent);
        });

    }

    private static void AddSource(SourceProductionContext spc, string fileName, string content)
    {
        const string extension = "g.cs";
        fileName = $"{nameof(MetaParser)}.{fileName}";
        if (!fileName.EndsWith(extension))
        {
            fileName = $"{nameof(MetaParser)}.{fileName}.{extension}";
        }

        spc.AddSource(fileName, content);
        //spc.ReportDiagnostic(Diagnostic.Create(DIAGNOSTIC_DEFS.Info, Location.None, $"Generated: {fileName}"));
    }
}
