//HintName: MetaParser.MetaParser.complex_tokens.tokens.compound.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static bool TryProcessCompound(global::System.ReadOnlySpan<byte> stream, out byte id, out int length)
        {
            switch (stream)
            {
                case [ TokenId.Char_Solidus, TokenId.Char_Asterisk, ..]:
                {
                    id = TokenId.Comment;
                    return consume_pattern_27(stream, out length);
                }
                case [ TokenId.Identifier, ..]:
                {
                    id = TokenId.Identifier;
                    length = 1;
                    return true;
                }
                case [ TokenId.Keyword_Var, ..]:
                {
                    id = TokenId.Typename;
                    length = 1;
                    return true;
                }
                case [ TokenId.Keyword_Byte, ..]:
                {
                    id = TokenId.Typename;
                    length = 1;
                    return true;
                }
                case [ TokenId.Keyword_Short, ..]:
                {
                    id = TokenId.Typename;
                    length = 1;
                    return true;
                }
                case [ TokenId.Keyword_Int, ..]:
                {
                    id = TokenId.Typename;
                    length = 1;
                    return true;
                }
                case [ TokenId.Keyword_Float, ..]:
                {
                    id = TokenId.Typename;
                    length = 1;
                    return true;
                }
                case [ TokenId.Char_Open_Bracket, ..]:
                {
                    id = TokenId.Codeblock;
                    return consume_pattern_28(stream, out length);
                }
            }
            id = default;
            length = default;
            return false;
            
            bool consume_pattern_27(global::System.ReadOnlySpan<byte> stream, out int length)
            {
                /*
                * TokenID: comment (#22)
                * ==[ CONSUMER_DATA ]==
                * PatternConsumer { TokenType = Compound, ConsumerType = Token, TokenIndex = 22, TokenName = comment, ConsumerIndex = 27, Start = PatternGroup { Length = 2, IsRawValues = True, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = AllOf, ConditionJoiner = , , MinLength = 2 }, Consume = , Stop = PatternGroup { Length = 2, IsRawValues = True, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = AllOf, ConditionJoiner = , , MinLength = 2 }, Escape = PatternGroup { Length = 1, IsRawValues = True, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = AllOf, ConditionJoiner = , , MinLength = 1 }, HasConstLenStart = True, HasConstLenConsume = True, HasConstLenStop = True, HasConstLenEscape = True, IsOpenEnded = True }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer = stream.Slice(2);
                while (buffer.Length > 0)
                {
                    if (buffer.StartsWith(stackalloc []{ TokenId.Char_Reverse_Solidus}))
                    {
                        /* look past the ESCAPE sequence */
                        var reader = buffer.Slice(1);
                        /* if the stop sequence immediately follows the ESCAPE sequence then they are consumed */
                        if (reader.StartsWith(stackalloc []{ TokenId.Char_Asterisk, TokenId.Char_Solidus}))
                        {
                            buffer = reader.Slice(2);
                            continue;
                        }
                    }
                    
                    if (buffer.StartsWith(stackalloc []{ TokenId.Char_Asterisk, TokenId.Char_Solidus}))
                    /* If we have a STOP sequence, check for it */
                    {
                        /* end */
                        break;
                    }
                    
                    /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
                    buffer = buffer.Slice(1);
                }
                
                if (buffer.StartsWith(stackalloc []{ TokenId.Char_Asterisk, TokenId.Char_Solidus}))
                {
                    length = 2 + (stream.Length - buffer.Length);
                    return true;
                }
                
                length = default;
                return false;
            }
            bool consume_pattern_28(global::System.ReadOnlySpan<byte> stream, out int length)
            {
                /*
                * TokenID: codeblock (#23)
                * ==[ CONSUMER_DATA ]==
                * PatternConsumer { TokenType = Compound, ConsumerType = Token, TokenIndex = 23, TokenName = codeblock, ConsumerIndex = 28, Start = PatternGroup { Length = 1, IsRawValues = True, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = AllOf, ConditionJoiner = , , MinLength = 1 }, Consume = , Stop = PatternGroup { Length = 1, IsRawValues = True, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = AllOf, ConditionJoiner = , , MinLength = 1 }, Escape = , HasConstLenStart = True, HasConstLenConsume = True, HasConstLenStop = True, HasConstLenEscape = True, IsOpenEnded = True }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer = stream.Slice(1);
                while (buffer.Length > 0)
                {
                    if (buffer.StartsWith(stackalloc []{ TokenId.Char_Close_Bracket}))
                    /* If we have a STOP sequence, check for it */
                    {
                        /* end */
                        break;
                    }
                    
                    /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
                    buffer = buffer.Slice(1);
                }
                
                if (buffer.StartsWith(stackalloc []{ TokenId.Char_Close_Bracket}))
                {
                    length = 1 + (stream.Length - buffer.Length);
                    return true;
                }
                
                length = default;
                return false;
            }
        }
    }
}
