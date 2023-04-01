using MetaParser.Core;
using MetaParser.Exceptions;

using System.Collections.Generic;

namespace MetaParser.Parsing.Constructs;

internal sealed record PatternTokenRef : PatternEntity
{
    #region Fields
    public readonly string TokenName;
    #endregion

    #region Constructors
    public PatternTokenRef(EntityRegistry registry, string tokenName) : base(EPatternKind.Token, registry)
    {
        TokenName = tokenName;
    }
    #endregion

    #region Accessors
    public override int Length => 1;
    public override bool IsDeterministic
    {
        get => GetToken().DependencyInfo.NodeDepth.Max == 0;
    }

    public override bool IsInlinable
    { 
        get => GetToken().DependencyInfo.NodeDepth.Max == 0;
    }
    public override bool IsConstantLength => true;
    public override bool IsSequence => false;
    public override int MinConditions => IsConditional ? 1 : 0;
    public override int MaxConditions => IsConditional ? 1 : 0;
    public override bool IsConditional => !IsInlinable;// if token isnt inlineable, it must be a logical check
    #endregion

    public override IEnumerable<EntityLink> ResolveLinks(EntityRegistry Registry)
    {
        if (!Registry.TryGetEntityByName<TokenEntity>(TokenName, out var token))
        {
            throw new UnknownTokenException(TokenName);
        }

        yield return new EntityLink(Key, token.Key);
    }

    public TokenEntity GetToken()
    {
        if (!Registry.TryGetEntityByName<TokenEntity>(TokenName, out var token))
        {
            throw new UnknownTokenException(TokenName);
        }
        return token;
    }

}
