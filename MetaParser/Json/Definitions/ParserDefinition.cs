using MetaParser.Json.Definitions;
using MetaParser.Json.JsonTypeConverters;

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
    [JsonConverter(typeof(JsonOneOrManyConverter))]
    public IReadOnlyDictionary<string, PatternDefinition[]>? Patterns { get; set; }

    [JsonPropertyName("definitions")]
    [JsonConverter(typeof(JsonOneOrManyConverter))]
    public IReadOnlyDictionary<string, IConsumerDefinition[]>? Definitions { get; set; }
}
