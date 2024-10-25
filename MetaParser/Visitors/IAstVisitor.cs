using MetaParser.Syntax;

namespace MetaParser.Visitors;

internal interface IAstVisitor
{
    void Visit(RedNode node);
}