using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace MetaParser.Trees;

internal sealed class KeyTree<T>
{
    #region Fields
    readonly Dictionary<T, KeyTreeNode<T>> nodes = new();
    readonly KeyTreeNode<T> _rootNode;
    #endregion

    #region Accessors
    public KeyTreeNode<T> RootNode => _rootNode;
    #endregion

    #region Constructors
    public KeyTree(KeyTreeNode<T> rootNode)
    {
        _rootNode = rootNode;
    }

    private KeyTree(KeyTree<T> other)
    {
        nodes = new(other.nodes);
        _rootNode = other._rootNode;
    }
    #endregion

    #region Methods
    public KeyTree<T> Clone()
    {
        return new KeyTree<T>(this);
    }
    #endregion

    #region Mutators
    public void Add(T value)
    {
        nodes.Add(value, new KeyTreeNode<T>(value, _rootNode));
    }

    public void AddEdge(T parent, T child)
    {
        if (!nodes.TryGetValue(parent, out var parentNode))
        {
            parentNode = new KeyTreeNode<T>(parent, _rootNode);
            nodes.Add(parent, parentNode);
        }

        if (nodes.TryGetValue(child, out var childNode))
        {
            childNode.SetParent(parentNode);
        }
        else
        {
            childNode = new KeyTreeNode<T>(child, parentNode);
            nodes.Add(child, childNode);
        }

        parentNode.Add(childNode);
    }

    public void RemoveEdge(T parent, T child)
    {
        if (nodes.TryGetValue(parent, out var parentNode))
        {
            if (nodes.TryGetValue(child, out var childNode))
            {
                parentNode.Remove(childNode);
            }
        }
    }
    #endregion

    #region Lookup
    public bool TryGetNode(T key, out KeyTreeNode<T> result)
    {
        if (nodes.TryGetValue(key, out var node))
        {
            result = node;
            return true;
        }

        Debug.Fail($"Unable to findkKey in tree. (Key: {key})");
        result = KeyTreeNode<T>.Default;
        return false;
    }

    public KeyTreeNode<T>? this[T key]
    {
        get 
        {
            if (nodes.TryGetValue(key, out var node))
            {
                return node;
            }

            Debug.Fail($"Unable to findkKey in tree. (Key: {key})");
            return null;
        }
    }
    #endregion
}

public sealed class KeyTreeNode<T> : IEnumerable<KeyTreeNode<T>>
{
    #region Statics
    public static readonly KeyTreeNode<T> Default = new KeyTreeNode<T>(default);
    #endregion

    #region Fields
    readonly T _value;
    readonly WeakReference<KeyTreeNode<T>?> _parent = new WeakReference<KeyTreeNode<T>?>(null);
    readonly List<KeyTreeNode<T>> _children = new();
    #endregion

    #region Accessors
    public T Value => _value;
    public KeyTreeNode<T>? Parent => _parent.TryGetTarget(out var parent) ? parent : null;
    public IReadOnlyList<KeyTreeNode<T>> Children => _children;
    #endregion

    #region Constructors
    public KeyTreeNode(T value)
    {
        _value = value;
    }

    public KeyTreeNode(T value, KeyTreeNode<T> parent)
    {
        _value = value;
        _parent.SetTarget(parent);
    }

    public KeyTreeNode(T value, List<KeyTreeNode<T>> children)
    {
        _value = value;
        _children = children;
    }
    #endregion

    #region IEnumerable
    public IEnumerator<KeyTreeNode<T>> GetEnumerator()
    {
        return ((IEnumerable<KeyTreeNode<T>>)_children).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_children).GetEnumerator();
    }
    #endregion

    #region Mutators
    internal void Add(KeyTreeNode<T> child)
    {
        _children.Add(child);
    }

    internal bool Remove(KeyTreeNode<T> child)
    {
        return _children.Remove(child);
    }

    internal void Clear()
    {
        _children.Clear();
    }

    internal void SetParent(KeyTreeNode<T> parent)
    {
        _parent.SetTarget(parent);
    }
    #endregion
}
