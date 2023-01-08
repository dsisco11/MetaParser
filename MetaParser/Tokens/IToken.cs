using System.Buffers;

namespace MetaParser.Tokens
{
    public interface IToken<T> : IEquatable<IToken<T>>
    {
        ReadOnlySequence<T> Value { get; }
    }
}