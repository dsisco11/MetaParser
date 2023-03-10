using MetaParser.Core;

namespace MetaParser.Graphs;

internal static class DependencyGraph
{
    public static void Build(MetaParserContext context)
    {
        context.DepsGraph = new DirectedGraph<NodeKey>(context.Registry.GetNodeIDs());
        foreach (var consumer in context.Registry.Consumers.Values)
        {
            consumer.Register_Dependencies(context);
        }
    }

    public static void Resolve(MetaParserContext context)
    {
        var results = context.DepsGraph.Resolve();
        foreach (var entry in results)
        {
            switch (entry.Key.Type)
            {
                case NodeType.Data:
                    break;
                case NodeType.Pattern:
                    {
                        context.Registry.Patterns[entry.Key].DependencyInfo = entry.Value;
                    }
                    break;
                case NodeType.Consumer:
                    {
                        context.Registry.Consumers[entry.Key].DependencyInfo = entry.Value;
                    }
                    break;
                case NodeType.Token:
                    {
                        context.Registry.Tokens[entry.Key].DependencyInfo = entry.Value;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
