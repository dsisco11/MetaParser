//HintName: MetaParser.MetaParser.linear_parser.tokens.compound.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static bool TryProcessCompound(global::System.ReadOnlySpan<byte> input, out byte id, out int length)
        {
            // Linear consumers
            switch (input)
            {
                case [ TokenId.Solidus, TokenId.Solidus, ..]:
                {
                    id = TokenId.Comment;
                    return consume_pattern_27(input, out length);
                }
                case [ TokenId.Solidus, TokenId.Asterisk, ..]:
                {
                    id = TokenId.Comment;
                    return consume_pattern_28(input, out length);
                }
                case [ TokenId.Keyword_Var, ..]:
                {
                    id = TokenId.Typename;
                    length = 1;
                    return true;
                }
                case [ TokenId.Keyword_Byte, ..]:
                {
                    id = TokenId.Typename;
                    length = 1;
                    return true;
                }
                case [ TokenId.Keyword_Short, ..]:
                {
                    id = TokenId.Typename;
                    length = 1;
                    return true;
                }
                case [ TokenId.Keyword_Int, ..]:
                {
                    id = TokenId.Typename;
                    length = 1;
                    return true;
                }
                case [ TokenId.Keyword_Float, ..]:
                {
                    id = TokenId.Typename;
                    length = 1;
                    return true;
                }
                case [ TokenId.Open_Bracket, ..]:
                {
                    id = TokenId.Codeblock;
                    return consume_pattern_29(input, out length);
                }
            }
            id = default;
            length = default;
            return false;
            
            bool consume_pattern_27(global::System.ReadOnlySpan<byte> input, out int length)
            {
                /*
                * TokenID: comment (#22)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = ResolvedNode { Key = Pattern_104, Order = 144, MinDepth = 2, MaxDepth = 2, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]] }, NodeID = Pattern_104, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 2 }
                * STOP: PatternGroup { DependencyInfo = ResolvedNode { Key = Pattern_106, Order = 148, MinDepth = 2, MaxDepth = 2, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]] }, NodeID = Pattern_106, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }
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
                * START: PatternGroup { DependencyInfo = ResolvedNode { Key = Pattern_109, Order = 145, MinDepth = 2, MaxDepth = 2, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]] }, NodeID = Pattern_109, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 2 }
                * STOP: PatternGroup { DependencyInfo = ResolvedNode { Key = Pattern_112, Order = 146, MinDepth = 2, MaxDepth = 2, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]] }, NodeID = Pattern_112, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 2 }
                * ESCAPE: PatternGroup { DependencyInfo = ResolvedNode { Key = Pattern_114, Order = 147, MinDepth = 2, MaxDepth = 2, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]] }, NodeID = Pattern_114, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }
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
                * START: PatternGroup { DependencyInfo = ResolvedNode { Key = Pattern_116, Order = 142, MinDepth = 2, MaxDepth = 2, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]] }, NodeID = Pattern_116, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }
                * STOP: PatternGroup { DependencyInfo = ResolvedNode { Key = Pattern_118, Order = 143, MinDepth = 2, MaxDepth = 2, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]] }, NodeID = Pattern_118, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }
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
