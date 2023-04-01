namespace MetaParser.Compiler.Structs;

internal record ConsumerClause
{
    #region Properties
    public PatternClause? Start { get; set; }
    public PatternClause? Consume { get; set; }
    public PatternClause? Stop { get; set; }
    public PatternClause? Escape { get; set; }
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
