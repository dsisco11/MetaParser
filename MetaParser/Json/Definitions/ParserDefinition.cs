using System.Collections.Generic;
using System.Linq;
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

    [JsonPropertyName("stages")]
    public IEnumerable<IParsingStageDefinition> Stages { get; set; } = Enumerable.Empty<IParsingStageDefinition>();
}
