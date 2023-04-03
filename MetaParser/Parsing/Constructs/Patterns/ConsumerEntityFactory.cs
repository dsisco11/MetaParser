using MetaParser.Compiler.Structs;
using MetaParser.Core;

using System;

namespace MetaParser.Parsing.Constructs.Patterns;

internal class ConsumerEntityFactory
{
    public static ConsumerEntity Create(EConsumerKind kind, ConsumerClause clause, string consumerTokenAlias, EntityRegistry registry)
    {
        if (clause.Start is null)
        {
            throw new ArgumentNullException(nameof(clause));
        }

        PatternEntity startPattern = PatternEntityFactory.Create(clause.Start!, registry);
        PatternEntity? consumePattern = null, stopPattern = null, escapePattern = null;

        if (clause.Consume is not null) consumePattern = PatternEntityFactory.Create(clause.Consume, registry);
        if (clause.Stop is not null) stopPattern = PatternEntityFactory.Create(clause.Stop, registry);
        if (clause.Escape is not null) escapePattern = PatternEntityFactory.Create(clause.Escape, registry);

        var consumerEntity = new ConsumerEntity(kind, registry, startPattern, consumePattern, stopPattern, escapePattern, consumerTokenAlias);
        // add the consumer to the registry
        registry.Add(consumerEntity);

        // Link all pattern entities to the consumer
        registry.Tree.AddEdge(parent: consumerEntity.Key, child: startPattern.Key);

        if (consumePattern is not null)
        {
            registry.Tree.AddEdge(parent: consumerEntity.Key, child: consumePattern.Key);
        }

        if (stopPattern is not null)
        {
            registry.Tree.AddEdge(parent: consumerEntity.Key, child: stopPattern.Key);
        }

        if (escapePattern is not null)
        {
            registry.Tree.AddEdge(parent: consumerEntity.Key, child: escapePattern.Key);
        }

        return consumerEntity;
    }
}
