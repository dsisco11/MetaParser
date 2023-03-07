using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace MetaParser.Graphs
{
    internal class VertexGraph
    {
        #region Records
        [DebuggerDisplay(@"[In: {Incoming.Count}] [Out: {Outgoing.Count}]", Name = @"{Id}")]
        private record VertexNode
        {
            public readonly int Index;
            public readonly HashSet<int> Incoming = new();
            public readonly HashSet<int> Outgoing = new();

            public VertexNode(int index)
            {
                Index = index;
            }
        }
        #endregion

        #region Fields
        private readonly ImmutableDictionary<int, VertexNode> nodes = ImmutableDictionary<int, VertexNode>.Empty;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public VertexGraph()
        {
        }

        public VertexGraph(IEnumerable<int> items)
        {
            nodes = items.ToImmutableDictionary(static (x) => x, static (x) => new VertexNode(x));
        }
        #endregion

        #region Accessors
        public int Count => nodes.Count;
        #endregion

        #region Links
        public bool TryLink(int leftNodeId, int rightNodeId)
        {
            if(!nodes.TryGetValue(leftNodeId, out var leftNode))
            {
                throw new Exception($@"Unable to locate node with id: {leftNodeId}");
            }

            if(!nodes.TryGetValue(rightNodeId, out var rightNode))
            {
                throw new Exception($@"Unable to locate node with id: {rightNodeId}");
            }

            rightNode.Incoming.Add(leftNode.Index);
            return leftNode.Outgoing.Add(rightNode.Index);
        }
        #endregion

        #region Sorting

        /// <summary>
        /// Returns ordered list of node-ids
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public ResolvedVertexNode[] Resolve()
        {
            var count = Count;
            var maxIndex = nodes.Values.Max(static (x) => x.Index);
            var remap = new Reindexer(nodes.Values.Select(static (x) => x.Index).ToArray());

            var in_degree = ArrayPool<int>.Shared.Rent(count);
            var resolved = new ResolvedVertexNode[count];

            foreach (var node in nodes)
            {
                var index = remap.GetNew(node.Value.Index);

                in_degree[index] = node.Value.Outgoing.Count;
                resolved[index] = new ResolvedVertexNode()
                {
                    Id = node.Key,
                    MinDepth = int.MaxValue,
                    MaxDepth = 0
                };
            }

            var q = new Queue<int>();
            for (int nidx = 0; nidx < count; nidx++)
            {
                if (in_degree[nidx] == 0)
                {
                    q.Enqueue(nidx);
                    resolved[nidx].MinDepth = 0;
                }
            }

            int order = 0;
            while (q.Count > 0)
            {
                var nIdx = q.Dequeue();
                var oIdx = remap.GetOld(nIdx);
                var node = resolved[oIdx];

                resolved[oIdx].Order = order++;
                foreach (var oid in nodes[oIdx].Incoming)
                {
                    var nid = remap.GetNew(oid);
                    in_degree[nid] -= 1;
                    resolved[nid].MinDepth = Math.Min(resolved[nid].MinDepth, node.MinDepth + 1);
                    resolved[nid].MaxDepth = Math.Max(resolved[nid].MaxDepth, node.MaxDepth + 1);

                    if (in_degree[nid] == 0)
                    {
                        q.Enqueue(nid);
                    }
                }
            }

            if (order != count)
            {// Cyclic dependencies exist in this graph
                for (int nIdx = 0; nIdx < in_degree.Length; nIdx++)
                {
                    if(in_degree[nIdx] > 0)
                    {
                        resolved[nIdx].IsRecursive = true;
                        resolved[nIdx].Order = order+1;
                    }
                }
            }

            ArrayPool<int>.Shared.Return(in_degree);

            return resolved;
        }
        #endregion
    }

    [DebuggerDisplay("Order[{Order}] | MinDepth[{MinDepth}] | MaxDepth[{MaxDepth}] | IsCyclic ({IsCyclic})", Name = "{Id}")]
    internal struct ResolvedVertexNode
    {
        public int Id;
        public int Order;
        public int MinDepth;
        public int MaxDepth;
        public bool IsRecursive;
    }
}
