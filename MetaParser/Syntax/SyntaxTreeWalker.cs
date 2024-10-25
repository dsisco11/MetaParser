using MetaParser.Trees;

using System;
using System.Collections.Generic;

namespace MetaParser.Syntax;


internal partial class SyntaxTreeWalker
{
    public delegate EFilterResult NodeFilter(RedNode node);

    #region Fields
    public readonly RedNode root;
    public readonly TraversalOrder order;
    public readonly NodeFilter? Filter;
    #endregion

    #region Constructors
    public SyntaxTreeWalker(RedNode root, TraversalOrder order)
    {
        this.root = root;
        this.order = order;
    }

    public SyntaxTreeWalker(RedNode root, NodeFilter filter, TraversalOrder order)
    {
        this.root = root;
        this.order = order;
        this.Filter = filter;
    }
    #endregion

    private EFilterResult FilterNode(RedNode node)
    {
        if (Filter is null) return EFilterResult.FILTER_ACCEPT;
        return Filter(node);
    }

    #region Red-Node Traversal
    public IEnumerable<RedNode> Traverse()
    {
        switch (order)
        {
            case TraversalOrder.PreOrder:
                return PreOrderTraversal(root);
            case TraversalOrder.PostOrder:
                return PostOrderTraversal(root);
            case TraversalOrder.LevelOrder:
                return LevelOrderTraversal(root);
            default:
                throw new NotSupportedException($"Traversal order {order} is not supported.");
        }
    }

    private IEnumerable<RedNode> PreOrderTraversal(RedNode node)
    {
        if (node is null)
        {
            yield break;
        }


        switch (FilterNode(node))
        {
            case EFilterResult.FILTER_REJECT:
                yield break;
            case EFilterResult.FILTER_ACCEPT:
                yield return node;
                break;
        }

        foreach (var child in node.Children)
        {
            foreach (var descendant in PreOrderTraversal(child))
            {
                switch (FilterNode(descendant))
                {
                    case EFilterResult.FILTER_REJECT:
                        yield break;
                    case EFilterResult.FILTER_ACCEPT:
                        yield return descendant;
                        break;
                }
            }
        }
    }

    private IEnumerable<RedNode> PostOrderTraversal(RedNode node)
    {
        if (node is null)
        {
            yield break;
        }

        foreach (var child in node.Children)
        {
            foreach (var descendant in PostOrderTraversal(child))
            {
                switch (FilterNode(descendant))
                {
                    case EFilterResult.FILTER_REJECT:
                        yield break;
                    case EFilterResult.FILTER_ACCEPT:
                        yield return descendant;
                        break;
                }
            }
        }

        switch (FilterNode(node))
        {
            case EFilterResult.FILTER_REJECT:
                yield break;
            case EFilterResult.FILTER_ACCEPT:
                yield return node;
                break;
        }
    }

    private IEnumerable<RedNode> LevelOrderTraversal(RedNode node)
    {
        if (node is null)
        {
            yield break;
        }

        var queue = new Queue<RedNode>();
        queue.Enqueue(node);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            switch (FilterNode(current))
            {
                case EFilterResult.FILTER_REJECT:
                    yield break;
                case EFilterResult.FILTER_ACCEPT:
                    yield return current;
                    break;
            }

            foreach (var child in current.Children)
            {
                queue.Enqueue(child);
            }
        }
    }
    #endregion

    #region Green-Node Traversal
    /// <summary>
    /// Traverses the tree in the specified order, returning all nodes of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public IEnumerable<TypedRedNode<T>> Traverse<T>() where T : GreenNode
    {
        switch (order)
        {
            case TraversalOrder.PreOrder:
                return Traverse_Green_PreOrder<T>(root);
            case TraversalOrder.PostOrder:
                return Traverse_Green_PostOrder<T>(root);
            case TraversalOrder.LevelOrder:
                return Traverse_Green_LevelOrder<T>(root);
            default:
                throw new NotSupportedException($"Traversal order {order} is not supported.");
        }
    }

    private static IEnumerable<TypedRedNode<T>> Traverse_Green_PreOrder<T>(RedNode redNode) where T : GreenNode
    {
        if (redNode is null)
        {
            yield break;
        }

        if (redNode.Green is T)
        {
            yield return new (redNode);
        }

        foreach (var child in redNode.Children)
        {
            foreach (var descendant in Traverse_Green_PreOrder<T>(child))
            {
                if (descendant.Green is T)
                {
                    yield return new (descendant);
                }
            }
        }
    }

    private static IEnumerable<TypedRedNode<T>> Traverse_Green_PostOrder<T>(RedNode node) where T : GreenNode
    {
        if (node is null)
        {
            yield break;
        }

        foreach (var child in node.Children)
        {
            foreach (var descendant in Traverse_Green_PostOrder<T>(child))
            {
                if (descendant.Green is T)
                {
                    yield return new (descendant);
                }
            }
        }

        if (node.Green is T)
        {
            yield return new (node);
        }
    }

    private static IEnumerable<TypedRedNode<T>> Traverse_Green_LevelOrder<T>(RedNode node) where T : GreenNode
    {
        if (node is null)
        {
            yield break;
        }

        var queue = new Queue<RedNode>();
        queue.Enqueue(node);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current.Green is T)
            {
                yield return new (current);
            }

            foreach (var child in current.Children)
            {
                queue.Enqueue(child);
            }
        }
    }
    #endregion
}
