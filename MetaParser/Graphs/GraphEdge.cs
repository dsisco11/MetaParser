using System;
using System.Collections.Generic;

namespace MetaParser.Graphs;

/// <summary>
/// Represents a directed edge in a graph.
/// Adapted from Graffs library for MetaParser's simpler use case.
/// </summary>
/// <typeparam name="TNode">The type of nodes in the graph.</typeparam>
internal readonly struct GraphEdge<TNode> : IEquatable<GraphEdge<TNode>>
    where TNode : notnull
{
    /// <summary>
    /// Gets the source node of this edge.
    /// </summary>
    public TNode Source { get; }
    
    /// <summary>
    /// Gets the target node of this edge.
    /// </summary>
    public TNode Target { get; }
    
    /// <summary>
    /// Creates a new graph edge.
    /// </summary>
    public GraphEdge(TNode source, TNode target)
    {
        Source = source ?? throw new ArgumentNullException(nameof(source));
        Target = target ?? throw new ArgumentNullException(nameof(target));
    }
    
    public bool Equals(GraphEdge<TNode> other) =>
        EqualityComparer<TNode>.Default.Equals(Source, other.Source) &&
        EqualityComparer<TNode>.Default.Equals(Target, other.Target);
    
    public override bool Equals(object? obj) => obj is GraphEdge<TNode> edge && Equals(edge);
    
    public override int GetHashCode()
    {
        unchecked
        {
            return (EqualityComparer<TNode>.Default.GetHashCode(Source) * 397) ^
                   EqualityComparer<TNode>.Default.GetHashCode(Target);
        }
    }
    
    public static bool operator ==(GraphEdge<TNode> left, GraphEdge<TNode> right) => left.Equals(right);
    public static bool operator !=(GraphEdge<TNode> left, GraphEdge<TNode> right) => !left.Equals(right);
    
    public override string ToString() => $"{Source} → {Target}";
}
