using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MetaParser.Trees;

public sealed class RedGreenTree<T>
{
	#region Fields
	readonly GreenNode<T> _rootNode;
    #endregion

    #region Accessors
    public RedNode<T> RootNode => new(_rootNode, null);
    #endregion

    #region Constructors
    public RedGreenTree(GreenNode<T> rootNode)
    {
        _rootNode = rootNode;
    }
    #endregion

    #region Methods
    public RedGreenTree<T> Clone()
    {
        return new RedGreenTree<T>(_rootNode);
    }
    #endregion
}

/// <summary>
/// Green nodes are *immutable* and hold a value and children, but no reference to a parent
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed record GreenNode<T> : IEnumerable<GreenNode<T>>
{
    // TODO: utilize array pool for instantiation
    #region Fields
    readonly T _value;
    #endregion

    #region Accessors
    public T Value => _value;
    public ImmutableArray<GreenNode<T>> Children { get; }
    #endregion

    #region Constructors
    public GreenNode(T value, GreenNode<T>[] children)
    {
        _value = value;
        Children = children.ToImmutableArray();
    }

    public GreenNode(T value, ImmutableArray<GreenNode<T>> children)
    {
        _value = value;
        Children = children;
    }
    public GreenNode(T value, IEnumerable<GreenNode<T>> children)
    {
        _value = value;
        Children = children.ToImmutableArray();
    }
    #endregion

    #region Enumerators
    public IEnumerator<GreenNode<T>> GetEnumerator()
    {
        foreach (var child in Children)
        {
            yield return child;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion
}

/// <summary>
/// Red node holds a value and children, but also a reference to its parent
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed record RedNode<T> : IEnumerable<RedNode<T>>
{
    #region Fields
    readonly GreenNode<T> _greenNode;
    readonly RedNode<T>? _parent;
    #endregion

    #region Accessors
    public T Value => _greenNode.Value;
    public RedNode<T>? Parent => _parent;
    public IEnumerable<RedNode<T>> Children => _greenNode.Children.Select((x) => new RedNode<T>(x, this)).ToList();
    #endregion

    #region Constructors
    public RedNode(GreenNode<T> greenNode, RedNode<T>? parent)
    {
        _greenNode = greenNode;
        _parent = parent;
    }

    public IEnumerator<RedNode<T>> GetEnumerator()
    {
        // async enumerate children of _greenNode
        foreach (var child in _greenNode)
        {
            // create a red node for each child
            var redChild = new RedNode<T>(child, this);
            yield return redChild;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion
}


