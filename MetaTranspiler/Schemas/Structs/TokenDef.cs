using System.Text.Json.Serialization;

namespace MetaTranspiler.Schemas.Structs
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(TokenDefConst), typeDiscriminator: "constant")]
    [JsonDerivedType(typeof(TokenDefCompound), typeDiscriminator: "compound")]
    [JsonDerivedType(typeof(TokenDefComplex), typeDiscriminator: "complex")]
    public abstract record TokenDef
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
