using MetaParser.Consumers;
using MetaParser.Core;
using System.Linq;

namespace MetaParser.Graphs;

internal static class DependencyGraph
{
    public static void Build(MetaParserContext context)
    {
        var tokenIdents = context.Tokens.Values.Select(static (x) => x.Identity);
        var consumerIdents = context.Consumers.CompleteSet.Select(static (x) => x.Identity);
        var allIdents = tokenIdents.Concat(consumerIdents);

        context.DepsGraph = new VertexGraph<TokenGraphId>(allIdents);
        // For each 'token' consumer, link it to all the tokens it references within the graph
        foreach (var consumer in context.Consumers.CompleteSet.Where(static c => c.Type == EConsumerType.Token))
        {
            consumer.Register_Dependencies(context);
        }
    }

    public static void Resolve(MetaParserContext context)
    {
        var results = context.DepsGraph.Resolve();
        foreach (var entry in results)
        {
            if (entry.Key.ConsumerIndex < 0)
            {
                continue;
            }

            var consumer = context.Consumers.CompleteSet.Single((x) => x.Identity == entry.Key);
            consumer.DependencyInfo = entry.Value;
        }
    }
}
