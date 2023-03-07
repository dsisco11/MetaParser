using MetaParser.Consumers;
using MetaParser.Core;

using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Graphs;

internal static class DependencyGraph
{
    public static void Build(MetaParserContext context)
    {
        var items = context.Tokens.Values;
        context.TokenGraph = new VertexGraph(items.Select(static (x) => x.Index));

        // For each token, add all of the other tokens which it references to its dependency node
        foreach (var token in items.Where(o => o.Consumers.Any(static (c) => c.Type == EConsumerType.Token)))
        {
            // Link all of the tokens consumers which are 'token' consumers
            IEnumerable<ConsumerInfo> consumers = token.Consumers.Where(static (c) => c.Type == EConsumerType.Token);
            foreach (var consumer in consumers)
            {
                consumer.Register_Dependencies(context);
            }
        }
    }

    public static void Resolve(MetaParserContext context)
    {
        var results = context.TokenGraph.Resolve();
        foreach (var node in results)
        {
            var token = context.Tokens.Values.Single((x) => x.Index == node.Id);
            foreach (var consumer in token.Consumers)
            {
                consumer.DependencyInfo = node;
            }
        }
    }
}
