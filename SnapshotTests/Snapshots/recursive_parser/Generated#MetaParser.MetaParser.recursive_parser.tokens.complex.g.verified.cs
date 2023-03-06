//HintName: MetaParser.MetaParser.recursive_parser.tokens.complex.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static bool TryProcessComplex(global::System.ReadOnlySpan<byte> input, out byte id, out int length)
        {
            switch (input)
            {
                case [ TokenId.Open_Bracket, ..]:
                {
                    id = TokenId.Codeblock;
                    return consume_pattern_15(input, out length);
                }
            }
            id = default;
            length = default;
            return false;
            
            bool consume_pattern_15(global::System.ReadOnlySpan<byte> input, out int length)
            {
                /*
                * TokenID: codeblock (#14)
                * ==[ CONSUMER_DATA ]==
                * ConsumerInfo { Stage = 2, Token = TokenInfo { Index = 14, Name = codeblock, Consumers = System.Collections.Generic.List`1[MetaParser.Consumers.ConsumerInfo] }, Index = 15, Type = Token, Start = PatternGroup { Length = 1, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }, Consume = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, HasChildren = True, Condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Stop = PatternGroup { Length = 1, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }, Escape = , IsOpen = False, IsDynamic = True, IsConstant = False }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer = input.Slice(1);
                while (buffer.Length > 0)
                {
                    if (buffer.StartsWith(stackalloc []{ TokenId.Close_Bracket}))
                    /* If we have a STOP sequence, check for it */
                    {
                        /* end */
                        break;
                    }
                    
                    
                    while (buffer.Length > 0)
                    {
                        if (buffer is [ TokenId.Declaration, ..])
                        /* If we have a set of valid CONSUME targets, then try and consume as many as possible (STOP sequence should be mutually exclusive with CONSUME sequence) */
                        {
                            buffer = buffer.Slice(1);
                            /* consumer forces moving on to next loop */
                            continue;
                        }
                        
                        /* otherwise, default behaviour is to exit loop */
                        break;
                    }
                    
                }
                
                if (buffer.StartsWith(stackalloc []{ TokenId.Close_Bracket}))
                {
                    length = 1 + (input.Length - buffer.Length);
                    return true;
                }
                
                length = default;
                return false;
            }
        }
    }
}
