namespace MetaParser.Schemas.Structs
{
    internal record CompoundValueString : CompoundItemValue
    {
        public char value { get; set; }
        public CompoundValueString(string value)
        {
            this.value = value[0];
        }
    }
}
