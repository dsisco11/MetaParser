using MetaParser.DepsGraph;
using MetaParser.Exceptions;
using MetaParser.Patternization;
using MetaParser.Tokens;

using System;

namespace MetaParser.Consumers;

internal record ConsumerInfo
{
    #region Fields
    private ConsumerClauseInfo assigned;
    private ConsumerClauseInfo specified;
    #endregion

    #region Properties
    public int Stage { get; private set; }
    public readonly TokenInfo Token;

    public readonly int Index;
    public readonly EConsumerType Type;
    #endregion

    #region Accessors
    public PatternGroup? Start => specified.Start;
    public PatternGroup? Consume => specified.Consume;
    public PatternGroup? Stop => specified.Stop;
    public PatternGroup? Escape => specified.Escape;
    #endregion

    #region Accessors
    /// <summary> A consumer is considered open if it is dynamic and has no STOP criteria. </summary>
    public bool IsOpen => IsDynamic && assigned.Stop is null;
    /// <summary> 
    /// A consumer is considered dynamic if it is not constant, specifically if it has either a CONSUME or STOP criteria.
    /// So a consumer is "dynamic" if it involves consuming a variable number of elements.
    /// </summary>
    public bool IsDynamic => assigned.Consume is not null || assigned.Stop is not null;
    public bool IsConstant => assigned.Start is not null && assigned.Consume is null && assigned.Stop is null;
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

        assigned = new ConsumerClauseInfo() 
        {
            Start = data.Start,
            Consume = data.Consume,
            Stop = data.Stop,
            Escape = data.Escape,
        };

        specified = new ConsumerClauseInfo()
        {
            Start = IsOpen switch
            {
                false when assigned.Start is not null => assigned.Start,
                // no start condition, only a CONSUME criteria
                true when assigned.Start is null => assigned.Consume!,
                // for open ended consumers it is implied that their consume criteria is part of their start condition
                true when assigned.Start is not null => assigned.Start.Combine(assigned.Consume!) as PatternGroup,
                _ => throw new NotImplementedException()
            },
            Consume = assigned.Consume,
            Stop = assigned.Stop,
            Escape = assigned.Escape,
        };
    }
    #endregion

    #region Resolving
    public void Resolve(DependencyGraph graph)
    {
        // basically check if the node has any outgoing links which make more than two jumps
        if (graph.TryGetNode(this, out var consumerNode))
        {
            Stage = consumerNode!.GetDepth();
        }
    }
    #endregion
}
