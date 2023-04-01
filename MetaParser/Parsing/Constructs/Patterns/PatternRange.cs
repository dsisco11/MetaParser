using MetaParser.Core;

namespace MetaParser.Parsing.Constructs;

internal sealed record PatternRange : PatternEntity
{
    #region Fields
    public readonly string Begin;
    public readonly string End;
    #endregion

    #region Constructors
    public PatternRange(EntityRegistry registry, string begin, string end) : base(EPatternKind.Range, registry)
    {
        Begin = begin;
        End = end;
    }
    #endregion

    #region Accessors
    public override bool IsDeterministic => false;
    public override bool IsInlinable => true;
    public override bool IsConstantLength => true;// a range always matches a single item
    public override int Length => 1;// a range always matches a single item
    public override bool IsSequence => false;
    public override int MinConditions => 2;// ranges require 2 logical checks
    public override int MaxConditions => 2;
    public override bool IsConditional => true;
    #endregion
}
