using MetaParser.Syntax;
using MetaParser.Syntax.Nodes;
using MetaParser.Trees;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Visitors;
internal class ParsingStageVisitor : AstVisitor
{
    public ParsingStageVisitor(SyntaxTree tree) : base(tree)
    {
    }

    public override void Visit(RedNode node)
    {
        // Find all the Parser Definition Nodes
        var walker = new SyntaxTreeWalker(node, SyntaxTreeWalker.TraversalOrder.LevelOrder);
        foreach (var parserNode in walker.Traverse<ParserDefinitionNode>())
        {
            VisitParserDefinitionNode(parserNode);
        }
    }

    void VisitParserDefinitionNode(RedNode parserNode)
    {
        DependencyGraphVisitor graphVisitor = new(Tree);
        graphVisitor.Visit(parserNode);
        var graph = graphVisitor.Graph;

        var walker = new SyntaxTreeWalker(parserNode, SyntaxTreeWalker.TraversalOrder.LevelOrder);
        var groupedConsumerNodes = walker.Traverse<TokenConsumerNode>().GroupBy((n) => { 
            graph.Data.TryGetValue(n.Value.Token.Name, out var data);
            return data?.Depth ?? throw new Exception("TokenConsumerNode not found in graph");
        }, 
        static n => n.Green);

        SyntaxTreeBuilder builder = this.Tree.CreateBuilder();
        List<ParsingStageNode> stageNodes = new();
        // go through each group of consumer nodes and create a stage node for each group
        foreach (var group in groupedConsumerNodes)
        {
            var stageNode = new ParsingStageNode(group.Key, group.ToArray());
            stageNodes.Add(stageNode);
        }

        // Now that we have all the stage nodes, we can insert them into the tree
        // We do this by mutating the parser node to replace the token consumer nodes with the stage nodes
        var parserNodeBuilder = parserNode.Green.CreateBuilder();
        foreach (var stageNode in stageNodes)
        {
            parserNodeBuilder.Remove(stageNode.Children);
            parserNodeBuilder.Add(stageNode);
        }

        var newParserNode = parserNodeBuilder.Build();
        builder.Replace(parserNode, newParserNode);
        Tree = builder.Build();
    }
}
