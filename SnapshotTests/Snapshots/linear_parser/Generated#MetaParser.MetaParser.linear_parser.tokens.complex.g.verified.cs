//HintName: MetaParser.MetaParser.linear_parser.tokens.complex.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static bool TryProcessComplex(global::System.ReadOnlySpan<byte> input, out byte id, out int length)
        {
            switch (input)
            {
            }
            id = default;
            length = default;
            return false;
            
            bool consume_pattern_27(global::System.ReadOnlySpan<byte> input, out int length)
            {
                /*
                * TokenID: comment (#22)
                * ==[ CONSUMER_DATA ]==
                * ConsumerInfo { Token = TokenInfo { Index = 22, Name = comment, Consumers = System.Collections.Generic.List`1[MetaParser.Consumers.ConsumerInfo] }, Index = 27, Type = Token, DependencyInfo = MetaParser.Graphs.ResolvedVertexNode, Start = PatternGroup { Length = 2, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 2 }, Consume = , Stop = PatternGroup { Length = 1, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }, Escape = , IsOpen = False, IsClosed = True, IsDynamic = True, IsConstant = False }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer = input.Slice(2);
                while (buffer.Length > 0)
                {
                    if (buffer.StartsWith(stackalloc []{ TokenId.Newline}))
                    /* If we have a STOP sequence, check for it */
                    {
                        /* end */
                        break;
                    }
                    
                    /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
                    buffer = buffer.Slice(1);
                }
                
                if (buffer.StartsWith(stackalloc []{ TokenId.Newline}))
                {
                    length = 1 + (input.Length - buffer.Length);
                    return true;
                }
                
                length = default;
                return false;
            }
            bool consume_pattern_28(global::System.ReadOnlySpan<byte> input, out int length)
            {
                /*
                * TokenID: comment (#22)
                * ==[ CONSUMER_DATA ]==
                * ConsumerInfo { Token = TokenInfo { Index = 22, Name = comment, Consumers = System.Collections.Generic.List`1[MetaParser.Consumers.ConsumerInfo] }, Index = 28, Type = Token, DependencyInfo = MetaParser.Graphs.ResolvedVertexNode, Start = PatternGroup { Length = 2, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 2 }, Consume = , Stop = PatternGroup { Length = 2, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 2 }, Escape = PatternGroup { Length = 1, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }, IsOpen = False, IsClosed = True, IsDynamic = True, IsConstant = False }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer = input.Slice(2);
                while (buffer.Length > 0)
                {
                    if (buffer.StartsWith(stackalloc []{ TokenId.Reverse_Solidus}))
                    {
                        /* look past the ESCAPE sequence */
                        var reader = buffer.Slice(1);
                        /* if the stop sequence immediately follows the ESCAPE sequence then they are consumed */
                        if (reader.StartsWith(stackalloc []{ TokenId.Asterisk, TokenId.Solidus}))
                        {
                            buffer = reader.Slice(2);
                            continue;
                        }
                    }
                    
                    if (buffer.StartsWith(stackalloc []{ TokenId.Asterisk, TokenId.Solidus}))
                    /* If we have a STOP sequence, check for it */
                    {
                        /* end */
                        break;
                    }
                    
                    /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
                    buffer = buffer.Slice(1);
                }
                
                if (buffer.StartsWith(stackalloc []{ TokenId.Asterisk, TokenId.Solidus}))
                {
                    length = 2 + (input.Length - buffer.Length);
                    return true;
                }
                
                length = default;
                return false;
            }
            bool consume_pattern_29(global::System.ReadOnlySpan<byte> input, out int length)
            {
                /*
                * TokenID: codeblock (#23)
                * ==[ CONSUMER_DATA ]==
                * ConsumerInfo { Token = TokenInfo { Index = 23, Name = codeblock, Consumers = System.Collections.Generic.List`1[MetaParser.Consumers.ConsumerInfo] }, Index = 29, Type = Token, DependencyInfo = MetaParser.Graphs.ResolvedVertexNode, Start = PatternGroup { Length = 1, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }, Consume = , Stop = PatternGroup { Length = 1, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }, Escape = , IsOpen = False, IsClosed = True, IsDynamic = True, IsConstant = False }
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
                    
                    /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
                    buffer = buffer.Slice(1);
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
