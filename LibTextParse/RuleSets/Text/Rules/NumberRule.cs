using MetaParser.Parsing.Definitions;
using MetaParser.Parsing.Tokens;
using MetaParser.RuleSets.Text.Tokens;

namespace MetaParser.RuleSets.Text.Rules
{
    // TODO:
    /// <summary>
    /// Causes number-like (integer/decimal) sequences to be emitted as a single number-type token
    /// </summary>
    public sealed class NumberRule : ITokenRule<char>
    {
        public bool Check(IReadOnlyTokenizer<char> Tokenizer, IToken<char> Previous)
        {
            return ParsingCommon.Is_Number_Start(Tokenizer.Get_Reader());
        }

        private ENumberKind Detect_Number_Kind(ITokenizer<char> Tokenizer)
        {
            var rd = Tokenizer.Get_Reader();
            rd.IsNext(UnicodeCommon.CHAR_PLUS_SIGN, advancePast: true);
            rd.IsNext(UnicodeCommon.CHAR_HYPHEN_MINUS, advancePast: true);

            rd.AdvancePastAny(UnicodeCommon.ASCII_DIGITS);

            if (rd.IsNext(UnicodeCommon.CHAR_FULL_STOP))
            {
                return ENumberKind.Decimal;
            }

            if (rd.IsNext(UnicodeCommon.CHAR_E_LOWER))
            {
                return ENumberKind.Decimal;
            }

            if (rd.IsNext(UnicodeCommon.CHAR_E_UPPER))
            {
                return ENumberKind.Decimal;
            }

            return ENumberKind.Integer;
        }

        public IToken<char>? Consume(ITokenizer<char> Tokenizer, IToken<char> Previous)
        {
            // First, determine if this is an integer or decimal
            ENumberKind Kind = Detect_Number_Kind(Tokenizer);
            var rd = Tokenizer.Get_Reader();
            if (Kind == ENumberKind.Integer)
            {
                if (ParsingCommon.Try_Parse_Integer(rd, out var outInteger))
                {
                    return new IntegerToken(Tokenizer.Consume(rd), outInteger);
                }

                return new BadNumberToken(Tokenizer.Consume(rd));
            }
            else if (Kind == ENumberKind.Decimal)
            {
                if (ParsingCommon.Try_Parse_FloatingPoint(rd, out var outDecimal))
                {
                    return new DecimalToken(Tokenizer.Consume(rd), outDecimal);
                }

                return new BadNumberToken(Tokenizer.Consume(rd));
            }

            rd.Advance(1);
            return new IdentToken(Tokenizer.Consume(rd));
        }
    }
}
