//HintName: MetaParser.MetaParser.complex_tokens.tokens.constant.g.cs
namespace UnitTestParser;
{
    public sealed partial class Parser
    {
        private static bool TryProcessConstant(global::System.ReadOnlySpan<char> stream, out byte id, out int length)
        {
            switch (stream)
            {
                case [ 'v', 'a', 'r', ..]:
                {
                    id = TokenId.Keyword_Var;
                    length = 3;
                    return true;
                }
                case [ 'f', 'u', 'n', 'c', 't', 'i', 'o', 'n', ..]:
                {
                    id = TokenId.Keyword_Function;
                    length = 8;
                    return true;
                }
                case [ 'b', 'y', 't', 'e', ..]:
                {
                    id = TokenId.Keyword_Byte;
                    length = 4;
                    return true;
                }
                case [ 's', 'h', 'o', 'r', 't', ..]:
                {
                    id = TokenId.Keyword_Short;
                    length = 5;
                    return true;
                }
                case [ 'i', 'n', 't', ..]:
                {
                    id = TokenId.Keyword_Int;
                    length = 3;
                    return true;
                }
                case [ 'f', 'l', 'o', 'a', 't', ..]:
                {
                    id = TokenId.Keyword_Float;
                    length = 5;
                    return true;
                }
                case [ '{', ..]:
                {
                    id = TokenId.Char_Open_Bracket;
                    length = 1;
                    return true;
                }
                case [ '}', ..]:
                {
                    id = TokenId.Char_Close_Bracket;
                    length = 1;
                    return true;
                }
                case [ '[', ..]:
                {
                    id = TokenId.Char_Open_Sqbracket;
                    length = 1;
                    return true;
                }
                case [ ']', ..]:
                {
                    id = TokenId.Char_Close_Sqbracket;
                    length = 1;
                    return true;
                }
                case [ '(', ..]:
                {
                    id = TokenId.Char_Open_Parenthesis;
                    length = 1;
                    return true;
                }
                case [ ')', ..]:
                {
                    id = TokenId.Char_Close_Parenthesis;
                    length = 1;
                    return true;
                }
                case [ '*', ..]:
                {
                    id = TokenId.Char_Asterisk;
                    length = 1;
                    return true;
                }
                case [ '/', ..]:
                {
                    id = TokenId.Char_Solidus;
                    length = 1;
                    return true;
                }
                case [ '\\', ..]:
                {
                    id = TokenId.Char_Reverse_Solidus;
                    length = 1;
                    return true;
                }
                case [ (' ' or '\t' or '\f'), ..]:
                {
                    id = TokenId.Whitespace;
                    return consume_pattern_16(stream, out length);
                }
                case [ (>='0' and <='9'), ..]:
                {
                    id = TokenId.Digits;
                    return consume_pattern_17(stream, out length);
                }
                case [ ((>='a' and <='z') or (>='A' and <='Z')), ..]:
                {
                    id = TokenId.Letters;
                    return consume_pattern_18(stream, out length);
                }
                case [ ('\r' or '\n'), ..]:
                {
                    id = TokenId.Newline;
                    return consume_pattern_19(stream, out length);
                }
                case [ ((>='a' and <='z') or (>='A' and <='Z')), ..]:
                {
                    id = TokenId.Identifier;
                    return consume_pattern_20(stream, out length);
                }
            }
            id = default;
            length = default;
            return false;
            
            bool consume_pattern_16(global::System.ReadOnlySpan<char> stream, out int length)
            {
                /*
                * TokenID: whitespace (#16)
                * ==[ CONSUMER_DATA ]==
                * PatternConsumer { TokenType = Constant, ConsumerType = Data, TokenIndex = 16, TokenName = whitespace, ConsumerIndex = 16, Start = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Consume = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Stop = , Escape = , HasConstLenStart = True, HasConstLenConsume = True, HasConstLenStop = True, HasConstLenEscape = True, IsOpenEnded = True }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer = stream.Slice(1);
                
                while (buffer.Length > 0)
                {
                    if (buffer is [ (' ' or '\t' or '\f'), ..])
                    /* If we have a set of valid CONSUME targets, then try and consume as many as possible (STOP sequence should be mutually exclusive with CONSUME sequence) */
                    {
                        buffer = buffer.Slice(1);
                        /* consumer forces moving on to next loop */
                        continue;
                    }
                    
                    /* otherwise, default behaviour is to exit loop */
                    break;
                }
                
                length = stream.Length - buffer.Length;
                return true;
            }
            bool consume_pattern_17(global::System.ReadOnlySpan<char> stream, out int length)
            {
                /*
                * TokenID: digits (#17)
                * ==[ CONSUMER_DATA ]==
                * PatternConsumer { TokenType = Constant, ConsumerType = Data, TokenIndex = 17, TokenName = digits, ConsumerIndex = 17, Start = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Consume = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Stop = , Escape = , HasConstLenStart = True, HasConstLenConsume = True, HasConstLenStop = True, HasConstLenEscape = True, IsOpenEnded = True }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer = stream.Slice(1);
                
                while (buffer.Length > 0)
                {
                    if (buffer is [ (>='0' and <='9'), ..])
                    /* If we have a set of valid CONSUME targets, then try and consume as many as possible (STOP sequence should be mutually exclusive with CONSUME sequence) */
                    {
                        buffer = buffer.Slice(1);
                        /* consumer forces moving on to next loop */
                        continue;
                    }
                    
                    /* otherwise, default behaviour is to exit loop */
                    break;
                }
                
                length = stream.Length - buffer.Length;
                return true;
            }
            bool consume_pattern_18(global::System.ReadOnlySpan<char> stream, out int length)
            {
                /*
                * TokenID: letters (#18)
                * ==[ CONSUMER_DATA ]==
                * PatternConsumer { TokenType = Constant, ConsumerType = Data, TokenIndex = 18, TokenName = letters, ConsumerIndex = 18, Start = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Consume = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Stop = , Escape = , HasConstLenStart = True, HasConstLenConsume = True, HasConstLenStop = True, HasConstLenEscape = True, IsOpenEnded = True }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer = stream.Slice(1);
                
                while (buffer.Length > 0)
                {
                    if (buffer is [ ((>='a' and <='z') or (>='A' and <='Z')), ..])
                    /* If we have a set of valid CONSUME targets, then try and consume as many as possible (STOP sequence should be mutually exclusive with CONSUME sequence) */
                    {
                        buffer = buffer.Slice(1);
                        /* consumer forces moving on to next loop */
                        continue;
                    }
                    
                    /* otherwise, default behaviour is to exit loop */
                    break;
                }
                
                length = stream.Length - buffer.Length;
                return true;
            }
            bool consume_pattern_19(global::System.ReadOnlySpan<char> stream, out int length)
            {
                /*
                * TokenID: newline (#19)
                * ==[ CONSUMER_DATA ]==
                * PatternConsumer { TokenType = Constant, ConsumerType = Data, TokenIndex = 19, TokenName = newline, ConsumerIndex = 19, Start = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Consume = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Stop = , Escape = , HasConstLenStart = True, HasConstLenConsume = True, HasConstLenStop = True, HasConstLenEscape = True, IsOpenEnded = True }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer = stream.Slice(1);
                
                while (buffer.Length > 0)
                {
                    if (buffer is [ ('\r' or '\n'), ..])
                    /* If we have a set of valid CONSUME targets, then try and consume as many as possible (STOP sequence should be mutually exclusive with CONSUME sequence) */
                    {
                        buffer = buffer.Slice(1);
                        /* consumer forces moving on to next loop */
                        continue;
                    }
                    
                    /* otherwise, default behaviour is to exit loop */
                    break;
                }
                
                length = stream.Length - buffer.Length;
                return true;
            }
            bool consume_pattern_20(global::System.ReadOnlySpan<char> stream, out int length)
            {
                /*
                * TokenID: identifier (#20)
                * ==[ CONSUMER_DATA ]==
                * PatternConsumer { TokenType = Constant, ConsumerType = Data, TokenIndex = 20, TokenName = identifier, ConsumerIndex = 20, Start = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = AllOf, ConditionJoiner = , , MinLength = 1 }, Consume = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Stop = , Escape = , HasConstLenStart = True, HasConstLenConsume = True, HasConstLenStop = True, HasConstLenEscape = True, IsOpenEnded = True }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer = stream.Slice(1);
                
                while (buffer.Length > 0)
                {
                    if (buffer is [ (((>='a' and <='z') or (>='A' and <='Z')) or (>='0' and <='9') or '-' or '_'), ..])
                    /* If we have a set of valid CONSUME targets, then try and consume as many as possible (STOP sequence should be mutually exclusive with CONSUME sequence) */
                    {
                        buffer = buffer.Slice(1);
                        /* consumer forces moving on to next loop */
                        continue;
                    }
                    
                    /* otherwise, default behaviour is to exit loop */
                    break;
                }
                
                length = stream.Length - buffer.Length;
                return true;
            }
        }
    }
}
