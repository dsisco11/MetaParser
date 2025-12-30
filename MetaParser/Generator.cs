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
        options.AddContext<SchemaJsonContext>();
        
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

        // Step 4: Validate schemas and report diagnostics
        var validatedSchemas = schemas.Select(static (data, cancellationToken) =>
        {
            var errors = ValidateSchema(data.Schema);
            return (data.File, data.Schema, Errors: errors);
        });

        // Step 5: Generate code for valid schemas
        context.RegisterSourceOutput(validatedSchemas, static (spc, data) =>
        {
            // Report any validation errors
            foreach (var error in data.Errors)
            {
                spc.ReportDiagnostic(Diagnostic.Create(
                    DIAGNOSTIC_DEFS.SchemaValidationError,
                    Location.None,
                    error));
            }

            // Skip code generation if there are errors or schema is null
            if (data.Errors.Length > 0 || data.Schema is null)
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

    private static string[] ValidateSchema(SchemaDefinition? schema)
    {
        if (schema is null)
            return new[] { "Failed to parse schema JSON" };

        var errors = new System.Collections.Generic.List<string>();

        // Validate required fields
        if (string.IsNullOrWhiteSpace(schema.Namespace))
            errors.Add("Schema must specify a 'namespace'");

        if (schema.Tokens.Count == 0)
            errors.Add("Schema must define at least one token");

        // Validate each token
        foreach (var kvp in schema.Tokens)
        {
            var name = kvp.Key;
            var token = kvp.Value;
            if (token.Consume is null)
                errors.Add($"Token '{name}' must have a 'consume' pattern");
        }

        // Validate token references
        foreach (var kvp in schema.Tokens)
        {
            var name = kvp.Key;
            var token = kvp.Value;
            ValidateTokenReferences(name, token.Start, schema, errors);
            ValidateTokenReferences(name, token.Consume, schema, errors);
            ValidateTokenReferences(name, token.Stop, schema, errors);
            ValidateTokenReferences(name, token.Escape, schema, errors);
        }

        // Validate trivia references
        foreach (var triviaName in schema.Trivia)
        {
            if (!schema.Tokens.ContainsKey(triviaName))
                errors.Add($"Trivia references undefined token '{triviaName}'");
        }

        // TODO: Add circular dependency detection (requires Graffs)

        return errors.ToArray();
    }

    private static void ValidateTokenReferences(
        string tokenName,
        PatternDefinition? pattern,
        SchemaDefinition schema,
        System.Collections.Generic.List<string> errors)
    {
        if (pattern is null)
            return;

        switch (pattern)
        {
            case TokenReferencePattern tokenRef:
                if (!schema.Tokens.ContainsKey(tokenRef.TokenName))
                    errors.Add($"Token '{tokenName}' references undefined token '{tokenRef.TokenName}'");
                break;

            case OneOfPattern oneOf:
                foreach (var child in oneOf.Patterns)
                    ValidateTokenReferences(tokenName, child, schema, errors);
                break;
        }
    }

    private static void GenerateCode(SourceProductionContext spc, FileData file, SchemaDefinition schema)
    {
        // Placeholder: Generate a simple marker file to verify the pipeline works
        var baseFileName = Common.Get_FileName(file.Path);
        
        var code = $@"// <auto-generated/>
// MetaParser v2 - Generated from {file.FileName}
// This is a placeholder. Full code generation coming in Phase 6.

namespace {schema.Namespace};

/// <summary>
/// Parser generated by MetaParser v2.
/// Tokens defined: {schema.Tokens.Count}
/// Trivia tokens: {schema.Trivia.Count}
/// </summary>
public static partial class {schema.Classname}
{{
    public const int TokenCount = {schema.Tokens.Count};
}}
";

        spc.AddSource($"MetaParser.{baseFileName}.g.cs", code);
    }
}
