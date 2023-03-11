using MetaParser.Exceptions;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace MetaParser.Graphs;

internal class DirectedGraph<T> where T : notnull, IEquatable<T>
{
    #region Records
    [DebuggerDisplay(@"[In: {Incoming.Count}] [Out: {Outgoing.Count}]", Name = @"{Id}")]
    public record Node
    {
        public readonly HashSet<T> Incoming = new();
        public readonly HashSet<T> Outgoing = new();
    }

    [DebuggerDisplay("Order[{Order}] | MinDepth[{MinDepth}] | MaxDepth[{MaxDepth}] | IsRecursive ({IsRecursive})", Name = "{Id}")]
    public record ResolvedNode
    {
        public T Key;
        public int Order;
        public int MinDepth = int.MaxValue;
        public int MaxDepth;
        public bool IsRecursive;
        public ImmutableHashSet<ResolvedNode> Incoming;
        public ImmutableHashSet<ResolvedNode> Outgoing;

        public ResolvedNode(T key)
        {
            Key = key;
        }
    }
    #endregion

    #region Fields
    private readonly ImmutableDictionary<T, Node> nodes = ImmutableDictionary<T, Node>.Empty;
    #endregion

    #region Properties
    #endregion

    #region Constructors
    public DirectedGraph()
    {
    }

    public DirectedGraph(IEnumerable<T> items)
    {
        nodes = items.ToImmutableDictionary(static (x) => x, static (x) => new Node());
    }
    #endregion

    #region Accessors
    public int Count => nodes.Count;
    #endregion

    #region Links
    public bool TryLink(T leftKey, T rightKey)
    {
        if(!nodes.TryGetValue(leftKey, out var leftNode))
        {
            throw new ArgumentException($@"Unable to locate node with id: {leftKey}", nameof(leftKey));
        }

        if(!nodes.TryGetValue(rightKey, out var rightNode))
        {
            throw new ArgumentException($@"Unable to locate node with id: {rightKey}", nameof(rightKey));
        }

        rightNode.Incoming.Add(leftKey);
        return leftNode.Outgoing.Add(rightKey);
    }
    #endregion

    #region Sorting

    /// <summary>
    /// Returns ordered list of node-ids
    /// </summary>
    /// <param name="graph"></param>
    /// <returns></returns>
    public Dictionary<T, ResolvedNode> Resolve()
    {
        var count = Count;
        var in_degrees = nodes.Keys.ToDictionary(static (x) => x, (x) => nodes[x].Outgoing.Count);
        var resolved = nodes.Keys.ToDictionary(static (x) => x, (x) => new ResolvedNode(x));
        var queue = new Queue<T>(in_degrees.Where(static (x) => x.Value == 0).Select(static (x) => x.Key));

        foreach (var item in resolved)
        {
            var node = nodes[item.Key];
            item.Value.Incoming = node.Incoming.Select((k) => resolved[k]).ToImmutableHashSet();
            item.Value.Outgoing = node.Outgoing.Select((k) => resolved[k]).ToImmutableHashSet();
            item.Value.MinDepth = node.Outgoing.Count == 0 ? 0 : int.MaxValue;
        }

        int order = 0;
        while (queue.Count > 0)
        {
            var key = queue.Dequeue();
            resolved[key].Order = order++;
            var ancestorNode = resolved[key];

            foreach (var id in nodes[key].Incoming)
            {
                in_degrees[id] -= 1;
                var node = resolved[id];
                node.MinDepth = Math.Min(node.MinDepth, ancestorNode.MinDepth + 1);
                node.MaxDepth = Math.Max(node.MaxDepth, ancestorNode.MaxDepth + 1);

                if (in_degrees[id] == 0)
                {
                    queue.Enqueue(id);
                }
            }
        }

        if (order != count)
        {// Cyclic dependencies exist in this graph
            foreach (var entry in in_degrees)
            {
                if (entry.Value > 0)
                {
                    var node = resolved[entry.Key];
                    node.Order = order + 1;
                    node.IsRecursive = true;
                }
            }
        }

        return resolved;
    }
    #endregion
}