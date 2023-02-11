namespace MetaTranspiler.Schemas.Structs
{
    public record CompoundValueString : CompoundItemValue
    {
        public char value { get; set; }
        public CompoundValueString(string value)
        {
            this.value = value[0];
        }
    }
}
