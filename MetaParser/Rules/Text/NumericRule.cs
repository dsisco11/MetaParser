using MetaParser.Rules;
using MetaParser.Tokens;
using MetaParser.Tokens.Text;

using System.Buffers;

namespace MetaParser.RuleSets.Text
{
    // TODO:
    /// <summary>
    /// Causes number-like (integer/decimal) sequences to be emitted as a single number-type token
    /// </summary>
    public sealed class NumericRule : ITokenRule<char>
    {
        private static ENumberKind Detect_Number_Kind(ITokenizer<char> Tokenizer)
        {
            var rd = Tokenizer.GetReader();
            rd.IsNext(UnicodeCommon.CHAR_PLUS_SIGN, advancePast: true);
            rd.IsNext(UnicodeCommon.CHAR_HYPHEN_MINUS, advancePast: true);

            rd.AdvancePastAny(UnicodeCommon.ASCII_DIGITS);

            return rd.IsNext(UnicodeCommon.CHAR_FULL_STOP)
                   || rd.IsNext(UnicodeCommon.CHAR_E_LOWER)
                   || rd.IsNext(UnicodeCommon.CHAR_E_UPPER)
                ? ENumberKind.Decimal
                : ENumberKind.Integer;
        }

        public bool TryConsume(ITokenizer<char> Tokenizer, IToken<char> Previous, out IToken<char>? outToken)
        {
            var tokenReader = Tokenizer.GetReader() as UnmanagedTokenReader<char>;
            SequenceReader<char> rd = tokenReader.GetReader();

            if (!ParsingCommon.Is_Number_Start(rd))
            {
                outToken = null;
                return false;
            }

            // First, determine if this is an integer or decimal
            ENumberKind Kind = Detect_Number_Kind(Tokenizer);
            if (Kind == ENumberKind.Integer)
            {
                bool success = ParsingCommon.TryParseInteger(ref rd, out var outInteger);
                tokenReader.Advance(rd.Consumed);
                Tokenizer.TryConsume(tokenReader, out var consumed);

                outToken = success
                    ? new IntegerToken(consumed, outInteger)
                    : new BadNumberToken(consumed);
            }
            else if (Kind == ENumberKind.Decimal)
            {
                var success = ParsingCommon.TryParseFloatingPoint(ref rd, out var outDecimal);
                tokenReader.Advance(rd.Consumed);
                Tokenizer.TryConsume(tokenReader, out var consumed);

                outToken = success
                    ? new DecimalToken(consumed, outDecimal)
                    : new BadNumberToken(consumed);
            }
            else
            {
                rd.Advance(1);
                tokenReader.Advance(rd.Consumed);
                Tokenizer.TryConsume(tokenReader, out var consumed);
                outToken = new IdentToken(consumed);
            }

            return true;
        }
    }
}
