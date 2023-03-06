using System;
using System.Collections.Generic;

namespace MetaParser.Patternization;

internal sealed record PatternTokenRef : Pattern
{
    #region Fields
    private readonly string _tokenName;
    #endregion

    #region Properties
    public string TokenName => _tokenName;
    #endregion

    #region Constructors
    public PatternTokenRef(string value)
    {
        _tokenName = value;
    }
    #endregion

    public override int Length => 1;
    public override bool IsRawValues => true;
    public override bool IsConstantLength => true;
    public override bool HasChildren => false;


    public override Pattern Combine(Pattern other)
    {
        throw new NotImplementedException();
    }

    public override IEnumerable<Pattern> GetSubPatterns()
    {
        yield break;
    }
}
