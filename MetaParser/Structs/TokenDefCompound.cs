using System;
using System.Text.Json.Serialization;

namespace MetaParser.Schemas.Structs
{
    internal record TokenDefCompound : TokenDef
    {
        [JsonPropertyName("consume")]
        [JsonConverter(typeof(JsonOneOrManyConverter))]
        public CompoundItemValue[] Values { get; set; } = Array.Empty<CompoundItemValue>();

        public TokenDefCompound(string type) : base(type)
        {
        }
    }
}
