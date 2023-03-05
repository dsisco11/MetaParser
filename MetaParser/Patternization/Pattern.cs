using System.Collections.Generic;

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
    public abstract bool ContainsItems { get; }

    public abstract IEnumerable<Pattern> GetSubPatterns();
    public abstract Pattern Combine(Pattern other);
}
