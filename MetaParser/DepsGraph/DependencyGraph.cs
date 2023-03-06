using MetaParser.Consumers;
using MetaParser.Patternization;
using MetaParser.Tokens;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MetaParser.DepsGraph
{
    internal class DependencyGraph
    {
        #region Properties
        protected readonly ImmutableDictionary<string, Node> Nodes = ImmutableDictionary<string, Node>.Empty;
        #endregion

        #region Constructors
        public DependencyGraph(IEnumerable<TokenInfo> items)
        {
            // Transform the given items into a dictionary of nodes
            Nodes = items.ToImmutableDictionary(tok => tok.Name, tok => new ParentNode(tok.Name) as Node);

            // For each token, add all of the other tokens which it references to its dependency node
            foreach (var token in items.Where(o => o.Consumers.Any(c => c.Type == EConsumerType.Token)))
            {
                graph_node(token);
            }
        }
        #endregion

        #region Graph Builders
        private void graph_node(TokenInfo token)
        {
            if (!Nodes.TryGetValue(token.Name, out var node))
            {
                throw new System.Exception($@"Unable to find any token named ""{{token.Name}}""");
            }

            // Link all of the tokens consumers which are 'token' consumers
            IEnumerable<ConsumerInfo> consumers = token.Consumers.Where(static (c) => c.Type == EConsumerType.Token);
            foreach (var consumer in consumers)
            {
                graph_consumer((ParentNode)node, token, consumer);
            }
        }

        private void graph_consumer(ParentNode parent, TokenInfo token, ConsumerInfo consumer)
        {
            var node = new Node($"{consumer.Index}", parent);

            if (consumer.Start is not null)
            {
                graph_pattern(node, token, consumer.Start);
            }
            if (consumer.Consume is not null)
            {
                graph_pattern(node, token, consumer.Consume);
            }

            if (consumer.Stop is not null)
            {
                graph_pattern(node, token, consumer.Stop);

                if (consumer.Escape is not null)
                {
                    graph_pattern(node, token, consumer.Escape);
                }
            }
        }

        private void graph_pattern(Node node, TokenInfo token, PatternGroup patternGroup)
        {
            foreach (var subPattern in patternGroup.GetSubPatterns().OfType<PatternTokenRef>())
            {
                // find this tokens dependency node so we can link it
                if (Nodes.TryGetValue(subPattern.Token.Name, out var subTokDep))
                {
                    node.Link(subTokDep);
                }
            }
        }
        #endregion
        
        public bool TryGetNode(ConsumerInfo consumer, out Node? outNode)
        {
            if (Nodes.TryGetValue(consumer.Token.Name, out var tokenNode))
            {
                var parentNode = (ParentNode)tokenNode;
                if (parentNode.Children.TryGetValue($"{consumer.Index}", out var outConsumerNode))
                {
                    outNode = outConsumerNode;
                    return true;
                }
            }

            outNode = default;
            return false;
        }
    }
}
