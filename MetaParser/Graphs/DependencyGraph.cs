using MetaParser.Consumers;
using MetaParser.Core;
using System.Linq;

namespace MetaParser.Graphs;

internal static class DependencyGraph
{
    public static void Build(MetaParserContext context)
    {
        var tokenIdents = context.Tokens.Values.Select(static (x) => x.NodeID);
        var consumerIdents = context.Consumers.CompleteSet.Select(static (x) => x.NodeID);
        var allIdents = tokenIdents.Concat(consumerIdents);

        context.DepsGraph = new DirectedGraph<GraphNodeKey>(allIdents);
        // For each 'token' consumer, link it to all the tokens it references within the graph
        foreach (var consumer in context.Consumers.CompleteSet.Where(static (c) => c.Type == EConsumerType.Token))
        {
            consumer.Register_Dependencies(context);
        }
    }

    public static void Resolve(MetaParserContext context)
    {
        var results = context.DepsGraph.Resolve();
        foreach (var entry in results)
        {
            if (entry.Key.Type == GraphNodeType.Token)
            {// This is a token
                context.Tokens.Values.Single((t) => t.NodeID == entry.Key).DependencyInfo = entry.Value;
                continue;
            }

            var consumer = context.Consumers.CompleteSet.Single((x) => x.NodeID == entry.Key);
            consumer.DependencyInfo = entry.Value;
        }
    }
}
