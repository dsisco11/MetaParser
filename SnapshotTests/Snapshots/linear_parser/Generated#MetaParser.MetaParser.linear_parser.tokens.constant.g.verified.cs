//HintName: MetaParser.MetaParser.linear_parser.tokens.constant.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static bool TryProcessConstant(global::System.ReadOnlySpan<char> input, out byte id, out int length)
        {
            // Linear consumers
            switch (input)
            {
                case [ 'f', 'u', 'n', 'c', 't', 'i', 'o', 'n', ..]:
                {
                    id = TokenId.Keyword_Function;
                    length = 8;
                    return true;
                }
                case [ 's', 'h', 'o', 'r', 't', ..]:
                {
                    id = TokenId.Keyword_Short;
                    length = 5;
                    return true;
                }
                case [ 'f', 'l', 'o', 'a', 't', ..]:
                {
                    id = TokenId.Keyword_Float;
                    length = 5;
                    return true;
                }
                case [ 'b', 'y', 't', 'e', ..]:
                {
                    id = TokenId.Keyword_Byte;
                    length = 4;
                    return true;
                }
                case [ 'v', 'a', 'r', ..]:
                {
                    id = TokenId.Keyword_Var;
                    length = 3;
                    return true;
                }
                case [ 'i', 'n', 't', ..]:
                {
                    id = TokenId.Keyword_Int;
                    length = 3;
                    return true;
                }
                case [ ((>='a' and <='z') or (>='A' and <='Z')), ((>='a' and <='z') or (>='A' and <='Z') or (>='0' and <='9') or '-' or '_'), ..]:
                {
                    id = TokenId.Identifier;
                    return consume_pattern_20(input, out length);
                }
                case [ '/', '*', ..]:
                {
                    id = TokenId.Comment;
                    return consume_pattern_26(input, out length);
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
                case [ '*', ..]:
                {
                    id = TokenId.Asterisk;
                    length = 1;
                    return true;
                }
                case [ '/', ..]:
                {
                    id = TokenId.Solidus;
                    length = 1;
                    return true;
                }
                case [ '\\', ..]:
                {
                    id = TokenId.Reverse_Solidus;
                    length = 1;
                    return true;
                }
                case [ (' ' or '\t' or '\f'), ..]:
                {
                    id = TokenId.Whitespace;
                    return consume_pattern_16(input, out length);
                }
                case [ (>='0' and <='9'), ..]:
                {
                    id = TokenId.Digits;
                    return consume_pattern_17(input, out length);
                }
                case [ ((>='a' and <='z') or (>='A' and <='Z')), ..]:
                {
                    id = TokenId.Letters;
                    return consume_pattern_18(input, out length);
                }
                case [ ('\r' or '\n'), ..]:
                {
                    id = TokenId.Newline;
                    return consume_pattern_19(input, out length);
                }
            }
            id = default;
            length = default;
            return false;
            
            bool consume_pattern_16(global::System.ReadOnlySpan<char> input, out int length)
            {
                /*
                * TokenID: whitespace (#16)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = ResolvedNode { Key = Pattern_62, Order = 128, MinDepth = 1, MaxDepth = 1, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]] }, NodeID = Pattern_62, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsConstantLength = True, HasChildren = True, Condition = OneOf, ConditionJoiner =  or , MinLength = 1 }
                * CONSUME: PatternGroup { DependencyInfo = ResolvedNode { Key = Pattern_62, Order = 128, MinDepth = 1, MaxDepth = 1, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]] }, NodeID = Pattern_62, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsConstantLength = True, HasChildren = True, Condition = OneOf, ConditionJoiner =  or , MinLength = 1 }
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
            bool consume_pattern_17(global::System.ReadOnlySpan<char> input, out int length)
            {
                /*
                * TokenID: digits (#17)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = ResolvedNode { Key = Pattern_64, Order = 129, MinDepth = 1, MaxDepth = 1, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]] }, NodeID = Pattern_64, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsConstantLength = True, HasChildren = True, Condition = OneOf, ConditionJoiner =  or , MinLength = 1 }
                * CONSUME: PatternGroup { DependencyInfo = ResolvedNode { Key = Pattern_64, Order = 129, MinDepth = 1, MaxDepth = 1, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]] }, NodeID = Pattern_64, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsConstantLength = True, HasChildren = True, Condition = OneOf, ConditionJoiner =  or , MinLength = 1 }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer = input.Slice(1);
                
                while (buffer.Length > 0)
                {
                    if (buffer is [ (>='0' and <='9'), ..])
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
            bool consume_pattern_18(global::System.ReadOnlySpan<char> input, out int length)
            {
                /*
                * TokenID: letters (#18)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = ResolvedNode { Key = Pattern_67, Order = 130, MinDepth = 1, MaxDepth = 1, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]] }, NodeID = Pattern_67, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsConstantLength = True, HasChildren = True, Condition = OneOf, ConditionJoiner =  or , MinLength = 1 }
                * CONSUME: PatternGroup { DependencyInfo = ResolvedNode { Key = Pattern_67, Order = 130, MinDepth = 1, MaxDepth = 1, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]] }, NodeID = Pattern_67, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsConstantLength = True, HasChildren = True, Condition = OneOf, ConditionJoiner =  or , MinLength = 1 }
                */
                /* Consume the START sequence which got us here in the first place, we already know its part of the token */
                var buffer = input.Slice(1);
                
                while (buffer.Length > 0)
                {
                    if (buffer is [ ((>='a' and <='z') or (>='A' and <='Z')), ..])
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
            bool consume_pattern_19(global::System.ReadOnlySpan<char> input, out int length)
            {
                /*
                * TokenID: newline (#19)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = ResolvedNode { Key = Pattern_70, Order = 131, MinDepth = 1, MaxDepth = 1, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]] }, NodeID = Pattern_70, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsConstantLength = True, HasChildren = True, Condition = OneOf, ConditionJoiner =  or , MinLength = 1 }
                * CONSUME: PatternGroup { DependencyInfo = ResolvedNode { Key = Pattern_70, Order = 131, MinDepth = 1, MaxDepth = 1, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]] }, NodeID = Pattern_70, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsConstantLength = True, HasChildren = True, Condition = OneOf, ConditionJoiner =  or , MinLength = 1 }
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
            bool consume_pattern_20(global::System.ReadOnlySpan<char> input, out int length)
            {
                /*
                * TokenID: identifier (#20)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = ResolvedNode { Key = Pattern_81, Order = 156, MinDepth = 2, MaxDepth = 2, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]] }, NodeID = Pattern_81, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsRawValues = False, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 2 }
                * CONSUME: PatternGroup { DependencyInfo = ResolvedNode { Key = Pattern_80, Order = 133, MinDepth = 1, MaxDepth = 1, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]] }, NodeID = Pattern_80, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = False, IsConstantLength = True, HasChildren = True, Condition = OneOf, ConditionJoiner =  or , MinLength = 1 }
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
            bool consume_pattern_26(global::System.ReadOnlySpan<char> input, out int length)
            {
                /*
                * TokenID: comment (#22)
                * ==[ CONSUMER_DATA ]==
                * START: PatternGroup { DependencyInfo = ResolvedNode { Key = Pattern_95, Order = 157, MinDepth = 2, MaxDepth = 2, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]] }, NodeID = Pattern_95, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 2 }
                * STOP: PatternGroup { DependencyInfo = ResolvedNode { Key = Pattern_99, Order = 158, MinDepth = 2, MaxDepth = 2, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]] }, NodeID = Pattern_99, Registry = MetaParser.Core.MetaParserRegistry, Length = 2, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 2 }
                * ESCAPE: PatternGroup { DependencyInfo = ResolvedNode { Key = Pattern_101, Order = 136, MinDepth = 1, MaxDepth = 1, IsRecursive = False, Incoming = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]], Outgoing = System.Collections.Immutable.ImmutableHashSet`1[MetaParser.Graphs.DirectedGraph`1+ResolvedNode[MetaParser.Graphs.NodeKey]] }, NodeID = Pattern_101, Registry = MetaParser.Core.MetaParserRegistry, Length = 1, IsRawValues = True, IsConstantLength = True, HasChildren = True, Condition = AllOf, ConditionJoiner = , , MinLength = 1 }
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
