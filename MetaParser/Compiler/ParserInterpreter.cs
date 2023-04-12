using MetaParser.Compiler.Structs;
using MetaParser.Core;
using MetaParser.Exceptions;
using MetaParser.Graphs;
using MetaParser.Json.Definitions;
using MetaParser.Parsing.Constructs;
using MetaParser.Parsing.Constructs.Patterns;
using MetaParser.Parsing.Constructs.Stages;

using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

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
    public ParserInterpreter ExecuteNext()
    {
        return Stage switch
        {
            EInterpreterStage.Declared => this with { Stage = EInterpreterStage.Assigned, Declared = Process_Declared(Definition) },
            EInterpreterStage.Assigned => this with { Stage = EInterpreterStage.Specified, Assigned = Process_Assigned(Declared) },
            EInterpreterStage.Specified => this with { Stage = EInterpreterStage.Computed, Specified = Process_Specified(Assigned) },
            EInterpreterStage.Computed => this with { Stage = EInterpreterStage.Used, Computed = Process_Computed(Specified) },
            EInterpreterStage.Used => this with { Stage = EInterpreterStage.Ready, Used = Process_Used(Computed) },
            EInterpreterStage.Ready => this,
            _ => throw new NotImplementedException()
        };
    }

    public ParserContext Compile()
    {
        var registry = new EntityRegistry();

        var mergedTokens = Get_Merged_Tokens(CurrentStep);
        // loop through each token clause and create an entity for each token, consumer, and pattern clause in the definition and without any duplicates
        foreach (var entry in mergedTokens)
        {
            var tokenList = entry.Value;
            // create a token for each token name
            foreach (var tokenClause in tokenList)
            {
                string tokenID = CodeCommon.Format_Token_Key(tokenClause.ID);
                string tokenName = CodeCommon.Format_Token_Key(tokenClause.Name);
                var tokenEntity = new TokenEntity(registry, tokenID, tokenName);
                // add the token to the registry
                registry.Add(tokenEntity);
                // add the token to the tree
                registry.Tree.Add(tokenEntity.Key);

                // create a consumer for each consumer declaration
                foreach (var consumerClause in tokenClause.Items)
                {
                    EConsumerKind consumerKind = tokenClause.Stage switch
                    {
                        EParsingStage.Lexer => EConsumerKind.Lexer,
                        EParsingStage.Syntax => EConsumerKind.Syntax,
                        _ => throw new NotImplementedException()
                    };

                    var consumerEntity = ConsumerEntityFactory.Create(consumerKind, consumerClause, tokenName, registry);

                    // add the consumer to the tree
                    registry.Tree.AddEdge(parent: tokenEntity.Key, child: consumerEntity.Key);
                }
            }
        }

        // set the id type to the smallest possible type that can hold all the token names
        var distinctTokenIds = registry.Tokens.Select(static (o) => o.ID).ToImmutableHashSet();
        _config.IdType = Common.Get_Integer_Type(distinctTokenIds.Count);

        // build the final graph
        registry.BuildGraphs();
        // Handle the lexers specially, to avoid any ordering mishaps
        var lexerConsumers = registry.Consumers.Where(static (o) => o.Kind == EConsumerKind.Lexer).ToImmutableHashSet();
        var stages = new List<ParsingStageContext>()
        {
            new ParsingStageContext(0, _config.InputType, _config.IdType, lexerConsumers)
        };

        // Group all consumers in the registry by max node depth and then put each of the groups into a ParsingStageContext object which is linked to the previous one
        var groups =registry.Consumers.Except(lexerConsumers).GroupBy(static (x) => x.Token.GraphInfo.Depth)
                                      .OrderBy(static (x) => x.Key);
        foreach (var group in groups)
        {
            stages.Add(new ParsingStageContext(group.Key, _config.IdType, _config.IdType, group.ToImmutableHashSet()));
        }
        // create the parsing stage contexts
        for (int i = 0; i < stages.Count; i++)
        {
            var stage = stages[i];
            if (i + 1 < stages.Count)
            {
                stage.Tail = stages[i + 1];
            }
            if (i - 1 >= 0)
            {
                stage.Head = stages[i - 1];
            }
        }

        var context = new ParserContext()
        {
            Config = _config,
            Registry = registry,
            Stages = stages.ToImmutableArray(),
            State = new CodeGenState(stages.First()),
        };
        return context;
    }
    #endregion

    #region Discrete Steps
    private delegate ConsumerClause StageConsumerTransformer(StageData stage, TokenClause token, ConsumerClause consumer);

    /// <summary>
    /// assigned values with simplification applied, meaning that patterns are inlined and any redundant patterns are removed
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private static InterpreterStep Process_Stage(InterpreterStep Data, StageConsumerTransformer consumerTransformer)
    {
        var stepData = new InterpreterStep();
        foreach (var defStage in Data.Stages)
        {
            var stage = new StageData(defStage.Value);
            stepData.Stages.Add(defStage.Key, stage);

            foreach (var defItem in defStage.Value.Items)
            {
                var tokenName = defItem.Key;
                var defToken = defItem.Value;

                // initialize if needed
                if (!stage.Items.TryGetValue(tokenName, out var tokenClause))
                {
                    tokenClause = new TokenClause(defToken);
                    stage.Items.Add(tokenName, tokenClause);
                }

                foreach (var defConsumer in defToken)
                {
                    var processed = consumerTransformer(stage, defToken, defConsumer);
                    tokenClause.Items.Add(processed);
                }
            }
        }

        return stepData;
    }

    /// <summary>
    /// Processess JSON structures from the definition and translates them into C# structures
    /// </summary>
    public static InterpreterStep Process_Declared(ParserDefinition definition)
    {
        var stepData = new InterpreterStep();
        // for each item in every stage of the definition, call Interpret on the item and add it to the data objects stage
        foreach (var defStage in definition.Stages)
        {
            var stage = new StageData(defStage.Type);
            stepData.Stages.Add(defStage.Type, stage);

            foreach (var defItem in defStage.Consumers)
            {
                string defTokenID = defItem.Key;
                IEnumerable<ConsumerDeclaration> defConsumerList = defItem.Value;

                // initialize the list of clauses if needed
                if (!stage.Items.TryGetValue(defTokenID, out var tokenClause))
                {
                    tokenClause = new TokenClause(defTokenID, stage.Stage) { 
                        Name = $"{stage.Stage.ToString().ToUpperInvariant()}_{defTokenID}"
                    };
                    stage.Items.Add(defTokenID, tokenClause);
                }

                // for each item in the consumer, call Interpret on the item and add it to the consumer items
                foreach (var defConsumer in defConsumerList)
                {
                    var seqStart = defConsumer.Start.Select(static (c) => c.Interpret()).Where(static x => x is not null).Select(static x => x!);
                    var seqConsume = defConsumer.Consume.Select(static (c) => c.Interpret()).Where(static x => x is not null).Select(static x => x!);
                    var seqStop = defConsumer.Stop.Select(static (c) => c.Interpret()).Where(static x => x is not null).Select(static x => x!);
                    var seqEscape = defConsumer.Escape.Select(static (c) => c.Interpret()).Where(static x => x is not null).Select(static x => x!);

                    if (seqStart is null && seqConsume is null)
                    {
                        throw new IllegalTokenException($@"Illegal consumer definition (""{defTokenID}"") (tokens require at minimum either a START or CONSUME sequence)");
                    }

                    var consumer = new ConsumerClause()
                    {
                        Start = seqStart.Any() ? new PatternSequenceClause(EPatternKind.AllOf, seqStart!) : null,
                        Consume = seqConsume.Any() ? new PatternSequenceClause(EPatternKind.OneOf, seqConsume!) : null,
                        Stop = seqStop.Any() ? new PatternSequenceClause(EPatternKind.AllOf, seqStop!) : null,
                        Escape = seqEscape.Any() ? new PatternSequenceClause(EPatternKind.AllOf, seqEscape!) : null,
                    };

                    tokenClause.Items.Add(consumer);
                }
            }
        }

        return stepData;
    }


    /// <summary>
    /// declared values with any "blanks" filled in with defaults, such as 'Start' becoming 'Start = None' if it was left blank
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static InterpreterStep Process_Assigned(InterpreterStep Data) => Process_Stage(Data, inline_implied_patterns);

    /// <summary>
    /// assigned values with simplification applied, meaning that patterns are inlined and any redundant patterns are removed
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static InterpreterStep Process_Specified(InterpreterStep Data) => Process_Stage(Data, perform_pattern_subclassing);

    /// <summary>
    /// Resolving specified values into concrete values, meaning that a pattern with collissions is expanded into virtual sub-patterns (as such, token collissions result in the creation of virtual tokens)
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static InterpreterStep Process_Computed(InterpreterStep Data) => Process_Stage(Data, simplify_patterns);

    /// <summary>
    /// Deduplication of patterns and registration of data structures with the registry
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static InterpreterStep Process_Used(InterpreterStep Data) => Process_Stage(Data, process_used);
    #endregion
    private static ConsumerClause simplify_patterns(StageData stage, TokenClause token, ConsumerClause consumer)
    {
        return new ConsumerClause()
        {
            Start = consumer.Start?.Reduce(),
            Consume = consumer.Consume?.Reduce(),
            Stop = consumer.Stop?.Reduce(),
            Escape = consumer.Escape?.Reduce(),
        };
    }

    private static ConsumerClause perform_pattern_subclassing(StageData stage, TokenClause token, ConsumerClause consumer)
    {
        return new ConsumerClause()
        {
            Start = process(stage, token, consumer.Start),
            Consume = process(stage, token, consumer.Consume),
            Stop = process(stage, token, consumer.Stop),
            Escape = process(stage, token, consumer.Escape),
        };


        static IPatternClause? process(StageData stage, TokenClause token, IPatternClause? pattern)
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
                            var computed = process(stage, token, item);
                            if (computed is not null) newSequence.Items.Add(computed);
                        }
                        return newSequence;
                    }
                case PatternItemClause literal:
                    {
                        if (stage.Stage == EParsingStage.Lexer)
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

    private static ConsumerClause inline_implied_patterns(StageData stage, TokenClause token, ConsumerClause consumer)
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
            Consume = consumer.Consume,
            Stop = consumer.Stop,
            Escape = consumer.Escape,
        };
    }

    private static ConsumerClause process_used(StageData stage, TokenClause token, ConsumerClause consumer)
    {
        return consumer;// with { };
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