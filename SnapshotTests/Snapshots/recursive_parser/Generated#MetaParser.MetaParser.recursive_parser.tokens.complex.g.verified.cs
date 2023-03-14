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
                * START: PatternGroup { DependencyInfo = Pattern_58 | Order: 80 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], Key = Pattern_58, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = True, IsInlinable = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }
                * CONSUME: PatternGroup { DependencyInfo = Pattern_60 | Order: 88 | IsRecursive: True | TreeDepth: [0, 0] | NodeDepth: [0, 0], Key = Pattern_60, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsInlinable = True, IsConstantLength = True, HasChildren = True, Condition = OneOf, ConditionJoiner =  or , MinLength = 1 }
                * STOP: PatternGroup { DependencyInfo = Pattern_62 | Order: 81 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], Key = Pattern_62, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = True, IsInlinable = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }
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
