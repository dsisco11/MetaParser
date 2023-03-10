using MetaParser.Parsing.Constructs.Core;

using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Parsing.Constructs.Patternization;

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
    public override bool IsConstantLength => !_items.Any(x => !x.IsConstantLength);
    public override bool HasChildren => true;


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
        foreach (Pattern item in _items)
        {
            if (item.HasChildren)
            {
                foreach (Pattern o in item)
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
}