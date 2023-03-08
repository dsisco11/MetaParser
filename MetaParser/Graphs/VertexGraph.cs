using MetaParser.Exceptions;

using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace MetaParser.Graphs;

internal class VertexGraph<T> where T : notnull, IEquatable<T>
{
    #region Records
    [DebuggerDisplay(@"[In: {Incoming.Count}] [Out: {Outgoing.Count}]", Name = @"{Id}")]
    private record VertexNode
    {
        public readonly HashSet<T> Incoming = new();
        public readonly HashSet<T> Outgoing = new();
    }
    #endregion

    #region Fields
    private readonly ImmutableDictionary<T, VertexNode> nodes = ImmutableDictionary<T, VertexNode>.Empty;
    #endregion

    #region Properties
    #endregion

    #region Constructors
    public VertexGraph()
    {
    }

    public VertexGraph(IEnumerable<T> items)
    {
        nodes = items.ToImmutableDictionary(static (x) => x, static (x) => new VertexNode());
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
            throw new UnknownTokenException($@"Unable to locate node with id: {leftKey}");
        }

        if(!nodes.TryGetValue(rightKey, out var rightNode))
        {
            throw new UnknownTokenException($@"Unable to locate node with id: {rightKey}");
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
    public Dictionary<T, ResolvedVertexNode> Resolve()
    {
        var count = Count;
        var in_degrees = nodes.Keys.ToDictionary(static (x) => x, (x) => nodes[x].Outgoing.Count);
        var resolved = nodes.Keys.ToDictionary(static (x) => x, (x) => new ResolvedVertexNode() { MaxDepth = 0, MinDepth = nodes[x].Outgoing.Count == 0 ? 0 : int.MaxValue });
        var queue = new Queue<T>(in_degrees.Where(static (x) => x.Value == 0).Select(static (x) => x.Key));

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

[DebuggerDisplay("Order[{Order}] | MinDepth[{MinDepth}] | MaxDepth[{MaxDepth}] | IsRecursive ({IsRecursive})", Name = "{Id}")]
internal record ResolvedVertexNode
{
    public int Order;
    public int MinDepth = int.MaxValue;
    public int MaxDepth;
    public bool IsRecursive;
}
