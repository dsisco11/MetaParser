using System;
using System.Linq;

namespace MetaParser.Syntax;

public class SyntaxTreeBuilder
{
    private GreenNode root;

    #region Accessors
    public RedNode Root => root.GetRed(0, null);
    #endregion

    #region Constructors
    internal SyntaxTreeBuilder(SyntaxTree tree)
    {
        root = tree.Root.Green;
    }

    internal SyntaxTreeBuilder(GreenNode rootGreen)
    {
        root = rootGreen;
    }

    public SyntaxTreeBuilder() : this(new RootNode())
    {
    }

    internal SyntaxTreeBuilder(GreenNode[] rootItems) : this(new RootNode(rootItems))
    {
    }
    #endregion

    public SyntaxTree Build() => new(root);

    #region Tree Mutation
    /// <summary>
    /// Swaps out a node in the tree with a new node and rebuilds the tree
    /// </summary>
    /// <param name="parentNode"></param>
    /// <param name="childNode"></param>
    /// <returns></returns>
    public void Replace(RedNode nodeToReplace, GreenNode newNode)
    {
        // walk up the tree, rebuilding the green node spine
        var currentParent = nodeToReplace.Parent;
        var currentRedNode = nodeToReplace;
        while (currentParent is not null)
        {
            // Create a copy of the parent green node so we can replace the child
            // Copy the children, replacing the current node with the new node
            var newChildren = new GreenNode[currentParent.Green.Children.Length];
            currentParent.Green.Children.CopyTo(newChildren, 0);
            var index = Array.IndexOf(newChildren, currentRedNode.Green);
            newChildren[index] = newNode;
            // Create updated green node
            newNode = currentParent.Green.Mutate(newChildren);
            // walk up the tree, rebuilding the green node spine
            currentRedNode = currentParent;
            currentParent = currentParent.Parent;
        }

        root = newNode;
    }

    public void Unparent(params RedNode[] nodes)
    {
        // group all the nodes by their parent
        // this will allow us to do a single rebuild of each parent
        var nodesByParent = nodes.GroupBy(static node => node.Parent);
        foreach (var parentGroup in nodesByParent)
        {
            var parent = parentGroup.Key;
            var children = parentGroup.Select(static rn => rn.Green).ToArray();

            Unparent(parent, children);
        }
    }

    public void Unparent(RedNode parentNode, params GreenNode[] children)
    {
        // Create a copy of the parent green node so we can replace the child
        // Copy the children, removing the current node
        var newChildren = new GreenNode[parentNode.Green.Children.Length - children.Length];
        var index = 0;
        foreach (var child in parentNode.Green.Children)
        {
            if (!children.Contains(child))
            {
                newChildren[index] = child;
                index++;
            }
        }
        // Create updated green node
        var newNode = parentNode.Green.Mutate(newChildren);
        Replace(parentNode, newNode);
    }

    public void Append(RedNode parentNode, GreenNode childNode)
    {
        // Create new list of children
        var newChildren = new GreenNode[parentNode.Green.Children.Length + 1];
        parentNode.Green.Children.CopyTo(newChildren, 0);
        newChildren[newChildren.Length - 1] = childNode;
        // Create updated green node
        var newGreenNode = parentNode.Green.Mutate(newChildren);

        Replace(parentNode, newGreenNode);
    }

    public void Reparent(RedNode newParent, params RedNode[] nodes)
    {
        Unparent(nodes);
        // Create a copy of the parent green node so we can replace the child
        // Copy the children, replacing the current node with the new node
        var newChildren = new GreenNode[newParent.Green.Children.Length + nodes.Length];
        newParent.Green.Children.CopyTo(newChildren, 0);
        for (int i = 0; i < nodes.Length; i++)
        {
            newChildren[newParent.Green.Children.Length + i] = nodes[i].Green;
        }
        // Create updated green node
        var newNode = newParent.Green.Mutate(newChildren);
        Replace(newParent, newNode);
    }
    #endregion
}
