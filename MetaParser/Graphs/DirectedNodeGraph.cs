using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MetaParser.Graphs
{
    internal sealed class DirectedNodeGraph<T> where T : IEquatable<T>
    {
        #region Fields
        private readonly Dictionary<T, Node<T>> nodes;
        private ImmutableDictionary<T, NodeData> data;
        private bool isDirty = true;
        #endregion

        #region Accessors
        public int Count => nodes.Count;
        public IReadOnlyDictionary<T, Node<T>> Nodes => nodes;
        public ImmutableDictionary<T, NodeData> Data
        {
            get { if (isDirty) { UpdateData(); } return data; }
        }
        #endregion

        #region Constructors
        public DirectedNodeGraph()
        {
            data = ImmutableDictionary<T, NodeData>.Empty;
            nodes = new();
        }

        public DirectedNodeGraph(IEnumerable<T> items)
        {
            data = ImmutableDictionary<T, NodeData>.Empty;
            nodes = items.ToDictionary(static (x) => x, static (x) => new Node<T>());
        }

        public DirectedNodeGraph(DirectedNodeGraph<T> other)
        {
            data = other.data;
            nodes = new(other.nodes);
            isDirty = other.isDirty;
        }
        #endregion

        #region Item Management

        public bool Contains(T key)
        {
            return nodes.ContainsKey(key);
        }

        public bool TryAdd(T key)
        {
            if (nodes.ContainsKey(key))
            {
                return false;
            }

            nodes.Add(key, new());
            return true;
        }

        public bool TryRemove(T key)
        {
            if (!nodes.ContainsKey(key))
            {
                return false;
            }

            var node = nodes[key];

            foreach (var linkedNode in node.Incoming)
            {
                nodes[linkedNode].Outgoing.Remove(key);
                isDirty = true;
            }

            foreach (var linkedNode in node.Outgoing)
            {
                nodes[linkedNode].Incoming.Remove(key);
                isDirty = true;
            }

            nodes.Remove(key);
            return true;
        }

        public bool TryGetNode(T key, out Node<T> node)
        {
            return nodes.TryGetValue(key, out node);
        }

        public void Clear()
        {
            nodes.Clear();
            isDirty = true;
        }
        #endregion

        #region Linking
        public bool TryLink(T source, T destination)
        {
            if (!nodes.TryGetValue(source, out var leftNode))
            {
                throw new InvalidOperationException($"Cannot link {source} to {destination}. {source} does not exist in graph.");
            }

            if (!nodes.TryGetValue(destination, out var rightNode))
            {
                throw new InvalidOperationException($"Cannot link {source} to {destination}. {destination} does not exist in graph.");
            }

            isDirty = true;
            rightNode.Incoming.Add(source);
            return leftNode.Outgoing.Add(destination);
        }
        #endregion

        #region Filtering
        public DirectedNodeGraph<T> Where(Predicate<T> predicate)
        {
            var newGraph = new DirectedNodeGraph<T>();
            foreach (var node in nodes)
            {
                if (predicate(node.Key))
                {
                    newGraph.nodes.Add(node.Key, new Node<T>());
                }
            }

            foreach (var newNode in newGraph.nodes)
            {
                var originalNode = nodes[newNode.Key];
                foreach (var outgoing in originalNode.Outgoing)
                {
                    if (newGraph.nodes.ContainsKey(outgoing))
                    {
                        newGraph.TryLink(newNode.Key, outgoing);
                    }
                }
            }

            return newGraph;
        }
        #endregion

        #region Resolving

        private void UpdateData()
        {
            data = Resolve();
            isDirty = false;
        }

        private ImmutableDictionary<T, NodeData> Resolve()
        {
            var count = Count;
            var in_degrees = nodes.Keys.ToDictionary(static (x) => x, (x) => nodes[x].Outgoing.Count);
            var queue = new Queue<T>(in_degrees.Where(static (x) => x.Value == 0).Select(static (x) => x.Key));
            Dictionary<T, NodeData> resolved = nodes.Keys.ToDictionary(static (k) => k, static (k) => new NodeData());
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
            if (resolved.Count != count)
            {
                throw new InvalidOperationException($@"Graph contains a cycle: {string.Join(", ", resolved.Where(static (x) => x.Value.Order == -1).Select(static (x) => x.Key.ToString()))}");
            }
            return resolved.ToImmutableDictionary();
        }
        #endregion
    }
}