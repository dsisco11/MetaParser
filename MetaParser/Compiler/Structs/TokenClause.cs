using MetaParser.Json.Definitions;
using System.Collections;
using System.Collections.Generic;

namespace MetaParser.Compiler.Structs;

internal record struct TokenClause : IEnumerable<ConsumerClause>
{
    #region Properties
    /// <summary> The public name which this token maps to </summary>
    public string ID { get; set; }
    /// <summary>The internal parser constant alias for this token</summary>
    public string Name { get; set; }
    public EParsingStage Stage { get; set; }
    public List<ConsumerClause> Items { get; set; }
    #endregion

    #region Constructors
    public TokenClause(string id, EParsingStage stage) : this(id, id, stage)
    {
    }

    public TokenClause(string id, string name, EParsingStage stage)
    {
        ID = id;
        Name = name;
        Stage = stage;
        Items = new();
    }

    public TokenClause(TokenClause other)
    {
        ID = other.ID;
        Name = other.Name;
        Stage = other.Stage;
        Items = new();
    }
    #endregion

    public IEnumerator<ConsumerClause> GetEnumerator()
    {
        return ((IEnumerable<ConsumerClause>)Items).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
}
