using MetaParser.Graphs;
using Xunit;

namespace UnitTests.Graphs;

/// <summary>
/// Unit tests for the directed graph implementation.
/// </summary>
public class DirectedGraphTests
{
    [Fact]
    public void EmptyGraph_HasNoNodes()
    {
        var graph = new DirectedGraph<string>();
        
        Assert.Equal(0, graph.NodeCount);
        Assert.Equal(0, graph.EdgeCount);
        Assert.Empty(graph.Nodes);
        Assert.Empty(graph.Edges);
    }
    
    [Fact]
    public void AddNode_AddsToNodeCollection()
    {
        var graph = new DirectedGraph<string>();
        
        graph.AddNode("A");
        graph.AddNode("B");
        
        Assert.Equal(2, graph.NodeCount);
        Assert.Contains("A", graph.Nodes);
        Assert.Contains("B", graph.Nodes);
    }
    
    [Fact]
    public void AddNode_Duplicate_DoesNotAddTwice()
    {
        var graph = new DirectedGraph<string>();
        
        graph.AddNode("A");
        graph.AddNode("A");
        
        Assert.Equal(1, graph.NodeCount);
    }
    
    [Fact]
    public void AddEdge_CreatesBothNodes()
    {
        var graph = new DirectedGraph<string>();
        
        graph.AddEdge("A", "B");
        
        Assert.Equal(2, graph.NodeCount);
        Assert.Equal(1, graph.EdgeCount);
        Assert.True(graph.ContainsNode("A"));
        Assert.True(graph.ContainsNode("B"));
    }
    
    [Fact]
    public void AddEdge_Duplicate_DoesNotAddTwice()
    {
        var graph = new DirectedGraph<string>();
        
        graph.AddEdge("A", "B");
        graph.AddEdge("A", "B");
        
        Assert.Equal(1, graph.EdgeCount);
    }
    
    [Fact]
    public void HasEdge_ReturnsTrue_WhenEdgeExists()
    {
        var graph = new DirectedGraph<string>();
        graph.AddEdge("A", "B");
        
        Assert.True(graph.HasEdge("A", "B"));
        Assert.False(graph.HasEdge("B", "A")); // Directed!
    }
    
    [Fact]
    public void GetDependencies_ReturnsIncomingEdges()
    {
        var graph = new DirectedGraph<string>();
        graph.AddEdge("A", "C"); // A → C
        graph.AddEdge("B", "C"); // B → C
        
        var dependencies = graph.GetDependencies("C");
        
        Assert.Equal(2, dependencies.Count);
        Assert.Contains("A", dependencies);
        Assert.Contains("B", dependencies);
    }
    
    [Fact]
    public void GetDependents_ReturnsOutgoingEdges()
    {
        var graph = new DirectedGraph<string>();
        graph.AddEdge("A", "B"); // A → B
        graph.AddEdge("A", "C"); // A → C
        
        var dependents = graph.GetDependents("A");
        
        Assert.Equal(2, dependents.Count);
        Assert.Contains("B", dependents);
        Assert.Contains("C", dependents);
    }
    
    [Fact]
    public void GetDependencies_NonexistentNode_ReturnsEmpty()
    {
        var graph = new DirectedGraph<string>();
        
        var dependencies = graph.GetDependencies("X");
        
        Assert.Empty(dependencies);
    }
}

/// <summary>
/// Unit tests for cycle detection.
/// </summary>
public class CycleDetectionTests
{
    [Fact]
    public void HasCycles_EmptyGraph_ReturnsFalse()
    {
        var graph = new DirectedGraph<string>();
        
        Assert.False(CycleDetection.HasCycles(graph));
    }
    
    [Fact]
    public void HasCycles_AcyclicGraph_ReturnsFalse()
    {
        var graph = new DirectedGraph<string>();
        graph.AddEdge("A", "B");
        graph.AddEdge("B", "C");
        graph.AddEdge("A", "C");
        
        Assert.False(CycleDetection.HasCycles(graph));
    }
    
    [Fact]
    public void HasCycles_SelfLoop_ReturnsTrue()
    {
        var graph = new DirectedGraph<string>();
        graph.AddEdge("A", "A");
        
        Assert.True(CycleDetection.HasCycles(graph));
    }
    
    [Fact]
    public void HasCycles_SimpleCycle_ReturnsTrue()
    {
        var graph = new DirectedGraph<string>();
        graph.AddEdge("A", "B");
        graph.AddEdge("B", "C");
        graph.AddEdge("C", "A");
        
        Assert.True(CycleDetection.HasCycles(graph));
    }
    
    [Fact]
    public void HasCycles_ComplexGraph_WithCycle_ReturnsTrue()
    {
        var graph = new DirectedGraph<string>();
        // Acyclic part
        graph.AddEdge("X", "Y");
        graph.AddEdge("Y", "Z");
        // Cyclic part
        graph.AddEdge("A", "B");
        graph.AddEdge("B", "C");
        graph.AddEdge("C", "A");
        
        Assert.True(CycleDetection.HasCycles(graph));
    }
    
    [Fact]
    public void FindFirstCycle_AcyclicGraph_ReturnsNull()
    {
        var graph = new DirectedGraph<string>();
        graph.AddEdge("A", "B");
        graph.AddEdge("B", "C");
        
        Assert.Null(CycleDetection.FindFirstCycle(graph));
    }
    
    [Fact]
    public void FindFirstCycle_SimpleCycle_ReturnsCycle()
    {
        var graph = new DirectedGraph<string>();
        graph.AddEdge("A", "B");
        graph.AddEdge("B", "C");
        graph.AddEdge("C", "A");
        
        var cycle = CycleDetection.FindFirstCycle(graph);
        
        Assert.NotNull(cycle);
        Assert.True(cycle.Count >= 2);
        // The cycle should contain at least some of A, B, C
        Assert.True(cycle.Contains("A") || cycle.Contains("B") || cycle.Contains("C"));
    }
    
    [Fact]
    public void GetCyclicNodes_AcyclicGraph_ReturnsEmpty()
    {
        var graph = new DirectedGraph<string>();
        graph.AddEdge("A", "B");
        graph.AddEdge("B", "C");
        
        var cyclicNodes = CycleDetection.GetCyclicNodes(graph);
        
        Assert.Empty(cyclicNodes);
    }
    
    [Fact]
    public void GetCyclicNodes_SimpleCycle_ReturnsAllCyclicNodes()
    {
        var graph = new DirectedGraph<string>();
        graph.AddEdge("A", "B");
        graph.AddEdge("B", "C");
        graph.AddEdge("C", "A");
        // Add non-cyclic node
        graph.AddNode("D");
        
        var cyclicNodes = CycleDetection.GetCyclicNodes(graph);
        
        Assert.Equal(3, cyclicNodes.Count);
        Assert.Contains("A", cyclicNodes);
        Assert.Contains("B", cyclicNodes);
        Assert.Contains("C", cyclicNodes);
    }
}

/// <summary>
/// Unit tests for topological sorting.
/// </summary>
public class TopologicalSortTests
{
    [Fact]
    public void KahnSort_EmptyGraph_ReturnsEmpty()
    {
        var graph = new DirectedGraph<string>();
        
        var result = TopologicalSort.KahnSort(graph);
        
        Assert.True(result.IsSuccess);
        Assert.Empty(result.SortedNodes);
    }
    
    [Fact]
    public void KahnSort_SingleNode_ReturnsNode()
    {
        var graph = new DirectedGraph<string>();
        graph.AddNode("A");
        
        var result = TopologicalSort.KahnSort(graph);
        
        Assert.True(result.IsSuccess);
        Assert.Single(result.SortedNodes);
        Assert.Equal("A", result.SortedNodes[0]);
    }
    
    [Fact]
    public void KahnSort_LinearChain_ReturnsDependencyOrder()
    {
        var graph = new DirectedGraph<string>();
        graph.AddEdge("A", "B"); // A → B
        graph.AddEdge("B", "C"); // B → C
        
        var result = TopologicalSort.KahnSort(graph);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(3, result.SortedNodes.Count);
        
        // A should come before B, B should come before C
        var indexA = result.SortedNodes.ToList().IndexOf("A");
        var indexB = result.SortedNodes.ToList().IndexOf("B");
        var indexC = result.SortedNodes.ToList().IndexOf("C");
        
        Assert.True(indexA < indexB);
        Assert.True(indexB < indexC);
    }
    
    [Fact]
    public void KahnSort_Diamond_ReturnsDependencyOrder()
    {
        var graph = new DirectedGraph<string>();
        //   A
        //  / \
        // B   C
        //  \ /
        //   D
        graph.AddEdge("A", "B");
        graph.AddEdge("A", "C");
        graph.AddEdge("B", "D");
        graph.AddEdge("C", "D");
        
        var result = TopologicalSort.KahnSort(graph);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(4, result.SortedNodes.Count);
        
        var sorted = result.SortedNodes.ToList();
        var indexA = sorted.IndexOf("A");
        var indexB = sorted.IndexOf("B");
        var indexC = sorted.IndexOf("C");
        var indexD = sorted.IndexOf("D");
        
        Assert.True(indexA < indexB);
        Assert.True(indexA < indexC);
        Assert.True(indexB < indexD);
        Assert.True(indexC < indexD);
    }
    
    [Fact]
    public void KahnSort_WithCycle_ReturnsFailure()
    {
        var graph = new DirectedGraph<string>();
        graph.AddEdge("A", "B");
        graph.AddEdge("B", "C");
        graph.AddEdge("C", "A");
        
        var result = TopologicalSort.KahnSort(graph);
        
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.CyclicNodes);
    }
    
    [Fact]
    public void DfsSort_LinearChain_ReturnsDependencyOrder()
    {
        var graph = new DirectedGraph<string>();
        graph.AddEdge("A", "B");
        graph.AddEdge("B", "C");
        
        var result = TopologicalSort.DfsSort(graph);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(3, result.SortedNodes.Count);
        
        var sorted = result.SortedNodes.ToList();
        var indexA = sorted.IndexOf("A");
        var indexB = sorted.IndexOf("B");
        var indexC = sorted.IndexOf("C");
        
        Assert.True(indexA < indexB);
        Assert.True(indexB < indexC);
    }
    
    [Fact]
    public void DfsSort_WithCycle_ReturnsFailure()
    {
        var graph = new DirectedGraph<string>();
        graph.AddEdge("A", "B");
        graph.AddEdge("B", "A");
        
        var result = TopologicalSort.DfsSort(graph);
        
        Assert.False(result.IsSuccess);
    }
}
