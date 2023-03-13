//HintName: MetaParser.MetaParser.recursive_parser.tokens.compound.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static bool TryProcessCompound(global::System.ReadOnlySpan<byte> input, out byte id, out int length)
        {
            // Recursive consumers
            if (is_codeblock_token_start(input))
            {
                id = TokenId.Codeblock;
                return consume_pattern_16(input, out length);
            }
            
            // Linear consumers
            switch (input)
            {
                case [ TokenId.Solidus, TokenId.Solidus, ..]:
                {
                    id = TokenId.Comment;
                    return consume_pattern_14(input, out length);
                }
                case [ TokenId.Identifier, TokenId.Colon, ..]:
                {
                    id = TokenId.Declaration;
                    return consume_pattern_15(input, out length);
                }
            }
            id = default;
            length = default;
            return false;
            
            bool is_codeblock_token_start(global::System.ReadOnlySpan<byte> input)
            {
                return input switch
                {
                    [TokenId.Open_Bracket, var buffer] when (is_declaration_token_start(buffer)) => true,
                    _ => false
                };
            }
            bool consume_pattern_14(global::System.ReadOnlySpan<byte> input, out int length)
            {
                /*
                * TokenID: comment (#13)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = , NodeID = Pattern_49, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 2 }
                * STOP: PatternGroup { DependencyInfo = , NodeID = Pattern_51, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }
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
            bool consume_pattern_15(global::System.ReadOnlySpan<byte> input, out int length)
            {
                /*
                * TokenID: declaration (#14)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = , NodeID = Pattern_54, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 2 }
                * STOP: PatternGroup { DependencyInfo = , NodeID = Pattern_56, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer = input.Slice(2);
                while (buffer.Length > 0)
                {
                    if (buffer.StartsWith(stackalloc []{ TokenId.Semicolon}))
                    /* If we have a STOP sequence, check for it */
                    {
                        /* end */
                        break;
                    }
                    
                    /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
                    buffer = buffer.Slice(1);
                }
                
                if (buffer.StartsWith(stackalloc []{ TokenId.Semicolon}))
                {
                    length = 1 + (input.Length - buffer.Length);
                    return true;
                }
                
                length = default;
                return false;
            }
            bool consume_pattern_16(global::System.ReadOnlySpan<byte> input, out int length)
            {
                /*
                * TokenID: codeblock (#15)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = , NodeID = Pattern_58, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }
                * CONSUME: PatternGroup { DependencyInfo = , NodeID = Pattern_60, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsConstantLength = True, HasChildren = True, Condition = OneOf, ConditionJoiner =  or , MinLength = 1 }
                * STOP: PatternGroup { DependencyInfo = , NodeID = Pattern_62, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }
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
                    
                    
                    while (buffer.Length > 0)
                    {
                        if (buffer is [ TokenId.Declaration, ..])
                        /* If we have a set of valid CONSUME targets, then try and consume as many as possible (STOP sequence should be mutually exclusive with CONSUME sequence) */
                        {
                            buffer = buffer.Slice(1);
                            /* consumer forces moving on to next loop */
                            continue;
                        }
                        
                        /* otherwise, default behaviour is to exit loop */
                        break;
                    }
                    
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
