using MetaParser.Core;

using System.Collections.Generic;

namespace MetaParser.Parsing.Constructs;

internal sealed record PatternRange : Pattern
{
    #region Fields
    private readonly string _begin;
    private readonly string _end;
    #endregion

    #region Properties
    public string Begin => _begin;
    public string End => _end;

    #endregion

    #region Constructors
    public PatternRange(string begin, string end, MetaParserContext context) : base(context)
    {
        _begin = begin;
        _end = end;
    }
    #endregion

    #region Accessors
    public override bool IsRawValues => false;
    public override bool IsInlinable => true;
    public override bool IsConstantLength => Begin.Length == End.Length;
    public override int Length => string.IsNullOrEmpty(Begin) && string.IsNullOrEmpty(End) ? 0 : 1;
    public override bool HasChildren => false;
    public override int MinLogicalLength => 2;// ranges require 2 logical checks
    public override int MaxLogicalLength => 2;
    public override bool IsLogical => true;
    #endregion

    public override Pattern Combine(Pattern other, MetaParserContext context)
    {
        return new PatternGroup(EPatternCondition.OneOf, context, this, other);
    }

    public override IEnumerator<Pattern> GetEnumerator()
    {
        yield break;
    }

    public override IEnumerable<EntityLink> ResolveLinks(MetaParserRegistry Registry)
    {
        yield break;
    }
}
