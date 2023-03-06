using System.Collections.Generic;

namespace MetaParser.Patternization;

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
    public PatternRange(string begin, string end)
    {
        _begin = begin;
        _end = end;
    }
    #endregion

    public override bool IsRawValues => false;
    public override bool IsConstantLength => Begin.Length == End.Length;
    public override int Length => (string.IsNullOrEmpty(Begin) && string.IsNullOrEmpty(End)) ? 0 : 1;
    public override bool HasChildren => false;

    public override Pattern Combine(Pattern other)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerable<Pattern> GetSubPatterns()
    {
        yield break;
    }
}
