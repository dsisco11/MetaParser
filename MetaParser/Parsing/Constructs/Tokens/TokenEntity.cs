using MetaParser.Core;
using MetaParser.Graphs;
using MetaParser.Parsing.Constructs.Consumers;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Parsing.Constructs;
internal record TokenEntity : GraphEntity, IComparable<TokenEntity>
{
    #region Fields
    public readonly string ID;
    public readonly string Name;
    #endregion

    #region Accessors
    public int Index => Key.Index;

    public IEnumerable<ConsumerEntity> GetConsumers()
    {
        if (!Registry.Tree.TryGetNode(Key, out var tokenNode))
        {
            yield break;
        }

        var consumerKeys = tokenNode.Where(static (n) => n.Value.Type == NodeType.Consumer).Select(static (n) => n.Value);
        foreach (var key in consumerKeys)
        {
            if (Registry.TryGetEntity<ConsumerEntity>(key, out var entity))
            {
                yield return entity;
            }
        }
    }

    public bool IsConstant => GetConsumers().All(static (c) => c.IsConstant);
    public bool IsDynamic => GetConsumers().All(static (c) => c.IsDynamic);
    public bool IsComplex => GetConsumers().Any(static (c) => c.IsDynamic) && GetConsumers().Any(static (c) => c.IsConstant);
    public bool HasLexing => GetConsumers().Any(static (c) => c.Kind == EConsumerKind.Lexer);
    public bool HasSyntax => GetConsumers().Any(static (c) => c.Kind == EConsumerKind.Syntax);

    #endregion

    #region Constructors
    public TokenEntity(EntityRegistry registry, string id, string name) : base(NodeType.Token, registry)
    {
        ID = id;
        Name = name;
    }
    #endregion

    #region IComparable
    public int CompareTo(TokenEntity other)
    {
        return TokenSorter.Instance.Compare(this, other);
    }
    #endregion

    public override string ToString()
    {
        return $"{Key} | Name: {Name} | DependencyInfo: {GraphInfo}";
    }
}
