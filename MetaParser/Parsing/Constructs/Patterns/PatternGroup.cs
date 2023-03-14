using MetaParser.Core;

using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Parsing.Constructs;

internal record PatternGroup : Pattern, IEnumerable<Pattern>
{
    #region Fields
    private readonly Pattern[] _items;
    private readonly EPatternCondition _condition = EPatternCondition.AllOf;
    #endregion

    #region Properties
    internal Pattern[] Items => _items;
    public EPatternCondition Condition => _condition;
    #endregion

    #region Constructors
    public PatternGroup(EPatternCondition condition, MetaParserContext context, params Pattern[] items) : base(context)
    {
        _condition = condition;
        _items = items;
    }
    #endregion

    #region Accessors
    public string ConditionJoiner
    {
        get => Condition switch
        {
            EPatternCondition.Only => string.Empty,
            EPatternCondition.AllOf => ", ",
            EPatternCondition.OneOf => " or ",
            _ => throw new System.NotImplementedException()
        };
    }

    public override int Length
    {
        get => Condition switch
        {
            EPatternCondition.OneOf => Items.Length > 0 ? Items.Max(x => x.Length) : 0,
            _ => Items.Sum((p) => p.Length)
        };
    }

    public int MinLength
    {
        get => Condition switch
        {
            EPatternCondition.OneOf => Items.Length > 0 ? Items.Min(x => x.Length) : 0,
            _ => Items.Sum((p) => p.Length)
        };
    }

    public override bool IsRawValues
    {
        get
        {
            return Condition switch
            {
                EPatternCondition.AllOf => !Items.Any(x => !x.IsRawValues),
                EPatternCondition.OneOf => false,
                _ => false
            };
        }
    }

    public override bool IsInlinable
    {
        get => Items.Length > 1 ? Items.All(x => x.IsInlinable) : Items.Length == 1 ? Items[0].IsInlinable : false;
    }

    public override bool IsConstantLength => !_items.Any(x => !x.IsConstantLength);
    public override bool HasChildren => true;
    #endregion

    public override Pattern Combine(Pattern other, MetaParserContext context)
    {
        List<Pattern> patterns = new List<Pattern>(_items)
        {
            other
        };

        return new PatternGroup(Condition, context, patterns.ToArray());
    }

    public override IEnumerator<Pattern> GetEnumerator()
    {
        return ((IEnumerable<Pattern>)_items).GetEnumerator();
    }

    public override IEnumerable<EntityLink> ResolveLinks(MetaParserRegistry Registry)
    {
        foreach(Pattern item in _items)
        {
            yield return new EntityLink(Key, item.Key);
        }

        yield break;
    }
}