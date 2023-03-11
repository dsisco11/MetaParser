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
    public PatternConst(string value, MetaParserContext context) : base(context)
    {
        _value = value;
    }
    #endregion

    public override bool IsRawValues => true;
    public override bool IsConstantLength => true;
    public override int Length => string.IsNullOrEmpty(Value) ? 0 : 1;
    public override bool HasChildren => false;


    public override Pattern Combine(Pattern other, MetaParserContext context)
    {
        return new PatternGroup(EPatternCondition.AllOf, context, this, other);
    }

    public override IEnumerator<Pattern> GetEnumerator()
    {
        yield break;
    }

    public override IEnumerable<EntityLink> ResolveLinks(MetaParserContext context)
    {
        yield break;
    }
}
