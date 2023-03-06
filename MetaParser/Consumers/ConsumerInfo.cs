using MetaParser.Contexts;
using MetaParser.Exceptions;
using MetaParser.Graphs;
using MetaParser.Patternization;
using MetaParser.Tokens;

using System;
using System.Linq;

namespace MetaParser.Consumers;

internal record ConsumerInfo
{
    #region Fields
    private readonly ConsumerClauseInfo assigned;
    private readonly ConsumerClauseInfo specified;

    public readonly TokenInfo Token;
    public readonly int Index;
    public readonly EConsumerType Type;
    #endregion

    #region Properties
    public ResolvedVertexNode DependencyInfo { get; set; }
    #endregion

    #region Accessors
    public PatternGroup Start => specified.Start!;
    public PatternGroup? Consume => specified.Consume;
    public PatternGroup? Stop => specified.Stop;
    public PatternGroup? Escape => specified.Escape;
    #endregion

    #region Accessors
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
    public ConsumerInfo(MetaParserContext context, TokenInfo token, ConsumerData data)
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
            Consume = assigned.Consume ?? IsClosed switch
            {
                true when Type == EConsumerType.Token => new PatternGroup(EPatternCondition.OneOf, Get_All_Other_Tokens(context)),
                _ => null,
            },
            Stop = assigned.Stop,
            Escape = assigned.Escape,
        };
    }
    #endregion

    private Pattern[] Get_All_Other_Tokens(MetaParserContext context)
    {
        var allOthers = context.Tokens.Values.Where((x) => x.Index != Token.Index);
        return allOthers.Select(static (x) => new PatternTokenRef(x.Name)).ToArray();
    }
}
