using System.Text.Json.Serialization;

namespace MetaParser.Schemas.Structs
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(TokenDefConstant), typeDiscriminator: "constant")]
    [JsonDerivedType(typeof(TokenDefCompound), typeDiscriminator: "compound")]
    [JsonDerivedType(typeof(TokenDefComplex), typeDiscriminator: "complex")]
    internal abstract record TokenDef
    {
        [JsonIgnore]
        public int Index { get; set; }

        [JsonIgnore]
        public string? Name { get; set; }

        [JsonPropertyName("$type")]
        public string Type { get; set; }

        protected TokenDef(string type)
        {
            Type = type;
        }
    }
}
