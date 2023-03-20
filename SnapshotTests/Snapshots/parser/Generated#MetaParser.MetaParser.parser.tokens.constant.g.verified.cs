//HintName: MetaParser.MetaParser.parser.tokens.constant.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static ConsumerResult TryProcessingLexerToken(global::System.ReadOnlySpan<char> buffer0)
        {
            switch (buffer0)
            {
                case ['f', 'u', 'n', 'c', 't', 'i', 'o', 'n', ..]:
                {
                    return new ConsumerResult (TokenId.Keyword_Function, 8);
                }
                case ['s', 'h', 'o', 'r', 't', ..]:
                {
                    return new ConsumerResult (TokenId.Keyword_Short, 5);
                }
                case ['f', 'l', 'o', 'a', 't', ..]:
                {
                    return new ConsumerResult (TokenId.Keyword_Float, 5);
                }
                case ['b', 'y', 't', 'e', ..]:
                {
                    return new ConsumerResult (TokenId.Keyword_Byte, 4);
                }
                case ['v', 'a', 'r', ..]:
                {
                    return new ConsumerResult (TokenId.Keyword_Var, 3);
                }
                case ['i', 'n', 't', ..]:
                {
                    return new ConsumerResult (TokenId.Keyword_Int, 3);
                }
                case [(' ' or '\t' or '\f'), ..]:
                {
                    return consume_pattern_18(buffer0);
                }
                case [('\r' or '\n'), ..]:
                {
                    return consume_pattern_20(buffer0);
                }
                case [':', ..]:
                {
                    return new ConsumerResult (TokenId.Char_Colon, 1);
                }
                case ['{', ..]:
                {
                    return new ConsumerResult (TokenId.Char_Open_Bracket, 1);
                }
                case ['}', ..]:
                {
                    return new ConsumerResult (TokenId.Char_Close_Bracket, 1);
                }
                case ['[', ..]:
                {
                    return new ConsumerResult (TokenId.Char_Open_Sqbracket, 1);
                }
                case [']', ..]:
                {
                    return new ConsumerResult (TokenId.Char_Close_Sqbracket, 1);
                }
                case [';', ..]:
                {
                    return new ConsumerResult (TokenId.Char_Semicolon, 1);
                }
                case [')', ..]:
                {
                    return new ConsumerResult (TokenId.Char_Close_Parenthesis, 1);
                }
                case ['*', ..]:
                {
                    return new ConsumerResult (TokenId.Char_Asterisk, 1);
                }
                case ['/', ..]:
                {
                    return new ConsumerResult (TokenId.Char_Solidus, 1);
                }
                case ['\\', ..]:
                {
                    return new ConsumerResult (TokenId.Char_Reverse_Solidus, 1);
                }
                case ['(', ..]:
                {
                    return new ConsumerResult (TokenId.Char_Open_Parenthesis, 1);
                }
                case [(>='0' and <='9'), ..]:
                {
                    return consume_pattern_19(buffer0);
                }
                case [((>='a' and <='z') or (>='A' and <='Z')), ((>='a' and <='z') or (>='A' and <='Z') or (>='0' and <='9') or '-' or '_'), ..]:
                {
                    return consume_pattern_21(buffer0);
                }
                case ['/', '*', ..]:
                {
                    return consume_pattern_22(buffer0);
                }
                case ['/', '/', ..]:
                {
                    return consume_pattern_23(buffer0);
                }
            }
            id = default;
            length = default;
            return false;
            
            ConsumerResult consume_pattern_18(global::System.ReadOnlySpan<char> buffer0)
            {
                /*
                * TokenID: whitespace (#18)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_66 | Order: 84 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_66, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Condition = OneOf, ConditionJoiner =  or  }
                * CONSUME: PatternGroup { DependencyInfo = Pattern_66 | Order: 84 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_66, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Condition = OneOf, ConditionJoiner =  or  }
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
                
                return new (TokenId.Whitespace, buffer0.Length - buffer1.Length);
            }
            ConsumerResult consume_pattern_20(global::System.ReadOnlySpan<char> buffer0)
            {
                /*
                * TokenID: newline (#20)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_71 | Order: 86 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_71, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Condition = OneOf, ConditionJoiner =  or  }
                * CONSUME: PatternGroup { DependencyInfo = Pattern_71 | Order: 86 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_71, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Condition = OneOf, ConditionJoiner =  or  }
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
                
                return new (TokenId.Newline, buffer0.Length - buffer1.Length);
            }
            ConsumerResult consume_pattern_19(global::System.ReadOnlySpan<char> buffer0)
            {
                /*
                * TokenID: digits (#19)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_68 | Order: 85 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_68, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 2, MaxConditions = 2, IsConditional = True, IsDeterministic = False, Condition = OneOf, ConditionJoiner =  or  }
                * CONSUME: PatternGroup { DependencyInfo = Pattern_68 | Order: 85 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_68, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 2, MaxConditions = 2, IsConditional = True, IsDeterministic = False, Condition = OneOf, ConditionJoiner =  or  }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer1 = buffer0.Slice(1);
                
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
                
                return new (TokenId.Digits, buffer0.Length - buffer1.Length);
            }
            ConsumerResult consume_pattern_21(global::System.ReadOnlySpan<char> buffer0)
            {
                /*
                * TokenID: identifier (#21)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_82 | Order: 119 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], Key = Pattern_82, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 2, MaxConditions = 4, IsConditional = True, IsDeterministic = False, Condition = AllOf, ConditionJoiner = ,  }
                * CONSUME: PatternGroup { DependencyInfo = Pattern_81 | Order: 88 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_81, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 2, IsConditional = True, IsDeterministic = False, Condition = OneOf, ConditionJoiner =  or  }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer1 = buffer0.Slice(2);
                
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
                
                return new (TokenId.Identifier, buffer0.Length - buffer1.Length);
            }
            ConsumerResult consume_pattern_22(global::System.ReadOnlySpan<char> buffer0)
            {
                /*
                * TokenID: comment (#22)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_86 | Order: 90 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_86, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Condition = AllOf, ConditionJoiner = ,  }
                * STOP: PatternGroup { DependencyInfo = Pattern_90 | Order: 92 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_90, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Condition = AllOf, ConditionJoiner = ,  }
                * ESCAPE: PatternGroup { DependencyInfo = Pattern_92 | Order: 93 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_92, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Condition = AllOf, ConditionJoiner = ,  }
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
                    return new (TokenId.Comment, 2 + (buffer0.Length - buffer1.Length));
                }
                
                return new (default, default);
            }
            ConsumerResult consume_pattern_23(global::System.ReadOnlySpan<char> buffer0)
            {
                /*
                * TokenID: comment (#22)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_96 | Order: 95 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_96, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Condition = AllOf, ConditionJoiner = ,  }
                * STOP: PatternGroup { DependencyInfo = Pattern_98 | Order: 96 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_98, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Condition = AllOf, ConditionJoiner = ,  }
                * ESCAPE: PatternGroup { DependencyInfo = Pattern_100 | Order: 97 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], Key = Pattern_100, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Condition = AllOf, ConditionJoiner = ,  }
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
                        if (buffer2[0] == '\n')
                        {
                            buffer1 = buffer2.Slice(1);
                            continue;
                        }
                    }
                    
                    if (buffer1[0] == '\n')
                    /* If we have a STOP sequence, check for it */
                    {
                        /* end */
                        break;
                    }
                    
                    /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
                    buffer1 = buffer1.Slice(1);
                }
                
                if (buffer1[0] == '\n')
                {
                    return new (TokenId.Comment, 1 + (buffer0.Length - buffer1.Length));
                }
                
                return new (default, default);
            }
        }
    }
}
