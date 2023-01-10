using MetaParser.Exceptions;
using MetaParser.Parsing;
using MetaParser.Rules.Text;
using MetaParser.Tokens;
using MetaParser.Tokens.Text;

namespace MetaParser.RuleSets.Text
{
    public sealed partial class StringRule : TextTokenRule
    {
        public override bool TryConsume(ITokenizer<char> Tokenizer, Token<TokenType<ETextToken>, char>? Previous, out Token<TokenType<ETextToken>, char>? Token)
        {
            bool isValid = Tokenizer.GetReader().AdvancePastAny(UnicodeCommon.SYMBOLS_QUOTATION_MARKS) > 0;
            if (!isValid)
            {
                Token = null;
                return false;
            }

            var rd = Tokenizer.GetReader();
            if (!rd.TryRead(out char closingChar))
            {
                throw new SyntaxException($"Exception while parsing @{Tokenizer}");
            }

            // TODO: Standards compliant character escape sequences
            //substream.Scan(UnicodeCommon.CHAR_REVERSE_SOLIDUS, )
            //char[] delims = new[] { closingChar, UnicodeCommon.CHAR_REVERSE_SOLIDUS };
            //LinkedList<ReadOnlySequenceSegment<char>> nodes = new();
            //if(rd.TryAdvanceToAny(delims, false))
            //{

            //}

            // Consume until we hit the next instance of the same character we consumed at the start of the string
            if (!rd.TryAdvanceTo(closingChar))
            {// We couldnt find a matching string quote char, this is a bad bad string...
                rd.AdvanceToEnd();
                Token = new TextToken(ETextToken.Bad_String, Tokenizer.Consume(ref rd));
            }
            else
            {
                Token = new TextToken(ETextToken.String, Tokenizer.Consume(ref rd));
            }

            return true;
        }
    }
}
