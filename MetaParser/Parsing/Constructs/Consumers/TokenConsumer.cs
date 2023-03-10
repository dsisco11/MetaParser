using MetaParser.Consumers;
using MetaParser.Core;
using MetaParser.Exceptions;
using MetaParser.Graphs;
using MetaParser.Json.Definitions;
using MetaParser.Parsing.Constructs.Core;
using MetaParser.Parsing.Constructs.Patternization;
using MetaParser.Parsing.Constructs.Tokens;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Parsing.Constructs.Consumers;
using static DirectedGraph<GraphNodeKey>;

internal record TokenConsumer
{
    #region Fields
    private readonly WeakReference<MetaParserRegistry> _registry;
    private readonly ConsumerClauseInfo assigned;
    private readonly ConsumerClauseInfo specified;

    public readonly EConsumerType Type;
    public readonly GraphNodeKey NodeID;
    #endregion

    #region Properties
    public ResolvedNode DependencyInfo { get; set; }
    #endregion

    #region Accessors
    public int Index => NodeID.Index;
    public TokenInfo Token
    {
        get
        {
            if (NodeID.Parent is null)
            {
                throw new MetaParserException($"Cannot resolve token for consumer without parent node: {NodeID}");
            }

            if (!_registry.TryGetTarget(out var registry))
            {
                throw new MetaParserException($"Cannot resolve token for consumer without registry: {NodeID}");
            }

            return registry.Tokens[NodeID.Parent];
        }
    }
    public PatternGroup Start => specified.Start!;
    public PatternGroup? Consume => specified.Consume;
    public PatternGroup? Stop => specified.Stop;
    public PatternGroup? Escape => specified.Escape;

    internal IEnumerable<Pattern> Patterns
    {
        get
        {
            yield return Start;
            if (Consume is not null)
            {
                yield return Consume;
            }
            if (Stop is not null)
            {
                yield return Stop;
            }
            if (Escape is not null)
            {
                yield return Escape;
            }
        }
    }
    #endregion

    #region State
    /// <summary> A consumer is considered open if it is dynamic and has no STOP criteria. </summary>
    public bool IsOpen => IsDynamic && assigned.Stop is null;
    /// <summary> A consumer is considered closed if it is dynamic and has a STOP criteria. </summary>
    public bool IsClosed => IsDynamic && assigned.Stop is not null;
    /// <summary> 
    /// A consumer is considered dynamic if it is not constant, specifically if it has either a CONSUME or STOP criteria.
    /// So a consumer is "dynamic" if it involves consuming a variable number of elements.
    /// </summary>
    public bool IsDynamic => assigned.Consume is not null || assigned.Stop is not null;
    public bool IsConstant => assigned.Start is not null && assigned.Consume is null && assigned.Stop is null;
    #endregion

    #region Constructors
    public TokenConsumer(MetaParserContext context, IConsumerDeclaration consumer)
    {
        var tokenInfo = context.WorkingSet.Tokens.Single();
        context.WorkingSet.Consumers[0] = this;

        _registry = new(context.Registry);
        Type = consumer.Type;
        NodeID = new GraphNodeKey(GraphNodeType.Consumer, context.Registry.GetNextConsumerIndex(), tokenInfo.NodeID);

        if (consumer.Start is null && consumer.Consume is null)
        {
            throw new IllegalTokenException($@"Illegal consumer definition (""{Token.Name}"") (tokens require at minimum either a START or CONSUME sequence)");
        }

        assigned = new ConsumerClauseInfo()
        {
            Start = consumer.Start.Any() ? new PatternGroup(EPatternCondition.AllOf, context, consumer.Start.Select(o => o.Resolve(context)).ToArray()) : null,
            Consume = consumer.Consume.Any() ? new PatternGroup(EPatternCondition.OneOf, context, consumer.Consume.Select(o => o.Resolve(context)).ToArray()) : null,
            Stop = consumer.Stop.Any() ? new PatternGroup(EPatternCondition.AllOf, context, consumer.Stop.Select(o => o.Resolve(context)).ToArray()) : null,
            Escape = consumer.Escape.Any() ? new PatternGroup(EPatternCondition.AllOf, context, consumer.Escape.Select(o => o.Resolve(context)).ToArray()) : null,
        };

        specified = new ConsumerClauseInfo()
        {
            Start = IsOpen switch
            {
                false when assigned.Start is not null => assigned.Start,
                // no start condition, only a CONSUME criteria
                true when assigned.Start is null => assigned.Consume!,
                // for open ended consumers it is implied that their consume criteria is part of their start condition
                true when assigned.Start is not null => assigned.Start.Combine(assigned.Consume!, context) as PatternGroup,
                _ => throw new NotImplementedException()
            },
            Consume = assigned.Consume,
            Stop = assigned.Stop,
            Escape = assigned.Escape,
        };
    }
    #endregion

    #region Dependencies
    public void Register_Dependencies(MetaParserContext context)
    {
        if (Type == EConsumerType.Data)
        {
            return;
        }

        var deps = Get_Dependencies(context);
        foreach (var tokenName in deps)
        {
            if (!context.Registry.TryGetToken(tokenName, out var token))
            {
                throw new UnknownTokenException($@"Unable to find token: ""{tokenName}""");
            }

            context.DepsGraph.TryLink(NodeID, token.NodeID);
            context.DepsGraph.TryLink(Token.NodeID, token.NodeID);
        }
    }

    private HashSet<string> Get_Dependencies(MetaParserContext context)
    {
        HashSet<string> refs = new();
        if (Start is not null)
        {
            foreach (var name in Get_Tokens_From_Pattern(Start))
            {
                refs.Add(name);
            }
        }

        if (Consume is not null)
        {
            foreach (var name in Get_Tokens_From_Pattern(Consume))
            {
                refs.Add(name);
            }
        }
        else if (IsClosed)
        {// Closed tokens with an ambiguous consume clause are inherently dependent on all other defined tokens as they can consume anything
            var allOthers = context.Registry.Tokens.Values.Where((x) => x.Index != Token.Index).Select(static (x) => x.Name);
            foreach (var tokenName in allOthers)
            {
                refs.Add(tokenName);
            }
        }

        if (Stop is not null)
        {
            foreach (var name in Get_Tokens_From_Pattern(Stop))
            {
                refs.Add(name);
            }

            if (Escape is not null)
            {
                foreach (var name in Get_Tokens_From_Pattern(Escape))
                {
                    refs.Add(name);
                }
            }
        }

        return refs;
    }

    private static IEnumerable<string> Get_Tokens_From_Pattern(Pattern pattern)
    {
        return pattern.OfType<PatternTokenRef>().Select(static (x) => x.TokenName);
    }
    #endregion
}
