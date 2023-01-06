using System.Buffers;

namespace MetaParser.Parsing.Tokens
{
    public interface IToken<Ty> : IEquatable<IToken<Ty>>
    {
        ReadOnlySequence<Ty> Value { get; }
    }
}