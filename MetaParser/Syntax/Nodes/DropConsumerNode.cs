using JetBrains.Annotations;

using MetaParser.Visitors;

namespace MetaParser.Trees;

internal record DropConsumerNode : ConsumerNode
{
    public DropConsumerNode([NotNull] PatternNode start, PatternNode? consume, PatternNode? stop = null, PatternNode? escape = null) : base (start, consume, stop, escape) { }
}
