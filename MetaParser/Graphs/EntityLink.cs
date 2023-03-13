using MetaParser.Graphs;

namespace MetaParser.Parsing.Constructs
{
    internal record struct EntityLink(EntityKey Source, EntityKey Target);
}
