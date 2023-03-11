using MetaParser.Core;
using MetaParser.Exceptions;

using System;
using System.Collections.Generic;

namespace MetaParser.Parsing.Constructs;

internal sealed record PatternTokenRef : Pattern
{
    #region Fields
    private readonly string _tokenName;
    #endregion

    #region Properties
    public string TokenName => _tokenName;
    #endregion

    #region Constructors
    public PatternTokenRef(string value, MetaParserContext context) : base(context)
    {
        _tokenName = value;
    }
    #endregion

    public override int Length => 1;
    public override bool IsRawValues => true;
    public override bool IsConstantLength => true;
    public override bool HasChildren => false;


    public override Pattern Combine(Pattern other, MetaParserContext context)
    {
        return new PatternGroup(EPatternCondition.OneOf, context, this, other);
    }

    public override IEnumerator<Pattern> GetEnumerator()
    {
        yield break;
    }

    public override IEnumerable<EntityLink> ResolveLinks(MetaParserContext context)
    {
        if (!context.Registry.TryGetToken(_tokenName, out var token))
        {
            throw new UnknownTokenException(_tokenName);
        }

        yield return new EntityLink(NodeID, token.NodeID);
    }
}
