using MetaParser.DepsGraph;
using MetaParser.Exceptions;
using MetaParser.Patternization;
using MetaParser.Tokens;

using System;

namespace MetaParser.Consumers;

internal record ConsumerInfo
{
    #region Properties
    public int Stage { get; private set; }
    public readonly TokenInfo Token;

    public readonly int Index;
    public readonly EConsumerType Type;

    public readonly PatternGroup? Start;
    public readonly PatternGroup? Consume;
    public readonly PatternGroup? Stop;
    public readonly PatternGroup? Escape;
    #endregion

    #region Accessors
    public Pattern EffectiveStart => Start ?? Consume ?? throw new NotImplementedException();
    /// <summary> A consumer is considered open if it is dynamic and has no STOP criteria. </summary>
    public bool IsOpen => IsDynamic && Stop is null;
    /// <summary> 
    /// A consumer is considered dynamic if it is not constant, specifically if it has either a CONSUME or STOP criteria.
    /// So a consumer is "dynamic" if it involves consuming a variable number of elements.
    /// </summary>
    public bool IsDynamic => Consume is not null || Stop is not null;
    public bool IsConstant => Start is not null && Consume is null && Stop is null;
    #endregion

    #region Constructors
    public ConsumerInfo(TokenInfo token, ConsumerData data)
    {
        Token = token;
        Type = data.Type;
        Index = data.Index;

        if (data.Start is null && data.Consume is null)
        {
            throw new IllegalTokenException($@"Illegal token definition (""{token.Name}"") (tokens require at minimum either a START or CONSUME sequence)");
        }

        Start = data.Start;
        Consume = data.Consume;
        Stop = data.Stop;
        Escape = data.Escape;
    }
    #endregion

    public void ResolveStage(DependencyGraph graph)
    {
        // basically check if the node has any outgoing links which make more than two jumps
        if (graph.TryGetNode(this, out var consumerNode))
        {
            Stage = consumerNode!.GetDepth();
        }
    }
}
