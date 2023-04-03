using MetaParser.Parsing.Constructs;
using System.Collections;
using System.Collections.Generic;

namespace MetaParser.Compiler.Structs;

internal record struct PatternItemClause : IPatternClause
{
    #region Fields
    private readonly EPatternKind _kind = EPatternKind.Literal;
    #endregion

    #region Properties
    public EPatternKind Kind => _kind;
    public string Value { get; set; }
    #endregion

    #region Constructors
    public PatternItemClause(EPatternKind kind, string value)
    {
        Value = value;
        this._kind = kind;
    }
    #endregion
    public bool IsReducible => false;
    public IPatternClause Reduce() => this;

    public IEnumerator<IPatternClause> GetEnumerator()
    {
        yield break;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        yield break;
    }
}

internal record PatternSequenceClause : IPatternClause
{
    #region Fields
    private readonly EPatternKind _kind;
    #endregion

    #region Properties
    public EPatternKind Kind => _kind; 
    public List<IPatternClause> Items { get; private set; }
    #endregion

    #region Constructors
    public PatternSequenceClause(EPatternKind condition, List<IPatternClause> items)
    {
        _kind = condition;
        Items = items;
    }

    public PatternSequenceClause(EPatternKind condition, IEnumerable<IPatternClause> items)
    {
        _kind = condition;
        Items = new(items);
    }

    public PatternSequenceClause(EPatternKind condition, params IPatternClause[] items)
    {
        _kind = condition;
        Items = new(items);
    }
    #endregion

    public IEnumerator<IPatternClause> GetEnumerator() => Items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

    public bool IsReducible
    {
        get
        {
            foreach (IPatternClause clause in Items)
            {
                if (clause.IsReducible || clause.Kind == _kind)
                    return true;
            }

            return false;
        }
    }

    public IPatternClause Reduce()
    {
        if (IsReducible)
        {
            var reduced = new List<IPatternClause>();
            foreach (var item in this)
            {
                if (item.IsReducible)
                {
                    reduced.Add(item.Reduce());
                }
                else if (item.Kind == Kind)
                {
                    if (item is PatternSequenceClause seq)
                    {
                        reduced.AddRange(seq.Items);
                    }
                    else
                    {
                        reduced.AddRange(item);
                    }
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
