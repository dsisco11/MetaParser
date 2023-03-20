//HintName: MetaParser.MetaParser.parser.tokens.compound.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static bool TryProcessingSyntaxToken(global::System.ReadOnlySpan<byte> buffer0, out byte id, out int length)
        {
            switch (buffer0)
            {
                case [TokenId.Keyword_Var, ..]:
                {
                    id = TokenId.Typename;
                    length = 1;
                    return true;
                }
                case [TokenId.Keyword_Byte, ..]:
                {
                    id = TokenId.Typename;
                    length = 1;
                    return true;
                }
                case [TokenId.Keyword_Short, ..]:
                {
                    id = TokenId.Typename;
                    length = 1;
                    return true;
                }
                case [TokenId.Keyword_Int, ..]:
                {
                    id = TokenId.Typename;
                    length = 1;
                    return true;
                }
                case [TokenId.Keyword_Float, ..]:
                {
                    id = TokenId.Typename;
                    length = 1;
                    return true;
                }
                case [(TokenId.Keyword_Var or TokenId.Keyword_Function), (TokenId.Whitespace or TokenId.Identifier or TokenId.Whitespace or TokenId.Char_Open_Parenthesis or TokenId.Whitespace or TokenId.Char_Close_Parenthesis or TokenId.Whitespace or TokenId.Char_Open_Bracket or TokenId.Whitespace or TokenId.Char_Close_Bracket), ..]:
                {
                    id = TokenId.Program;
                    return consume_pattern_33(buffer0, out length);
                }
                case [TokenId.Char_Solidus, TokenId.Char_Asterisk, ..]:
                {
                    id = TokenId.Comment;
                    return consume_pattern_25(buffer0, out length);
                }
                case [TokenId.Char_Solidus, TokenId.Char_Solidus, ..]:
                {
                    id = TokenId.Comment;
                    return consume_pattern_24(buffer0, out length);
                }
                case [TokenId.Identifier, TokenId.Char_Colon, ..]:
                {
                    id = TokenId.Declaration;
                    return consume_pattern_31(buffer0, out length);
                }
            }
            id = default;
            length = default;
            return false;
            
            bool consume_pattern_33(global::System.ReadOnlySpan<byte> buffer0, out int length)
            {
                /*
                * TokenID: program (#26)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_150 | Order: 200 | IsRecursive: False | TreeDepth: [6, 7] | NodeDepth: [2, 2], Key = Pattern_150, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Condition = AllOf, ConditionJoiner = ,  }
                * CONSUME: PatternGroup { DependencyInfo = Pattern_149 | Order: 197 | IsRecursive: False | TreeDepth: [5, 6] | NodeDepth: [1, 1], Key = Pattern_149, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Condition = OneOf, ConditionJoiner =  or  }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer1 = buffer0.Slice(2);
                
                while (buffer1.Length > 0)
                {
                    if (buffer1 is [ (TokenId.Whitespace or TokenId.Identifier or TokenId.Whitespace or TokenId.Char_Open_Parenthesis or TokenId.Whitespace or TokenId.Char_Close_Parenthesis or TokenId.Whitespace or TokenId.Char_Open_Bracket or TokenId.Whitespace or TokenId.Char_Close_Bracket), ..])
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
            bool consume_pattern_25(global::System.ReadOnlySpan<byte> buffer0, out int length)
            {
                /*
                * TokenID: comment (#22)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_108 | Order: 182 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], Key = Pattern_108, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Condition = AllOf, ConditionJoiner = ,  }
                * STOP: PatternGroup { DependencyInfo = Pattern_111 | Order: 183 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], Key = Pattern_111, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Condition = AllOf, ConditionJoiner = ,  }
                * ESCAPE: PatternGroup { DependencyInfo = Pattern_113 | Order: 184 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], Key = Pattern_113, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Condition = AllOf, ConditionJoiner = ,  }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer1 = buffer0.Slice(2);
                while (buffer1.Length > 0)
                {
                    if (buffer1[0] == TokenId.Char_Reverse_Solidus)
                    {
                        /* look past the ESCAPE sequence */
                        var buffer2 = buffer1.Slice(1);
                        /* if the stop sequence immediately follows the ESCAPE sequence then they are consumed */
                        if (buffer2.StartsWith(stackalloc []{ TokenId.Char_Asterisk, TokenId.Char_Solidus }))
                        {
                            buffer1 = buffer2.Slice(2);
                            continue;
                        }
                    }
                    
                    if (buffer1.StartsWith(stackalloc []{ TokenId.Char_Asterisk, TokenId.Char_Solidus }))
                    /* If we have a STOP sequence, check for it */
                    {
                        /* end */
                        break;
                    }
                    
                    /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
                    buffer1 = buffer1.Slice(1);
                }
                
                if (buffer1.StartsWith(stackalloc []{ TokenId.Char_Asterisk, TokenId.Char_Solidus }))
                {
                    length = 2 + (buffer0.Length - buffer1.Length);
                    return true;
                }
                
                length = default;
                return false;
            }
            bool consume_pattern_31(global::System.ReadOnlySpan<byte> buffer0, out int length)
            {
                /*
                * TokenID: declaration (#24)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_126 | Order: 196 | IsRecursive: False | TreeDepth: [5, 6] | NodeDepth: [1, 1], Key = Pattern_126, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Condition = AllOf, ConditionJoiner = ,  }
                * STOP: PatternGroup { DependencyInfo = Pattern_128 | Order: 180 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], Key = Pattern_128, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Condition = AllOf, ConditionJoiner = ,  }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer1 = buffer0.Slice(2);
                while (buffer1.Length > 0)
                {
                    if (buffer1[0] == TokenId.Char_Semicolon)
                    /* If we have a STOP sequence, check for it */
                    {
                        /* end */
                        break;
                    }
                    
                    /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
                    buffer1 = buffer1.Slice(1);
                }
                
                if (buffer1[0] == TokenId.Char_Semicolon)
                {
                    length = 1 + (buffer0.Length - buffer1.Length);
                    return true;
                }
                
                length = default;
                return false;
            }
            bool consume_pattern_24(global::System.ReadOnlySpan<byte> buffer0, out int length)
            {
                /*
                * TokenID: comment (#22)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_103 | Order: 181 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], Key = Pattern_103, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Condition = AllOf, ConditionJoiner = ,  }
                * STOP: PatternGroup { DependencyInfo = Pattern_105 | Order: 185 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], Key = Pattern_105, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Condition = AllOf, ConditionJoiner = ,  }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer1 = buffer0.Slice(2);
                while (buffer1.Length > 0)
                {
                    if (buffer1[0] == TokenId.Newline)
                    /* If we have a STOP sequence, check for it */
                    {
                        /* end */
                        break;
                    }
                    
                    /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
                    buffer1 = buffer1.Slice(1);
                }
                
                if (buffer1[0] == TokenId.Newline)
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
