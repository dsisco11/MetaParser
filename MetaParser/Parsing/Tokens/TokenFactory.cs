using System.Buffers;

namespace MetaParser.Parsing.Tokens
{
    public delegate IToken<T> TokenFactory<T>(ReadOnlySequence<T> seq);
}
