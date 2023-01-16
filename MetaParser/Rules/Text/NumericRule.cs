using MetaParser.Parsing;
using MetaParser.Rules;

namespace MetaParser.RuleSets.Text
{
    enum ENumberKind { Decimal, Integer }
    /// <summary>
    /// Causes number-like (integer/decimal) sequences to be emitted as a single number-type token
    /// </summary>
    public sealed class NumericRule : ITokenRule<byte, char>
    {
        public RuleSpecificty Specificity => new RuleSpecificty(true, false);

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

        public bool TryConsume(ITokenizer<char> Tokenizer, byte? Previous, out byte TokenType, out long TokenLength)
        {
            var rd = Tokenizer.GetReader();
            if (!ParsingCommon.Is_Number_Start(rd))
            {
                TokenType = default;
                TokenLength = default;
                return false;
            }

            // First, determine if this is an integer or decimal
            ENumberKind Kind = Detect_Number_Kind(Tokenizer);
            if (Kind == ENumberKind.Integer)
            {
                //    Token = ParsingCommon.TryParseInteger(ref rd, out var outInteger)
                //        ? new IntegerToken(Tokenizer.Consume(ref rd), outInteger)
                //        : new TextToken(ETextToken.Bad_Number, Tokenizer.Consume(ref rd));

                TokenType = ParsingCommon.TryParseInteger(ref rd, out var outInteger)
                    ? ETextToken.Number
                    : ETextToken.Bad_Number;
                TokenLength = rd.Consumed;
            }
            else if (Kind == ENumberKind.Decimal)
            {
                TokenType = ParsingCommon.TryParseFloatingPoint(ref rd, out var outDecimal)
                    ? ETextToken.Number
                    : ETextToken.Bad_Number;
                TokenLength = rd.Consumed;
            }
            else
            {
                TokenType = ETextToken.Ident;
                TokenLength = 1;
            }

            return true;
        }
    }
}
