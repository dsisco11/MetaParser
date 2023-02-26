namespace MetaParser.Patternization;

internal record PatternRange : Pattern
{
    public readonly string begin;
    public readonly string end;

    public PatternRange(string start, string end)
    {
        this.begin = start;
        this.end = end;
    }

    public override bool IsConstant => false;
    public override int Length => (string.IsNullOrEmpty(begin) && string.IsNullOrEmpty(end)) ? 0 : 1;
}
