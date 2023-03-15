//HintName: MetaParser.MetaParser.linear_parser.tokens.constant.g.cs
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
                    return consume_pattern_26(buffer0, out length);
                }
                case ['f', 'u', 'n', 'c', 't', 'i', 'o', 'n', ..]:
                {
                    id = TokenId.Keyword_Function;
                    length = 8;
                    return true;
                }
                case ['s', 'h', 'o', 'r', 't', ..]:
                {
                    id = TokenId.Keyword_Short;
                    length = 5;
                    return true;
                }
                case ['f', 'l', 'o', 'a', 't', ..]:
                {
                    id = TokenId.Keyword_Float;
                    length = 5;
                    return true;
                }
                case ['b', 'y', 't', 'e', ..]:
                {
                    id = TokenId.Keyword_Byte;
                    length = 4;
                    return true;
                }
                case [((>='a' and <='z') or (>='A' and <='Z')), ((>='a' and <='z') or (>='A' and <='Z') or (>='0' and <='9') or '-' or '_'), ..]:
                {
                    id = TokenId.Identifier;
                    return consume_pattern_20(buffer0, out length);
                }
                case ['v', 'a', 'r', ..]:
                {
                    id = TokenId.Keyword_Var;
                    length = 3;
                    return true;
                }
                case ['i', 'n', 't', ..]:
                {
                    id = TokenId.Keyword_Int;
                    length = 3;
                    return true;
                }
                case [(>='0' and <='9'), ..]:
                {
                    id = TokenId.Digits;
                    return consume_pattern_17(buffer0, out length);
                }
                case [((>='a' and <='z') or (>='A' and <='Z')), ..]:
                {
                    id = TokenId.Letters;
                    return consume_pattern_18(buffer0, out length);
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
                case ['*', ..]:
                {
                    id = TokenId.Asterisk;
                    length = 1;
                    return true;
                }
                case ['/', ..]:
                {
                    id = TokenId.Solidus;
                    length = 1;
                    return true;
                }
                case ['\\', ..]:
                {
                    id = TokenId.Reverse_Solidus;
                    length = 1;
                    return true;
                }
                case [(' ' or '\t' or '\f'), ..]:
                {
                    id = TokenId.Whitespace;
                    return consume_pattern_16(buffer0, out length);
                }
                case [('\r' or '\n'), ..]:
                {
                    id = TokenId.Newline;
                    return consume_pattern_19(buffer0, out length);
                }
            }
            id = default;
            length = default;
            return false;
            
            bool consume_pattern_16(global::System.ReadOnlySpan<char> buffer0, out int length)
            {
                /*
                * TokenID: whitespace (#16)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_62 | Order: 72 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_62, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 1, MaxLogicalLength = 1, IsLogical = True, Condition = OneOf, ConditionJoiner =  or  }
                * CONSUME: PatternGroup { DependencyInfo = Pattern_62 | Order: 72 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_62, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 1, MaxLogicalLength = 1, IsLogical = True, Condition = OneOf, ConditionJoiner =  or  }
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
            bool consume_pattern_17(global::System.ReadOnlySpan<char> buffer0, out int length)
            {
                /*
                * TokenID: digits (#17)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_64 | Order: 73 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_64, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 2, MaxLogicalLength = 2, IsLogical = True, Condition = OneOf, ConditionJoiner =  or  }
                * CONSUME: PatternGroup { DependencyInfo = Pattern_64 | Order: 73 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_64, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 2, MaxLogicalLength = 2, IsLogical = True, Condition = OneOf, ConditionJoiner =  or  }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer1 = buffer0.Slice(2);
                
                while (buffer1.Length > 0)
                {
                    if (buffer1 is [ (>='0' and <='9'), ..])
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
            bool consume_pattern_18(global::System.ReadOnlySpan<char> buffer0, out int length)
            {
                /*
                * TokenID: letters (#18)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_67 | Order: 74 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_67, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 2, MaxLogicalLength = 2, IsLogical = True, Condition = OneOf, ConditionJoiner =  or  }
                * CONSUME: PatternGroup { DependencyInfo = Pattern_67 | Order: 74 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_67, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 2, MaxLogicalLength = 2, IsLogical = True, Condition = OneOf, ConditionJoiner =  or  }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer1 = buffer0.Slice(2);
                
                while (buffer1.Length > 0)
                {
                    if (buffer1 is [ ((>='a' and <='z') or (>='A' and <='Z')), ..])
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
            bool consume_pattern_19(global::System.ReadOnlySpan<char> buffer0, out int length)
            {
                /*
                * TokenID: newline (#19)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_70 | Order: 75 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_70, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 1, MaxLogicalLength = 1, IsLogical = True, Condition = OneOf, ConditionJoiner =  or  }
                * CONSUME: PatternGroup { DependencyInfo = Pattern_70 | Order: 75 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_70, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 1, MaxLogicalLength = 1, IsLogical = True, Condition = OneOf, ConditionJoiner =  or  }
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
            bool consume_pattern_20(global::System.ReadOnlySpan<char> buffer0, out int length)
            {
                /*
                * TokenID: identifier (#20)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_81 | Order: 101 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], Key = Pattern_81, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsRawValues = False, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 3, MaxLogicalLength = 4, IsLogical = True, Condition = AllOf, ConditionJoiner = ,  }
                * CONSUME: PatternGroup { DependencyInfo = Pattern_80 | Order: 77 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_80, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 1, MaxLogicalLength = 2, IsLogical = True, Condition = OneOf, ConditionJoiner =  or  }
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
            bool consume_pattern_26(global::System.ReadOnlySpan<char> buffer0, out int length)
            {
                /*
                * TokenID: comment (#22)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_95 | Order: 102 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], Key = Pattern_95, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsRawValues = True, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 2, MaxLogicalLength = 2, IsLogical = False, Condition = AllOf, ConditionJoiner = ,  }
                * STOP: PatternGroup { DependencyInfo = Pattern_99 | Order: 103 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], Key = Pattern_99, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsRawValues = True, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 2, MaxLogicalLength = 2, IsLogical = False, Condition = AllOf, ConditionJoiner = ,  }
                * ESCAPE: PatternGroup { DependencyInfo = Pattern_101 | Order: 80 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_101, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = True, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 1, MaxLogicalLength = 1, IsLogical = False, Condition = AllOf, ConditionJoiner = ,  }
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
