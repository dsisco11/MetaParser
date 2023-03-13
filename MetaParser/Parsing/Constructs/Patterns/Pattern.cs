using MetaParser.Core;
using MetaParser.Graphs;

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
    /// Indicates whether the fully resolved pattern only represents values which can all be inline evaluated
    /// </summary>
    public abstract bool IsInline { get; }
    /// <summary>
    /// Indicates whether the pattern will always match a predetermined count of items, or if the patterns length can vary
    /// </summary>
    public abstract bool IsConstantLength { get; }
    /// <summary>
    /// Indicates whether the pattern contains other patterns
    /// </summary>
    public abstract bool HasChildren { get; }
    #endregion

    #region Constructors
    public Pattern(MetaParserContext context) : base(new(NodeType.Pattern, context.Registry.GetNextPatternIndex(), context.WorkingSet.Consumers.Single().NodeID), context)
    {
        context.Registry.AddPattern(this);
    }
    #endregion

    public abstract Pattern Combine(Pattern other, MetaParserContext context);

    #region Enumerability
    public abstract IEnumerator<Pattern> GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Pattern>)this).GetEnumerator();
    #endregion
}
