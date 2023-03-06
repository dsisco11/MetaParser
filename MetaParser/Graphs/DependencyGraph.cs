using MetaParser.Consumers;
using MetaParser.Contexts;
using MetaParser.Patternization;

using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Graphs
{
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
                    if (consumer.Start is not null)
                    {
                        link_pattern(context, consumer, consumer.Start);
                    }
                    if (consumer.Consume is not null)
                    {
                        link_pattern(context, consumer, consumer.Consume);
                    }

                    if (consumer.Stop is not null)
                    {
                        link_pattern(context, consumer, consumer.Stop);

                        if (consumer.Escape is not null)
                        {
                            link_pattern(context, consumer, consumer.Escape);
                        }
                    }
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

        #region Utility
        private static void link_pattern(MetaParserContext context, ConsumerInfo consumer, PatternGroup patternGroup)
        {
            foreach (var subPattern in patternGroup.GetSubPatterns().OfType<PatternTokenRef>())
            {
                if (!context.Tokens.TryGetValue(subPattern.TokenName, out var subDependencyToken))
                {
                    throw new System.Exception($@"Unable to find token: ""{subPattern.TokenName}""");
                }

                context.TokenGraph.TryLink(consumer.Token.Index, subDependencyToken.Index);
            }
        }
        #endregion
    }
}
