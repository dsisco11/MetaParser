//HintName: MetaParser.MetaParser.parser.consumers.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static ConsumerResult consume_pattern_26(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: comment (#26)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_117 | Order: 239 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_117, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_121 | Order: 240 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_121, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * ESCAPE: PatternSequence { DependencyInfo = Pattern_123 | Order: 175 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_123, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
            
            if (buffer1.StartsWith(stackalloc []{ '*', '/' }))
            {
                return new (TokenId.Lexer_Comment, 2 + (buffer0.Length - buffer1.Length));
            }
            
            return new (default, default);
        }
        private static ConsumerResult consume_pattern_27(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: comment (#26)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_127 | Order: 241 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_127, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_129 | Order: 177 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_129, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * ESCAPE: PatternSequence { DependencyInfo = Pattern_131 | Order: 178 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_131, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
            
            if (buffer1[0] == '\n')
            {
                return new (TokenId.Lexer_Comment, 1 + (buffer0.Length - buffer1.Length));
            }
            
            return new (default, default);
        }
        private static ConsumerResult consume_pattern_24(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: digits (#24)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_95 | Order: 168 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_95, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 2, MaxConditions = 2, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_97 | Order: 169 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_97, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 2, MaxConditions = 2, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
        private static ConsumerResult consume_pattern_1(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: newline (#1)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_10 | Order: 144 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_10, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 2, MaxConditions = 2, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_13 | Order: 145 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_13, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 2, MaxConditions = 2, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
            * START: PatternSequence { DependencyInfo = Pattern_3 | Order: 142 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_3, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 3, MaxConditions = 3, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_7 | Order: 143 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_7, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 3, MaxConditions = 3, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
            * START: PatternSequence { DependencyInfo = Pattern_107 | Order: 238 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_107, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 7, MaxConditions = 7, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_113 | Order: 172 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_113, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 5, MaxConditions = 5, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
        private static ConsumerResult consume_pattern_35(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: string_single_line (#28)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_147 | Order: 186 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_147, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_150 | Order: 249 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_150, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            * STOP: PatternSequence { DependencyInfo = Pattern_152 | Order: 188 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_152, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * ESCAPE: PatternSequence { DependencyInfo = Pattern_154 | Order: 189 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_154, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            */
            /* Consume the START sequence which got us here in the first place, we already know its part of the token */
            var buffer1 = buffer0.Slice(1);
            while (buffer1.Length > 0)
            {
                if (buffer1[0] == TokenId.Char_Reverse_Solidus)
                {
                    /* look past the ESCAPE sequence */
                    var buffer2 = buffer1.Slice(1);
                    /* if the stop sequence immediately follows the ESCAPE sequence then they are consumed */
                    if (buffer2[0] == TokenId.Char_Single_Quote)
                    {
                        buffer1 = buffer2.Slice(1);
                        continue;
                    }
                }
                
                if (buffer1[0] == TokenId.Char_Single_Quote)
                /* If we have a STOP sequence, check for it */
                {
                    /* end */
                    break;
                }
                
                
                while (buffer1.Length > 0)
                {
                    if (buffer1 is [ not TokenId.Newline, ..])
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
            
            if (buffer1[0] == TokenId.Char_Single_Quote)
            {
                return new (TokenId.Syntax_String_Single_Line, 1 + (buffer0.Length - buffer1.Length));
            }
            
            return new (default, default);
        }
        private static ConsumerResult consume_pattern_36(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: string_single_line (#28)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_156 | Order: 190 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_156, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_159 | Order: 250 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_159, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            * STOP: PatternSequence { DependencyInfo = Pattern_161 | Order: 192 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_161, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * ESCAPE: PatternSequence { DependencyInfo = Pattern_163 | Order: 193 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_163, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            */
            /* Consume the START sequence which got us here in the first place, we already know its part of the token */
            var buffer1 = buffer0.Slice(1);
            while (buffer1.Length > 0)
            {
                if (buffer1[0] == TokenId.Char_Reverse_Solidus)
                {
                    /* look past the ESCAPE sequence */
                    var buffer2 = buffer1.Slice(1);
                    /* if the stop sequence immediately follows the ESCAPE sequence then they are consumed */
                    if (buffer2[0] == TokenId.Char_Double_Quote)
                    {
                        buffer1 = buffer2.Slice(1);
                        continue;
                    }
                }
                
                if (buffer1[0] == TokenId.Char_Double_Quote)
                /* If we have a STOP sequence, check for it */
                {
                    /* end */
                    break;
                }
                
                
                while (buffer1.Length > 0)
                {
                    if (buffer1 is [ not TokenId.Newline, ..])
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
            
            if (buffer1[0] == TokenId.Char_Double_Quote)
            {
                return new (TokenId.Syntax_String_Single_Line, 1 + (buffer0.Length - buffer1.Length));
            }
            
            return new (default, default);
        }
        private static ConsumerResult consume_pattern_37(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: string_multi_line (#29)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_166 | Order: 194 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_166, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_168 | Order: 195 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_168, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * ESCAPE: PatternSequence { DependencyInfo = Pattern_170 | Order: 196 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_170, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
                    if (buffer2[0] == TokenId.Char_Single_Quote)
                    {
                        buffer1 = buffer2.Slice(1);
                        continue;
                    }
                }
                
                if (buffer1[0] == TokenId.Char_Single_Quote)
                /* If we have a STOP sequence, check for it */
                {
                    /* end */
                    break;
                }
                
                /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
                buffer1 = buffer1.Slice(1);
            }
            
            if (buffer1[0] == TokenId.Char_Single_Quote)
            {
                return new (TokenId.Syntax_String_Multi_Line, 1 + (buffer0.Length - buffer1.Length));
            }
            
            return new (default, default);
        }
        private static ConsumerResult consume_pattern_38(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: string_multi_line (#29)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_173 | Order: 197 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_173, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_175 | Order: 198 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_175, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * ESCAPE: PatternSequence { DependencyInfo = Pattern_177 | Order: 199 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_177, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
                    if (buffer2[0] == TokenId.Char_Double_Quote)
                    {
                        buffer1 = buffer2.Slice(1);
                        continue;
                    }
                }
                
                if (buffer1[0] == TokenId.Char_Double_Quote)
                /* If we have a STOP sequence, check for it */
                {
                    /* end */
                    break;
                }
                
                /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
                buffer1 = buffer1.Slice(1);
            }
            
            if (buffer1[0] == TokenId.Char_Double_Quote)
            {
                return new (TokenId.Syntax_String_Multi_Line, 1 + (buffer0.Length - buffer1.Length));
            }
            
            return new (default, default);
        }
        private static ConsumerResult consume_pattern_39(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: comment (#30)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_180 | Order: 200 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_180, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_182 | Order: 201 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_182, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
        private static ConsumerResult consume_pattern_40(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: comment (#30)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_185 | Order: 202 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_185, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_188 | Order: 203 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_188, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * ESCAPE: PatternSequence { DependencyInfo = Pattern_190 | Order: 204 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_190, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
        private static ConsumerResult consume_pattern_41(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: declaration (#31)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_193 | Order: 205 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_193, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_195 | Order: 206 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_195, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
        private static ConsumerResult consume_pattern_43(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: program (#33)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_216 | Order: 257 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_216, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 12, MaxConditions = 12, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_227 | Order: 212 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_227, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 10, MaxConditions = 10, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
        private static ConsumerResult consume_pattern_42(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: codeblock (#32)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_197 | Order: 207 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_197, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_199 | Order: 208 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_199, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = False, IsConstantLength = True, IsSequence = False, MinConditions = 1, MaxConditions = 1, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            * STOP: PatternSequence { DependencyInfo = Pattern_201 | Order: 209 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_201, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = False, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
