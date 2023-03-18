using MetaParser.Core;
using MetaParser.Graphs;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Parsing.Constructs;
internal abstract record Pattern : GraphableEntity, IEnumerable<Pattern>
{
    #region Accessors
    /// <summary>
    /// Indicates the length of this pattern when rendered as a sequence
    /// </summary>
    public abstract int Length { get; }
    /// <summary>
    /// Indicates whether the fully resolved pattern represents only constant values
    /// </summary>
    public abstract bool IsRawValues { get; }
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
    public abstract bool HasChildren { get; }
    /// <summary>
    /// Indicates the minimum number of logical/conditional checks the pattern requires
    /// </summary>
    public abstract int MinLogicalLength { get; }
    /// <summary>
    /// Indicates the maximum number of logical/conditional checks the pattern requires
    /// </summary>
    public abstract int MaxLogicalLength { get; }
    /// <summary>
    /// Indicated whether the pattern involves a logical operation sequence such as (x and y) or (x or y)
    /// </summary>
    public abstract bool IsLogical { get; }
    #endregion

    #region Constructors
    public Pattern(MetaParserContext context) : base(new(NodeType.Pattern, context.Registry.GetNextPatternIndex()), context)
    {
        var consumerKey = context.WorkingSet.Consumers.Single().Key;
        context.Registry.AddPattern(this, consumerKey);
    }
    #endregion

    public abstract Pattern Combine(Pattern other, MetaParserContext context);

    #region Enumerability
    public abstract IEnumerator<Pattern> GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Pattern>)this).GetEnumerator();
    #endregion
}
