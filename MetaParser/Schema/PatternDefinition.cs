using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MetaParser.Schema;

/// <summary>
/// Represents a pattern in the MetaParser v2 schema.
/// Patterns can be literals, character ranges, or token references.
/// </summary>
[JsonConverter(typeof(PatternDefinitionConverter))]
public abstract class PatternDefinition
{
    /// <summary>
    /// The kind of pattern this represents.
    /// </summary>
    public abstract PatternKind Kind { get; }
}

/// <summary>
/// Enumeration of pattern types supported by MetaParser v2.
/// </summary>
public enum PatternKind
{
    /// <summary>Matches an exact string literal (e.g., "if", "+").</summary>
    Literal,

    /// <summary>Matches any character in an inclusive range (e.g., ['0', '9']).</summary>
    Range,

    /// <summary>References another token by name using $token syntax.</summary>
    TokenReference,

    /// <summary>Matches any one of multiple patterns (array of patterns).</summary>
    OneOf
}

/// <summary>
/// Matches an exact string literal.
/// </summary>
/// <example>
/// JSON: "if" or "+"
/// </example>
public sealed class LiteralPattern : PatternDefinition
{
    public override PatternKind Kind => PatternKind.Literal;

    /// <summary>
    /// The literal string to match.
    /// </summary>
    public string Value { get; }

    public LiteralPattern(string value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }
}

/// <summary>
/// Matches any character within an inclusive range.
/// </summary>
/// <example>
/// JSON: { "range": ["0", "9"] }
/// </example>
public sealed class RangePattern : PatternDefinition
{
    public override PatternKind Kind => PatternKind.Range;

    /// <summary>
    /// The start character of the range (inclusive).
    /// </summary>
    public char Start { get; }

    /// <summary>
    /// The end character of the range (inclusive).
    /// </summary>
    public char End { get; }

    public RangePattern(char start, char end)
    {
        if (start > end)
            throw new ArgumentException($"Invalid range: start '{start}' is greater than end '{end}'");

        Start = start;
        End = end;
    }

    /// <summary>
    /// Creates a RangePattern from string values (single characters).
    /// </summary>
    public static RangePattern FromStrings(string start, string end)
    {
        if (string.IsNullOrEmpty(start) || start.Length != 1)
            throw new ArgumentException("Range start must be a single character", nameof(start));
        if (string.IsNullOrEmpty(end) || end.Length != 1)
            throw new ArgumentException("Range end must be a single character", nameof(end));

        return new RangePattern(start[0], end[0]);
    }
}

/// <summary>
/// References another token by name.
/// This creates a dependency, placing this token at a higher consumer level.
/// </summary>
/// <example>
/// JSON: { "$token": "digit" }
/// </example>
public sealed class TokenReferencePattern : PatternDefinition
{
    public override PatternKind Kind => PatternKind.TokenReference;

    /// <summary>
    /// The name of the referenced token.
    /// </summary>
    public string TokenName { get; }

    public TokenReferencePattern(string tokenName)
    {
        TokenName = tokenName ?? throw new ArgumentNullException(nameof(tokenName));
    }
}

/// <summary>
/// Matches any one of multiple patterns.
/// </summary>
/// <example>
/// JSON: [" ", "\t", "\n"] or [{ "range": ["a", "z"] }, "_"]
/// </example>
public sealed class OneOfPattern : PatternDefinition
{
    public override PatternKind Kind => PatternKind.OneOf;

    /// <summary>
    /// The patterns to match (any one must match).
    /// </summary>
    public PatternDefinition[] Patterns { get; }

    public OneOfPattern(PatternDefinition[] patterns)
    {
        Patterns = patterns ?? throw new ArgumentNullException(nameof(patterns));
        if (patterns.Length == 0)
            throw new ArgumentException("OneOf pattern must have at least one pattern", nameof(patterns));
    }
}

/// <summary>
/// JSON converter for PatternDefinition that handles the polymorphic deserialization.
/// </summary>
internal sealed class PatternDefinitionConverter : JsonConverter<PatternDefinition>
{
    public override PatternDefinition? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
                // Simple literal: "if"
                return new LiteralPattern(reader.GetString()!);

            case JsonTokenType.StartArray:
                // Array of patterns: [" ", "\t"] or [{ "range": [...] }, "_"]
                return ReadOneOfPattern(ref reader, options);

            case JsonTokenType.StartObject:
                // Object pattern: { "range": [...] } or { "$token": "name" }
                return ReadObjectPattern(ref reader, options);

            default:
                throw new JsonException($"Unexpected token type for pattern: {reader.TokenType}");
        }
    }

    private static PatternDefinition ReadOneOfPattern(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var patterns = new System.Collections.Generic.List<PatternDefinition>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
                break;

            var converter = new PatternDefinitionConverter();
            var pattern = converter.Read(ref reader, typeof(PatternDefinition), options);
            if (pattern != null)
                patterns.Add(pattern);
        }

        // Optimization: if only one pattern, return it directly
        if (patterns.Count == 1)
            return patterns[0];

        return new OneOfPattern(patterns.ToArray());
    }

    private static PatternDefinition ReadObjectPattern(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        PatternDefinition? result = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read(); // Move to value

                switch (propertyName)
                {
                    case "range":
                        result = ReadRangePattern(ref reader);
                        break;

                    case "$token":
                        var tokenName = reader.GetString();
                        if (string.IsNullOrEmpty(tokenName))
                            throw new JsonException("$token value cannot be empty");
                        result = new TokenReferencePattern(tokenName!);
                        break;

                    default:
                        throw new JsonException($"Unknown pattern property: {propertyName}");
                }
            }
        }

        if (result is null)
            throw new JsonException("Empty object pattern");
        
        return result;
    }

    private static RangePattern ReadRangePattern(ref Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException("Range pattern must be an array");

        string? start = null;
        string? end = null;

        reader.Read();
        if (reader.TokenType == JsonTokenType.String)
            start = reader.GetString();

        reader.Read();
        if (reader.TokenType == JsonTokenType.String)
            end = reader.GetString();

        reader.Read(); // End array

        if (string.IsNullOrEmpty(start) || string.IsNullOrEmpty(end))
            throw new JsonException("Range pattern must have exactly two string elements");

        return RangePattern.FromStrings(start!, end!);
    }

    public override void Write(Utf8JsonWriter writer, PatternDefinition value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case LiteralPattern literal:
                writer.WriteStringValue(literal.Value);
                break;

            case RangePattern range:
                writer.WriteStartObject();
                writer.WriteStartArray("range");
                writer.WriteStringValue(range.Start.ToString());
                writer.WriteStringValue(range.End.ToString());
                writer.WriteEndArray();
                writer.WriteEndObject();
                break;

            case TokenReferencePattern tokenRef:
                writer.WriteStartObject();
                writer.WriteString("$token", tokenRef.TokenName);
                writer.WriteEndObject();
                break;

            case OneOfPattern oneOf:
                writer.WriteStartArray();
                foreach (var pattern in oneOf.Patterns)
                    Write(writer, pattern, options);
                writer.WriteEndArray();
                break;

            default:
                throw new JsonException($"Unknown pattern type: {value.GetType().Name}");
        }
    }
}
