using JetBrains.Annotations;

using MetaParser.Parsing.Constructs.Tokens;
using MetaParser.Visitors;

namespace MetaParser.Trees;

internal record TokenConsumerNode : ConsumerNode
{
    public TokenInfo Token { get; protected set; }

    public TokenConsumerNode(TokenInfo token, [NotNull] PatternNode start, PatternNode? consume, PatternNode? stop = null, PatternNode? escape = null) : base (start, consume, stop, escape)
    {
        Token = token;
    }
}