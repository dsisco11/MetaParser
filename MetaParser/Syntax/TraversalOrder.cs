namespace MetaParser.Syntax;


internal partial class SyntaxTreeWalker
{
    public enum TraversalOrder
    {


        /// <summary>
        /// Level-order traversal visits the nodes level by level, starting from the root and proceeding through each level in a left-to-right manner.
        /// Also known as breadth-first traversal, this method visits all nodes at the current depth before moving on to nodes at the next depth.
        /// </summary>
        LevelOrder,

        /// <summary>
        /// Pre-order traversal visits the current node before its child nodes.
        /// In this traversal method, the root node is visited first, then the left subtree, and finally the right subtree.
        /// </summary>
        PreOrder,

        /// <summary>
        /// Post-order traversal visits the current node after its child nodes.
        /// In this traversal method, the left subtree is visited first, then the right subtree, and finally the root node.
        /// </summary>
        PostOrder,
    }
}
