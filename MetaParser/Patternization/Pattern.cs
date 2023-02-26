namespace MetaParser.Patternization;
internal abstract record Pattern
{
    public static Pattern Empty = new PatternEmpty();
    public abstract int Length { get; }
    public abstract bool IsConstant { get; }
}
internal record PatternEmpty : Pattern
{
    public override int Length => 0;
    public override bool IsConstant => true;
}
