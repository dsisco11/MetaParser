namespace MetaParser.Patternization;

internal record PatternConst : Pattern
{
    public readonly string value;

    public PatternConst(string value)
    {
        this.value = value;
    }

    public override bool IsConstant => true;
    public override int Length => string.IsNullOrEmpty(value) ? 0 : 1;
}
