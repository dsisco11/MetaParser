using MetaParser.Parsing;

using System.Buffers;

namespace MetaParser.Tokens.Text
{
    public enum ENumberKind { Integer, Decimal }
    public abstract record NumberToken : TextToken
    {
        public abstract ENumberKind Kind { get; }
        public NumberToken(ReadOnlySequence<char> value) : base(ETextToken.Number, value)
        {
        }
    }

    public sealed record IntegerToken : NumberToken
    {
        public override ENumberKind Kind => ENumberKind.Integer;
        public long Number { get; init; }

        public IntegerToken(ReadOnlySequence<char> value, long number) : base(value)
        {
            Number = number;
        }
    }

    public sealed record DecimalToken : NumberToken
    {
        public override ENumberKind Kind => ENumberKind.Decimal;
        public double Number { get; init; }
        public DecimalToken(ReadOnlySequence<char> value, double number) : base(value)
        {
            Number = number;
        }
    }
}
