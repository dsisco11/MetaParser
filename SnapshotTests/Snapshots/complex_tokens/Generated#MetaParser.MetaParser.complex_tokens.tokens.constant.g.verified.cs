//HintName: MetaParser.MetaParser.complex_tokens.tokens.constant.g.cs
namespace Foo.Bar.Tokens;
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
                return consume_pattern_8(stream, out length);
            }
            case [ (>='0' and <='9'), ..]:
            {
                id = TokenId.Digits;
                return consume_pattern_9(stream, out length);
            }
            case [ ((>='a' and <='z') or (>='A' and <='Z')), ..]:
            {
                id = TokenId.Letters;
                return consume_pattern_10(stream, out length);
            }
            case [ '\n', ..]:
            {
                id = TokenId.Newline;
                return consume_pattern_11(stream, out length);
            }
        }
        id = default;
        length = default;
        return false;
        
        bool consume_pattern_8(global::System.ReadOnlySpan<char> stream, out int length)
        {
            /*
            * TokenID: whitespace (#8)
            * ==[ CONSUMER_DATA ]==
            * PatternConsumer { Type = Constant, TokenIndex = 8, TokenName = whitespace, ConsumerIndex = 8, Start = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Consume = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Stop = , Escape = , IsOpenEnded = True }
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
        bool consume_pattern_9(global::System.ReadOnlySpan<char> stream, out int length)
        {
            /*
            * TokenID: digits (#9)
            * ==[ CONSUMER_DATA ]==
            * PatternConsumer { Type = Constant, TokenIndex = 9, TokenName = digits, ConsumerIndex = 9, Start = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Consume = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Stop = , Escape = , IsOpenEnded = True }
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
        bool consume_pattern_10(global::System.ReadOnlySpan<char> stream, out int length)
        {
            /*
            * TokenID: letters (#10)
            * ==[ CONSUMER_DATA ]==
            * PatternConsumer { Type = Constant, TokenIndex = 10, TokenName = letters, ConsumerIndex = 10, Start = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Consume = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Stop = , Escape = , IsOpenEnded = True }
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
        bool consume_pattern_11(global::System.ReadOnlySpan<char> stream, out int length)
        {
            /*
            * TokenID: newline (#11)
            * ==[ CONSUMER_DATA ]==
            * PatternConsumer { Type = Constant, TokenIndex = 11, TokenName = newline, ConsumerIndex = 11, Start = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Consume = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Stop = , Escape = , IsOpenEnded = True }
            */
            /* Consume the START sequence which got us here in the first place, we already know its part of the token */
            var buffer = stream.Slice(1);
            while (buffer.Length > 0)
            {
                if (buffer is [ '\n', ..])
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
