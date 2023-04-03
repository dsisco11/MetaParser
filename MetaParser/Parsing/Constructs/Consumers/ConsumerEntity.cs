using MetaParser.Core;
using MetaParser.Exceptions;
using MetaParser.Graphs;
using MetaParser.Parsing.Constructs.Consumers;

using System;
using System.Collections.Generic;
using System.Text;

namespace MetaParser.Parsing.Constructs;
internal record ConsumerEntity : GraphEntity, IComparable<ConsumerEntity>
{
    #region Fields
    public readonly EConsumerKind Kind;
    /// <summary>ID of the token which this consumer will output</summary>
    public readonly string OutputTokenAlias;
    public PatternEntity Start;
    public PatternEntity? Consume;
    public PatternEntity? Stop;
    public PatternEntity? Escape;
    #endregion

    #region Properties
    #endregion

    #region Accessors
    public int Index => Key.Index;
    public TokenEntity Token
    {
        get
        {
            var parent = HierarchyNode.Parent ?? throw new MetaParserException($"Cannot resolve token for consumer without parent node: {Key}");
            var tokenKey = parent.Value;
            if (!Registry.TryGetEntity<TokenEntity>(tokenKey, out var token))
            {
                throw new MetaParserException($"Cannot resolve token for consumer: {Key}");
            }

            return token;
        }
    }

    internal IEnumerable<PatternEntity> Patterns
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
    /// <summary> A consumer is considered constant if it has ONLY a START criteria. </summary>
    public bool IsConstant => Start is not null && Consume is null && Stop is null;
    /// <summary> A consumer is considered open if it is dynamic and has no STOP criteria. </summary>
    public bool IsOpen => IsDynamic && Stop is null;
    /// <summary> A consumer is considered closed if it is dynamic and has a STOP criteria. </summary>
    public bool IsClosed => IsDynamic && Stop is not null;
    /// <summary> 
    /// A consumer is considered dynamic if it is not constant, specifically if it has either a CONSUME or STOP criteria.
    /// In other words; a dynamic consumer involves consuming a variable number of elements.
    /// </summary>
    public bool IsDynamic => Consume is not null || Stop is not null;
    #endregion

    #region Constructors
    public ConsumerEntity(EConsumerKind type, EntityRegistry registry, PatternEntity start, PatternEntity? consume, PatternEntity? stop, PatternEntity? escape, string outputTokenAlias) : base(NodeType.Consumer, registry)
    {
        Kind = type;
        Start = start;
        Consume = consume;
        Stop = stop;
        Escape = escape;
        OutputTokenAlias = outputTokenAlias;
    }
    #endregion

    #region IComparable
    public int CompareTo(ConsumerEntity other)
    {
        return ConsumerSorter.Instance.Compare(this, other);
    }
    #endregion

    #region Dependency Link Resolution
    public override IEnumerable<EntityLink> ResolveLinks(EntityRegistry Registry)
    {
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
            // ideally we would link to tokens which are possible inputs to the stage this consumer is in, meaning those whose maximum node depth is less than ours
            // However; this is not possible as we do not have access to the stage graph at this point

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
        StringBuilder sb = new StringBuilder($"{Key} | Type: {Kind}");

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
