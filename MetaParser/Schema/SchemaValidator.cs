using System.Collections.Generic;

namespace MetaParser.Schema;

/// <summary>
/// Validates MetaParser v2 schema definitions.
/// </summary>
internal static class SchemaValidator
{
    /// <summary>
    /// Validates a schema definition and returns any errors found.
    /// </summary>
    public static ValidationResult Validate(SchemaDefinition? schema)
    {
        var result = new ValidationResult();

        if (schema is null)
        {
            result.AddError("Failed to parse schema JSON");
            return result;
        }

        // Validate required fields
        ValidateRequiredFields(schema, result);

        // Validate each token definition
        ValidateTokenDefinitions(schema, result);

        // Validate all token references
        ValidateTokenReferences(schema, result);

        // Validate trivia references
        ValidateTriviaReferences(schema, result);

        // Validate circular dependencies using dependency graph
        ValidateCircularDependencies(schema, result);

        return result;
    }

    private static void ValidateRequiredFields(SchemaDefinition schema, ValidationResult result)
    {
        if (string.IsNullOrWhiteSpace(schema.Namespace))
            result.AddError("Schema must specify a 'namespace'");

        if (schema.Tokens.Count == 0)
            result.AddError("Schema must define at least one token");
    }

    private static void ValidateTokenDefinitions(SchemaDefinition schema, ValidationResult result)
    {
        foreach (var kvp in schema.Tokens)
        {
            var name = kvp.Key;
            var token = kvp.Value;

            // Note: Consume pattern is optional - single-char tokens like "+" don't need one
            // The lexer will only match the start pattern in that case

            // Validate patterns within the token
            ValidatePattern(name, "start", token.Start, result);
            ValidatePattern(name, "consume", token.Consume, result);
            ValidatePattern(name, "stop", token.Stop, result);
            ValidatePattern(name, "escape", token.Escape, result);
        }
    }

    private static void ValidatePattern(string tokenName, string patternLocation, PatternDefinition? pattern, ValidationResult result)
    {
        if (pattern is null)
            return;

        switch (pattern)
        {
            case RangePattern range:
                ValidateRangePattern(tokenName, patternLocation, range, result);
                break;

            case OneOfPattern oneOf:
                for (int i = 0; i < oneOf.Patterns.Length; i++)
                {
                    ValidatePattern(tokenName, $"{patternLocation}[{i}]", oneOf.Patterns[i], result);
                }
                break;

            case LiteralPattern literal:
                if (string.IsNullOrEmpty(literal.Value))
                {
                    result.AddError($"Token '{tokenName}' has empty literal in '{patternLocation}'");
                }
                break;
        }
    }

    private static void ValidateRangePattern(string tokenName, string patternLocation, RangePattern range, ValidationResult result)
    {
        // RangePattern constructor already validates Start <= End,
        // but we add a diagnostic-friendly message here
        if (range.Start > range.End)
        {
            result.AddError($"Token '{tokenName}' has invalid range in '{patternLocation}': " +
                          $"start '{range.Start}' is greater than end '{range.End}'");
        }
    }

    private static void ValidateTokenReferences(SchemaDefinition schema, ValidationResult result)
    {
        foreach (var kvp in schema.Tokens)
        {
            var name = kvp.Key;
            var token = kvp.Value;

            ValidatePatternReferences(name, "start", token.Start, schema, result);
            ValidatePatternReferences(name, "consume", token.Consume, schema, result);
            ValidatePatternReferences(name, "stop", token.Stop, schema, result);
            ValidatePatternReferences(name, "escape", token.Escape, schema, result);
        }
    }

    private static void ValidatePatternReferences(
        string tokenName,
        string patternLocation,
        PatternDefinition? pattern,
        SchemaDefinition schema,
        ValidationResult result)
    {
        if (pattern is null)
            return;

        switch (pattern)
        {
            case TokenReferencePattern tokenRef:
                if (!schema.Tokens.ContainsKey(tokenRef.TokenName))
                {
                    result.AddError($"Token '{tokenName}' references undefined token '{tokenRef.TokenName}' in '{patternLocation}'");
                }
                else if (tokenRef.TokenName == tokenName)
                {
                    // Self-reference is a form of circular dependency
                    result.AddError($"Token '{tokenName}' has self-reference in '{patternLocation}'");
                }
                break;

            case OneOfPattern oneOf:
                for (int i = 0; i < oneOf.Patterns.Length; i++)
                {
                    ValidatePatternReferences(tokenName, $"{patternLocation}[{i}]", oneOf.Patterns[i], schema, result);
                }
                break;
        }
    }

    private static void ValidateTriviaReferences(SchemaDefinition schema, ValidationResult result)
    {
        var seenTrivia = new HashSet<string>();

        foreach (var triviaName in schema.Trivia)
        {
            // Check for duplicates in trivia array
            if (!seenTrivia.Add(triviaName))
            {
                result.AddWarning($"Trivia '{triviaName}' is listed multiple times");
                continue;
            }

            // Check that trivia references an existing token
            if (!schema.Tokens.ContainsKey(triviaName))
            {
                result.AddError($"Trivia references undefined token '{triviaName}'");
            }
        }
    }

    private static void ValidateCircularDependencies(SchemaDefinition schema, ValidationResult result)
    {
        var analyzer = new TokenDependencyAnalyzer(schema);
        analyzer.Analyze();

        // Check for circular dependencies
        var cycle = analyzer.FindFirstCycle();
        if (cycle != null && cycle.Count > 0)
        {
            var cycleStr = string.Join(" → ", cycle);
            result.AddError($"Circular dependency detected: {cycleStr} → {cycle[0]}");
        }

        // Warn about unreferenced tokens (potential dead code)
        // Skip trivia tokens as they're referenced implicitly
        var unreferenced = analyzer.GetUnreferencedTokens();
        foreach (var token in unreferenced)
        {
            // Don't warn about trivia - they're entry points
            if (!schema.IsTrivia(token))
            {
                // Only warn if the token references other tokens (meaning it's not a simple leaf)
                // A token that references nothing and is unreferenced is likely an entry point
                var deps = analyzer.Graph.GetDependencies(token);
                if (deps.Count > 0)
                {
                    result.AddWarning($"Token '{token}' is never referenced by other tokens");
                }
            }
        }
    }
}

/// <summary>
/// Result of schema validation containing errors and warnings.
/// </summary>
internal sealed class ValidationResult
{
    private readonly List<string> _errors = new();
    private readonly List<string> _warnings = new();

    /// <summary>
    /// Gets all validation errors.
    /// </summary>
    public IReadOnlyList<string> Errors => _errors;

    /// <summary>
    /// Gets all validation warnings.
    /// </summary>
    public IReadOnlyList<string> Warnings => _warnings;

    /// <summary>
    /// Gets whether the schema is valid (no errors).
    /// </summary>
    public bool IsValid => _errors.Count == 0;

    /// <summary>
    /// Gets whether there are any diagnostics (errors or warnings).
    /// </summary>
    public bool HasDiagnostics => _errors.Count > 0 || _warnings.Count > 0;

    public void AddError(string message) => _errors.Add(message);
    public void AddWarning(string message) => _warnings.Add(message);
}
