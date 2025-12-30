using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Schema;

/// <summary>
/// Root schema definition for MetaParser v2.
/// Contains all configuration and token definitions for a parser.
/// </summary>
public sealed class SchemaDefinition
{
    /// <summary>
    /// JSON schema URL for IDE validation (optional).
    /// </summary>
    [JsonPropertyName("$schema")]
    public string? Schema { get; set; }

    /// <summary>
    /// Namespace for generated code (required).
    /// </summary>
    [JsonPropertyName("namespace")]
    public string Namespace { get; set; } = string.Empty;

    /// <summary>
    /// Parser class name (optional, defaults to "Parser").
    /// </summary>
    [JsonPropertyName("classname")]
    public string Classname { get; set; } = "Parser";

    /// <summary>
    /// Token definitions keyed by token name (required).
    /// Order matters: earlier tokens have higher priority.
    /// </summary>
    [JsonPropertyName("tokens")]
    public Dictionary<string, TokenDefinition> Tokens { get; set; } = new();

    /// <summary>
    /// Token names to treat as trivia (optional).
    /// Trivia tokens are attached to regular tokens as leading/trailing content.
    /// </summary>
    [JsonPropertyName("trivia")]
    public List<string> Trivia { get; set; } = new();

    /// <summary>
    /// Gets whether this schema definition is valid.
    /// </summary>
    [JsonIgnore]
    public bool IsValid => !string.IsNullOrWhiteSpace(Namespace) && Tokens.Count > 0;

    /// <summary>
    /// Gets the priority (0-based index) of a token by name.
    /// Earlier defined tokens have lower priority values (higher priority).
    /// </summary>
    public int GetTokenPriority(string tokenName)
    {
        int index = 0;
        foreach (var kvp in Tokens)
        {
            if (kvp.Key == tokenName)
                return index;
            index++;
        }
        return -1; // Not found
    }

    /// <summary>
    /// Checks if a token name is defined as trivia.
    /// </summary>
    public bool IsTrivia(string tokenName) => Trivia.Contains(tokenName);
}
