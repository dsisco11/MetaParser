using System.Text.Json.Serialization;

namespace MetaParser.Schemas.Structs
{
    internal record CompoundValueRange : CompoundItemValue
    {
        private string[] range;
        public char Start => range[0][0];
        public char End => range[1][0];

        [JsonConstructor()]
        public CompoundValueRange(string[] range)
        {
            this.range = range;
        }
    }
}
