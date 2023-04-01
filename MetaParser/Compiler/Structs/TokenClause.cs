using MetaParser.Json.Definitions;
using System.Collections;
using System.Collections.Generic;

namespace MetaParser.Compiler.Structs;

internal record TokenClause : IEnumerable<ConsumerClause>
{
    #region Properties
    /// <summary> The final name which this token maps to </summary>
    public string ID { get; set; }
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
        Items = new();
        Stage = stage;
    }
    #endregion

    public IEnumerator<ConsumerClause> GetEnumerator()
    {
        return ((IEnumerable<ConsumerClause>)Items).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
}
