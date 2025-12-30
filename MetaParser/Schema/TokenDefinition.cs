using System.Text.Json.Serialization;

namespace MetaParser.Schema;

/// <summary>
/// Defines a token in the MetaParser v2 schema.
/// Tokens specify patterns for matching input text.
/// </summary>
public sealed class TokenDefinition
{
    /// <summary>
    /// Pattern that must match to begin consuming (optional).
    /// If defined, it must match first and is consumed.
    /// </summary>
    [JsonPropertyName("start")]
    public PatternDefinition? Start { get; set; }

    /// <summary>
    /// Pattern(s) to consume repeatedly (required).
    /// This is the main matching pattern for the token.
    /// </summary>
    [JsonPropertyName("consume")]
    public PatternDefinition? Consume { get; set; }

    /// <summary>
    /// Pattern that ends consumption (optional).
    /// When matched, consumption stops but the stop pattern is NOT consumed.
    /// </summary>
    [JsonPropertyName("stop")]
    public PatternDefinition? Stop { get; set; }

    /// <summary>
    /// Pattern that escapes the next character/token (optional).
    /// When matched, the next item is consumed literally.
    /// </summary>
    [JsonPropertyName("escape")]
    public PatternDefinition? Escape { get; set; }

    /// <summary>
    /// Gets whether this token definition is valid (has a consume pattern).
    /// </summary>
    [JsonIgnore]
    public bool IsValid => Consume is not null;
}
