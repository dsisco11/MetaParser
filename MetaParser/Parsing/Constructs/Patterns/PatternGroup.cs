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
            EPatternCondition.Only => string.Empty,// an only is a single item, so no joiner
            EPatternCondition.AllOf => ", ",// an allof is a comma separated list
            EPatternCondition.OneOf => " or ",// a oneof is an or separated list
            _ => throw new System.NotImplementedException()
        };
    }

    public override bool IsSequence => _items.Length > 0;
    public override int Length
    {
        get => Condition switch
        {
            EPatternCondition.OneOf => _items.Length > 0 ? _items.Min(static (x) => x.Length) : 0,// a oneof is the minimum length of all items
            _ => _items.Length// an allof is the sum of all items
        };
    }

    public override bool IsDeterministic
    {
        get
        {
            return Condition switch
            {
                EPatternCondition.AllOf => _items.All(static (x) => x.IsDeterministic),// an allof is deterministic if all items are deterministic
                EPatternCondition.OneOf => false,// a oneof is never deterministic, as it can be any of the items
                _ => false
            };
        }
    }

    public override bool IsInlinable
    {
        get => _items.Length > 1 ? _items.All(static (x) => x.IsInlinable) : _items.Length == 1 && _items[0].IsInlinable;// a group is inlinable if all items are inlinable
    }

    public override bool IsConstantLength => _items.Length > 1 ? _items.All(static (x) => x.IsConstantLength) : _items.Length == 1 && _items[0].IsConstantLength;// a group is constant length if all items are constant length
    public override int MinConditions
    {
        get
        {
            if (_items.Length == 1) return _items[0].MinConditions;
            return Condition switch
            {
                EPatternCondition.OneOf => _items.Min(static (x) => x.MinConditions),// a oneof is the minimum number of conditions of all items
                _ => _items.Sum(static (x) => x.MinConditions)// an allof is the sum of all items
            };
        }
    }

    public override int MaxConditions
    {
        get
        {
            if (_items.Length == 1) return _items[0].MaxConditions;
            return Condition switch
            {
                EPatternCondition.OneOf => _items.Max(static (x) => x.MaxConditions),// a oneof is the maximum number of conditions of all items
                _ => _items.Sum(static (x) => x.MaxConditions)// an allof is the sum of all items
            };
        }
    }

    public override bool IsConditional
    {
        get
        {
            if (_items.Length == 1) return _items[0].IsConditional;
            return Condition switch
            {
                EPatternCondition.OneOf when _items.Length > 1 => true,// a oneof is always conditional, as it can be any of the items
                _ => _items.Any(static (x) => x.IsConditional)// otherwise it is conditional if any of the items are conditional
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

