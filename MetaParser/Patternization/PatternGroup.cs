using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Patternization;

internal record PatternGroup : Pattern
{
    public readonly Pattern[] items;
    public readonly EPatternCondition condition = EPatternCondition.AllOf;

    public string ConditionJoiner
    {
        get => condition switch
        {
            EPatternCondition.Only => string.Empty,
            EPatternCondition.AllOf => ", ",
            EPatternCondition.OneOf => " or ",
            _ => throw new System.NotImplementedException()
        };
    }

    public PatternGroup(EPatternCondition condition, params Pattern[] items)
    {
        this.condition = condition;
        this.items = items;
    }

    public override int Length
    {
        get => condition switch
        {
            EPatternCondition.OneOf => items.Length > 0 ? items.Max(x => x.Length) : 0,
            _ => items.Sum((Pattern p) => p.Length)
        };
    }

    public int MinLength
    {
        get => condition switch
        {
            EPatternCondition.OneOf => items.Length > 0 ? items.Min(x => x.Length) : 0,
            _ => items.Sum((Pattern p) => p.Length)
        };
    }

    public override bool IsRawValues
    {
        get
        {
            return condition switch
            {
                EPatternCondition.AllOf => !items.Any(x => !x.IsRawValues),
                EPatternCondition.OneOf => false,
                _ => false
            };
        }
    }
    public override bool IsConstantLength => !items.Any(x => !x.IsConstantLength);
    public override bool ContainsItems => true;


    public override IEnumerable<Pattern> GetSubPatterns()
    {
        foreach(var item in items)
        {
            if (item.ContainsItems)
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
        List<Pattern> patterns = new List<Pattern>(items)
        {
            other
        };

        return new PatternGroup(condition, patterns.ToArray());
    }
}