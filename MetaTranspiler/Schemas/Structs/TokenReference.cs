using System.Text.Json.Serialization;

namespace MetaTranspiler.Schemas.Structs
{
    public record TokenReference
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
    }
}
