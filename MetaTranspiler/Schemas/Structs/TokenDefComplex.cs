using System.Text.Json.Serialization;

namespace MetaTranspiler.Schemas.Structs
{
    public record TokenDefComplex : TokenDef
    {
        #region Properties
        [JsonPropertyName("start")]
        [JsonConverter(typeof(JsonOneOrManyConverter))]
        public string[] Start { get; set; }

        [JsonPropertyName("consume")]
        [JsonConverter(typeof(JsonOneOrManyConverter))]
        public string[] Consume { get; set; }

        [JsonPropertyName("end")]
        public ComplexTokenEnd? End { get; set; }

        #endregion

        [JsonConstructor]
        public TokenDefComplex(string type, string[] start, string[] consume, ComplexTokenEnd? end) : base(type)
        {
            Start = start;
            Consume = consume;
            End = end;
        }
    }

    public record ComplexTokenEnd
    {
        #region Properties
        [JsonPropertyName("terminator")]
        [JsonConverter(typeof(JsonOneOrManyConverter))]
        public string[] Terminator { get; set; }

        [JsonPropertyName("escape")]
        [JsonConverter(typeof(JsonOneOrManyConverter))]
        public string[]? Escape { get; set; }
        #endregion

        [JsonConstructor]
        public ComplexTokenEnd(string[] terminator, string[]? escape)
        {
            Terminator = terminator;
            Escape = escape;
        }
    }
}
