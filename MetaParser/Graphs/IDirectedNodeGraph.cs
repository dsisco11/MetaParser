using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MetaParser.Graphs;

internal interface IDirectedNodeGraph<T> where T : IEquatable<T>
{
    int Count { get; }
    ImmutableDictionary<T, NodeData> Data { get; }
    IReadOnlyDictionary<T, Node<T>> Nodes { get; }

    void Clear();
    bool Contains(T key);
    bool TryAdd(T key);
    bool TryGetNode(T key, out Node<T> node);
    bool TryAddEdge(T source, T destination);
    bool TryRemove(T key);
}