namespace MetaParser.Compiler.Structs;

internal record struct ConsumerClause
{
    #region Properties
    public IPatternClause? Start { get; set; }
    public IPatternClause? Consume { get; set; }
    public IPatternClause? Stop { get; set; }
    public IPatternClause? Escape { get; set; }
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
}
