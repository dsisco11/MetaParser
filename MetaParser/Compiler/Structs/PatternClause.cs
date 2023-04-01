using MetaParser.Parsing.Constructs;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Compiler.Structs;


/// <summary>
/// Represents an abstract pattern which can be used to create a matching sequence
/// </summary>
internal abstract record PatternClause : IEnumerable<PatternClause>
{
    public abstract EPatternKind Kind { get; }
    public abstract IEnumerator<PatternClause> GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary> Indicates whether the pattern is resolvable, meaning it can be represented as a simpler item </summary>
    public bool IsReducible => this.Any(x => x.Kind == Kind || x.IsReducible);

    public PatternClause Reduce()
    {
        if (IsReducible)
        {
            var reduced = new List<PatternClause>();
            foreach (var item in this)
            {
                if (item.IsReducible)
                {
                    reduced.Add(item.Reduce());
                }
                else if (item.Kind == Kind)
                {
                    reduced.AddRange(item);
                }
                else
                {
                    reduced.Add(item);
                }
            }
            return new PatternSequenceClause(Kind, reduced);
        }
        else
        {
            return this;
        }
    }
}

internal record PatternItemClause : PatternClause
{
    #region Fields
    private readonly EPatternKind kind = EPatternKind.Literal;
    #endregion

    #region Properties
    public override EPatternKind Kind => kind;
    public string Value { get; set; }
    #endregion

    #region Constructors
    public PatternItemClause(EPatternKind kind, string value)
    {
        Value = value;
        this.kind = kind;
    }
    #endregion

    public override IEnumerator<PatternClause> GetEnumerator()
    {
        yield break;
    }
}

internal record PatternSequenceClause : PatternClause
{
    #region Properties
    public override EPatternKind Kind { get; }
    public List<PatternClause> Items { get; set; }
    #endregion

    #region Constructors
    public PatternSequenceClause(EPatternKind condition, List<PatternClause> items)
    {
        Kind = condition;
        Items = items;
    }

    public PatternSequenceClause(EPatternKind condition, IEnumerable<PatternClause> items)
    {
        Kind = condition;
        Items = new (items);
    }

    public PatternSequenceClause(EPatternKind condition, params PatternClause[] items)
    {
        Kind = condition;
        Items = new (items);
    }
    #endregion

    public override IEnumerator<PatternClause> GetEnumerator()
    {
        return Items.GetEnumerator();
    }
}
