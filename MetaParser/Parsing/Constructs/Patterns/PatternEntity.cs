using MetaParser.Core;
using MetaParser.Graphs;
using MetaParser.Parsing.Constructs.Patterns;

using System;
using System.Collections;
using System.Collections.Generic;

namespace MetaParser.Parsing.Constructs;
internal abstract record PatternEntity : GraphEntity, IEnumerable<PatternEntity>, IComparable<PatternEntity>
{
    #region Fields
    public readonly EPatternKind Kind;
    #endregion

    #region Accessors

    /// <summary>
    /// Indicates the length of this pattern when rendered as a sequence
    /// </summary>
    public abstract int Length { get; }
    /// <summary>
    /// Indicates whether the fully resolved pattern only represents values which can always be expressed as inline statements, eg no function calls
    /// </summary>
    public abstract bool IsInlinable { get; }
    /// <summary>
    /// Indicates whether the pattern will always match a predetermined count of items, or if the number of matched items varies
    /// </summary>
    public abstract bool IsConstantLength { get; }
    /// <summary>
    /// Indicates whether the pattern contains nested patterns
    /// </summary>
    public abstract bool IsSequence { get; }
    /// <summary>
    /// Indicates the minimum number of logical/conditional checks the pattern requires
    /// </summary>
    public abstract int MinConditions { get; }
    /// <summary>
    /// Indicates the maximum number of logical/conditional checks the pattern requires
    /// </summary>
    public abstract int MaxConditions { get; }
    /// <summary>
    /// Indicates whether the pattern involves a logical operation sequence such as (x and y) or (x or y)
    /// </summary>
    public abstract bool IsConditional { get; }

    /// <summary>
    /// Indicates whether the pattern is deterministic, meaning it can only match a single possible token sequence
    /// </summary>
    public abstract bool IsDeterministic { get; }
    #endregion

    #region Constructors
    public PatternEntity(EPatternKind kind, EntityRegistry registry) : base(NodeType.Pattern, registry)
    {
        Kind = kind;
    }
    #endregion

    #region Enumerability
    public virtual IEnumerator<PatternEntity> GetEnumerator() { yield break; }
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<PatternEntity>)this).GetEnumerator();
    #endregion

    #region Comparison
    public int CompareTo(PatternEntity other)
    {
        return PatternSorter.Instance.Compare(this, other);
    }
    #endregion
}
