using MetaParser.Generation;
using MetaParser.Schema;

using Microsoft.CodeAnalysis;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace MetaParser;

/// <summary>
/// MetaParser v2 incremental source generator.
/// Transforms .metaparser.json schema files into parser code.
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class Generator : IIncrementalGenerator
{
    private static readonly JsonSerializerOptions s_serializerOptions = CreateSerializerOptions();

    private static JsonSerializerOptions CreateSerializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };
        
        // Add the custom pattern converter
        options.Converters.Add(new PatternDefinitionConverter());
        
        // Add the source-generated context for other types
        options.TypeInfoResolverChain.Add(SchemaJsonContext.Default);
        
        return options;
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Step 1: Find all .metaparser.json files
        var schemaFiles = context.AdditionalTextsProvider
            .Where(static file => file.Path.EndsWith(Common.MetaParserFileExtension, StringComparison.OrdinalIgnoreCase));

        // Step 2: Read file contents
        var fileContents = schemaFiles.Select(static (text, cancellationToken) =>
        {
            var content = text.GetText(cancellationToken)?.ToString() ?? string.Empty;
            return new FileData(
                Common.Get_FileName(text.Path),
                text.Path,
                content);
        });

        // Step 3: Parse JSON into SchemaDefinition
        var schemas = fileContents.Select(static (file, cancellationToken) =>
        {
            var schema = ParseSchema(file.Content, cancellationToken);
            return (File: file, Schema: schema);
        });

        // Step 4: Validate schemas using SchemaValidator
        var validatedSchemas = schemas.Select(static (data, cancellationToken) =>
        {
            var result = SchemaValidator.Validate(data.Schema);
            return (data.File, data.Schema, Validation: result);
        });

        // Step 5: Generate code for valid schemas
        context.RegisterSourceOutput(validatedSchemas, static (spc, data) =>
        {
            // Report any validation errors
            foreach (var error in data.Validation.Errors)
            {
                spc.ReportDiagnostic(Diagnostic.Create(
                    DIAGNOSTIC_DEFS.SchemaValidationError,
                    Location.None,
                    error));
            }

            // Report any validation warnings
            foreach (var warning in data.Validation.Warnings)
            {
                spc.ReportDiagnostic(Diagnostic.Create(
                    DIAGNOSTIC_DEFS.SchemaValidationWarning,
                    Location.None,
                    warning));
            }

            // Skip code generation if there are errors or schema is null
            if (!data.Validation.IsValid || data.Schema is null)
                return;

            // TODO: Implement v2 code generation
            // Phase 5: Graffs integration for dependency analysis
            // Phase 6: Code generation pipeline
            
            GenerateCode(spc, data.File, data.Schema);
        });
    }

    private static SchemaDefinition? ParseSchema(string json, CancellationToken cancellationToken)
    {
        try
        {
            return JsonSerializer.Deserialize<SchemaDefinition>(json, s_serializerOptions);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static void GenerateCode(SourceProductionContext spc, FileData file, SchemaDefinition schema)
    {
        // Phase 3: Green node infrastructure
        spc.AddSource(GreenNodeGenerator.Generate(schema));
        spc.AddSource(GreenTriviaGenerator.Generate(schema));
        spc.AddSource(GreenTokenGenerator.Generate(schema));
        spc.AddSource(GreenTokenFactoryGenerator.Generate(schema));

        // Phase 4: Token kinds, consumers, and lexer
        spc.AddSource(TokenKindGenerator.Generate(schema));
        spc.AddSource(ConsumerInterfaceGenerator.Generate(schema));
        spc.AddSource(SequenceReaderExtensionsGenerator.Generate(schema));
        spc.AddSource(TokenConsumerGenerator.Generate(schema));

        // Phase 5: Trivia attachment and lexer
        spc.AddSource(TriviaAttachmentGenerator.Generate(schema));
        spc.AddSource(LexerGenerator.Generate(schema));

        // Phase 6: Red node wrappers
        spc.AddSource(RedNodeGenerator.Generate(schema));
        spc.AddSource(RedTokenGenerator.Generate(schema));
        spc.AddSource(SyntaxTriviaGenerator.Generate(schema));

        // Phase 7: Syntax tree and parser
        spc.AddSource(SyntaxTreeGenerator.Generate(schema));
        spc.AddSource(ParserGenerator.Generate(schema));
    }
}
