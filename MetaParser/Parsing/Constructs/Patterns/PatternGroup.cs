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
        _items = CollapseInnerGroups(items, condition).ToArray();
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

    public override bool HasChildren => _items.Length > 0;
    public override int Length
    {
        get => Condition switch
        {
            EPatternCondition.OneOf => _items.Length > 0 ? _items.Min(static (x) => x.Length) : 0,
            _ => _items.Length
        };
    }

    public override bool IsRawValues
    {
        get
        {
            return Condition switch
            {
                EPatternCondition.AllOf => !_items.Any(static (x) => !x.IsRawValues),
                EPatternCondition.OneOf => false,
                _ => false
            };
        }
    }

    public override bool IsInlinable
    {
        get => _items.Length > 1 ? _items.All(static (x) => x.IsInlinable) : _items.Length == 1 && _items[0].IsInlinable;
    }

    public override bool IsConstantLength => _items.Length > 1 ? _items.All(static (x) => x.IsConstantLength) : _items.Length == 1 && _items[0].IsConstantLength;
    public override int MinLogicalLength
    {
        get
        {
            if (_items.Length == 1) return _items[0].MinLogicalLength;
            return Condition switch
            {
                EPatternCondition.OneOf => _items.Min(static (x) => x.MinLogicalLength),
                _ => _items.Sum(static (x) => x.MinLogicalLength)
            };
        }
    }

    public override int MaxLogicalLength
    {
        get
        {
            if (_items.Length == 1) return _items[0].MaxLogicalLength;
            return Condition switch
            {
                EPatternCondition.OneOf => _items.Max(static (x) => x.MaxLogicalLength),
                _ => _items.Sum(static (x) => x.MaxLogicalLength)
            };
        }
    }

    public override bool IsLogical
    {
        get
        {
            if (_items.Length == 1) return _items[0].IsLogical;
            return Condition switch
            {
                EPatternCondition.OneOf when _items.Length > 1 => true,
                _ => _items.Any(static (x) => x.IsLogical)
            };
        }
    }
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
        foreach (Pattern item in _items)
        {
            yield return new EntityLink(Key, item.Key);
        }
    }

    #region Sequence Flattening
    private static List<Pattern> CollapseInnerGroups(Pattern[] patterns, EPatternCondition condition)
    {
        List<Pattern> results = new List<Pattern>();
        foreach (Pattern item in patterns)
        {
            // If this pattern group is the same type as us, then we can flatten it in the enumeration
            if (item is PatternGroup subGroup && subGroup.Condition == condition)
            {
                foreach (Pattern subItem in subGroup)
                {
                    results.Add(subItem);
                }
            }
            else
            {
                results.Add(item);
            }
        }

        return results;
    }
    #endregion
}

