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
    public int Index => Key.Index;

    public IEnumerable<Consumer> GetConsumers()
    {
        if (!Registry.Tree.TryGetNode(Key, out var tokenNode))
        {
            yield break;
        }

        var consumerKeys = tokenNode.Where(static (n) => n.Value.Type == NodeType.Consumer).Select(static (n) => n.Value);
        foreach (var key in consumerKeys)
        {
            yield return Registry.Consumers[key];
        }
    }

    public bool IsConstant => GetConsumers().All(static (c) => c.IsConstant);
    public bool IsDynamic => GetConsumers().All(static (c) => c.IsDynamic);
    public bool IsComplex => GetConsumers().Any(static (c) => c.IsDynamic) && GetConsumers().Any(static (c) => c.IsConstant);

    public bool HasLexing => GetConsumers().Any(static (c) => c.Type == EConsumerType.Lexer);
    public bool HasSyntax => GetConsumers().Any(static (c) => c.Type == EConsumerType.Syntax);

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
        return Key.CompareTo(other.Key);
    }
    #endregion

    #region IDependencyGraphEntity
    public override IEnumerable<EntityLink> ResolveLinks(MetaParserRegistry Registry)
    {
        yield break;
    }
    #endregion

    public override string ToString()
    {
        return $"{Key} | Name: {Name} | DependencyInfo: {DependencyInfo}";
    }
}
