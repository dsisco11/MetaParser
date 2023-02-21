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

        [JsonPropertyName("end")]
        [JsonConverter(typeof(JsonOneOrManyConverter))]
        public string[] End { get; set; }

        [JsonPropertyName("escape")]
        [JsonConverter(typeof(JsonOneOrManyConverter))]
        public string[] Escape { get; set; }

        #endregion

        [JsonConstructor]
        public TokenDefComplex(string type, string[] start, string[]? consume, string[]? end, string[]? escape) : base(type)
        {
            Start = start;
            Consume = consume ?? Array.Empty<string>();
            End = end ?? Array.Empty<string>();
            Escape = escape ?? Array.Empty<string>();
        }
    }
}
