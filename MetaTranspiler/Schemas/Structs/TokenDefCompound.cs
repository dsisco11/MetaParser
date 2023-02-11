using System.Text.Json.Serialization;

namespace MetaTranspiler.Schemas.Structs
{
    public record TokenDefCompound : TokenDef
    {
        [JsonPropertyName("consume")]
        [JsonConverter(typeof(JsonOneOrManyConverter))]
        public CompoundItemValue[] Values { get; set; }

        public TokenDefCompound(string type) : base(type)
        {
        }
    }
}
