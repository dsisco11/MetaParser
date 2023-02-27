namespace MetaParser.Patternization;

internal sealed record PatternRange : Pattern
{
    public readonly string begin;
    public readonly string end;

    public PatternRange(string start, string end)
    {
        this.begin = start;
        this.end = end;
    }

    public override bool IsRawValues => false;
    public override bool IsConstantLength => begin.Length == end.Length;
    public override int Length => (string.IsNullOrEmpty(begin) && string.IsNullOrEmpty(end)) ? 0 : 1;
}
