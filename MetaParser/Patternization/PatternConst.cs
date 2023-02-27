namespace MetaParser.Patternization;

internal sealed record PatternConst : Pattern
{
    public readonly string value;

    public PatternConst(string value)
    {
        this.value = value;
    }

    public override bool IsRawValues => true;
    public override bool IsConstantLength => true;
    public override int Length => string.IsNullOrEmpty(value) ? 0 : 1;
}
