using MetaParser.Core;
using MetaParser.Parsing.Constructs.Core;

using System;
using System.Linq;

namespace MetaParser.Parsing.Constructs.Patternization;
using static CodeCommon;

internal static class PatternFormatter
{
    public static void WriteTo(MetaParserContext context, Pattern pattern, bool hasBufferAccess = false, bool allowSpanOperations = false)
    {
        var writer = context.writer;

        if (hasBufferAccess && allowSpanOperations && pattern.IsRawValues)
        {
            writer.Write(".StartsWith(stackalloc []{ ");
            writer.Write(ToString(pattern));
            writer.Write("})");
        }
        else
        {
            if (hasBufferAccess)
            {
                writer.Write(" is ");
            }

            writer.Write("[ ");
            writer.Write(ToString(pattern));
            writer.Write(", ..]");
        }
    }

    public static string ToString(Pattern pattern)
    {
        return pattern switch
        {
            PatternConst c => c.Value,
            PatternRange r => $"(>={r.Begin} and <={r.End})",
            PatternGroup g when g.Condition == EPatternCondition.OneOf && g.Items.Length > 1 => $"({string.Join(g.ConditionJoiner, g.Items.Select(o => ToString(o)))})",
            PatternGroup g => string.Join(g.ConditionJoiner, g.Items.Select(o => ToString(o))),
            PatternEmpty _ => string.Empty,
            PatternTokenRef t => Format_Token_Id_Const_Ref(t.TokenName),
            _ => throw new NotImplementedException()
        };
    }
}
