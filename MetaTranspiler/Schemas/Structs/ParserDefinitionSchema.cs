using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaTranspiler.Schemas.Structs
{
    public record ParserDefinitionSchema
    {
        [JsonPropertyName("namespace")]
        public string? Namespace { get; set; }

        [JsonPropertyName("tokens")]
        public IReadOnlyDictionary<string, TokenDef>? Tokens { get; set; }
    }
}
