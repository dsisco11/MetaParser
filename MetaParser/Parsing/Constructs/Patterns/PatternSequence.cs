using MetaParser.Core;

using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Parsing.Constructs;

internal record PatternSequence : PatternEntity, IEnumerable<PatternEntity>
{
    #region Fields
    public readonly PatternEntity[] Items;
    #endregion

    #region Constructors
    public PatternSequence(EPatternKind kind, EntityRegistry registry, params PatternEntity[] items) : base(kind, registry)
    {
        Items = items;
    }
    #endregion

    #region Accessors
    public string ConditionJoiner
    {
        get => Kind switch
        {
            EPatternKind.AllOf => ", ",// an 'allof' is a comma separated list
            EPatternKind.OneOf => " or ",// a 'oneof' is an 'or' separated list
            EPatternKind.Not => " and ",// a 'not' is an 'and' separated list
            _ => string.Empty// no joiner
        };
    }

    public override bool IsSequence => Items.Length > 1;
    public override int Length
    {
        get => Kind switch
        {
            EPatternKind.OneOf => Items.Length > 0 ? Items.Min(static (x) => x.Length) : 0,// a 'oneof' is the minimum length of all items
            EPatternKind.AllOf => Items.Sum(static (x) => x.Length),// sequences which do not represent an open set of possibilities, are the sum of all items condition lengths
            _ => Items.Length// most sequence lengths are the sum of all items
        };
    }

    public override bool IsDeterministic
    {
        get
        {
            return Kind switch
            {
                EPatternKind.AllOf => Items.All(static (x) => x.IsDeterministic),// an 'allof' is deterministic if all items are deterministic
                EPatternKind.OneOf => false,// a 'oneof' is never deterministic, as it can be any of the items
                EPatternKind.Not => false,// a 'not' is never deterministic, as it is a logical exclusion
                _ => false
            };
        }
    }

    public override bool IsInlinable
    {
        get => Items.Length > 1 ? Items.All(static (x) => x.IsInlinable) : Items.Length == 1 && Items[0].IsInlinable;// a group is inlinable if all items are inlinable
    }

    public override bool IsConstantLength => Items.Length > 1 ? Items.All(static (x) => x.IsConstantLength) : Items.Length == 1 && Items[0].IsConstantLength;// a group is constant length if all items are constant length
    public override int MinConditions
    {
        get
        {
            if (Items.Length == 1) return Items[0].MinConditions;
            return Kind switch
            {
                // when pattern is a 'oneof' then the minimum number of conditions is the number of items which are not sequences, plus the minimum number of conditions of all sequences
                EPatternKind.OneOf when Items.Length == 1 && Items[0] is not PatternSequence => 0,
                EPatternKind.OneOf when Items.Length == 1 && Items[0] is PatternSequence s => s.MinConditions,
                EPatternKind.OneOf => Items.Count(static (x) => x is not PatternSequence) + Items.Min(static (x) => x is PatternSequence s ? s.MinConditions : 0),
                _ => Items.Sum(static (x) => x.MinConditions)// sequences which do not represent an open set of possibilities, are the sum of all items condition lengths
            };
        }
    }

    public override int MaxConditions
    {
        get
        {
            if (Items.Length == 1) return Items[0].MaxConditions;
            return Kind switch
            {
                // when pattern is a 'oneof' the maximum number of conditions is the number of items which are not sequences, plus the maximum number of conditions of all sequences
                EPatternKind.OneOf when Items.Length == 1 && Items[0] is not PatternSequence => 0,
                EPatternKind.OneOf when Items.Length == 1 && Items[0] is PatternSequence s => s.MaxConditions,
                EPatternKind.OneOf => Items.Count(static (x) => x is not PatternSequence) + Items.Max(static (x) => x is PatternSequence s ? s.MaxConditions : 0),
                _ => Items.Sum(static (x) => x.MaxConditions)// sequences which do not represent an open set of possibilities, are the sum of all items condition lengths
            };
        }
    }

    public override bool IsConditional
    {
        get
        {
            if (Items.Length == 1) return Items[0].IsConditional;
            return Kind switch
            {
                EPatternKind.OneOf when Items.Length > 1 => true,// a 'oneof' is always conditional, as it can be any of the items
                EPatternKind.Not => true,// a 'not' is always conditional, as it is a logical exclusion
                _ => Items.Any(static (x) => x.IsConditional)// otherwise it is conditional if any of the items are conditional
            };
        }
    }
    #endregion

    public override IEnumerator<PatternEntity> GetEnumerator()
    {
        return ((IEnumerable<PatternEntity>)Items).GetEnumerator();
    }

    public override IEnumerable<EntityLink> ResolveLinks(EntityRegistry Registry)
    {
        foreach (PatternEntity item in Items)
        {
            yield return new EntityLink(Key, item.Key);
        }
    }
}

