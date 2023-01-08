using MetaParser.Rules;
using MetaParser.Tokens;
using MetaParser.Tokens.Text;

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
            var rd = Tokenizer.GetReader();
            if (!ParsingCommon.Is_Number_Start(rd))
            {
                outToken = null;
                return false;
            }

            // First, determine if this is an integer or decimal
            ENumberKind Kind = Detect_Number_Kind(Tokenizer);
            if (Kind == ENumberKind.Integer)
            {
                outToken = ParsingCommon.TryParseInteger(ref rd, out var outInteger)
                    ? new IntegerToken(Tokenizer.Consume(ref rd), outInteger)
                    : new BadNumberToken(Tokenizer.Consume(ref rd));
            }
            else if (Kind == ENumberKind.Decimal)
            {
                outToken = ParsingCommon.TryParseFloatingPoint(ref rd, out var outDecimal)
                    ? new DecimalToken(Tokenizer.Consume(ref rd), outDecimal)
                    : new BadNumberToken(Tokenizer.Consume(ref rd));
            }
            else
            {
                rd.Advance(1);
                outToken = new IdentToken(Tokenizer.Consume(ref rd));
            }

            return true;
        }
    }
}
