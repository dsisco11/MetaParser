using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MetaTranspiler.Schemas.Structs
{
    public record TokenDefConst : TokenDef
    {
        [JsonPropertyName("value")]
        public JsonElement value { get; set; }

        [JsonIgnore]
        public string[] Values => value.ValueKind switch
        {
            JsonValueKind.Array => value.Deserialize<string[]>() ?? Array.Empty<string>(),
            JsonValueKind.String => new[] { value.GetString()! },
            _ => Array.Empty<string>()
        };

        [JsonConstructor]
        public TokenDefConst(string type, JsonElement value) : base(type)
        {
            this.value = value;
        }
    }
}
