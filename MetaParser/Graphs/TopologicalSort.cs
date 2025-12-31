using System;
using System.Collections.Generic;

namespace MetaParser.Graphs;

/// <summary>
/// Result of a topological sort operation.
/// </summary>
/// <typeparam name="TNode">The type of nodes in the graph.</typeparam>
internal sealed class TopologicalSortResult<TNode>
    where TNode : notnull
{
    /// <summary>
    /// Gets whether the sort was successful (no cycles detected).
    /// </summary>
    public bool IsSuccess { get; }
    
    /// <summary>
    /// Gets the sorted nodes in topological order.
    /// For a dependency graph, dependencies come before dependents.
    /// </summary>
    public IReadOnlyList<TNode> SortedNodes { get; }
    
    /// <summary>
    /// Gets the nodes that are part of cycles (only populated if sort failed).
    /// </summary>
    public IReadOnlyCollection<TNode> CyclicNodes { get; }
    
    private TopologicalSortResult(
        bool isSuccess,
        IReadOnlyList<TNode> sortedNodes,
        IReadOnlyCollection<TNode> cyclicNodes)
    {
        IsSuccess = isSuccess;
        SortedNodes = sortedNodes;
        CyclicNodes = cyclicNodes;
    }
    
    internal static TopologicalSortResult<TNode> Success(IReadOnlyList<TNode> sortedNodes) =>
        new TopologicalSortResult<TNode>(true, sortedNodes, Array.Empty<TNode>());
    
    internal static TopologicalSortResult<TNode> WithCycles(
        IReadOnlyList<TNode> partialSort,
        IReadOnlyCollection<TNode> cyclicNodes) =>
        new TopologicalSortResult<TNode>(false, partialSort, cyclicNodes);
}

/// <summary>
/// Provides topological sorting algorithms for directed graphs.
/// Adapted from Graffs library's TopologicalSort for MetaParser's use case.
/// </summary>
internal static class TopologicalSort
{
    /// <summary>
    /// Performs topological sorting using Kahn's algorithm.
    /// Works by repeatedly removing nodes with no incoming edges.
    /// </summary>
    /// <typeparam name="TNode">The type of nodes in the graph.</typeparam>
    /// <param name="graph">The directed graph to sort.</param>
    /// <returns>A result containing sorted nodes or cycle information.</returns>
    public static TopologicalSortResult<TNode> KahnSort<TNode>(DirectedGraph<TNode> graph)
        where TNode : notnull
    {
        if (graph == null) throw new ArgumentNullException(nameof(graph));
        
        if (graph.NodeCount == 0)
            return TopologicalSortResult<TNode>.Success(Array.Empty<TNode>());
        
        // Calculate in-degrees for all nodes
        var inDegree = new Dictionary<TNode, int>();
        foreach (var node in graph.Nodes)
        {
            inDegree[node] = graph.GetDependencies(node).Count;
        }
        
        // Initialize queue with nodes that have no dependencies
        var queue = new Queue<TNode>();
        foreach (var kvp in inDegree)
        {
            if (kvp.Value == 0)
                queue.Enqueue(kvp.Key);
        }
        
        var result = new List<TNode>();
        
        // Process nodes in topological order
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            result.Add(current);
            
            // Update in-degrees of dependent nodes
            foreach (var dependent in graph.GetDependents(current))
            {
                inDegree[dependent]--;
                if (inDegree[dependent] == 0)
                    queue.Enqueue(dependent);
            }
        }
        
        // Check for cycles
        if (result.Count < graph.NodeCount)
        {
            // Nodes with non-zero in-degree are part of cycles
            var cyclicNodes = new List<TNode>();
            foreach (var kvp in inDegree)
            {
                if (kvp.Value > 0)
                    cyclicNodes.Add(kvp.Key);
            }
            
            return TopologicalSortResult<TNode>.WithCycles(result, cyclicNodes);
        }
        
        return TopologicalSortResult<TNode>.Success(result);
    }
    
    /// <summary>
    /// Performs topological sorting using depth-first search.
    /// Returns nodes in reverse post-order.
    /// </summary>
    /// <typeparam name="TNode">The type of nodes in the graph.</typeparam>
    /// <param name="graph">The directed graph to sort.</param>
    /// <returns>A result containing sorted nodes or cycle information.</returns>
    public static TopologicalSortResult<TNode> DfsSort<TNode>(DirectedGraph<TNode> graph)
        where TNode : notnull
    {
        if (graph == null) throw new ArgumentNullException(nameof(graph));
        
        if (graph.NodeCount == 0)
            return TopologicalSortResult<TNode>.Success(Array.Empty<TNode>());
        
        var visited = new HashSet<TNode>();
        var onStack = new HashSet<TNode>();
        var result = new List<TNode>();
        var hasCycle = false;
        
        void Dfs(TNode node)
        {
            if (hasCycle) return;
            if (onStack.Contains(node))
            {
                hasCycle = true;
                return;
            }
            if (visited.Contains(node)) return;
            
            visited.Add(node);
            onStack.Add(node);
            
            foreach (var dependent in graph.GetDependents(node))
            {
                Dfs(dependent);
            }
            
            onStack.Remove(node);
            result.Add(node);
        }
        
        foreach (var node in graph.Nodes)
        {
            if (!visited.Contains(node))
                Dfs(node);
        }
        
        if (hasCycle)
        {
            var cyclicNodes = CycleDetection.GetCyclicNodes(graph);
            return TopologicalSortResult<TNode>.WithCycles(result, cyclicNodes);
        }
        
        // DFS gives us reverse topological order, so reverse the result
        result.Reverse();
        return TopologicalSortResult<TNode>.Success(result);
    }
}
