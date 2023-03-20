//HintName: MetaParser.MetaParser.parser.tokens.complex.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static ConsumerResult TryProcessComplex(global::System.ReadOnlySpan<byte> buffer0)
        {
            // Recursive tokens
            if (starts_codeblock_token(buffer0))
            {
                return consume_pattern_32(buffer0);
            }
            
            id = default;
            length = default;
            return false;
            
            ConsumerResult consume_pattern_32(global::System.ReadOnlySpan<byte> buffer0)
            {
                /*
                * TokenID: codeblock (#25)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_130 | Order: 178 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], Key = Pattern_130, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Condition = AllOf, ConditionJoiner = ,  }
                * CONSUME: PatternGroup { DependencyInfo = Pattern_132 | Order: 204 | IsRecursive: True | TreeDepth: [0, 0] | NodeDepth: [0, 0], Key = Pattern_132, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsInlinable = False, IsConstantLength = True, IsSequence = True, MinConditions = 1, MaxConditions = 1, IsConditional = True, IsDeterministic = False, Condition = OneOf, ConditionJoiner =  or  }
                * STOP: PatternGroup { DependencyInfo = Pattern_134 | Order: 179 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], Key = Pattern_134, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Condition = AllOf, ConditionJoiner = ,  }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer1 = buffer0.Slice(1);
                while (buffer1.Length > 0)
                {
                    if (buffer1[0] == TokenId.Char_Close_Bracket)
                    /* If we have a STOP sequence, check for it */
                    {
                        /* end */
                        break;
                    }
                    
                    
                    while (buffer1.Length > 0)
                    {
                        if (starts_declaration_token(buffer1))
                        /* If we have a set of valid CONSUME targets, then try and consume as many as possible (STOP sequence should be mutually exclusive with CONSUME sequence) */
                        {
                            buffer1 = buffer1.Slice(1);
                            /* consumer forces moving on to next loop */
                            continue;
                        }
                        
                        /* otherwise, default behaviour is to exit loop */
                        break;
                    }
                    
                }
                
                if (buffer1[0] == TokenId.Char_Close_Bracket)
                {
                    return new (TokenId.Codeblock, 1 + (buffer0.Length - buffer1.Length));
                }
                
                return new (default, default);
            }
        }
    }
}
