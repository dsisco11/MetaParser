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
        var tokenNode = Registry.Tree.GetNode(NodeID);
        var consumerKeys = tokenNode.Where(static (n) => n.Value.Type == NodeType.Consumer).Select(static (n) => n.Value);
        foreach (var key in consumerKeys)
        {
            yield return Registry.Consumers[key];
        }
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
