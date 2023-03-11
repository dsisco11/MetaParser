using MetaParser.Core;
using MetaParser.Parsing.Constructs;

namespace MetaParser.Graphs;

internal static class DependencyGraph
{
    public static void Build(MetaParserContext context)
    {
        context.DepsGraph = new TokenGraph(context.Registry.GetNodeIDs());
        foreach (IGraphableEntity entity in context.Registry.GetGraphEntities())
        {
            var resolvedLinks = entity.ResolveLinks(context);
            foreach (var link in resolvedLinks)
            {
                context.DepsGraph.TryLink(link.Source, link.Target);
            }
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
