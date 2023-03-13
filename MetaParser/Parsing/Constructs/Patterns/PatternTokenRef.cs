using MetaParser.Core;
using MetaParser.Exceptions;
using MetaParser.Graphs;

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
    public override bool IsInlinable
    { 
        get
        {
            if (!Registry.TryGetToken(_tokenName, out var token))
            {
                throw new UnknownTokenException(_tokenName);
            }

            // if this token depends on another non-data token, then it has complex requirements and cannot be inlined
            return token.DependencyInfo.Depth[(int)NodeType.Token].Max < 2;
        }
    }
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

    public override IEnumerable<EntityLink> ResolveLinks(MetaParserRegistry Registry)
    {
        if (!Registry.TryGetToken(_tokenName, out var token))
        {
            throw new UnknownTokenException(_tokenName);
        }

        yield return new EntityLink(NodeID, token.NodeID);
    }
}
