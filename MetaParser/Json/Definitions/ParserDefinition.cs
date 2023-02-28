using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal record ParserDefinition
{
    [JsonPropertyName("namespace")]
    public string? Namespace { get; set; }

    [JsonPropertyName("classname")]
    public string? ClassName { get; set; }

    [JsonPropertyName("$type")]
    public string? ParserType { get; set; }

    [JsonPropertyName("patterns")]
    public Dictionary<string, IEnumerable<PatternDeclaration>>? Patterns { get; set; }

    [JsonPropertyName("definitions")]
    public Dictionary<string, IEnumerable<IConsumerDeclaration>>? Definitions { get; set; }
}
