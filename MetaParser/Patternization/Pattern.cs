namespace MetaParser.Patternization;
internal abstract record Pattern
{
    public static Pattern Empty = new PatternEmpty();
    public abstract int Length { get; }
    /// <summary>
    /// Indicates whether the fully resolved pattern represents only constant values
    /// </summary>
    public abstract bool IsRawValues { get; }
    /// <summary>
    /// Indicates whether the pattern will always match a predetermined count of items, or if the patterns length can vary
    /// </summary>
    public abstract bool IsConstantLength { get; }
}

internal record PatternEmpty : Pattern
{
    public override int Length => 0;
    public override bool IsRawValues => true;
    public override bool IsConstantLength => true;

}
