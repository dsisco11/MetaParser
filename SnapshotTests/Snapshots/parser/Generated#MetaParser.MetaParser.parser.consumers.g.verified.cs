//HintName: MetaParser.MetaParser.parser.consumers.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static ConsumerResult consume_pattern_1(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: newline (#1)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_10 | Order: 125 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_10, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_13 | Order: 126 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_13, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
            * START: PatternSequence { DependencyInfo = Pattern_3 | Order: 123 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_3, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_7 | Order: 124 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_7, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
        private static ConsumerResult consume_pattern_21(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: digits (#21)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_89 | Order: 146 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_89, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 2, MaxConditions = 2, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_91 | Order: 147 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_91, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 2, MaxConditions = 2, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
        private static ConsumerResult consume_pattern_23(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: comment (#23)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_111 | Order: 200 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_111, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_115 | Order: 201 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_115, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * ESCAPE: PatternSequence { DependencyInfo = Pattern_117 | Order: 153 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_117, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
        private static ConsumerResult consume_pattern_24(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: comment (#23)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_121 | Order: 202 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_121, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_123 | Order: 155 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_123, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * ESCAPE: PatternSequence { DependencyInfo = Pattern_125 | Order: 156 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_125, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
        private static ConsumerResult consume_pattern_22(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: identifier (#22)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_101 | Order: 199 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_101, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 2, MaxConditions = 4, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_107 | Order: 150 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_107, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 2, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
        private static ConsumerResult consume_pattern_32(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: comment (#25)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_142 | Order: 164 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_142, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_144 | Order: 165 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_144, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
        private static ConsumerResult consume_pattern_33(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: comment (#25)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_147 | Order: 166 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_147, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_150 | Order: 167 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_150, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * ESCAPE: PatternSequence { DependencyInfo = Pattern_152 | Order: 168 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_152, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
        private static ConsumerResult consume_pattern_34(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: declaration (#26)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_155 | Order: 169 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_155, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_157 | Order: 170 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_157, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
        private static ConsumerResult consume_pattern_36(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: program (#28)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_178 | Order: 214 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_178, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_189 | Order: 176 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_189, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
        private static ConsumerResult consume_pattern_35(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: codeblock (#27)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_159 | Order: 171 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_159, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_161 | Order: 172 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_161, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = False, IsConstantLength = True, IsSequence = True, MinConditions = 1, MaxConditions = 1, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            * STOP: PatternSequence { DependencyInfo = Pattern_163 | Order: 173 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_163, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
