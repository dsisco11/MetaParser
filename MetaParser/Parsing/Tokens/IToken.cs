using System.Buffers;

namespace MetaParser.Parsing.Tokens
{
    public interface IToken<T> : IEquatable<IToken<T>>
    {
        ReadOnlySequence<T> Value { get; }
    }
}