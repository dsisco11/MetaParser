using MetaParser.Graphs;
using MetaParser.Parsing.Constructs;

using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Mermaid;

internal static class RegistryEntityFormatter
{
    public static string Format_Entity_Title(EntityKey Key, GraphEntity entity)
    {
        switch (Key.Type)
        {
            case NodeType.Data:
                break;
            case NodeType.Pattern:
                {
                    //writer.Write("(");
                    return Get_Pattern_Contents((PatternEntity)entity);
                    //writer.Write(")");
                }
            case NodeType.Consumer:
                {
                    return $"{Key.Index}";
                    //return $@"{Key}{{""{Key.Index}""}}";
                }
            case NodeType.Token:
                {
                    //writer.Write(@"[""");
                    return ((TokenEntity)entity).Name;
                    //writer.Write(@"""]");
                }
        }

        return string.Empty;
    }

    private static string Get_Pattern_Contents(PatternEntity pattern)
    {
        return pattern switch
        {
            PatternConst c => $@"{SymbolDisplay.FormatLiteral(c.Value, true)}",
            PatternRange r => $"[{r.Begin}, {r.End}]",
            //PatternSequence g => $@"""{{{g.Key.Index}}}""",
            PatternSequence g => "Group",
            PatternTokenRef t => $@"""#{t.Value}""",
            _ => throw new System.NotImplementedException(),
        };
    }
}
