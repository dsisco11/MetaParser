using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Patternization;

internal record PatternGroup : Pattern
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
    public PatternGroup(EPatternCondition condition, params Pattern[] items)
    {
        _condition = condition;
        _items = items;
    }
    #endregion

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
            _ => Items.Sum((Pattern p) => p.Length)
        };
    }

    public int MinLength
    {
        get => Condition switch
        {
            EPatternCondition.OneOf => Items.Length > 0 ? Items.Min(x => x.Length) : 0,
            _ => Items.Sum((Pattern p) => p.Length)
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
    public override bool IsConstantLength => !Items.Any(x => !x.IsConstantLength);
    public override bool HasChildren => true;


    public override IEnumerable<Pattern> GetSubPatterns()
    {
        foreach(var item in Items)
        {
            if (item.HasChildren)
            {
                foreach (var o in item.GetSubPatterns())
                {
                    yield return o;
                }
            }
            else
            {
                yield return item;
            }
        }

        yield break;
    }

    public override Pattern Combine(Pattern other)
    {
        List<Pattern> patterns = new List<Pattern>(Items)
        {
            other
        };

        return new PatternGroup(Condition, patterns.ToArray());
    }
}