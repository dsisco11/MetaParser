using MetaParser.Core;
using MetaParser.Graphs;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Parsing.Constructs;
internal record TokenInfo : GraphableEntity, IComparable<TokenInfo>
{
    #region Fields
    public readonly string Name;
    #endregion

    #region Accessors
    public int Index => NodeID.Index;

    public IEnumerable<Consumer> GetConsumers()
    {
        var registry = Registry;
        return DependencyInfo.Outgoing.Union(DependencyInfo.Incoming)
            .Where(c => c.Key.Type == NodeType.Consumer && c.Key.Parent == NodeID)
            .Select(c => registry.Consumers[c.Key]);
    }
    #endregion

    #region Constructors
    public TokenInfo(string name, MetaParserContext context) : base(new EntityKey(NodeType.Token, context.Registry.GetNextTokenIndex()), context)
    {
        Name = name;
    }
    #endregion

    #region IComparable
    public int CompareTo(TokenInfo other)
    {
        return NodeID.CompareTo(other.NodeID);
    }
    #endregion

    #region IDependencyGraphEntity
    public override IEnumerable<EntityLink> ResolveLinks(MetaParserRegistry Registry)
    {
        yield break;
    }
    #endregion
}
