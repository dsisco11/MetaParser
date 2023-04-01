using MetaParser.Core;
using MetaParser.Parsing.Constructs;

using System.Linq;

namespace MetaParser.Graphs;

internal static class DependencyGraph
{
    public static DirectedGraph Build(EntityRegistry Registry)
    {
        var graph = new DirectedGraph(Registry.Entities.Keys);
        foreach (IGraphEntity entity in Registry.Entities.Values)
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
            Registry.Entities[entry.Key].DependencyInfo = entry.Value;
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
