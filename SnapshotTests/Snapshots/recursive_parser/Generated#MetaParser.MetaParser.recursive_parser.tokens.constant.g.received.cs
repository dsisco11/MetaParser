//HintName: MetaParser.MetaParser.recursive_parser.tokens.constant.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static bool TryProcessConstant(global::System.ReadOnlySpan<char> input, out byte id, out int length)
        {
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
                * TokenConsumer { Token = TokenInfo { Index = 10, Name = whitespace, Consumers = System.Collections.Generic.List`1[MetaParser.Consumers.TokenConsumer], Identity = TokenGraphId { TokenIndex = 10, ConsumerIndex = -1 } }, Index = 10, Type = Data, Identity = TokenGraphId { TokenIndex = 10, ConsumerIndex = 10 }, DependencyInfo = ResolvedVertexNode { Order = 16, MinDepth = 0, MaxDepth = 0, IsRecursive = False }, Start = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, HasChildren = True, Condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Consume = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, HasChildren = True, Condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Stop = , Escape = , IsOpen = True, IsClosed = False, IsDynamic = True, IsConstant = False }
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
                * TokenConsumer { Token = TokenInfo { Index = 11, Name = newline, Consumers = System.Collections.Generic.List`1[MetaParser.Consumers.TokenConsumer], Identity = TokenGraphId { TokenIndex = 11, ConsumerIndex = -1 } }, Index = 11, Type = Data, Identity = TokenGraphId { TokenIndex = 11, ConsumerIndex = 11 }, DependencyInfo = ResolvedVertexNode { Order = 7, MinDepth = 0, MaxDepth = 0, IsRecursive = False }, Start = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, HasChildren = True, Condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Consume = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, HasChildren = True, Condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Stop = , Escape = , IsOpen = True, IsClosed = False, IsDynamic = True, IsConstant = False }
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
                * TokenConsumer { Token = TokenInfo { Index = 12, Name = identifier, Consumers = System.Collections.Generic.List`1[MetaParser.Consumers.TokenConsumer], Identity = TokenGraphId { TokenIndex = 12, ConsumerIndex = -1 } }, Index = 12, Type = Data, Identity = TokenGraphId { TokenIndex = 12, ConsumerIndex = 12 }, DependencyInfo = ResolvedVertexNode { Order = 24, MinDepth = 0, MaxDepth = 0, IsRecursive = False }, Start = PatternGroup { Length = 2, IsRawValues = False, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 2 }, Consume = PatternGroup { Length = 1, IsRawValues = False, IsConstantLength = True, HasChildren = True, Condition = OneOf, ConditionJoiner =  or , MinLength = 1 }, Stop = , Escape = , IsOpen = True, IsClosed = False, IsDynamic = True, IsConstant = False }
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
                * TokenConsumer { Token = TokenInfo { Index = 13, Name = comment, Consumers = System.Collections.Generic.List`1[MetaParser.Consumers.TokenConsumer], Identity = TokenGraphId { TokenIndex = 13, ConsumerIndex = -1 } }, Index = 13, Type = Data, Identity = TokenGraphId { TokenIndex = 13, ConsumerIndex = 13 }, DependencyInfo = ResolvedVertexNode { Order = 14, MinDepth = 0, MaxDepth = 0, IsRecursive = False }, Start = PatternGroup { Length = 2, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 2 }, Consume = , Stop = PatternGroup { Length = 2, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 2 }, Escape = PatternGroup { Length = 1, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }, IsOpen = False, IsClosed = True, IsDynamic = True, IsConstant = False }
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
