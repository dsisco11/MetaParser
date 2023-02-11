using MetaTranspiler.Generators;
using MetaTranspiler.Schemas.Structs;

using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using Json.Schema;
using System.CodeDom.Compiler;
using System.Reflection.Metadata;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Text;

namespace MetaTranspiler
{

    [Generator(LanguageNames.CSharp)]
    public partial class ParserGenerator : IIncrementalGenerator
    {
        #region Constants
        const string fileExtension = "g.cs";
        #endregion

        public void Initialize(IncrementalGeneratorInitializationContext initContext)
        {
            // define the execution pipeline here via a series of transformations:

            // find all additional files that end with .parser-meta.json
            IncrementalValuesProvider<AdditionalText> textFiles = initContext.AdditionalTextsProvider.Where(static file => file.Path.EndsWith(Common.MetaParserFileExtension));

            // read their contents and save their name
            IncrementalValuesProvider<FileData> namesAndContents = textFiles.Select(static (text, cancellationToken) => new FileData(Common.Get_FileName(text.Path.AsMemory()), text.Path, text.GetText(cancellationToken)!.ToString()));

            IncrementalValuesProvider<(FileData, JsonDocument)> jsonDocDef = namesAndContents.Select((Func<FileData, CancellationToken, (FileData, JsonDocument)>)(static (FileData file, CancellationToken cancellationToken) =>
            {
                var doc = JsonDocument.Parse(file.Content, new JsonDocumentOptions() { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true });
                return (file, doc);
            }));

            initContext.RegisterSourceOutput(jsonDocDef, static (SourceProductionContext spc, (FileData file, JsonDocument jsonDoc) data) =>
            {
                FileData file = data.file;
                JsonDocument jsonDoc = data.jsonDoc;
                var schema = Common.ParsingSchema;
                var result = schema.Validate(jsonDoc, Common.SchemaOptions);
                if (!result.IsValid)
                {
                    Convert_Schema_Results_To_Diagnostics(spc, file, result);
                }
            });

            IncrementalValuesProvider<ParserGeneratorContext> parserDef = jsonDocDef.Select((Func<(FileData file,JsonDocument jsonDoc), CancellationToken, ParserGeneratorContext>)(static ((FileData file,JsonDocument doc) data, CancellationToken cancellationToken) =>
            {
                FileData file = data.file;
                JsonDocument jsonDoc = data.doc;
                try
                {
                    var doc = jsonDoc.Deserialize<ParserDefinitionSchema>((System.Text.Json.Serialization.Metadata.JsonTypeInfo<ParserDefinitionSchema>)TokenContext.Default.ParserDefinitionSchema)!;
                    int idx = 0;
                    // copy token definitions from dictionary to array
                    var Tokens = new List<TokenDef>(doc.Tokens.Count);
                    foreach (var tok in doc.Tokens)
                    {
                        tok.Value.Name = tok.Key;
                        tok.Value.Index = ++idx;
                        Tokens.Add(tok.Value);
                    }

                    return new ParserGeneratorContext() { 
                        BaseFileName = file.FileName,
                        Namespace = doc.Namespace!,
                        DefinedTokens = Tokens.ToImmutableArray()
                    };
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }));

            var constantTokens = parserDef.Select(static (ParserGeneratorContext parser, CancellationToken cancellationToken) => (parser with { ConstantTokens = parser.DefinedTokens.OfType<TokenDefConst>().ToImmutableArray() }));
            var compoundTokens = parserDef.Select(static (ParserGeneratorContext parser, CancellationToken cancellationToken) => (parser with { CompoundTokens = parser.DefinedTokens.OfType<TokenDefCompound>().ToImmutableArray() }));
            var complexTokens = parserDef.Select(static (ParserGeneratorContext parser, CancellationToken cancellationToken) => (parser with { ComplexTokens = parser.DefinedTokens.OfType<TokenDefComplex>().ToImmutableArray() }));

            // generate

            // Constant-Type Tokens
            initContext.RegisterSourceOutput(constantTokens, static (SourceProductionContext spc, ParserGeneratorContext context) =>
            {
                if (context.ConstantTokens.Length <= 0) return;

                using StringWriter baseWriter = new();
                using IndentedTextWriter writer = new(baseWriter);

                writer.WriteLine($"namespace {context.Namespace};");
                writer.WriteLine(Common.PartialParserClassDeclaration);
                writer.WriteLine("{");
                writer.Indent++;
                ConstantTokens.Generate_Consumer_Logic(writer, context, context.ConstantTokens);
                writer.Indent--;
                writer.WriteLine("}");
                var strContent = writer.InnerWriter.ToString();


                if (string.IsNullOrWhiteSpace(strContent)) return;
                spc.AddSource($"{context.BaseFileName}.tokens.constant.{fileExtension}", strContent);
            });

            // Compound-Type Tokens
            initContext.RegisterSourceOutput(compoundTokens, static (SourceProductionContext spc, ParserGeneratorContext context) =>
            {
                if (context.CompoundTokens.Length <= 0) return;

                using StringWriter baseWriter = new();
                using IndentedTextWriter writer = new(baseWriter);

                writer.WriteLine($"namespace {context.Namespace};");
                writer.WriteLine(Common.PartialParserClassDeclaration);
                writer.WriteLine("{");
                writer.Indent++;
                CompoundTokens.Generate_Consumer_Logic(writer, context, context.CompoundTokens);
                writer.Indent--;
                writer.WriteLine("}");
                var strContent = writer.InnerWriter.ToString();

                if (string.IsNullOrWhiteSpace(strContent)) return;
                spc.AddSource($"{context.BaseFileName}.tokens.compound.{fileExtension}", strContent);
            });

            initContext.RegisterSourceOutput(complexTokens, static (SourceProductionContext spc, ParserGeneratorContext context) =>
            {
                if (context.ComplexTokens.Length <= 0) return;

                using StringWriter baseWriter = new();
                using IndentedTextWriter writer = new(baseWriter);

                writer.WriteLine($"namespace {context.Namespace};");
                writer.WriteLine(Common.PartialParserClassDeclaration);
                writer.WriteLine("{");
                writer.Indent++;
                ComplexTokens.Generate_Consumer_Logic(writer, context, context.ComplexTokens);
                writer.Indent--;
                writer.WriteLine("}");

                var strContent = writer.InnerWriter.ToString();
                if (string.IsNullOrWhiteSpace(strContent)) return;
                spc.AddSource($"{context.BaseFileName}.tokens.complex.{fileExtension}", strContent);
            });

            // Enums
            initContext.RegisterSourceOutput(parserDef, static (SourceProductionContext spc, ParserGeneratorContext context) => {
                if (context.DefinedTokens.Length <= 0) return;

                using StringWriter baseWriter = new();
                using IndentedTextWriter writer = new(baseWriter);
                writer.WriteLine($"namespace {context.Namespace};");
                writer.WriteLine("public enum EToken");
                writer.WriteLine("{");
                writer.Indent++;

                foreach (var token in context.DefinedTokens)
                {
                    var enumName = Common.Get_TokenId_Enum_Name(token.Name!);
                    writer.WriteLine($"{enumName}: {token.Index},");
                }

                writer.Indent--;
                writer.WriteLine("}");

                var strContent = writer.InnerWriter.ToString();
                if (string.IsNullOrWhiteSpace(strContent)) return;
                spc.AddSource($"{context.BaseFileName}.enum.{fileExtension}", strContent);
            });

            // Constants
            initContext.RegisterSourceOutput(parserDef, static (SourceProductionContext spc, ParserGeneratorContext context) => {
                if (context.DefinedTokens.Length <= 0) return;

                using StringWriter baseWriter = new();
                using IndentedTextWriter writer = new(baseWriter);
                writer.WriteLine($"namespace {context.Namespace};");
                writer.WriteLine("internal static class TokenId");
                writer.WriteLine("{");
                writer.Indent++;

                foreach (var token in context.DefinedTokens)
                {
                    writer.WriteLine($"public const {context.IdValueTypeName} {Common.Get_TokenId_Constant_Name(token.Name!)} = {token.Index};");
                }

                writer.Indent--;
                writer.WriteLine("}");

                var strContent = writer.InnerWriter.ToString();
                if (string.IsNullOrWhiteSpace(strContent)) return;
                spc.AddSource($"{context.BaseFileName}.tokenids.{fileExtension}", strContent);
            });

        }

        private static void Convert_Schema_Results_To_Diagnostics(SourceProductionContext spc, FileData file, ValidationResults Results)
        {
            if (Results.IsValid)
            {
                return;
            }

            if (Results.HasNestedResults)
            {
                foreach (var nestedResults in Results.NestedResults)
                {
                    Convert_Schema_Results_To_Diagnostics(spc, file, nestedResults);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(Results.Message))
                {
                    //var loc = Location.Create(file.Path, new TextSpan(), new LinePositionSpan());
                    var data = new Dictionary<string, string?>()
                    {
                        { "Schema", Results.AbsoluteSchemaLocation.Fragment },
                        { "Location", Results.InstanceLocation.Source }
                    }.ToImmutableDictionary();
                    var diag = Diagnostic.Create(DIAGNOSTIC_ERRORS.SchemaException, Location.None, properties: data, Results.InstanceLocation.Source, Results.Message);
                    spc.ReportDiagnostic(diag);
                }
            }
        }
    }
}
