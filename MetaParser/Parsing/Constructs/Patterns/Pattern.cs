using MetaParser.Core;
using MetaParser.Graphs;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Parsing.Constructs;
using static DirectedGraph<GraphNodeKey>;

internal abstract record Pattern : IEnumerable<Pattern>
{
    #region Statics
    public static Pattern Empty = new PatternEmpty();
    #endregion

    #region Dependency Graph
    public readonly GraphNodeKey NodeID;
    public ResolvedNode? DependencyInfo { get; set; }
    #endregion

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
    /// Indicates whether the pattern will always match a predetermined count of items, or if the patterns length can vary
    /// </summary>
    public abstract bool IsConstantLength { get; }
    /// <summary>
    /// Indicates whether the pattern contains other patterns
    /// </summary>
    public abstract bool HasChildren { get; }
    #endregion

    #region Constructors
    protected Pattern()
    {
    }

    public Pattern(MetaParserContext context)
    {
        NodeID = new(GraphNodeType.Pattern, context.Registry.GetNextPatternIndex(), context.WorkingSet.Consumers.Single().NodeID);
        context.Registry.AddPattern(this);
    }
    #endregion

    public abstract Pattern Combine(Pattern other, MetaParserContext context);

    #region Enumerability
    public abstract IEnumerator<Pattern> GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Pattern>)this).GetEnumerator();
    #endregion
}
