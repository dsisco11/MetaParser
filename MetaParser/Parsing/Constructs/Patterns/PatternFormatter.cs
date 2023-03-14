using MetaParser.Core;

using System;
using System.Linq;

namespace MetaParser.Parsing.Constructs;
using static CodeCommon;

internal static class PatternFormatter
{
    public static void WriteTo(MetaParserContext context, Pattern pattern, bool hasBufferAccess = false, bool allowSpanOperations = false)
    {
        var writer = context.writer;

        if (!hasBufferAccess || !allowSpanOperations || !pattern.IsRawValues || !pattern.IsInlinable)
        {// "buffer is [x, y, z, ..]"
            if (hasBufferAccess)
            {
                writer.Write(" is ");
            }

            writer.Write("[ ");
            writer.Write(Format(pattern));
            writer.Write(", ..]");
        }
        else
        {// "buffer.StartsWith(stackalloc []{ x, y, z }"
            writer.Write(".StartsWith(stackalloc []{ ");
            writer.Write(Format(pattern));
            writer.Write("})");
        }
    }

    public static string Format(Pattern pattern)
    {
        return pattern switch
        {
            PatternConst c => c.Value,
            PatternRange r => $"(>={r.Begin} and <={r.End})",
            PatternGroup g when g.Condition == EPatternCondition.OneOf && g.Items.Length > 1 => $"({string.Join(g.ConditionJoiner, g.Items.Select(o => Format(o)))})",
            PatternGroup g => string.Join(g.ConditionJoiner, g.Items.Select(o => Format(o))),
            PatternTokenRef t when t.IsInlinable => Format_Token_Id_Const_Ref(t.TokenName),
            PatternTokenRef t when !t.IsInlinable => $"{Format_Token_Start_Detection_Function_Name(t.TokenName)}()",
            _ => throw new NotImplementedException()
        };
    }
}
