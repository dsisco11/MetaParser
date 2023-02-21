using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MetaParser.Schemas.Structs
{
    internal record TokenDefConstant : TokenDef
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
        public TokenDefConstant(string type, JsonElement value) : base(type)
        {
            this.value = value;
        }
    }
}
