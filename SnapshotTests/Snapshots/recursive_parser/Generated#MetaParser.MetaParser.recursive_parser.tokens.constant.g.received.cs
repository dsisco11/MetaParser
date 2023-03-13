//HintName: MetaParser.MetaParser.recursive_parser.tokens.constant.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static bool TryProcessConstant(global::System.ReadOnlySpan<char> input, out byte id, out int length)
        {
            // Linear consumers
            switch (input)
            {
                case [ ((>='a' and <='z') or (>='A' and <='Z')), ((>='a' and <='z') or (>='A' and <='Z') or (>='0' and <='9') or '-' or '_'), ..]:
                {
                    id = TokenId.Identifier;
                    return consume_pattern_12(input, out length);
                }
                case [ '/', '*', ..]:
                {
                    id = TokenId.Comment;
                    return consume_pattern_13(input, out length);
                }
                case [ '/', ..]:
                {
                    id = TokenId.Solidus;
                    length = 1;
                    return true;
                }
                case [ ':', ..]:
                {
                    id = TokenId.Colon;
                    length = 1;
                    return true;
                }
                case [ ';', ..]:
                {
                    id = TokenId.Semicolon;
                    length = 1;
                    return true;
                }
                case [ '{', ..]:
                {
                    id = TokenId.Open_Bracket;
                    length = 1;
                    return true;
                }
                case [ '}', ..]:
                {
                    id = TokenId.Close_Bracket;
                    length = 1;
                    return true;
                }
                case [ '[', ..]:
                {
                    id = TokenId.Open_Sqbracket;
                    length = 1;
                    return true;
                }
                case [ ']', ..]:
                {
                    id = TokenId.Close_Sqbracket;
                    length = 1;
                    return true;
                }
                case [ '(', ..]:
                {
                    id = TokenId.Open_Parenthesis;
                    length = 1;
                    return true;
                }
                case [ ')', ..]:
                {
                    id = TokenId.Close_Parenthesis;
                    length = 1;
                    return true;
                }
                case [ (' ' or '\t' or '\f'), ..]:
                {
                    id = TokenId.Whitespace;
                    return consume_pattern_10(input, out length);
                }
                case [ ('\r' or '\n'), ..]:
                {
                    id = TokenId.Newline;
                    return consume_pattern_11(input, out length);
                }
            }
            id = default;
            length = default;
            return false;
            
            bool consume_pattern_10(global::System.ReadOnlySpan<char> input, out int length)
            {
                /*
                * TokenID: whitespace (#10)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_22 | Order[68] | IsRecursive (False) | Depth[N: [0, 1], D: [0, 0], P: [1, 1], C: [0, 0], T: [0, 0]], NodeID = Pattern_22, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsInline = True, IsConstantLength = True, HasChildren = True, Condition = OneOf, ConditionJoiner =  or , MinLength = 1 }
                * CONSUME: PatternGroup { DependencyInfo = Pattern_22 | Order[68] | IsRecursive (False) | Depth[N: [0, 1], D: [0, 0], P: [1, 1], C: [0, 0], T: [0, 0]], NodeID = Pattern_22, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsInline = True, IsConstantLength = True, HasChildren = True, Condition = OneOf, ConditionJoiner =  or , MinLength = 1 }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer = input.Slice(1);
                
                while (buffer.Length > 0)
                {
                    if (buffer is [ (' ' or '\t' or '\f'), ..])
                    /* If we have a set of valid CONSUME targets, then try and consume as many as possible (STOP sequence should be mutually exclusive with CONSUME sequence) */
                    {
                        buffer = buffer.Slice(1);
                        /* consumer forces moving on to next loop */
                        continue;
                    }
                    
                    /* otherwise, default behaviour is to exit loop */
                    break;
                }
                
                length = input.Length - buffer.Length;
                return true;
            }
            bool consume_pattern_11(global::System.ReadOnlySpan<char> input, out int length)
            {
                /*
                * TokenID: newline (#11)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_25 | Order[69] | IsRecursive (False) | Depth[N: [0, 1], D: [0, 0], P: [1, 1], C: [0, 0], T: [0, 0]], NodeID = Pattern_25, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsInline = True, IsConstantLength = True, HasChildren = True, Condition = OneOf, ConditionJoiner =  or , MinLength = 1 }
                * CONSUME: PatternGroup { DependencyInfo = Pattern_25 | Order[69] | IsRecursive (False) | Depth[N: [0, 1], D: [0, 0], P: [1, 1], C: [0, 0], T: [0, 0]], NodeID = Pattern_25, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsInline = True, IsConstantLength = True, HasChildren = True, Condition = OneOf, ConditionJoiner =  or , MinLength = 1 }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer = input.Slice(1);
                
                while (buffer.Length > 0)
                {
                    if (buffer is [ ('\r' or '\n'), ..])
                    /* If we have a set of valid CONSUME targets, then try and consume as many as possible (STOP sequence should be mutually exclusive with CONSUME sequence) */
                    {
                        buffer = buffer.Slice(1);
                        /* consumer forces moving on to next loop */
                        continue;
                    }
                    
                    /* otherwise, default behaviour is to exit loop */
                    break;
                }
                
                length = input.Length - buffer.Length;
                return true;
            }
            bool consume_pattern_12(global::System.ReadOnlySpan<char> input, out int length)
            {
                /*
                * TokenID: identifier (#12)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_36 | Order[82] | IsRecursive (False) | Depth[N: [0, 2], D: [0, 0], P: [2, 2], C: [0, 0], T: [0, 0]], NodeID = Pattern_36, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsRawValues = False, IsInline = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 2 }
                * CONSUME: PatternGroup { DependencyInfo = Pattern_35 | Order[71] | IsRecursive (False) | Depth[N: [0, 1], D: [0, 0], P: [1, 1], C: [0, 0], T: [0, 0]], NodeID = Pattern_35, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsInline = True, IsConstantLength = True, HasChildren = True, Condition = OneOf, ConditionJoiner =  or , MinLength = 1 }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer = input.Slice(2);
                
                while (buffer.Length > 0)
                {
                    if (buffer is [ ((>='a' and <='z') or (>='A' and <='Z') or (>='0' and <='9') or '-' or '_'), ..])
                    /* If we have a set of valid CONSUME targets, then try and consume as many as possible (STOP sequence should be mutually exclusive with CONSUME sequence) */
                    {
                        buffer = buffer.Slice(1);
                        /* consumer forces moving on to next loop */
                        continue;
                    }
                    
                    /* otherwise, default behaviour is to exit loop */
                    break;
                }
                
                length = input.Length - buffer.Length;
                return true;
            }
            bool consume_pattern_13(global::System.ReadOnlySpan<char> input, out int length)
            {
                /*
                * TokenID: comment (#13)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = Pattern_40 | Order[83] | IsRecursive (False) | Depth[N: [0, 2], D: [0, 0], P: [2, 2], C: [0, 0], T: [0, 0]], NodeID = Pattern_40, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsRawValues = True, IsInline = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 2 }
                * STOP: PatternGroup { DependencyInfo = Pattern_44 | Order[84] | IsRecursive (False) | Depth[N: [0, 2], D: [0, 0], P: [2, 2], C: [0, 0], T: [0, 0]], NodeID = Pattern_44, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsRawValues = True, IsInline = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 2 }
                * ESCAPE: PatternGroup { DependencyInfo = Pattern_46 | Order[74] | IsRecursive (False) | Depth[N: [0, 1], D: [0, 0], P: [1, 1], C: [0, 0], T: [0, 0]], NodeID = Pattern_46, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = True, IsInline = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer = input.Slice(2);
                while (buffer.Length > 0)
                {
                    if (buffer.StartsWith(stackalloc []{ '\\'}))
                    {
                        /* look past the ESCAPE sequence */
                        var reader = buffer.Slice(1);
                        /* if the stop sequence immediately follows the ESCAPE sequence then they are consumed */
                        if (reader.StartsWith(stackalloc []{ '*', '/'}))
                        {
                            buffer = reader.Slice(2);
                            continue;
                        }
                    }
                    
                    if (buffer.StartsWith(stackalloc []{ '*', '/'}))
                    /* If we have a STOP sequence, check for it */
                    {
                        /* end */
                        break;
                    }
                    
                    /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
                    buffer = buffer.Slice(1);
                }
                
                if (buffer.StartsWith(stackalloc []{ '*', '/'}))
                {
                    length = 2 + (input.Length - buffer.Length);
                    return true;
                }
                
                length = default;
                return false;
            }
        }
    }
}
