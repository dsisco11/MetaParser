using MetaParser.Builders;
using MetaParser.Compiler.Structs;
using MetaParser.Core;
using MetaParser.Exceptions;
using MetaParser.Graphs;
using MetaParser.Json.Definitions;
using MetaParser.Parsing.Constructs;
using MetaParser.Parsing.Constructs.Patterns;
using MetaParser.Parsing.Constructs.Stages;
using MetaParser.Parsing.Constructs.Tokens;
using MetaParser.Syntax;
using MetaParser.Syntax.Nodes;
using MetaParser.Trees;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using SyntaxTree = MetaParser.Syntax.SyntaxTree;

namespace MetaParser.Compiler;

/// <summary>
/// Intakes a <see cref="ParserDefinition"/> and compiles it into a <see cref="ParserContext"/> instance.
/// </summary>
internal sealed record ParserInterpreter
{
    #region Fields
    private readonly string _name;
    private readonly ParserDefinition _definition;
    private readonly ParserConfiguration _config;
    #endregion

    #region Properties
    public InterpreterStep Declared;
    public InterpreterStep Assigned;
    public InterpreterStep Specified;
    public InterpreterStep Computed;
    public InterpreterStep Used;
    #endregion

    #region Accessors
    public string Name => _name;
    public ParserDefinition Definition => _definition;
    public ParserConfiguration Config => _config;
    public EInterpreterStage Stage { get; private set; }
    public EInterpreterStage NextStage => Stage + 1;
    public InterpreterStep CurrentStep => Stage switch
    {
        EInterpreterStage.Declared => Declared,
        EInterpreterStage.Assigned => Assigned,
        EInterpreterStage.Specified => Specified,
        EInterpreterStage.Computed => Computed,
        EInterpreterStage.Used => Used,
        EInterpreterStage.Ready => Used,
        _ => throw new NotImplementedException()
    };
    #endregion

    #region Constructors
    public ParserInterpreter(string name, ParserDefinition definition)
    {
        _name = name;
        _definition = definition;
        _config = new ParserConfiguration()
        {
            BaseFileName = name,
            Namespace = definition.Namespace!,
            ClassName = definition?.ClassName,
            ParserType = definition?.ParserType
        };
    }
    #endregion

    #region Validation
    public void Validate(SourceProductionContext context)
    {
        // TODO: Should this all be done inline within the analyzer pipeline?
        Ensure_No_Duplicate_Token_Names(context);
    }

    private bool Ensure_No_Duplicate_Token_Names(SourceProductionContext context)
    {
        var mergedTokens = Get_Merged_Tokens(CurrentStep);
        var collissions = mergedTokens.Where(static (x) => x.Value.Count > 1).ToImmutableDictionary(static (t) => t.Key, static (t) => t.Value.Min(static (o) => o.Stage));
        if (collissions.Count > 0)
        {
            var message = "Tokens with the same name were found in different stages: ";
            foreach (var kvp in collissions)
            {
                message += $"{kvp.Key} in stage {kvp.Value}, ";
            }

            // TODO:
            //context.ReportDiagnostic()

            return false;
        }

        return true;
    }
    #endregion

    #region Compiling
    //public ParserContext Compile()
    //{
    //    StageData lexingStage = CurrentStep.Stages[EParsingStage.Lexer];
    //    StageData syntaxStage = CurrentStep.Stages[EParsingStage.Syntax];

    //    var registry = new EntityRegistry();
    //    var mergedTokens = Get_Merged_Tokens(CurrentStep);

    //    // loop through each token clause and create an entity for each token, consumer, and pattern clause in the definition and without any duplicates
    //    foreach (var entry in mergedTokens)
    //    {
    //        var tokenList = entry.Value;
    //        // create a token for each token name
    //        foreach (var tokenClause in tokenList)
    //        {
    //            string tokenID = CodeCommon.Format_Token_Key(tokenClause.ID);
    //            string tokenName = CodeCommon.Format_Token_Key(tokenClause.Name);
    //            var tokenEntity = new TokenEntity(registry, tokenID, tokenName);
    //            // add the token to the registry
    //            registry.Add(tokenEntity);
    //            // add the token to the tree
    //            registry.Tree.Add(tokenEntity.Key);

    //            // create a consumer for each consumer declaration
    //            foreach (var consumerClause in tokenClause.Items)
    //            {
    //                EConsumerKind consumerKind = get_consumer_kind_for_stage(tokenClause.Stage);

    //                var consumerEntity = ConsumerEntityFactory.Create(consumerKind, consumerClause, tokenName, registry);

    //                // add the consumer to the tree
    //                registry.Tree.AddEdge(parent: tokenEntity.Key, child: consumerEntity.Key);
    //            }
    //        }
    //    }

    //    List<ConsumerEntity> lexerDropped = new();
    //    // add any dropped consumers to the graph aswell
    //    foreach (var consumerClause in syntaxStage.Dropped)
    //    {
    //        EConsumerKind consumerKind = get_consumer_kind_for_stage(EParsingStage.Lexer);
    //        var consumerEntity = ConsumerEntityFactory.Create(consumerKind, consumerClause, string.Empty, registry);
    //        lexerDropped.Add(consumerEntity);
    //    }

    //    List<ConsumerEntity> syntaxDropped = new();
    //    // add any dropped consumers to the graph aswell
    //    foreach (var consumerClause in syntaxStage.Dropped)
    //    {
    //        EConsumerKind consumerKind = get_consumer_kind_for_stage(EParsingStage.Syntax);
    //        var consumerEntity = ConsumerEntityFactory.Create(consumerKind, consumerClause, string.Empty, registry);
    //        syntaxDropped.Add(consumerEntity);
    //    }

    //    // set the id type to the smallest possible type that can hold all the token names
    //    var distinctTokenIds = registry.Tokens.Select(static (o) => o.ID).ToImmutableHashSet();
    //    _config.IdType = Common.Get_Integer_Type(distinctTokenIds.Count);

    //    // build the final graph
    //    registry.BuildGraphs();

    //    // Handle the lexers specially, to avoid any ordering mishaps
    //    var lexerConsumers = registry.Consumers.Where(static (o) => o.Kind == EConsumerKind.Lexer).Except(lexerDropped).ToImmutableHashSet();
    //    var stages = new List<ParsingStageContext>()
    //    {
    //        new ParsingStageContext(0, _config.InputType, _config.IdType, lexerConsumers, ImmutableHashSet<TokenEntity>.Empty, lexerDropped.ToImmutableHashSet())
    //    };

    //    // get all token entities from the registry which are listed by name in syntaxStage.Ignored
    //    var ignoredTokens = registry.Tokens.Where((o) => syntaxStage.Ignored.Contains(o.Name)).ToImmutableHashSet();

    //    // Group all consumers in the registry by max node depth and then put each of the groups into a ParsingStageContext object which is linked to the previous one
    //    var groups = registry.Consumers.Except(lexerConsumers).Except(syntaxDropped).GroupBy(static (x) => x.Token.GraphInfo.Depth)
    //                                  .OrderBy(static (x) => x.Key);
    //    foreach (var group in groups)
    //    {
    //        var groupIgnored = ignoredTokens.Where(x => x.GraphInfo.Depth <= group.Key).ToImmutableHashSet();
    //        var groupDropped = syntaxDropped.Where(x => x.Token.GraphInfo.Depth <= group.Key).ToImmutableHashSet();
    //        stages.Add(new ParsingStageContext(group.Key, _config.IdType, _config.IdType, group.ToImmutableHashSet(), groupIgnored, groupDropped));
    //    }
    //    // create the parsing stage contexts
    //    for (int i = 0; i < stages.Count; i++)
    //    {
    //        var stage = stages[i];
    //        if (i + 1 < stages.Count)
    //        {
    //            stage.Tail = stages[i + 1];
    //        }
    //        if (i - 1 >= 0)
    //        {
    //            stage.Head = stages[i - 1];
    //        }
    //    }

    //    var context = new ParserContext()
    //    {
    //        Config = _config,
    //        Registry = registry,
    //        Stages = stages.ToImmutableArray(),
    //        State = new CodeGenState(stages.First()),
    //    };
    //    return context;

    //    static EConsumerKind get_consumer_kind_for_stage(EParsingStage stage)
    //    {
    //        return stage switch
    //        {
    //            EParsingStage.Lexer => EConsumerKind.Lexer,
    //            EParsingStage.Syntax => EConsumerKind.Syntax,
    //            _ => throw new NotImplementedException()
    //        };
    //    }
    //}
    #endregion

    #region AST Tree Construction
    public static Syntax.SyntaxTree Build(ParserDefinition definition)
    {
        var rootItems = new List<GreenNode>();
        foreach (var stageDef in definition.Stages)
        {
            foreach (var dropDef in stageDef.Dropped)
            {
                var clause = process_consumer_declaration_into_clause(dropDef);
                var start = clause.Start is null ? null : pattern_clause_to_node(clause.Start);
                var consume = clause.Consume is null ? null : pattern_clause_to_node(clause.Consume);
                var stop = clause.Stop is null ? null : pattern_clause_to_node(clause.Stop);
                var escape = clause.Escape is null ? null : pattern_clause_to_node(clause.Escape);

                var consumerNode = new DropConsumerNode(start, consume, stop, escape);

                rootItems.Add(consumerNode);
            }

            foreach (var tokenDef in stageDef.Consumers)
            {
                foreach (var consumerDef in tokenDef.Value)
                {
                    var clause = process_consumer_declaration_into_clause(consumerDef);

                    var start = clause.Start is null ? null : pattern_clause_to_node(clause.Start);
                    var consume = clause.Consume is null ? null : pattern_clause_to_node(clause.Consume);
                    var stop = clause.Stop is null ? null : pattern_clause_to_node(clause.Stop);
                    var escape = clause.Escape is null ? null : pattern_clause_to_node(clause.Escape);

                    var tokenInfo = new TokenInfo(tokenDef.Key, stageDef.Type, false);

                    var consumerNode = new TokenConsumerNode(tokenInfo, start, consume, stop, escape);
                    rootItems.Add(consumerNode);
                }
            }
        }

        return new SyntaxTree(new[] { new ParserDefinitionNode(definition.ClassName, definition.Namespace, rootItems.ToArray()) });
    }
    #endregion

    #region Converters
    private static PatternNode pattern_clause_to_node(IPatternClause patternClause)
    {
        return patternClause switch { 
            PatternItemClause clause when clause.Kind == EPatternKind.Literal => NodeFactory.Create_Pattern_Literal(clause.Value),
            PatternItemClause clause when clause.Kind == EPatternKind.Token => NodeFactory.Create_Pattern_Token(clause.Value),
            PatternSequenceClause clause => new PatternNode(clause.Kind, clause.Items.Select(pattern_clause_to_node).ToArray()),
            _ => throw new NotImplementedException()
        };
    }
    #endregion

    #region Discrete Steps
    private static ConsumerClause process_consumer_declaration_into_clause(ConsumerDeclaration declaration)
    {
        var seqStart = declaration.Start.Select(static (c) => c.Interpret()).Where(static x => x is not null).Select(static x => x!);
        var seqConsume = declaration.Consume.Select(static (c) => c.Interpret()).Where(static x => x is not null).Select(static x => x!);
        var seqStop = declaration.Stop.Select(static (c) => c.Interpret()).Where(static x => x is not null).Select(static x => x!);
        var seqEscape = declaration.Escape.Select(static (c) => c.Interpret()).Where(static x => x is not null).Select(static x => x!);

        if (seqStart is null && seqConsume is null)
        {
            throw new IllegalTokenException($@"Illegal consumer definition ({declaration}) (tokens require at minimum either a START or CONSUME sequence)");
        }

        var clause = new ConsumerClause()
        {
            Start = seqStart.Any() ? new PatternSequenceClause(EPatternKind.AllOf, seqStart!) : null,
            Consume = seqConsume.Any() ? new PatternSequenceClause(EPatternKind.OneOf, seqConsume!) : null,
            Stop = seqStop.Any() ? new PatternSequenceClause(EPatternKind.AllOf, seqStop!) : null,
            Escape = seqEscape.Any() ? new PatternSequenceClause(EPatternKind.AllOf, seqEscape!) : null,
        };

        clause = inline_implied_patterns(clause);
        clause = simplify_patterns(clause);
        clause = perform_pattern_subclassing(declaration.Stage, clause);

        return clause;
    }

    private static ConsumerClause inline_implied_patterns(ConsumerClause consumer)
    {
        return new ConsumerClause()
        {
            Start = consumer.IsOpen switch
            {
                false when consumer.Start is not null => consumer.Start,
                // no start condition, only a CONSUME criteria
                true when consumer.Start is null => consumer.Consume!,
                // for open ended consumers it is implied that their consume criteria is part of their start condition
                true when consumer.Start is not null => new PatternSequenceClause(EPatternKind.AllOf, consumer.Start, consumer.Consume!),
                _ => throw new NotImplementedException()
            },
            Consume = consumer.Consume switch
            {                
                // if there is no consume criteria, but there is a stop criteria, then the consume criteria is a wildcard
                null when consumer.Stop is not null => new PatternSequenceClause(EPatternKind.Any),
                // if there is no consume criteria, then there is no consume criteria
                null => null,
                _ => consumer.Consume
            },
            Stop = consumer.Stop,
            Escape = consumer.Escape,
        };
    }

    #endregion
    private static ConsumerClause simplify_patterns(ConsumerClause consumer)
    {
        return new ConsumerClause()
        {
            Start = consumer.Start?.Reduce(),
            Consume = consumer.Consume?.Reduce(),
            Stop = consumer.Stop?.Reduce(),
            Escape = consumer.Escape?.Reduce(),
        };
    }

    private static ConsumerClause perform_pattern_subclassing(EParsingStage stage, ConsumerClause consumer)
    {
        return new ConsumerClause()
        {
            Start = process(stage, consumer.Start),
            Consume = process(stage, consumer.Consume),
            Stop = process(stage, consumer.Stop),
            Escape = process(stage, consumer.Escape),
        };


        static IPatternClause? process(EParsingStage stage, IPatternClause? pattern)
        {
            if (pattern is null) return null;

            // break up 'literal' type patterns with a value longer than 1 into a sequence of pattern items which each contain a single character
            switch (pattern)
            {
                case PatternSequenceClause sequence:
                    {
                        var newSequence = new PatternSequenceClause(sequence.Kind);
                        foreach (var item in sequence.Items)
                        {
                            var computed = process(stage, item);
                            if (computed is not null) newSequence.Items.Add(computed);
                        }
                        return newSequence;
                    }
                case PatternItemClause literal:
                    {
                        if (stage == EParsingStage.Lexer)
                        {
                            if (literal.Value.Length > 1)
                            {
                                var newSequence = new PatternSequenceClause(EPatternKind.AllOf);
                                foreach (char ch in literal.Value)
                                {
                                    newSequence.Items.Add(new PatternItemClause(EPatternKind.Literal, ch.ToString()));
                                }
                                return newSequence;
                            }
                        }
                        else
                        {// patterns from outside the lexer stage are Token patterns
                            return new PatternItemClause(EPatternKind.Token, literal.Value);
                        }
                        break;
                    }
            }

            return pattern;
        }
    }

    #region Statics
    private static Dictionary<string, List<TokenClause>> Get_Merged_Tokens(InterpreterStep Data)
    {
        // create a single merged dictionary for all stages in Data
        var mergedTokens = new Dictionary<string, List<TokenClause>>();
        foreach (var stage in Data.Stages)
        {
            foreach (var item in stage.Value.Items)
            {
                var token = item.Value;
                var tokenName = token.Name;
                if (!mergedTokens.TryGetValue(tokenName, out var tokenList))
                {
                    tokenList = new List<TokenClause>();
                    mergedTokens.Add(tokenName, tokenList);
                }
                tokenList.Add(token);
            }
        }
        return mergedTokens;
    }

    public static Dictionary<string, List<ConsumerDeclaration>> Get_Merged_Consumers(ParserDefinition parserDefinition)
    {
        var results = new Dictionary<string, List<ConsumerDeclaration>>();
        foreach (var stage in parserDefinition.Stages)
        {
            foreach (var consumer in stage.Consumers)
            {
                if (!results.TryGetValue(consumer.Key, out var consumers))
                {
                    consumers = new List<ConsumerDeclaration>();
                    results.Add(consumer.Key, consumers);
                }
                consumers.AddRange(consumer.Value);
            }
        }

        return results;
    }
    #endregion

    #region Records
    record TokenKey
    {
        public readonly string Name;
        public readonly EParsingStage Stage;

        public TokenKey(string name, EParsingStage stage)
        {
            Name = name;
            Stage = stage;
        }
    }
    #endregion
}