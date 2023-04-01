using MetaParser.Compiler.Structs;
using MetaParser.Core;
using MetaParser.Exceptions;

using System.Linq;

namespace MetaParser.Parsing.Constructs.Patterns;

internal static class PatternEntityFactory
{
    public static PatternEntity Create(PatternClause clause, EntityRegistry registry)
    {
        PatternEntity entity = clause switch
        {
            PatternItemClause item when clause.Kind == EPatternKind.Token => new PatternTokenRef(registry, item.Value),
            PatternItemClause item => new PatternConst(registry, item.Value),

            PatternSequenceClause sequence when clause.Kind == EPatternKind.Range => new PatternRange(registry,
                                                                                                      (sequence.Items[0] as PatternItemClause)?.Value ?? string.Empty,
                                                                                                      (sequence.Items[1] as PatternItemClause)?.Value ?? string.Empty),
            PatternSequenceClause sequence => new PatternSequence(clause.Kind, registry, sequence.Select(x => Create(x, registry)).ToArray()),
            _ => throw new MetaParserException($"Cannot create pattern from clause of type '{clause.GetType().Name}'")
        };

        // add pattern to registry
        registry.Add(entity);

        if (entity is PatternSequence group)
        {
            // add pattern children to graph & tree
            foreach (var child in group.Items)
            {
                registry.Tree.AddEdge(child.Key, entity.Key);
            }
        }

        return entity;
    }
}
