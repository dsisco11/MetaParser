using System.Collections.Generic;

namespace MetaParser.Syntax;

public class GreenNodeBuilder : List<GreenNode>
{
    #region Fields
    private GreenNode Node;
    #endregion

    #region Constructors
    public GreenNodeBuilder(GreenNode node) : base(node.Children)
    {
        Node = node;
    }
    #endregion

    #region Methods
    public GreenNode Build()
    {
        return Node.Mutate(ToArray());
    }

    public void Replace(GreenNode nodeToReplace, GreenNode newNode)
    {
        var index = IndexOf(nodeToReplace);
        this[index] = newNode;
    }

    public void Replace(GreenNode nodeToReplace, IEnumerable<GreenNode> newNodes)
    {
        var index = IndexOf(nodeToReplace);
        RemoveAt(index);
        InsertRange(index, newNodes);
    }

    public void Replace(GreenNode nodeToReplace, params GreenNode[] newNodes)
    {
        var index = IndexOf(nodeToReplace);
        RemoveAt(index);
        InsertRange(index, newNodes);
    }

    public void Remove(params GreenNode[] greenNodes)
    {
        for (int i = 0; i < greenNodes.Length; i++)
        {
            GreenNode? node = greenNodes[i];
            Remove(node);
        }
    }
    #endregion
}
