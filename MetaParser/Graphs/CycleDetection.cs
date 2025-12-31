using System;
using System.Collections.Generic;

namespace MetaParser.Graphs;

/// <summary>
/// Provides cycle detection algorithms for directed graphs.
/// Adapted from Graffs library's CycleDetection for MetaParser's simpler use case.
/// </summary>
internal static class CycleDetection
{
    /// <summary>
    /// Node visit states for DFS-based cycle detection.
    /// </summary>
    private enum VisitState
    {
        /// <summary>Unvisited node.</summary>
        White,
        /// <summary>Currently being visited (on the recursion stack).</summary>
        Gray,
        /// <summary>Completely visited.</summary>
        Black
    }
    
    /// <summary>
    /// Quickly checks if the graph contains any cycles using DFS.
    /// Optimized for early termination.
    /// </summary>
    /// <typeparam name="TNode">The type of nodes in the graph.</typeparam>
    /// <param name="graph">The graph to analyze.</param>
    /// <returns>True if the graph contains cycles, false otherwise.</returns>
    public static bool HasCycles<TNode>(DirectedGraph<TNode> graph)
        where TNode : notnull
    {
        if (graph == null) throw new ArgumentNullException(nameof(graph));
        if (graph.NodeCount == 0) return false;
        
        var states = new Dictionary<TNode, VisitState>();
        foreach (var node in graph.Nodes)
        {
            states[node] = VisitState.White;
        }
        
        foreach (var node in graph.Nodes)
        {
            if (states[node] == VisitState.White)
            {
                if (HasCyclesDfs(graph, node, states))
                    return true;
            }
        }
        
        return false;
    }
    
    /// <summary>
    /// Finds the first cycle in the graph, if any exists.
    /// </summary>
    /// <typeparam name="TNode">The type of nodes in the graph.</typeparam>
    /// <param name="graph">The graph to analyze.</param>
    /// <returns>A list of nodes forming a cycle, or null if no cycle exists.</returns>
    public static IReadOnlyList<TNode>? FindFirstCycle<TNode>(DirectedGraph<TNode> graph)
        where TNode : notnull
    {
        if (graph == null) throw new ArgumentNullException(nameof(graph));
        if (graph.NodeCount == 0) return null;
        
        var states = new Dictionary<TNode, VisitState>();
        var parent = new Dictionary<TNode, TNode>();
        
        foreach (var node in graph.Nodes)
        {
            states[node] = VisitState.White;
        }
        
        foreach (var node in graph.Nodes)
        {
            if (states[node] == VisitState.White)
            {
                var cycle = FindCycleDfs(graph, node, states, parent);
                if (cycle != null)
                    return cycle;
            }
        }
        
        return null;
    }
    
    /// <summary>
    /// Gets all nodes that are part of any cycle in the graph.
    /// </summary>
    /// <typeparam name="TNode">The type of nodes in the graph.</typeparam>
    /// <param name="graph">The graph to analyze.</param>
    /// <returns>A set of nodes that participate in cycles.</returns>
    public static IReadOnlyCollection<TNode> GetCyclicNodes<TNode>(DirectedGraph<TNode> graph)
        where TNode : notnull
    {
        if (graph == null) throw new ArgumentNullException(nameof(graph));
        if (graph.NodeCount == 0) return Array.Empty<TNode>();
        
        // Use Kahn's algorithm: nodes with remaining in-degree > 0 are in cycles
        var inDegree = new Dictionary<TNode, int>();
        foreach (var node in graph.Nodes)
        {
            inDegree[node] = graph.GetDependencies(node).Count;
        }
        
        var queue = new Queue<TNode>();
        foreach (var kvp in inDegree)
        {
            if (kvp.Value == 0)
                queue.Enqueue(kvp.Key);
        }
        
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            foreach (var dependent in graph.GetDependents(current))
            {
                inDegree[dependent]--;
                if (inDegree[dependent] == 0)
                    queue.Enqueue(dependent);
            }
        }
        
        // Nodes with remaining in-degree > 0 are part of cycles
        var cyclicNodes = new List<TNode>();
        foreach (var kvp in inDegree)
        {
            if (kvp.Value > 0)
                cyclicNodes.Add(kvp.Key);
        }
        
        return cyclicNodes;
    }
    
    private static bool HasCyclesDfs<TNode>(
        DirectedGraph<TNode> graph,
        TNode node,
        Dictionary<TNode, VisitState> states)
        where TNode : notnull
    {
        states[node] = VisitState.Gray;
        
        foreach (var dependent in graph.GetDependents(node))
        {
            if (states[dependent] == VisitState.Gray)
                return true; // Back edge found = cycle
            
            if (states[dependent] == VisitState.White)
            {
                if (HasCyclesDfs(graph, dependent, states))
                    return true;
            }
        }
        
        states[node] = VisitState.Black;
        return false;
    }
    
    private static List<TNode>? FindCycleDfs<TNode>(
        DirectedGraph<TNode> graph,
        TNode node,
        Dictionary<TNode, VisitState> states,
        Dictionary<TNode, TNode> parent)
        where TNode : notnull
    {
        states[node] = VisitState.Gray;
        
        foreach (var dependent in graph.GetDependents(node))
        {
            if (states[dependent] == VisitState.Gray)
            {
                // Found a cycle - reconstruct it
                var cycle = new List<TNode> { dependent };
                var current = node;
                while (!EqualityComparer<TNode>.Default.Equals(current, dependent))
                {
                    cycle.Add(current);
                    if (!parent.TryGetValue(current, out current!))
                        break;
                }
                cycle.Reverse();
                return cycle;
            }
            
            if (states[dependent] == VisitState.White)
            {
                parent[dependent] = node;
                var cycle = FindCycleDfs(graph, dependent, states, parent);
                if (cycle != null)
                    return cycle;
            }
        }
        
        states[node] = VisitState.Black;
        return null;
    }
}
