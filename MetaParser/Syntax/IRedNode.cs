using System.Collections.Generic;

using MetaParser.Syntax;
using MetaParser.Visitors;

namespace MetaParser.Trees
{
    internal interface IRedNode
    {
        IEnumerable<RedNode> Children { get; }
        GreenNode Green { get; }
        RedNode? Parent { get; set; }
        int Position { get; set; }

        void Accept(IAstVisitor visitor);
        IEnumerator<RedNode> GetEnumerator();
        bool TryReplaceChild(RedNode oldChild, RedNode newChild);
    }
}