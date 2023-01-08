using System.Buffers;

namespace MetaParser.Tokens
{
    public delegate IToken<T> TokenFactory<T>(ReadOnlySequence<T> seq);
}
