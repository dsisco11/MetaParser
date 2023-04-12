using MetaParser.Core;
using MetaParser.Exceptions;
using MetaParser.Graphs;

using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Parsing.Constructs;

internal sealed record PatternTokenRef : PatternEntity
{
    #region Fields
    public readonly string Value;
    #endregion

    #region Constructors
    public PatternTokenRef(EntityRegistry registry, string tokenName) : base(EPatternKind.Token, registry)
    {
        Value = tokenName;
    }
    #endregion

    #region Accessors
    public override int Length => 1;
    public override bool IsDeterministic
    {
        get => GetToken().GraphInfo.Depth == 0;
    }

    public override bool IsInlinable
    { 
        get => GetToken().GraphInfo.Depth == 0;
    }
    public override bool IsConstantLength => true;
    public override bool IsSequence => false;
    public override int MinConditions => IsConditional ? 1 : 0;
    public override int MaxConditions => IsConditional ? 1 : 0;
    public override bool IsConditional => !IsInlinable;// if token isnt inlineable, it must be a logical check
    #endregion

    public override IEnumerable<EntityLink> ResolveLinks(EntityRegistry Registry)
    {
        if (!Registry.TryGetEntityByName<TokenEntity>(Value, out var token))
        {
            throw new UnknownTokenException(Value);
        }

        // find our parent token
        //var parentToken = KeyTreeNodeWalker.Walk(HierarchyNode, static (x) => x.Type == NodeType.Token, KeyTreeNodeWalker.TraversalOrder.Ascending).Single() ?? throw new MetaParserException($"Unable to find parent token for pattern: {this}");
        var parentToken = KeyTreeNodeWalker.Walk(HierarchyNode, static (x) => x.Type == NodeType.Token, KeyTreeNodeWalker.TraversalOrder.Ascending).Single();
        yield return new EntityLink(parentToken, token.Key);
    }

    public TokenEntity GetToken()
    {
        if (!Registry.TryGetEntityByName<TokenEntity>(Value, out var token))
        {
            throw new UnknownTokenException(Value);
        }
        return token;
    }

}
