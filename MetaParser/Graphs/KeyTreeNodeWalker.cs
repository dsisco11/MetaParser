using MetaParser.Trees;

using System;
using System.Collections.Generic;

namespace MetaParser.Graphs;

/// <summary>
/// Provides helper functions for walking a tree of <see cref="KeyTreeNode{T}"/>s.
/// </summary>
internal static class KeyTreeNodeWalker
{
    #region Non-Predicated
    /// <summary>
    /// Walks a tree of <see cref="KeyTreeNode{T}"/>s, invoking a callback for each node visited.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in each node.</typeparam>
    /// <param name="root">The root node of the tree to walk.</param>
    /// <param name="predicate">A callback which will be invoked for each node visited.</param>
    /// <param name="traversalOrder">The order in which the tree will be traversed.</param>
    public static IEnumerable<T> Walk<T>(KeyTreeNode<T> root, TraversalOrder traversalOrder = TraversalOrder.DepthFirstPreOrder)
    {
        if (root is null)
        {
            throw new ArgumentNullException(nameof(root));
        }

        switch (traversalOrder)
        {
            case TraversalOrder.DepthFirstPreOrder:
                return WalkDepthFirstPreOrder(root);
            case TraversalOrder.DepthFirstPostOrder:
                return WalkDepthFirstPostOrder(root);
            case TraversalOrder.BreadthFirst:
                return WalkBreadthFirst(root);
            case TraversalOrder.Ascending:
                return WalkAscending(root);
            default:
                throw new ArgumentException($"Unknown traversal order: {traversalOrder}", nameof(traversalOrder));
        }
    }

    private static IEnumerable<T> WalkDepthFirstPreOrder<T>(KeyTreeNode<T> node)
    {
        // walk through the tree, using the predicate as a filter for items to yield
        if (node is not null)
        {
            yield return node.Value;
            foreach (var child in node.Children)
            {
                WalkDepthFirstPostOrder(child);
            }
        }
    }

    private static IEnumerable<T> WalkDepthFirstPostOrder<T>(KeyTreeNode<T> node)
    {
        if (node is not null)
        {
                yield return node.Value;
            foreach (var child in node.Children)
            {
                WalkDepthFirstPostOrder(child);
            }
        }
    }

    private static IEnumerable<T> WalkBreadthFirst<T>(KeyTreeNode<T> node)
    {
        var queue = new Queue<KeyTreeNode<T>>();
        queue.Enqueue(node);
        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            foreach (var child in currentNode.Children)
            {
                queue.Enqueue(child);
            }
                yield return currentNode.Value;
        }
    }

    private static IEnumerable<T> WalkAscending<T>(KeyTreeNode<T> node)
    {
        // walk through the tree, using the predicate as a filter for items to yield
        var currentNode = node;
        while (currentNode is not null)
        {
            yield return currentNode.Value;
            currentNode = currentNode.Parent;
        }
    }
    #endregion

    #region Predicated
    /// <summary>
    /// Walks a tree of <see cref="KeyTreeNode{T}"/>s, invoking a callback for each node visited.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in each node.</typeparam>
    /// <param name="root">The root node of the tree to walk.</param>
    /// <param name="predicate">A callback which will be invoked for each node visited.</param>
    /// <param name="traversalOrder">The order in which the tree will be traversed.</param>
    public static IEnumerable<T> Walk<T>(KeyTreeNode<T> root, Predicate<T> predicate, TraversalOrder traversalOrder = TraversalOrder.DepthFirstPreOrder)
    {
        if (root is null)
        {
            throw new ArgumentNullException(nameof(root));
        }
        if (predicate is null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }
        switch (traversalOrder)
        {
            case TraversalOrder.DepthFirstPreOrder:
                return WalkDepthFirstPreOrder(root, predicate);
            case TraversalOrder.DepthFirstPostOrder:
                return WalkDepthFirstPostOrder(root, predicate);
            case TraversalOrder.BreadthFirst:
                return WalkBreadthFirst(root, predicate);
            case TraversalOrder.Ascending:
                return WalkAscending(root, predicate);
            default:
                throw new ArgumentException($"Unknown traversal order: {traversalOrder}", nameof(traversalOrder));
        }
    }

    private static IEnumerable<T> WalkDepthFirstPreOrder<T>(KeyTreeNode<T> node, Predicate<T> predicate)
    {
        // walk through the tree, using the predicate as a filter for items to yield
        if (node is not null)
        {
            if (predicate(node.Value))
                yield return node.Value;
            foreach (var child in node.Children)
            {
                WalkDepthFirstPostOrder(child, predicate);
            }
        }
    }

    private static IEnumerable<T> WalkDepthFirstPostOrder<T>(KeyTreeNode<T> node, Predicate<T> predicate)
    {
        if (node is not null)
        {
            if (predicate(node.Value))
                yield return node.Value;
            foreach (var child in node.Children)
            {
                WalkDepthFirstPostOrder(child, predicate);
            }
        }
    }

    private static IEnumerable<T> WalkBreadthFirst<T>(KeyTreeNode<T> node, Predicate<T> predicate)
    {
        var queue = new Queue<KeyTreeNode<T>>();
        queue.Enqueue(node);
        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            foreach (var child in currentNode.Children)
            {
                queue.Enqueue(child);
            }
            if (predicate(currentNode.Value))
            {
                yield return currentNode.Value;
            }
        }
    }

    private static IEnumerable<T> WalkAscending<T>(KeyTreeNode<T> node, Predicate<T> predicate)
    {
        // walk through the tree, using the predicate as a filter for items to yield
        var currentNode = node;
        while (currentNode is not null)
        {
            if (predicate(currentNode.Value))
            {
                yield return currentNode.Value;
            }
            currentNode = currentNode.Parent;
        }
    }
    #endregion

    /// The order in which a tree will be traversed.
    /// </summary>
    public enum TraversalOrder
    {
        /// <summary>
        /// </summary>
        Ascending,
        /// <summary>
        /// Traverse the tree in a depth-first, pre-order fashion.
        /// </summary>
        DepthFirstPreOrder,
        /// <summary>
        /// Traverse the tree in a depth-first, post-order fashion.
        /// </summary>
        DepthFirstPostOrder,
        /// <summary>
        /// Traverse the tree in a breadth-first fashion.
        /// </summary>
        BreadthFirst,
    }
}
