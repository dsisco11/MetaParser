using System.Buffers;

namespace MetaParser.Parsing.Tokens
{
    public delegate IToken<Ty> TokenFactory<Ty>(ReadOnlySequence<Ty> seq);
}
