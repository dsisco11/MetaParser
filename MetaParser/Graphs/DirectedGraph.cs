using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Graphs;

/// <summary>
/// A simple directed graph implementation using adjacency lists.
/// Adapted from Graffs library's AdjacencyListGraph for MetaParser's use case.
/// 
/// This implementation is optimized for:
/// - Sparse graphs (typical of token dependencies)
/// - Fast dependency/dependent lookups
/// - Cycle detection and topological sorting
/// </summary>
/// <typeparam name="TNode">The type of nodes in the graph.</typeparam>
internal sealed class DirectedGraph<TNode>
    where TNode : notnull
{
    private readonly HashSet<TNode> _nodes;
    private readonly Dictionary<TNode, HashSet<TNode>> _outgoing;  // node → dependents
    private readonly Dictionary<TNode, HashSet<TNode>> _incoming;  // node → dependencies
    private readonly List<GraphEdge<TNode>> _edges;
    
    /// <summary>
    /// Gets all nodes in the graph.
    /// </summary>
    public IReadOnlyCollection<TNode> Nodes => _nodes;
    
    /// <summary>
    /// Gets all edges in the graph.
    /// </summary>
    public IReadOnlyList<GraphEdge<TNode>> Edges => _edges;
    
    /// <summary>
    /// Gets the number of nodes in the graph.
    /// </summary>
    public int NodeCount => _nodes.Count;
    
    /// <summary>
    /// Gets the number of edges in the graph.
    /// </summary>
    public int EdgeCount => _edges.Count;
    
    /// <summary>
    /// Creates an empty directed graph.
    /// </summary>
    public DirectedGraph()
    {
        _nodes = new HashSet<TNode>();
        _outgoing = new Dictionary<TNode, HashSet<TNode>>();
        _incoming = new Dictionary<TNode, HashSet<TNode>>();
        _edges = new List<GraphEdge<TNode>>();
    }
    
    /// <summary>
    /// Adds a node to the graph if it doesn't already exist.
    /// </summary>
    public void AddNode(TNode node)
    {
        if (node == null) throw new ArgumentNullException(nameof(node));
        
        if (_nodes.Add(node))
        {
            _outgoing[node] = new HashSet<TNode>();
            _incoming[node] = new HashSet<TNode>();
        }
    }
    
    /// <summary>
    /// Adds an edge from source to target.
    /// Both nodes are added if they don't exist.
    /// </summary>
    public void AddEdge(TNode source, TNode target)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (target == null) throw new ArgumentNullException(nameof(target));
        
        AddNode(source);
        AddNode(target);
        
        if (_outgoing[source].Add(target))
        {
            _incoming[target].Add(source);
            _edges.Add(new GraphEdge<TNode>(source, target));
        }
    }
    
    /// <summary>
    /// Gets nodes that the specified node depends on (incoming edges).
    /// </summary>
    public IReadOnlyCollection<TNode> GetDependencies(TNode node)
    {
        if (node == null) throw new ArgumentNullException(nameof(node));
        return _incoming.TryGetValue(node, out var deps) ? deps : Array.Empty<TNode>();
    }
    
    /// <summary>
    /// Gets nodes that depend on the specified node (outgoing edges).
    /// </summary>
    public IReadOnlyCollection<TNode> GetDependents(TNode node)
    {
        if (node == null) throw new ArgumentNullException(nameof(node));
        return _outgoing.TryGetValue(node, out var deps) ? deps : Array.Empty<TNode>();
    }
    
    /// <summary>
    /// Checks if there is a direct edge from source to target.
    /// </summary>
    public bool HasEdge(TNode source, TNode target)
    {
        if (source == null || target == null) return false;
        return _outgoing.TryGetValue(source, out var targets) && targets.Contains(target);
    }
    
    /// <summary>
    /// Checks if the specified node exists in the graph.
    /// </summary>
    public bool ContainsNode(TNode node) => node != null && _nodes.Contains(node);
}
