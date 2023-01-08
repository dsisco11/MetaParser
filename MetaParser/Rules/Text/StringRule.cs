﻿using MetaParser.Exceptions;
using MetaParser.Rules;
using MetaParser.Tokens;
using MetaParser.Tokens.Text;

namespace MetaParser.RuleSets.Text
{
    public sealed partial class StringRule : ITokenRule<char>
    {
        public bool Check(IReadOnlyTokenizer<char> Tokenizer, IToken<char> Previous)
        {
            return Tokenizer.GetReader().AdvancePastAny(UnicodeCommon.SYMBOLS_QUOTATION_MARKS) > 0;
        }

        public IToken<char>? Consume(ITokenizer<char> Tokenizer, IToken<char> Previous)
        {
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
                return new BadStringToken(Tokenizer.Consume(ref rd));
            }

            return new StringToken(Tokenizer.Consume(ref rd));
        }
    }
}