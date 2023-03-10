//HintName: MetaParser.MetaParser.linear_parser.tokens.complex.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static bool TryProcessComplex(global::System.ReadOnlySpan<byte> input, out byte id, out int length)
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
                * START: PatternGroup { NodeID = GraphNodeKey { Parent = GraphNodeKey { Parent = GraphNodeKey { Parent = , Type = Token, Index = 22 }, Type = Consumer, Index = 27 }, Type = Pattern, Index = 104 }, DependencyInfo = ResolvedNode { Key = GraphNodeKey { Parent = GraphNodeKey { Parent = GraphNodeKey { Parent = , Type = Token, Index = 22 }, Type = Consumer, Index = 27 }, Type = Pattern, Index = 104 }, Order = 125, MinDepth = 0, MaxDepth = 0, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.GraphNodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.GraphNodeKey]] }, Length = 2, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 2 }
                * STOP: PatternGroup { NodeID = GraphNodeKey { Parent = GraphNodeKey { Parent = GraphNodeKey { Parent = , Type = Token, Index = 22 }, Type = Consumer, Index = 27 }, Type = Pattern, Index = 106 }, DependencyInfo = ResolvedNode { Key = GraphNodeKey { Parent = GraphNodeKey { Parent = GraphNodeKey { Parent = , Type = Token, Index = 22 }, Type = Consumer, Index = 27 }, Type = Pattern, Index = 106 }, Order = 127, MinDepth = 0, MaxDepth = 0, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.GraphNodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.GraphNodeKey]] }, Length = 1, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }
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
                * START: PatternGroup { NodeID = GraphNodeKey { Parent = GraphNodeKey { Parent = GraphNodeKey { Parent = , Type = Token, Index = 22 }, Type = Consumer, Index = 28 }, Type = Pattern, Index = 109 }, DependencyInfo = ResolvedNode { Key = GraphNodeKey { Parent = GraphNodeKey { Parent = GraphNodeKey { Parent = , Type = Token, Index = 22 }, Type = Consumer, Index = 28 }, Type = Pattern, Index = 109 }, Order = 58, MinDepth = 0, MaxDepth = 0, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.GraphNodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.GraphNodeKey]] }, Length = 2, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 2 }
                * STOP: PatternGroup { NodeID = GraphNodeKey { Parent = GraphNodeKey { Parent = GraphNodeKey { Parent = , Type = Token, Index = 22 }, Type = Consumer, Index = 28 }, Type = Pattern, Index = 112 }, DependencyInfo = ResolvedNode { Key = GraphNodeKey { Parent = GraphNodeKey { Parent = GraphNodeKey { Parent = , Type = Token, Index = 22 }, Type = Consumer, Index = 28 }, Type = Pattern, Index = 112 }, Order = 61, MinDepth = 0, MaxDepth = 0, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.GraphNodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.GraphNodeKey]] }, Length = 2, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 2 }
                * ESCAPE: PatternGroup { NodeID = GraphNodeKey { Parent = GraphNodeKey { Parent = GraphNodeKey { Parent = , Type = Token, Index = 22 }, Type = Consumer, Index = 28 }, Type = Pattern, Index = 114 }, DependencyInfo = ResolvedNode { Key = GraphNodeKey { Parent = GraphNodeKey { Parent = GraphNodeKey { Parent = , Type = Token, Index = 22 }, Type = Consumer, Index = 28 }, Type = Pattern, Index = 114 }, Order = 63, MinDepth = 0, MaxDepth = 0, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.GraphNodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.GraphNodeKey]] }, Length = 1, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }
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
                * START: PatternGroup { NodeID = GraphNodeKey { Parent = GraphNodeKey { Parent = GraphNodeKey { Parent = , Type = Token, Index = 23 }, Type = Consumer, Index = 29 }, Type = Pattern, Index = 116 }, DependencyInfo = ResolvedNode { Key = GraphNodeKey { Parent = GraphNodeKey { Parent = GraphNodeKey { Parent = , Type = Token, Index = 23 }, Type = Consumer, Index = 29 }, Type = Pattern, Index = 116 }, Order = 136, MinDepth = 0, MaxDepth = 0, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.GraphNodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.GraphNodeKey]] }, Length = 1, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }
                * STOP: PatternGroup { NodeID = GraphNodeKey { Parent = GraphNodeKey { Parent = GraphNodeKey { Parent = , Type = Token, Index = 23 }, Type = Consumer, Index = 29 }, Type = Pattern, Index = 118 }, DependencyInfo = ResolvedNode { Key = GraphNodeKey { Parent = GraphNodeKey { Parent = GraphNodeKey { Parent = , Type = Token, Index = 23 }, Type = Consumer, Index = 29 }, Type = Pattern, Index = 118 }, Order = 138, MinDepth = 0, MaxDepth = 0, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.GraphNodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.GraphNodeKey]] }, Length = 1, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }
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
