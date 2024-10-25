using MetaParser.Syntax;

namespace MetaParser.Trees;

public record ValueLiteralNode : GreenNode
{
    #region Fields
    public readonly string Value;
    #endregion

    public ValueLiteralNode(string value)
    {
        Value = value;
    }
}