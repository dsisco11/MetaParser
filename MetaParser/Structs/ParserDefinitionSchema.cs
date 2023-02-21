using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Schemas.Structs
{
    internal record ParserDefinitionSchema
    {
        [JsonPropertyName("namespace")]
        public string? Namespace { get; set; }

        [JsonPropertyName("tokens")]
        public IReadOnlyDictionary<string, TokenDef>? Tokens { get; set; }
    }
}
