//HintName: MetaParser.MetaParser.parser.consumers.g.cs
namespace UnitTestParser;
public sealed partial class Parser
{
    private static ConsumerResult consume_pattern_26(global::System.ReadOnlySpan<char> buffer0)
    {
        /*
        * TokenID: comment (#26)
        * ==[ CONSUMER_DATA ]==
        * START: PatternSequence { GraphInfo = NodeData { Order = 173, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_108, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        * STOP: PatternSequence { GraphInfo = NodeData { Order = 174, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_111, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        * ESCAPE: PatternSequence { GraphInfo = NodeData { Order = 175, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_113, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
        
        if (buffer1.Length > 0)
        {
            if (buffer1.StartsWith(stackalloc []{ '*', '/' }))
            {
                return new (TokenId.Lexer_Comment, 2 + (buffer0.Length - buffer1.Length));
            }
        }
        
        return new (default, default);
    }
    private static ConsumerResult consume_pattern_38(global::System.ReadOnlySpan<byte> buffer0)
    {
        /*
        * TokenID: string_multi_line (#29)
        * ==[ CONSUMER_DATA ]==
        * START: PatternSequence { GraphInfo = NodeData { Order = 197, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_162, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        * STOP: PatternSequence { GraphInfo = NodeData { Order = 198, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_164, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        * ESCAPE: PatternSequence { GraphInfo = NodeData { Order = 199, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_166, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        */
        /* Consume the START sequence which got us here in the first place, we already know its part of the token */
        var buffer1 = buffer0.Slice(2);
        while (buffer1.Length > 0)
        {
            if (buffer1[0] == TokenId.Lexer_Char_Reverse_Solidus)
            {
                /* look past the ESCAPE sequence */
                var buffer2 = buffer1.Slice(1);
                /* if the stop sequence immediately follows the ESCAPE sequence then they are consumed */
                if (buffer2[0] == TokenId.Lexer_Char_Double_Quote)
                {
                    buffer1 = buffer2.Slice(1);
                    continue;
                }
            }
            
            if (buffer1[0] == TokenId.Lexer_Char_Double_Quote)
            /* If we have a STOP sequence, check for it */
            {
                /* end */
                break;
            }
            
            /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
            buffer1 = buffer1.Slice(1);
        }
        
        if (buffer1.Length > 0)
        {
            if (buffer1[0] == TokenId.Lexer_Char_Double_Quote)
            {
                return new (TokenId.Syntax_String_Multi_Line, 1 + (buffer0.Length - buffer1.Length));
            }
        }
        
        return new (default, default);
    }
    private static ConsumerResult consume_pattern_37(global::System.ReadOnlySpan<byte> buffer0)
    {
        /*
        * TokenID: string_multi_line (#29)
        * ==[ CONSUMER_DATA ]==
        * START: PatternSequence { GraphInfo = NodeData { Order = 194, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_155, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        * STOP: PatternSequence { GraphInfo = NodeData { Order = 195, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_157, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        * ESCAPE: PatternSequence { GraphInfo = NodeData { Order = 196, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_159, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        */
        /* Consume the START sequence which got us here in the first place, we already know its part of the token */
        var buffer1 = buffer0.Slice(2);
        while (buffer1.Length > 0)
        {
            if (buffer1[0] == TokenId.Lexer_Char_Reverse_Solidus)
            {
                /* look past the ESCAPE sequence */
                var buffer2 = buffer1.Slice(1);
                /* if the stop sequence immediately follows the ESCAPE sequence then they are consumed */
                if (buffer2[0] == TokenId.Lexer_Char_Single_Quote)
                {
                    buffer1 = buffer2.Slice(1);
                    continue;
                }
            }
            
            if (buffer1[0] == TokenId.Lexer_Char_Single_Quote)
            /* If we have a STOP sequence, check for it */
            {
                /* end */
                break;
            }
            
            /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
            buffer1 = buffer1.Slice(1);
        }
        
        if (buffer1.Length > 0)
        {
            if (buffer1[0] == TokenId.Lexer_Char_Single_Quote)
            {
                return new (TokenId.Syntax_String_Multi_Line, 1 + (buffer0.Length - buffer1.Length));
            }
        }
        
        return new (default, default);
    }
    private static ConsumerResult consume_pattern_40(global::System.ReadOnlySpan<byte> buffer0)
    {
        /*
        * TokenID: comment (#30)
        * ==[ CONSUMER_DATA ]==
        * START: PatternSequence { GraphInfo = NodeData { Order = 202, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_174, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        * STOP: PatternSequence { GraphInfo = NodeData { Order = 203, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_177, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        * ESCAPE: PatternSequence { GraphInfo = NodeData { Order = 204, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_179, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        */
        /* Consume the START sequence which got us here in the first place, we already know its part of the token */
        var buffer1 = buffer0.Slice(2);
        while (buffer1.Length > 0)
        {
            if (buffer1[0] == TokenId.Lexer_Char_Reverse_Solidus)
            {
                /* look past the ESCAPE sequence */
                var buffer2 = buffer1.Slice(1);
                /* if the stop sequence immediately follows the ESCAPE sequence then they are consumed */
                if (buffer2.StartsWith(stackalloc []{ TokenId.Lexer_Char_Asterisk, TokenId.Lexer_Char_Solidus }))
                {
                    buffer1 = buffer2.Slice(2);
                    continue;
                }
            }
            
            if (buffer1.StartsWith(stackalloc []{ TokenId.Lexer_Char_Asterisk, TokenId.Lexer_Char_Solidus }))
            /* If we have a STOP sequence, check for it */
            {
                /* end */
                break;
            }
            
            /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
            buffer1 = buffer1.Slice(1);
        }
        
        if (buffer1.Length > 0)
        {
            if (buffer1.StartsWith(stackalloc []{ TokenId.Lexer_Char_Asterisk, TokenId.Lexer_Char_Solidus }))
            {
                return new (TokenId.Syntax_Comment, 2 + (buffer0.Length - buffer1.Length));
            }
        }
        
        return new (default, default);
    }
    private static ConsumerResult consume_pattern_39(global::System.ReadOnlySpan<byte> buffer0)
    {
        /*
        * TokenID: comment (#30)
        * ==[ CONSUMER_DATA ]==
        * START: PatternSequence { GraphInfo = NodeData { Order = 200, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_169, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        * STOP: PatternSequence { GraphInfo = NodeData { Order = 201, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_171, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        */
        /* Consume the START sequence which got us here in the first place, we already know its part of the token */
        var buffer1 = buffer0.Slice(2);
        while (buffer1.Length > 0)
        {
            if (buffer1[0] == TokenId.Lexer_Newline)
            /* If we have a STOP sequence, check for it */
            {
                /* end */
                break;
            }
            
            /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
            buffer1 = buffer1.Slice(1);
        }
        
        if (buffer1.Length > 0)
        {
            if (buffer1[0] == TokenId.Lexer_Newline)
            {
                return new (TokenId.Syntax_Comment, 1 + (buffer0.Length - buffer1.Length));
            }
        }
        
        return new (default, default);
    }
    private static ConsumerResult consume_pattern_41(global::System.ReadOnlySpan<byte> buffer0)
    {
        /*
        * TokenID: declaration (#31)
        * ==[ CONSUMER_DATA ]==
        * START: PatternSequence { GraphInfo = NodeData { Order = 205, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_182, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        * STOP: PatternSequence { GraphInfo = NodeData { Order = 206, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_184, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        */
        /* Consume the START sequence which got us here in the first place, we already know its part of the token */
        var buffer1 = buffer0.Slice(2);
        while (buffer1.Length > 0)
        {
            if (buffer1[0] == TokenId.Lexer_Char_Semicolon)
            /* If we have a STOP sequence, check for it */
            {
                /* end */
                break;
            }
            
            /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
            buffer1 = buffer1.Slice(1);
        }
        
        if (buffer1.Length > 0)
        {
            if (buffer1[0] == TokenId.Lexer_Char_Semicolon)
            {
                return new (TokenId.Syntax_Declaration, 1 + (buffer0.Length - buffer1.Length));
            }
        }
        
        return new (default, default);
    }
    private static ConsumerResult consume_pattern_27(global::System.ReadOnlySpan<char> buffer0)
    {
        /*
        * TokenID: comment (#26)
        * ==[ CONSUMER_DATA ]==
        * START: PatternSequence { GraphInfo = NodeData { Order = 176, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_116, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        * STOP: PatternSequence { GraphInfo = NodeData { Order = 177, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_118, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        * ESCAPE: PatternSequence { GraphInfo = NodeData { Order = 178, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_120, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
        
        if (buffer1.Length > 0)
        {
            if (buffer1[0] == '\n')
            {
                return new (TokenId.Lexer_Comment, 1 + (buffer0.Length - buffer1.Length));
            }
        }
        
        return new (default, default);
    }
    private static ConsumerResult consume_pattern_1(global::System.ReadOnlySpan<char> buffer0)
    {
        /*
        * TokenID: newline (#1)
        * ==[ CONSUMER_DATA ]==
        * START: PatternSequence { GraphInfo = NodeData { Order = 144, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_10, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 2, MaxConditions = 2, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
        * CONSUME: PatternSequence { GraphInfo = NodeData { Order = 145, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_13, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 2, MaxConditions = 2, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
        
        return new (TokenId.Lexer_Newline, buffer0.Length - buffer1.Length);
    }
    private static ConsumerResult consume_pattern_0(global::System.ReadOnlySpan<char> buffer0)
    {
        /*
        * TokenID: whitespace (#0)
        * ==[ CONSUMER_DATA ]==
        * START: PatternSequence { GraphInfo = NodeData { Order = 142, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_3, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 3, MaxConditions = 3, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
        * CONSUME: PatternSequence { GraphInfo = NodeData { Order = 143, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_7, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 3, MaxConditions = 3, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
        
        return new (TokenId.Lexer_Whitespace, buffer0.Length - buffer1.Length);
    }
    private static ConsumerResult consume_pattern_25(global::System.ReadOnlySpan<char> buffer0)
    {
        /*
        * TokenID: identifier (#25)
        * ==[ CONSUMER_DATA ]==
        * START: PatternSequence { GraphInfo = NodeData { Order = 213, Depth = 2, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_99, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 7, MaxConditions = 7, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        * CONSUME: PatternSequence { GraphInfo = NodeData { Order = 172, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_105, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 5, MaxConditions = 5, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
        
        return new (TokenId.Lexer_Identifier, buffer0.Length - buffer1.Length);
    }
    private static ConsumerResult consume_pattern_43(global::System.ReadOnlySpan<byte> buffer0)
    {
        /*
        * TokenID: program (#33)
        * ==[ CONSUMER_DATA ]==
        * START: PatternSequence { GraphInfo = NodeData { Order = 216, Depth = 2, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_205, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 12, MaxConditions = 12, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        * CONSUME: PatternSequence { GraphInfo = NodeData { Order = 212, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_216, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 10, MaxConditions = 10, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
        */
        /* Consume the START sequence which got us here in the first place, we already know its part of the token */
        var buffer1 = buffer0.Slice(2);
        
        while (buffer1.Length > 0)
        {
            if (buffer1 is [ (TokenId.Lexer_Whitespace or TokenId.Lexer_Identifier or TokenId.Lexer_Whitespace or TokenId.Lexer_Char_Open_Parenthesis or TokenId.Lexer_Whitespace or TokenId.Lexer_Char_Close_Parenthesis or TokenId.Lexer_Whitespace or TokenId.Lexer_Char_Open_Bracket or TokenId.Lexer_Whitespace or TokenId.Lexer_Char_Close_Bracket), ..])
            /* If we have a set of valid CONSUME targets, then try and consume as many as possible (STOP sequence should be mutually exclusive with CONSUME sequence) */
            {
                buffer1 = buffer1.Slice(1);
                /* consumer forces moving on to next loop */
                continue;
            }
            
            /* otherwise, default behaviour is to exit loop */
            break;
        }
        
        return new (TokenId.Syntax_Program, buffer0.Length - buffer1.Length);
    }
    private static ConsumerResult consume_pattern_36(global::System.ReadOnlySpan<byte> buffer0)
    {
        /*
        * TokenID: string_single_line (#28)
        * ==[ CONSUMER_DATA ]==
        * START: PatternSequence { GraphInfo = NodeData { Order = 190, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_145, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        * CONSUME: PatternSequence { GraphInfo = NodeData { Order = 215, Depth = 2, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_148, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
        * STOP: PatternSequence { GraphInfo = NodeData { Order = 192, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_150, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        * ESCAPE: PatternSequence { GraphInfo = NodeData { Order = 193, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_152, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        */
        /* Consume the START sequence which got us here in the first place, we already know its part of the token */
        var buffer1 = buffer0.Slice(1);
        while (buffer1.Length > 0)
        {
            if (buffer1[0] == TokenId.Lexer_Char_Reverse_Solidus)
            {
                /* look past the ESCAPE sequence */
                var buffer2 = buffer1.Slice(1);
                /* if the stop sequence immediately follows the ESCAPE sequence then they are consumed */
                if (buffer2[0] == TokenId.Lexer_Char_Double_Quote)
                {
                    buffer1 = buffer2.Slice(1);
                    continue;
                }
            }
            
            if (buffer1[0] == TokenId.Lexer_Char_Double_Quote)
            /* If we have a STOP sequence, check for it */
            {
                /* end */
                break;
            }
            
            
            while (buffer1.Length > 0)
            {
                if (buffer1 is [ not TokenId.Lexer_Newline, ..])
                /* If we have a set of valid CONSUME targets, then try and consume as many as possible (STOP sequence should be mutually exclusive with CONSUME sequence) */
                {
                    buffer1 = buffer1.Slice(1);
                    /* consumer forces moving on to next loop */
                    continue;
                }
                
                /* otherwise, default behaviour is to exit loop */
                break;
            }
            
        }
        
        if (buffer1.Length > 0)
        {
            if (buffer1[0] == TokenId.Lexer_Char_Double_Quote)
            {
                return new (TokenId.Syntax_String_Single_Line, 1 + (buffer0.Length - buffer1.Length));
            }
        }
        
        return new (default, default);
    }
    private static ConsumerResult consume_pattern_35(global::System.ReadOnlySpan<byte> buffer0)
    {
        /*
        * TokenID: string_single_line (#28)
        * ==[ CONSUMER_DATA ]==
        * START: PatternSequence { GraphInfo = NodeData { Order = 186, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_136, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        * CONSUME: PatternSequence { GraphInfo = NodeData { Order = 214, Depth = 2, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_139, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
        * STOP: PatternSequence { GraphInfo = NodeData { Order = 188, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_141, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        * ESCAPE: PatternSequence { GraphInfo = NodeData { Order = 189, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_143, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        */
        /* Consume the START sequence which got us here in the first place, we already know its part of the token */
        var buffer1 = buffer0.Slice(1);
        while (buffer1.Length > 0)
        {
            if (buffer1[0] == TokenId.Lexer_Char_Reverse_Solidus)
            {
                /* look past the ESCAPE sequence */
                var buffer2 = buffer1.Slice(1);
                /* if the stop sequence immediately follows the ESCAPE sequence then they are consumed */
                if (buffer2[0] == TokenId.Lexer_Char_Single_Quote)
                {
                    buffer1 = buffer2.Slice(1);
                    continue;
                }
            }
            
            if (buffer1[0] == TokenId.Lexer_Char_Single_Quote)
            /* If we have a STOP sequence, check for it */
            {
                /* end */
                break;
            }
            
            
            while (buffer1.Length > 0)
            {
                if (buffer1 is [ not TokenId.Lexer_Newline, ..])
                /* If we have a set of valid CONSUME targets, then try and consume as many as possible (STOP sequence should be mutually exclusive with CONSUME sequence) */
                {
                    buffer1 = buffer1.Slice(1);
                    /* consumer forces moving on to next loop */
                    continue;
                }
                
                /* otherwise, default behaviour is to exit loop */
                break;
            }
            
        }
        
        if (buffer1.Length > 0)
        {
            if (buffer1[0] == TokenId.Lexer_Char_Single_Quote)
            {
                return new (TokenId.Syntax_String_Single_Line, 1 + (buffer0.Length - buffer1.Length));
            }
        }
        
        return new (default, default);
    }
    private static ConsumerResult consume_pattern_42(global::System.ReadOnlySpan<byte> buffer0)
    {
        /*
        * TokenID: codeblock (#32)
        * ==[ CONSUMER_DATA ]==
        * START: PatternSequence { GraphInfo = NodeData { Order = 207, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_186, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        * CONSUME: PatternSequence { GraphInfo = NodeData { Order = 208, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_188, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = False, IsConstantLength = True, IsSequence = False, MinConditions = 1, MaxConditions = 1, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
        * STOP: PatternSequence { GraphInfo = NodeData { Order = 209, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_190, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
        */
        /* Consume the START sequence which got us here in the first place, we already know its part of the token */
        var buffer1 = buffer0.Slice(1);
        while (buffer1.Length > 0)
        {
            if (buffer1[0] == TokenId.Lexer_Char_Close_Bracket)
            /* If we have a STOP sequence, check for it */
            {
                /* end */
                break;
            }
            
            
            while (buffer1.Length > 0)
            {
                if (starts_syntax_declaration_token(buffer1))
                /* If we have a set of valid CONSUME targets, then try and consume as many as possible (STOP sequence should be mutually exclusive with CONSUME sequence) */
                {
                    buffer1 = buffer1.Slice(1);
                    /* consumer forces moving on to next loop */
                    continue;
                }
                
                /* otherwise, default behaviour is to exit loop */
                break;
            }
            
        }
        
        if (buffer1.Length > 0)
        {
            if (buffer1[0] == TokenId.Lexer_Char_Close_Bracket)
            {
                return new (TokenId.Syntax_Codeblock, 1 + (buffer0.Length - buffer1.Length));
            }
        }
        
        return new (default, default);
    }
    private static ConsumerResult consume_pattern_24(global::System.ReadOnlySpan<char> buffer0)
    {
        /*
        * TokenID: digits (#24)
        * ==[ CONSUMER_DATA ]==
        * START: PatternSequence { GraphInfo = NodeData { Order = 168, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_87, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 2, MaxConditions = 2, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
        * CONSUME: PatternSequence { GraphInfo = NodeData { Order = 169, Depth = 1, IsRecursive = False }, HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_89, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 2, MaxConditions = 2, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
        
        return new (TokenId.Lexer_Digits, buffer0.Length - buffer1.Length);
    }
}
