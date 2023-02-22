using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Schemas.Structs
{
    internal record ParserDefinitionSchema
    {
        [JsonPropertyName("namespace")]
        public string? Namespace { get; set; }

        [JsonPropertyName("classname")]
        public string? ClassName { get; set; }

        [JsonPropertyName("$type")]
        public string? ParserType { get; set; }

        [JsonPropertyName("definitions")]
        public IReadOnlyDictionary<string, TokenDef>? Definitions { get; set; }
    }
}
