using MetaParser.Parsing.Constructs;

using System.Collections;

namespace MetaParser.Trees;

internal static class NodeFactory
{
    public static PatternNode Create_Pattern_Literal(string value) => new PatternNode(EPatternKind.Literal, new[] { new ValueLiteralNode(value) });
    public static PatternNode Create_Pattern_Range(string start, string end) => new PatternNode(EPatternKind.Range, new[] { new ValueLiteralNode(start), new ValueLiteralNode(end) });
    public static PatternNode Create_Pattern_Token(string token) => new PatternNode(EPatternKind.Token, new[] { new ValueLiteralNode(token) });
}
