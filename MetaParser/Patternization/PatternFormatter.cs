using MetaParser.Core;

using System;
using System.Linq;

namespace MetaParser.Patternization;
using static CodeCommon;

internal static class PatternFormatter
{
    public static void WriteTo(MetaParserContext context, Pattern pattern, bool hasBufferAccess = false, bool allowSpanOperations = false)
    {
        var writer = context.writer;

        if (hasBufferAccess && allowSpanOperations && pattern.IsRawValues)
        {
            writer.Write(".StartsWith(stackalloc []{ ");
            writer.Write(Translate(context, pattern));
            writer.Write("})");
        }
        else
        {
            if (hasBufferAccess)
            {
                writer.Write(" is ");
            }

            writer.Write("[ ");
            writer.Write(Translate(context, pattern));
            writer.Write(", ..]");
        }
    }

    private static string Translate(MetaParserContext context, Pattern pattern)
    {
        return pattern switch
        {
            PatternConst c => c.Value,
            PatternRange r => $"(>={r.Begin} and <={r.End})",
            PatternGroup g when g.Condition == EPatternCondition.OneOf && g.Items.Length > 1 => $"({string.Join(g.ConditionJoiner, g.Items.Select(o => Translate(context, o)))})",
            PatternGroup g => string.Join(g.ConditionJoiner, g.Items.Select(o => Translate(context, o))),
            PatternEmpty _ => string.Empty,
            PatternTokenRef t => Format_Token_Id_Const_Ref(t.TokenName),
            _ => throw new NotImplementedException()
        };
    }
}
