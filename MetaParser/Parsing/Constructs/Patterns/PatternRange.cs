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
    public PatternRange(string begin, string end, ParserContext context) : base(context)
    {
        _begin = begin;
        _end = end;
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

    public override Pattern Combine(Pattern other, ParserContext context)
    {
        return new PatternGroup(EPatternCondition.OneOf, context, this, other);
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
