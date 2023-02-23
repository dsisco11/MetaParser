using System;
using System.Text.Json.Serialization;

namespace MetaParser.Schemas.Structs
{
    internal record TokenDefComplex : TokenDef
    {
        #region Properties
        [JsonPropertyName("start")]
        [JsonConverter(typeof(JsonOneOrManyConverter))]
        public string[] Start { get; set; }

        [JsonPropertyName("consume")]
        [JsonConverter(typeof(JsonOneOrManyConverter))]
        public string[] Consume { get; set; }

        [JsonPropertyName("stop")]
        [JsonConverter(typeof(JsonOneOrManyConverter))]
        public string[] Stop { get; set; }

        [JsonPropertyName("escape")]
        [JsonConverter(typeof(JsonOneOrManyConverter))]
        public string[] Escape { get; set; }
        #endregion

        [JsonConstructor]
        public TokenDefComplex(string type, string[]? start, string[]? consume, string[]? stop, string[]? escape) : base(type)
        {
            Start = start ?? Array.Empty<string>();
            Consume = consume ?? Array.Empty<string>();
            Stop = stop ?? Array.Empty<string>();
            Escape = escape ?? Array.Empty<string>();
        }
    }
}
