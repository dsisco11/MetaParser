using MetaParser.Core;
using MetaParser.Parsing.Constructs;

using System.Linq;

namespace MetaParser.Graphs;

internal static class DependencyGraph
{
    public static DirectedGraph Build(TokenRegistry Registry)
    {
        var graph = new DirectedGraph(Registry.GetNodeIDs());
        foreach (IGraphableEntity entity in Registry.GetGraphEntities())
        {
            var resolvedLinks = entity.ResolveLinks(Registry);
            foreach (var link in resolvedLinks)
            {
                graph.TryLink(link.Source, link.Target);
            }
        }

        var resolved = graph.Resolve();
        foreach (var entry in resolved)
        {
            switch (entry.Key.Type)
            {
                case NodeType.Data:
                    break;
                case NodeType.Pattern:
                    {
                        Registry.Patterns[entry.Key].DependencyInfo = entry.Value;
                    }
                    break;
                case NodeType.Consumer:
                    {
                        Registry.Consumers[entry.Key].DependencyInfo = entry.Value;
                    }
                    break;
                case NodeType.Token:
                    {
                        Registry.Tokens[entry.Key].DependencyInfo = entry.Value;
                    }
                    break;
                default:
                    break;
            }
        }

        return graph;
    }

    static void Simplify_Graph(DirectedGraph graph)
    {
        // we only want to see a graph of our token relationships, so we'll remove everything else from the graph
        var trash = graph.Nodes.Keys.Where(static k => k.Type != NodeType.Token && k.Type != NodeType.Consumer).ToList();
        foreach (var key in trash)
        {
            graph.TryRemove(key);
        }
    }
}
