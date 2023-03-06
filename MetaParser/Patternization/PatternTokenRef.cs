using MetaParser.Tokens;

using System;
using System.Collections.Generic;

namespace MetaParser.Patternization;

internal sealed record PatternTokenRef : Pattern
{
    #region Fields
    private readonly WeakReference<TokenInfo> _token;
    #endregion

    #region Properties
    public TokenInfo? Token => _token.TryGetTarget(out var outPtr) ? outPtr : null;
    #endregion

    #region Constructors
    public PatternTokenRef(TokenInfo value)
    {
        this._token = new (value);
    }
    #endregion

    public override int Length => 1;
    public override bool IsRawValues => true;
    public override bool IsConstantLength => true;
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
