//HintName: MetaParser.MetaParser.parser.consumers.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static ConsumerResult consume_pattern_35(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: whitespace (#17)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_107 | Order: 196 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_107, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_111 | Order: 197 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_111, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
        private static ConsumerResult consume_pattern_34(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: whitespace (#17)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_99 | Order: 194 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_99, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_103 | Order: 195 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_103, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
        private static ConsumerResult consume_pattern_39(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: newline (#19)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_128 | Order: 204 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_128, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_131 | Order: 205 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_131, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
        private static ConsumerResult consume_pattern_38(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: newline (#19)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_122 | Order: 202 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_122, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_125 | Order: 203 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_125, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
        private static ConsumerResult consume_pattern_63(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: codeblock (#25)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_257 | Order: 236 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_257, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_259 | Order: 237 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_259, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            * STOP: PatternSequence { DependencyInfo = Pattern_261 | Order: 238 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_261, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            */
            /* Consume the START sequence which got us here in the first place, we already know its part of the token */
            var buffer1 = buffer0.Slice(1);
            while (buffer1.Length > 0)
            {
                if (buffer1[0] == "char_close_bracket")
                /* If we have a STOP sequence, check for it */
                {
                    /* end */
                    break;
                }
                
                
                while (buffer1.Length > 0)
                {
                    if (buffer1 is [ "declaration", ..])
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
            
            if (buffer1[0] == "char_close_bracket")
            {
                return new (TokenId.Syntax_Codeblock, 1 + (buffer0.Length - buffer1.Length));
            }
            
            return new (default, default);
        }
        private static ConsumerResult consume_pattern_62(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: codeblock (#25)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_251 | Order: 368 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_251, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_253 | Order: 401 | IsRecursive: True | TreeDepth: [0, 0] | NodeDepth: [0, 0], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_253, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = False, IsConstantLength = True, IsSequence = True, MinConditions = 1, MaxConditions = 1, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            * STOP: PatternSequence { DependencyInfo = Pattern_255 | Order: 369 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_255, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
        private static ConsumerResult consume_pattern_37(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: digits (#18)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_117 | Order: 200 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_117, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 2, MaxConditions = 2, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_119 | Order: 201 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_119, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 2, MaxConditions = 2, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
        private static ConsumerResult consume_pattern_36(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: digits (#18)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_113 | Order: 198 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_113, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 2, MaxConditions = 2, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_115 | Order: 199 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_115, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 2, MaxConditions = 2, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
        private static ConsumerResult consume_pattern_65(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: program (#26)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_302 | Order: 298 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_302, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_313 | Order: 241 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_313, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
            */
            /* Consume the START sequence which got us here in the first place, we already know its part of the token */
            var buffer1 = buffer0.Slice(2);
            
            while (buffer1.Length > 0)
            {
                if (buffer1 is [ ("whitespace" or "identifier" or "whitespace" or "char_open_parenthesis" or "whitespace" or "char_close_parenthesis" or "whitespace" or "char_open_bracket" or "whitespace" or "char_close_bracket"), ..])
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
        private static ConsumerResult consume_pattern_64(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: program (#26)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_276 | Order: 396 | IsRecursive: False | TreeDepth: [6, 7] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_276, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_287 | Order: 389 | IsRecursive: False | TreeDepth: [5, 6] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_287, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
        private static ConsumerResult consume_pattern_40(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: identifier (#20)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_141 | Order: 282 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_141, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 2, MaxConditions = 4, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_147 | Order: 208 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_147, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 2, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
        private static ConsumerResult consume_pattern_41(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: identifier (#20)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_157 | Order: 283 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_157, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 2, MaxConditions = 4, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * CONSUME: PatternSequence { DependencyInfo = Pattern_163 | Order: 211 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_163, Registry = MetaParser.Core.EntityRegistry, Kind = OneOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 2, IsConditional = True, IsDeterministic = False, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner =  or  }
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
        private static ConsumerResult consume_pattern_49(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: comment (#22)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_214 | Order: 226 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_214, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_217 | Order: 227 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_217, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * ESCAPE: PatternSequence { DependencyInfo = Pattern_219 | Order: 228 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_219, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            */
            /* Consume the START sequence which got us here in the first place, we already know its part of the token */
            var buffer1 = buffer0.Slice(2);
            while (buffer1.Length > 0)
            {
                if (buffer1[0] == "char_reverse_solidus")
                {
                    /* look past the ESCAPE sequence */
                    var buffer2 = buffer1.Slice(1);
                    /* if the stop sequence immediately follows the ESCAPE sequence then they are consumed */
                    if (buffer2.StartsWith(stackalloc []{ "char_asterisk", "char_solidus" }))
                    {
                        buffer1 = buffer2.Slice(2);
                        continue;
                    }
                }
                
                if (buffer1.StartsWith(stackalloc []{ "char_asterisk", "char_solidus" }))
                /* If we have a STOP sequence, check for it */
                {
                    /* end */
                    break;
                }
                
                /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
                buffer1 = buffer1.Slice(1);
            }
            
            if (buffer1.StartsWith(stackalloc []{ "char_asterisk", "char_solidus" }))
            {
                return new (TokenId.Syntax_Comment, 2 + (buffer0.Length - buffer1.Length));
            }
            
            return new (default, default);
        }
        private static ConsumerResult consume_pattern_47(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: comment (#22)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_201 | Order: 372 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_201, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_204 | Order: 373 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_204, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * ESCAPE: PatternSequence { DependencyInfo = Pattern_206 | Order: 374 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_206, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
        private static ConsumerResult consume_pattern_61(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: declaration (#24)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_247 | Order: 234 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_247, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_249 | Order: 235 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_249, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            */
            /* Consume the START sequence which got us here in the first place, we already know its part of the token */
            var buffer1 = buffer0.Slice(2);
            while (buffer1.Length > 0)
            {
                if (buffer1[0] == "char_semicolon")
                /* If we have a STOP sequence, check for it */
                {
                    /* end */
                    break;
                }
                
                /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
                buffer1 = buffer1.Slice(1);
            }
            
            if (buffer1[0] == "char_semicolon")
            {
                return new (TokenId.Syntax_Declaration, 1 + (buffer0.Length - buffer1.Length));
            }
            
            return new (default, default);
        }
        private static ConsumerResult consume_pattern_60(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: declaration (#24)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_242 | Order: 387 | IsRecursive: False | TreeDepth: [5, 6] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_242, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_244 | Order: 370 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_244, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
        private static ConsumerResult consume_pattern_48(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: comment (#22)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_209 | Order: 224 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_209, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_211 | Order: 225 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_211, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            */
            /* Consume the START sequence which got us here in the first place, we already know its part of the token */
            var buffer1 = buffer0.Slice(2);
            while (buffer1.Length > 0)
            {
                if (buffer1[0] == "newline")
                /* If we have a STOP sequence, check for it */
                {
                    /* end */
                    break;
                }
                
                /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
                buffer1 = buffer1.Slice(1);
            }
            
            if (buffer1[0] == "newline")
            {
                return new (TokenId.Syntax_Comment, 1 + (buffer0.Length - buffer1.Length));
            }
            
            return new (default, default);
        }
        private static ConsumerResult consume_pattern_46(global::System.ReadOnlySpan<byte> buffer0)
        {
            /*
            * TokenID: comment (#22)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_196 | Order: 371 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_196, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 2, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_198 | Order: 375 | IsRecursive: False | TreeDepth: [5, 5] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_198, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
        private static ConsumerResult consume_pattern_44(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: comment (#21)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_183 | Order: 218 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_183, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_185 | Order: 219 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_185, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * ESCAPE: PatternSequence { DependencyInfo = Pattern_187 | Order: 220 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_187, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
                    if (buffer2[0] == "*/")
                    {
                        buffer1 = buffer2.Slice(1);
                        continue;
                    }
                }
                
                if (buffer1[0] == "*/")
                /* If we have a STOP sequence, check for it */
                {
                    /* end */
                    break;
                }
                
                /* Token doesn't specify any explicit consumables, so ALL items are considered valid consumables */
                buffer1 = buffer1.Slice(1);
            }
            
            if (buffer1[0] == "*/")
            {
                return new (TokenId.Lexer_Comment, 1 + (buffer0.Length - buffer1.Length));
            }
            
            return new (default, default);
        }
        private static ConsumerResult consume_pattern_45(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: comment (#21)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_189 | Order: 221 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_189, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_191 | Order: 222 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_191, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * ESCAPE: PatternSequence { DependencyInfo = Pattern_193 | Order: 223 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_193, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
        private static ConsumerResult consume_pattern_43(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: comment (#21)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_177 | Order: 286 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_177, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_179 | Order: 216 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_179, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * ESCAPE: PatternSequence { DependencyInfo = Pattern_181 | Order: 217 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_181, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
        private static ConsumerResult consume_pattern_42(global::System.ReadOnlySpan<char> buffer0)
        {
            /*
            * TokenID: comment (#21)
            * ==[ CONSUMER_DATA ]==
            * START: PatternSequence { DependencyInfo = Pattern_167 | Order: 284 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_167, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * STOP: PatternSequence { DependencyInfo = Pattern_171 | Order: 285 | IsRecursive: False | TreeDepth: [2, 2] | NodeDepth: [2, 2], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_171, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
            * ESCAPE: PatternSequence { DependencyInfo = Pattern_173 | Order: 214 | IsRecursive: False | TreeDepth: [1, 1] | NodeDepth: [1, 1], HierarchyNode = MetaParser.Trees.KeyTreeNode`1[MetaParser.Graphs.EntityKey], Key = Pattern_173, Registry = MetaParser.Core.EntityRegistry, Kind = AllOf, Length = 1, IsInlinable = True, IsConstantLength = True, IsSequence = True, MinConditions = 0, MaxConditions = 0, IsConditional = False, IsDeterministic = True, Items = MetaParser.Parsing.Constructs.PatternEntity[], ConditionJoiner = ,  }
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
    }
}
