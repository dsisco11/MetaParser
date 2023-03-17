//HintName: MetaParser.MetaParser.parser.tokens.complex.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static bool TryProcessComplex(global::System.ReadOnlySpan<byte> buffer0, out byte id, out int length)
        {
            // Recursive consumers
            if (is_codeblock_token_start(buffer0))
            {
                id = TokenId.Codeblock;
                return consume_pattern_33(buffer0, out length);
            }
            
            id = default;
            length = default;
            return false;
            
            bool consume_pattern_33(global::System.ReadOnlySpan<byte> buffer0, out int length)
            {
                /*
                * TokenID: codeblock (#26)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_133 | Order: 177 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], Key = Pattern_133, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = True, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 1, MaxLogicalLength = 1, IsLogical = False, Condition = AllOf, ConditionJoiner = ,  }
                * CONSUME: PatternGroup { DependencyInfo = Pattern_135 | Order: 209 | IsRecursive: True | TreeDepth: [0, 0] | NodeDepth: [0, 0], Key = Pattern_135, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsInlinable = False, IsConstantLength = True, HasChildren = True, MinLogicalLength = 1, MaxLogicalLength = 1, IsLogical = False, Condition = OneOf, ConditionJoiner =  or  }
                * STOP: PatternGroup { DependencyInfo = Pattern_137 | Order: 178 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], Key = Pattern_137, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = True, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 1, MaxLogicalLength = 1, IsLogical = False, Condition = AllOf, ConditionJoiner = ,  }
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
                        if (is_declaration_token_start(buffer1))
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
                    length = 1 + (buffer0.Length - buffer1.Length);
                    return true;
                }
                
                length = default;
                return false;
            }
        }
    }
}
