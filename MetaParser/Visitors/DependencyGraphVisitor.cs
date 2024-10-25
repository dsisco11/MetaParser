using MetaParser.Graphs;
using MetaParser.Syntax;
using MetaParser.Trees;

using System;

namespace MetaParser.Visitors;
internal class DependencyGraphVisitor : AstVisitor
{
    #region Fields
    public readonly DirectedNodeGraph<string> Graph;
    #endregion

    public DependencyGraphVisitor(Syntax.SyntaxTree tree) : base(tree)
    {
        Graph = new DirectedNodeGraph<string>();
    }

    public override void Visit(RedNode node)
    {
        var walker = new SyntaxTreeWalker(node, SyntaxTreeWalker.TraversalOrder.LevelOrder);

        foreach (var next in walker.Traverse<TokenConsumerNode>())
        {
            ProcessConsumerNode(next);
        }
    }

    private void ProcessConsumerNode(RedNode node)
    {
        if (node.Green is TokenConsumerNode consumer)
        {
            var token = consumer.Token.Name;
            Graph.TryAdd(token);

            var walker = new SyntaxTreeWalker(node, SyntaxTreeWalker.TraversalOrder.LevelOrder);

            foreach (var patternNode in walker.Traverse<PatternNode>())
            {
                ProcessPatternNode(patternNode.Value, consumer);
            }
        }
    }

    private void ProcessPatternNode(PatternNode node, TokenConsumerNode tokenConsumerNode)
    {
        var valueLiteral = node.Children[0] as ValueLiteralNode;
        var token = valueLiteral?.Value ?? throw new Exception("Pattern Token node did not have a value literal child");
        Graph.TryAdd(token);
        Graph.TryAddEdge(tokenConsumerNode.Token.Name, token);
    }
}
