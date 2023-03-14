using MetaParser.Core;
using MetaParser.Exceptions;
using MetaParser.Graphs;
using MetaParser.Json.Definitions;
using MetaParser.Trees;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetaParser.Parsing.Constructs;
internal record Consumer : GraphableEntity, IComparable<Consumer>
{
    #region Fields
    private readonly ConsumerClauseInfo assigned;
    private readonly ConsumerClauseInfo specified;

    public readonly EConsumerType Type;
    #endregion

    #region Properties
    #endregion

    #region Accessors
    public int Index => Key.Index;
    public KeyTreeNode<EntityKey> HierarchyNode => Registry.Tree[Key] ?? throw new MetaParserException($"Cannot find hierarchy node for '{Key}'");
    public TokenInfo Token
    {
        get
        {
            var parent = HierarchyNode.Parent;
            if (parent is null)
            {
                throw new MetaParserException($"Cannot resolve token for consumer without parent node: {Key}");
            }

            var tokenKey = parent.Value;
            return Registry.Tokens[tokenKey];
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
    public Consumer(MetaParserContext context, IConsumerDeclaration consumer) : base(new EntityKey(NodeType.Consumer, context.Registry.GetNextConsumerIndex()), context)
    {
        Type = consumer.Type;
        context.WorkingSet.Consumers[0] = this;
        context.Registry.AddConsumer(this, context.WorkingSet.Tokens.Single().Key);

        if (consumer.Start is null && consumer.Consume is null)
        {
            throw new IllegalTokenException($@"Illegal consumer definition (""{Token.Name}"") (tokens require at minimum either a START or CONSUME sequence)");
        }

        assigned = new ConsumerClauseInfo()
        {
            Start = consumer.Start.Any() ? new PatternGroup(EPatternCondition.AllOf, context, consumer.Start.Select(o => o.Resolve(context)!).ToArray()) : null,
            Consume = consumer.Consume.Any() ? new PatternGroup(EPatternCondition.OneOf, context, consumer.Consume.Select(o => o.Resolve(context)!).ToArray()) : null,
            Stop = consumer.Stop.Any() ? new PatternGroup(EPatternCondition.AllOf, context, consumer.Stop.Select(o => o.Resolve(context)!).ToArray()) : null,
            Escape = consumer.Escape.Any() ? new PatternGroup(EPatternCondition.AllOf, context, consumer.Escape.Select(o => o.Resolve(context)!).ToArray()) : null,
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

    #region IComparable
    public int CompareTo(Consumer other)
    {
        return Key.CompareTo(other.Key);
    }
    #endregion

    #region Dependency Link Resolution
    public override IEnumerable<EntityLink> ResolveLinks(MetaParserRegistry Registry)
    {
        //if (Type == EConsumerType.Data)
        //{
        //    yield break;
        //}

        // Link the consumers token to it
        yield return new EntityLink(Token.Key, Key);

        // link the consumer to all of its immediate pattern groups
        if (Start is not null)
        {
            yield return new EntityLink(Key, Start.Key);
        }

        if (Consume is not null)
        {
            yield return new EntityLink(Key, Consume.Key);
        }
        else if (IsClosed)
        {// Closed tokens with an ambiguous consume clause are inherently dependent on all other defined tokens as they can consume anything
            var allOthers = Registry.Tokens.Values.Except(new []{ Token }).Select(static (x) => x.Name);
            foreach (var tokenName in allOthers)
            {
                if (Registry.TryGetToken(tokenName, out var outToken))
                {
                    yield return new EntityLink(Token.Key, outToken.Key);
                }
            }
        }

        if (Stop is not null)
        {
            yield return new EntityLink(Key, Stop.Key);
        }

        if (Escape is not null)
        {
            yield return new EntityLink(Key, Escape.Key);
        }

        yield break;
    }
    #endregion

    // ToString
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder($"{Key} | Type: {Type}");

        sb.Append($" | DependencyInfo: {DependencyInfo}");
        sb.Append($" | Start: {Start}");
        // write consume
        if (Consume is null)
        {
            sb.Append($"| Consume: {Consume}");
        }

        // write stop
        if (Stop is not null)
        {
            sb.Append($"| Stop: {Stop}");
        }

        // write escape
        if (Escape is not null)
        {
            sb.Append($"| Escape: {Escape}");
        }

        // write state
        sb.Append($"| IsDynamic: {IsDynamic}, IsConstant: {IsConstant}, IsOpen: {IsOpen}, IsClosed: {IsClosed}");

        return sb.ToString();
    }
}
