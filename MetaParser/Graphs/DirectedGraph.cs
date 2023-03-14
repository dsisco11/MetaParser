using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MetaParser.Graphs;

internal partial class DirectedGraph
{
    #region Fields
    private readonly Dictionary<EntityKey, Node> nodes = new();
    #endregion

    #region Accessors
    public IReadOnlyDictionary<EntityKey, Node> Nodes => nodes;
    #endregion

    #region Constructors
    public DirectedGraph()
    {
    }

    public DirectedGraph(IEnumerable<EntityKey> items)
    {
        nodes = items.ToDictionary(static (x) => x, static (x) => new Node());
    }

    public DirectedGraph(DirectedGraph other)
    {
        foreach (var node in other.Nodes)
        {
            Nodes[node.Key].Incoming.UnionWith(node.Value.Incoming);
            Nodes[node.Key].Outgoing.UnionWith(node.Value.Outgoing);
        }
   }
    #endregion

    #region Accessors
    public int Count => nodes.Count;
    #endregion

    #region Item Management
    public bool TryAdd(EntityKey key)
    {
        if (nodes.ContainsKey(key))
        {
            return false;
        }

        nodes.Add(key, new Node());
        return true;
    }

    public bool TryRemove(EntityKey key)
    {
        if (!nodes.ContainsKey(key))
        {
            return false;
        }

        var node = nodes[key];

        foreach (var linkedNode in node.Incoming)
        {
            nodes[linkedNode].Outgoing.Remove(key);
        }

        foreach (var linkedNode in node.Outgoing)
        {
            nodes[linkedNode].Incoming.Remove(key);
        }

        nodes.Remove(key);
        return true;
    }
    #endregion

    #region Linking
    public bool TryLink(EntityKey leftKey, EntityKey rightKey)
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
    public ImmutableDictionary<EntityKey, ResolvedNode> Resolve()
    {
        var count = Count;
        var in_degrees = nodes.Keys.ToDictionary(static (x) => x, (x) => nodes[x].Outgoing.Count);
        var queue = new Queue<EntityKey>(in_degrees.Where(static (x) => x.Value == 0).Select(static (x) => x.Key));
        Dictionary<EntityKey, ResolutionData> resolved = nodes.Keys.ToDictionary(static (k) => k, static (k) => new ResolutionData(k.Type));

        int order = 0;
        while (queue.Count > 0)
        {
            var key = queue.Dequeue();
            resolved[key].Order = order++;
            var ancestorNode = resolved[key];

            foreach (var id in nodes[key].Incoming)
            {
                in_degrees[id] -= 1;// "remove" the edge from the node
                var node = resolved[id];
                node.Update_Depth(ancestorNode);

                if (in_degrees[id] == 0)
                {
                    queue.Enqueue(id);
                }
            }
        }

        if (order != count)
        {// Cyclic dependencies exist in this graph
            var lastIndice = order + 1;
            foreach (var entry in in_degrees)
            {
                if (entry.Value > 0)
                {
                    var node = resolved[entry.Key];
                    node.Order = lastIndice;
                    node.IsRecursive = true;
                }
            }
        }

        return resolved.ToImmutableDictionary(static (kvp) => kvp.Key, (kvp) =>
        {
            var data = kvp.Value;
            var node = nodes[kvp.Key];
            return new ResolvedNode(kvp.Key,
                                    node.Incoming.ToImmutableHashSet(),
                                    node.Outgoing.ToImmutableHashSet(),
                                    data.Order,
                                    data.IsRecursive,
                                    data.TreeDepth,
                                    data.NodeDepth);
        });
    }
    #endregion

    #region Classes
    private class ResolutionData
    {
        #region Fields
        public readonly NodeType Type;
        public int Order { get; set; } = int.MaxValue;
        public bool IsRecursive { get; set; }
        public NodeDepth? TreeDepth { get; private set; }
        public NodeDepth? NodeDepth { get; private set; }
        #endregion

        #region Constructors
        public ResolutionData(NodeType type)
        {
            Type = type;
        }
        #endregion

        public void Update_Depth(ResolutionData ancestorNode)
        {
            TreeDepth = new()
            {
                Min = Math.Min(TreeDepth?.Min ?? int.MaxValue, (ancestorNode.TreeDepth?.Min + 1) ?? 1),
                Max = Math.Max(TreeDepth?.Max ?? int.MinValue, (ancestorNode.TreeDepth?.Max + 1) ?? 1)
            };

            if (Type == ancestorNode.Type)
            {
                NodeDepth = new()
                {
                    Min = Math.Min(NodeDepth?.Min ?? int.MaxValue, (ancestorNode.NodeDepth?.Min + 1) ?? 1),
                    Max = Math.Max(NodeDepth?.Max ?? int.MinValue, (ancestorNode.NodeDepth?.Max + 1) ?? 1)
                };
            }
        }
    }
    #endregion
}