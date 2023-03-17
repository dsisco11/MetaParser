using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using MetaParser.Parsing.Constructs;

using System;
using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class WritePatternAsExpression : MetaCodeBuilder
{
    protected override void Write(MetaParserContext context)
    {
        var writer = context.Writer;
        var pattern = context.WorkingSet.Patterns.Single();

        writer.Write(Format(pattern));
    }

    private static string Format(Pattern pattern)
    {
        return pattern switch
        {
            PatternConst c => c.Value,
            PatternRange r => $"(>={r.Begin} and <={r.End})",
            // tokens
            PatternTokenRef t when !t.IsInlinable => $"{Format_Token_Start_Detection_Function_Name(t.TokenName)}",
            PatternTokenRef t => Format_Token_Id_Const_Ref(t.TokenName),
            // groups
            PatternGroup g when g.Condition == EPatternCondition.OneOf && g.Items.Length > 1 => $"({string.Join(g.ConditionJoiner, g.Items.Select(Format))})",
            PatternGroup g when g.MaxLogicalLength == 1 => Format(g.Items.Single()),
            PatternGroup g => string.Join(g.ConditionJoiner, g.Items.Select(Format)),
            _ => throw new NotImplementedException()
        };
    }
}
