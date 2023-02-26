using System.Linq;

namespace MetaParser.Patternization;

internal record PatternGroup : Pattern
{
    public readonly Pattern[] items;
    public readonly EPatternCondition condition = EPatternCondition.All;

    public string ConditionJoiner
    {
        get => condition switch
        {
            EPatternCondition.Single => string.Empty,
            EPatternCondition.All => ", ",
            EPatternCondition.Any => " or ",
            _ => throw new System.NotImplementedException()
        };
    }

    public PatternGroup(EPatternCondition condition, params Pattern[] items)
    {
        this.condition = condition;
        this.items = items;
    }

    public override bool IsConstant => items.Any(x => !x.IsConstant);
    public override int Length
    {
        get => condition switch
        {
            EPatternCondition.Any => items.Length == 0 ? 0 : 1,// For 'any' type patterns, which describe a sequence of alternate options, the length is always 1 or 0
            _ => items.Sum((Pattern p) => p.Length)
        };
    }
}