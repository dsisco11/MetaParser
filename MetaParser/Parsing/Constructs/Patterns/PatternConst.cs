using MetaParser.Core;

using System.Collections.Generic;

namespace MetaParser.Parsing.Constructs;

internal sealed record PatternConst : Pattern
{
    #region Fields
    private readonly string _value;
    #endregion

    #region Properties
    public string Value => _value;
    #endregion

    #region Constructors
    public PatternConst(string value, ParserContext context) : base(context)
    {
        _value = value;
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


    public override Pattern Combine(Pattern other, ParserContext context)
    {
        return new PatternGroup(EPatternCondition.AllOf, context, this, other);
    }

    public override IEnumerator<Pattern> GetEnumerator()
    {
        yield break;
    }

    public override IEnumerable<EntityLink> ResolveLinks(TokenRegistry Registry)
    {
        yield break;
    }
}
