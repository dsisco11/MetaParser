//HintName: MetaParser.MetaParser.parser.consumers.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static ConsumerResult consume_pattern_17(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: whitespace (#17)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_65 | Order: 130 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_65, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_69 | Order: 131 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_69, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
        private static ConsumerResult consume_pattern_19(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: newline (#19)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_76 | Order: 134 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_76, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_79 | Order: 135 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_79, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
        private static ConsumerResult consume_pattern_18(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: digits (#18)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_71 | Order: 132 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_71, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 2, MaxConditions = 2, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_73 | Order: 133 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_73, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 2, MaxConditions = 2, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            */
            /* Consume the START sequence which got us here in the first place, we already know its part of the token */
            var buffer1 = buffer0.Slice(1);
            
            while (buffer1.Length > 0)
            {
                if (buffer1 is [ (>="0" and <="9"), ..])
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
        private static ConsumerResult consume_pattern_22(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: comment (#21)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_109 | Order: 186 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_109, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_111 | Order: 143 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_111, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * ESCAPE: PatternSequence { DependencyInfo = Pattern_113 | Order: 144 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_113, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            */
            /* Consume the START sequence which got us here in the first place, we already know its part of the token */
            var buffer1 = buffer0.Slice(1);
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
            
            if (buffer1[0] == '\n')
            {
                return new (TokenId.Lexer_Comment, 1 + (buffer0.Length - buffer1.Length));
            }
            
            return new (default, default);
        }
        private static ConsumerResult consume_pattern_21(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: comment (#21)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_99 | Order: 184 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_99, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_103 | Order: 185 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_103, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * ESCAPE: PatternSequence { DependencyInfo = Pattern_105 | Order: 141 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_105, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            */
            /* Consume the START sequence which got us here in the first place, we already know its part of the token */
            var buffer1 = buffer0.Slice(1);
            while (buffer1.Length > 0)
            {
                if (buffer1[0] == '\\')
                {
                    /* look past the ESCAPE sequence */
                    var buffer2 = buffer1.Slice(1);
                    /* if the stop sequence immediately follows the ESCAPE sequence then they are consumed */
                    if (buffer2[0] == '*', '/')
                    {
                        buffer1 = buffer2.Slice(1);
                        continue;
                    }
                }
                
                if (buffer1[0] == '*', '/')
                /* If we have a STOP sequence, check for it */
                {
                    /* end */
                    break;
                }
                
                /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
                buffer1 = buffer1.Slice(1);
            }
            
            if (buffer1[0] == '*', '/')
            {
                return new (TokenId.Lexer_Comment, 1 + (buffer0.Length - buffer1.Length));
            }
            
            return new (default, default);
        }
        private static ConsumerResult consume_pattern_20(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: identifier (#20)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_89 | Order: 183 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_89, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 2, MaxConditions = 4, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_95 | Order: 138 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_95, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 2, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            */
            /* Consume the START sequence which got us here in the first place, we already know its part of the token */
            var buffer1 = buffer0.Slice(2);
            
            while (buffer1.Length > 0)
            {
                if (buffer1 is [ ((>="a" and <="z") or (>="A" and <="Z") or (>="0" and <="9") or '-' or '_'), ..])
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
        private static ConsumerResult consume_pattern_29(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: comment (#23)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_131 | Order: 152 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_131, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_134 | Order: 153 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_134, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * ESCAPE: PatternSequence { DependencyInfo = Pattern_136 | Order: 154 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_136, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
                return new (TokenId.Syntax_Comment, 2 + (buffer0.Length - buffer1.Length));
            }
            
            return new (default, default);
        }
        private static ConsumerResult consume_pattern_30(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: declaration (#24)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_139 | Order: 155 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_139, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_141 | Order: 156 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_141, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
                return new (TokenId.Syntax_Declaration, 1 + (buffer0.Length - buffer1.Length));
            }
            
            return new (default, default);
        }
        private static ConsumerResult consume_pattern_28(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: comment (#23)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_126 | Order: 150 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_126, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_128 | Order: 151 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_128, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
                return new (TokenId.Syntax_Comment, 1 + (buffer0.Length - buffer1.Length));
            }
            
            return new (default, default);
        }
        private static ConsumerResult consume_pattern_32(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: program (#26)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_162 | Order: 196 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_162, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_173 | Order: 162 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_173, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
            
            return new (TokenId.Syntax_Program, buffer0.Length - buffer1.Length);
        }
        private static ConsumerResult consume_pattern_31(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: codeblock (#25)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_143 | Order: 157 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_143, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_145 | Order: 158 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_145, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = False, IsConstantLength = True, IsSequence = True, MinConditions = 1, MaxConditions = 1, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            * STOP: PatternSequence { DependencyInfo = Pattern_147 | Order: 159 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_147, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            */
            /* Consume the START sequence which got us here in the first place, we already know its part of the token */
            var buffer1 = buffer0.Slice(1);
            while (buffer1.Length > 0)
            {
                if (buffer1[0] == TokenId.Char_Close_Bracket)
                /* If we have a STOP sequence, check for it */
                {
                    /* end */
                    break;
                }
                
                
                while (buffer1.Length > 0)
                {
                    if (starts_declaration_token(buffer1))
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
            
            if (buffer1[0] == TokenId.Char_Close_Bracket)
            {
                return new (TokenId.Syntax_Codeblock, 1 + (buffer0.Length - buffer1.Length));
            }
            
            return new (default, default);
        }
    }
}
