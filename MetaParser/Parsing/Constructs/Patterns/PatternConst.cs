using MetaParser.Core;

namespace MetaParser.Parsing.Constructs;

internal sealed record PatternConst : PatternEntity
{
    #region Fields
    public readonly string Value;
    #endregion

    #region Constructors
    public PatternConst(EntityRegistry registry, string value) : base(EPatternKind.Literal, registry)
    {
        Value = value;
    }
    #endregion

    #region Accessors
    public override bool IsDeterministic => true;
    public override bool IsInlinable => true;
    public override bool IsConstantLength => true;
    public override int Length => string.IsNullOrEmpty(Value) ? 0 : 1;
    public override bool IsSequence => false;
    public override int MinConditions => 0;
    public override int MaxConditions => 0;
    public override bool IsConditional => false;
    #endregion
}
