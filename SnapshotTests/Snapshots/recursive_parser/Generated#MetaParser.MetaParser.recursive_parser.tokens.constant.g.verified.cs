//HintName: MetaParser.MetaParser.recursive_parser.tokens.constant.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static bool TryProcessConstant(global::System.ReadOnlySpan<char> buffer0, out byte id, out int length)
        {
            // Linear consumers
            switch (buffer0)
            {
                case ['/', '*', ..]:
                {
                    id = TokenId.Comment;
                    return consume_pattern_13(buffer0, out length);
                }
                case [((>='a' and <='z') or (>='A' and <='Z')), ((>='a' and <='z') or (>='A' and <='Z') or (>='0' and <='9') or '-' or '_'), ..]:
                {
                    id = TokenId.Identifier;
                    return consume_pattern_12(buffer0, out length);
                }
                case ['/', ..]:
                {
                    id = TokenId.Solidus;
                    length = 1;
                    return true;
                }
                case [':', ..]:
                {
                    id = TokenId.Colon;
                    length = 1;
                    return true;
                }
                case [';', ..]:
                {
                    id = TokenId.Semicolon;
                    length = 1;
                    return true;
                }
                case ['{', ..]:
                {
                    id = TokenId.Open_Bracket;
                    length = 1;
                    return true;
                }
                case ['}', ..]:
                {
                    id = TokenId.Close_Bracket;
                    length = 1;
                    return true;
                }
                case ['[', ..]:
                {
                    id = TokenId.Open_Sqbracket;
                    length = 1;
                    return true;
                }
                case [']', ..]:
                {
                    id = TokenId.Close_Sqbracket;
                    length = 1;
                    return true;
                }
                case ['(', ..]:
                {
                    id = TokenId.Open_Parenthesis;
                    length = 1;
                    return true;
                }
                case [')', ..]:
                {
                    id = TokenId.Close_Parenthesis;
                    length = 1;
                    return true;
                }
                case [(' ' or '\t' or '\f'), ..]:
                {
                    id = TokenId.Whitespace;
                    return consume_pattern_10(buffer0, out length);
                }
                case [('\r' or '\n'), ..]:
                {
                    id = TokenId.Newline;
                    return consume_pattern_11(buffer0, out length);
                }
            }
            id = default;
            length = default;
            return false;
            
            bool consume_pattern_10(global::System.ReadOnlySpan<char> buffer0, out int length)
            {
                /*
                * TokenID: whitespace (#10)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_22 | Order: 35 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_22, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 1, MaxLogicalLength = 1, IsLogical = True, Condition = OneOf, ConditionJoiner =  or  }
                * CONSUME: PatternGroup { DependencyInfo = Pattern_22 | Order: 35 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_22, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 1, MaxLogicalLength = 1, IsLogical = True, Condition = OneOf, ConditionJoiner =  or  }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer1 = buffer0.Slice(1);
                
                while (buffer1.Length > 0)
                {
                    if (buffer1 is [ (' ' or '\t' or '\f'), ..])
                    /* If we have a set of valid CONSUME targets, then try and consume as many as possible (STOP sequence should be mutually exclusive with CONSUME sequence) */
                    {
                        buffer1 = buffer1.Slice(1);
                        /* consumer forces moving on to next loop */
                        continue;
                    }
                    
                    /* otherwise, default behaviour is to exit loop */
                    break;
                }
                
                length = buffer0.Length - buffer1.Length;
                return true;
            }
            bool consume_pattern_11(global::System.ReadOnlySpan<char> buffer0, out int length)
            {
                /*
                * TokenID: newline (#11)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_25 | Order: 36 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_25, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 1, MaxLogicalLength = 1, IsLogical = True, Condition = OneOf, ConditionJoiner =  or  }
                * CONSUME: PatternGroup { DependencyInfo = Pattern_25 | Order: 36 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_25, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 1, MaxLogicalLength = 1, IsLogical = True, Condition = OneOf, ConditionJoiner =  or  }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer1 = buffer0.Slice(1);
                
                while (buffer1.Length > 0)
                {
                    if (buffer1 is [ ('\r' or '\n'), ..])
                    /* If we have a set of valid CONSUME targets, then try and consume as many as possible (STOP sequence should be mutually exclusive with CONSUME sequence) */
                    {
                        buffer1 = buffer1.Slice(1);
                        /* consumer forces moving on to next loop */
                        continue;
                    }
                    
                    /* otherwise, default behaviour is to exit loop */
                    break;
                }
                
                length = buffer0.Length - buffer1.Length;
                return true;
            }
            bool consume_pattern_12(global::System.ReadOnlySpan<char> buffer0, out int length)
            {
                /*
                * TokenID: identifier (#12)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_36 | Order: 54 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], Key = Pattern_36, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsRawValues = False, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 3, MaxLogicalLength = 4, IsLogical = True, Condition = AllOf, ConditionJoiner = ,  }
                * CONSUME: PatternGroup { DependencyInfo = Pattern_35 | Order: 38 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_35, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 1, MaxLogicalLength = 2, IsLogical = True, Condition = OneOf, ConditionJoiner =  or  }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer1 = buffer0.Slice(3);
                
                while (buffer1.Length > 0)
                {
                    if (buffer1 is [ ((>='a' and <='z') or (>='A' and <='Z') or (>='0' and <='9') or '-' or '_'), ..])
                    /* If we have a set of valid CONSUME targets, then try and consume as many as possible (STOP sequence should be mutually exclusive with CONSUME sequence) */
                    {
                        buffer1 = buffer1.Slice(1);
                        /* consumer forces moving on to next loop */
                        continue;
                    }
                    
                    /* otherwise, default behaviour is to exit loop */
                    break;
                }
                
                length = buffer0.Length - buffer1.Length;
                return true;
            }
            bool consume_pattern_13(global::System.ReadOnlySpan<char> buffer0, out int length)
            {
                /*
                * TokenID: comment (#13)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_40 | Order: 55 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], Key = Pattern_40, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsRawValues = True, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 2, MaxLogicalLength = 2, IsLogical = False, Condition = AllOf, ConditionJoiner = ,  }
                * STOP: PatternGroup { DependencyInfo = Pattern_44 | Order: 56 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], Key = Pattern_44, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsRawValues = True, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 2, MaxLogicalLength = 2, IsLogical = False, Condition = AllOf, ConditionJoiner = ,  }
                * ESCAPE: PatternGroup { DependencyInfo = Pattern_46 | Order: 41 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_46, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = True, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 1, MaxLogicalLength = 1, IsLogical = False, Condition = AllOf, ConditionJoiner = ,  }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer1 = buffer0.Slice(2);
                while (buffer1.Length > 0)
                {
                    if (buffer1[0] == '\\')
                    {
                        /* look past the ESCAPE sequence */
                        var buffer2 = buffer1.Slice(1);
                        /* if the stop sequence immediately follows the ESCAPE sequence then they are consumed */
                        if (buffer2.StartsWith(stackalloc []{ '*', '/' }))
                        {
                            buffer1 = buffer2.Slice(2);
                            continue;
                        }
                    }
                    
                    if (buffer1.StartsWith(stackalloc []{ '*', '/' }))
                    /* If we have a STOP sequence, check for it */
                    {
                        /* end */
                        break;
                    }
                    
                    /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
                    buffer1 = buffer1.Slice(1);
                }
                
                if (buffer1.StartsWith(stackalloc []{ '*', '/' }))
                {
                    length = 2 + (buffer0.Length - buffer1.Length);
                    return true;
                }
                
                length = default;
                return false;
            }
        }
    }
}
