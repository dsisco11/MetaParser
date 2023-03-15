//HintName: MetaParser.MetaParser.linear_parser.tokens.compound.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static bool TryProcessCompound(global::System.ReadOnlySpan<byte> buffer0, out byte id, out int length)
        {
            // Linear consumers
            switch (buffer0)
            {
                case [TokenId.Solidus, TokenId.Solidus, ..]:
                {
                    id = TokenId.Comment;
                    return consume_pattern_27(buffer0, out length);
                }
                case [TokenId.Solidus, TokenId.Asterisk, ..]:
                {
                    id = TokenId.Comment;
                    return consume_pattern_28(buffer0, out length);
                }
                case [TokenId.Open_Bracket, ..]:
                {
                    id = TokenId.Codeblock;
                    return consume_pattern_29(buffer0, out length);
                }
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
            }
            id = default;
            length = default;
            return false;
            
            bool consume_pattern_27(global::System.ReadOnlySpan<byte> buffer0, out int length)
            {
                /*
                * TokenID: comment (#22)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_104 | Order: 149 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], Key = Pattern_104, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsRawValues = True, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 2, MaxLogicalLength = 2, IsLogical = False, Condition = AllOf, ConditionJoiner = ,  }
                * STOP: PatternGroup { DependencyInfo = Pattern_106 | Order: 153 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], Key = Pattern_106, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = True, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 1, MaxLogicalLength = 1, IsLogical = False, Condition = AllOf, ConditionJoiner = ,  }
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
            bool consume_pattern_28(global::System.ReadOnlySpan<byte> buffer0, out int length)
            {
                /*
                * TokenID: comment (#22)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_109 | Order: 150 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], Key = Pattern_109, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsRawValues = True, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 2, MaxLogicalLength = 2, IsLogical = False, Condition = AllOf, ConditionJoiner = ,  }
                * STOP: PatternGroup { DependencyInfo = Pattern_112 | Order: 151 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], Key = Pattern_112, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsRawValues = True, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 2, MaxLogicalLength = 2, IsLogical = False, Condition = AllOf, ConditionJoiner = ,  }
                * ESCAPE: PatternGroup { DependencyInfo = Pattern_114 | Order: 152 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], Key = Pattern_114, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = True, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 1, MaxLogicalLength = 1, IsLogical = False, Condition = AllOf, ConditionJoiner = ,  }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer1 = buffer0.Slice(2);
                while (buffer1.Length > 0)
                {
                    if (buffer1[0] == TokenId.Reverse_Solidus)
                    {
                        /* look past the ESCAPE sequence */
                        var buffer2 = buffer1.Slice(1);
                        /* if the stop sequence immediately follows the ESCAPE sequence then they are consumed */
                        if (buffer2.StartsWith(stackalloc []{ TokenId.Asterisk, TokenId.Solidus }))
                        {
                            buffer1 = buffer2.Slice(2);
                            continue;
                        }
                    }
                    
                    if (buffer1.StartsWith(stackalloc []{ TokenId.Asterisk, TokenId.Solidus }))
                    /* If we have a STOP sequence, check for it */
                    {
                        /* end */
                        break;
                    }
                    
                    /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
                    buffer1 = buffer1.Slice(1);
                }
                
                if (buffer1.StartsWith(stackalloc []{ TokenId.Asterisk, TokenId.Solidus }))
                {
                    length = 2 + (buffer0.Length - buffer1.Length);
                    return true;
                }
                
                length = default;
                return false;
            }
            bool consume_pattern_29(global::System.ReadOnlySpan<byte> buffer0, out int length)
            {
                /*
                * TokenID: codeblock (#23)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_116 | Order: 147 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], Key = Pattern_116, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = True, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 1, MaxLogicalLength = 1, IsLogical = False, Condition = AllOf, ConditionJoiner = ,  }
                * STOP: PatternGroup { DependencyInfo = Pattern_118 | Order: 148 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], Key = Pattern_118, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = True, IsInlinable = True, IsConstantLength = True, HasChildren = True, MinLogicalLength = 1, MaxLogicalLength = 1, IsLogical = False, Condition = AllOf, ConditionJoiner = ,  }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer1 = buffer0.Slice(1);
                while (buffer1.Length > 0)
                {
                    if (buffer1[0] == TokenId.Close_Bracket)
                    /* If we have a STOP sequence, check for it */
                    {
                        /* end */
                        break;
                    }
                    
                    /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
                    buffer1 = buffer1.Slice(1);
                }
                
                if (buffer1[0] == TokenId.Close_Bracket)
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
