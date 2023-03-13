//HintName: MetaParser.MetaParser.recursive_parser.tokens.complex.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static bool TryProcessComplex(global::System.ReadOnlySpan<byte> input, out byte id, out int length)
        {
            // Recursive consumers
            if (is_codeblock_token_start(input))
            {
                id = TokenId.Codeblock;
                return consume_pattern_16(input, out length);
            }
            
            id = default;
            length = default;
            return false;
            
            bool consume_pattern_16(global::System.ReadOnlySpan<byte> input, out int length)
            {
                /*
                * TokenID: codeblock (#15)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_58 | Order[77] | IsRecursive (False) | Depth[N: [0, 2], D: [0, 0], P: [1, 1], C: [0, 0], T: [0, 0]], NodeID = Pattern_58, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = True, IsInline = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }
                * CONSUME: PatternGroup { DependencyInfo = Pattern_60 | Order[88] | IsRecursive (True) | Depth[N: [0, 0], D: [0, 0], P: [2147483647, -2147483648], C: [0, 0], T: [0, 0]], NodeID = Pattern_60, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsInline = True, IsConstantLength = True, HasChildren = True, Condition = OneOf, ConditionJoiner =  or , MinLength = 1 }
                * STOP: PatternGroup { DependencyInfo = Pattern_62 | Order[78] | IsRecursive (False) | Depth[N: [0, 2], D: [0, 0], P: [1, 1], C: [0, 0], T: [0, 0]], NodeID = Pattern_62, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = True, IsInline = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }
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
