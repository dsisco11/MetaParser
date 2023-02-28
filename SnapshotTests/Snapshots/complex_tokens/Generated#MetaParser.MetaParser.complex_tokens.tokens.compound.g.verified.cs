//HintName: MetaParser.MetaParser.complex_tokens.tokens.compound.g.cs
namespace Foo.Bar.Tokens;
public sealed partial class Parser
{
    private static bool TryProcessCompound(global::System.ReadOnlySpan<byte> stream, out byte id, out int length)
    {
        switch (stream)
        {
            case [ TokenId.Letters, ..]:
            {
                id = TokenId.Identifier;
                return consume_pattern_12(stream, out length);
            }
            case [ TokenId.Char_Solidus, TokenId.Char_Asterisk, ..]:
            {
                id = TokenId.Comment;
                return consume_pattern_13(stream, out length);
            }
        }
        id = default;
        length = default;
        return false;
        
        bool consume_pattern_12(global::System.ReadOnlySpan<byte> stream, out int length)
        {
            /*
            * TokenID: identifier (#12)
            * ==[ CONSUMER_DATA ]==
            * PatternConsumer { Type = Compound, TokenIndex = 12, TokenName = identifier, ConsumerIndex = 12, Start = PatternGroup { Length = 1, IsRawValues = True, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = AllOf, ConditionJoiner = , , MinLength = 1 }, Consume = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Stop = , Escape = , IsOpenEnded = True }
            */
            /* Consume the START sequence which got us here in the first place, we already know its part of the token */
            var buffer = stream.Slice(1);
            while (buffer.Length > 0)
            {
                if (buffer is [ (TokenId.Letters or TokenId.Digits), ..])
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
        bool consume_pattern_13(global::System.ReadOnlySpan<byte> stream, out int length)
        {
            /*
            * TokenID: comment (#13)
            * ==[ CONSUMER_DATA ]==
            * PatternConsumer { Type = Compound, TokenIndex = 13, TokenName = comment, ConsumerIndex = 13, Start = PatternGroup { Length = 2, IsRawValues = True, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = AllOf, ConditionJoiner = , , MinLength = 2 }, Consume = , Stop = PatternGroup { Length = 2, IsRawValues = True, IsConstantLength = True, items = MetaParser.Patternization.Pattern[], condition = AllOf, ConditionJoiner = , , MinLength = 2 }, Escape = , IsOpenEnded = True }
            */
            /* Consume the START sequence which got us here in the first place, we already know its part of the token */
            var buffer = stream.Slice(2);while (buffer.Length > 0)
            {
                if (buffer.StartsWith(stackalloc []{ TokenId.Char_Asterisk, TokenId.Char_Solidus}))
                /* If we have a STOP sequence, check for it */
                {
                    /* end */
                    break;
                }
                
                /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
                buffer = buffer.Slice(1);
            }
            
            if (buffer .StartsWith(stackalloc []{ TokenId.Char_Asterisk, TokenId.Char_Solidus}))
            {
                length = 2 + (stream.Length - buffer.Length);
                return true;
            }
            
            length = default;
            return false;
        }
    }
}
