using MetaParser.Parsing.Constructs;
using MetaParser.Syntax;

namespace MetaParser.Trees;

internal record PatternNode : GreenNode
{
    #region Fields
    public EPatternKind Kind { get; protected set; }
    #endregion

    public PatternNode(EPatternKind kind, GreenNode[] children) : base(children)
    {
        Kind = kind;
    }
}
