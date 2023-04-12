using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using MetaParser.Parsing.Constructs;

using Microsoft.CodeAnalysis.CSharp;

using System;
using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class WritePatternAsExpression : MetaCodeBuilder
{
    protected override void Write(ParserContext context)
    {
        var writer = context.Writer ?? throw new InvalidOperationException("Writer is null");
        var pattern = context.State.Targets.Patterns.Single();

        writer.Write(Format(pattern));
    }

    private static string Format(PatternEntity pattern)
    {
        return pattern switch
        {
            PatternConst c when c.Kind == EPatternKind.Literal && c.Value.Length == 1 => SymbolDisplay.FormatLiteral(c.Value[0], true),
            PatternConst c => SymbolDisplay.FormatLiteral(c.Value, true),
            // ranges
            PatternRange r => $"(>={SymbolDisplay.FormatLiteral(r.Begin[0], true)} and <={SymbolDisplay.FormatLiteral(r.End[0], true)})",
            // tokens
            PatternTokenRef t when !t.IsInlinable => $"{Format_Token_Start_Detection_Function_Name(t.GetToken().Name)}",
            PatternTokenRef t => Format_Token_Id_Const_Ref(t.GetToken().Name),
            // groups
            PatternSequence g when g.Kind == EPatternKind.Not && g.Items.Length > 1 => $"not ({string.Join(g.ConditionJoiner, g.Items.Select(Format))})",
            PatternSequence g when g.Kind == EPatternKind.Not => $"not {string.Join(g.ConditionJoiner, g.Items.Select(Format))}",
            PatternSequence g when g.Kind == EPatternKind.OneOf && g.Items.Length > 1 => $"({string.Join(g.ConditionJoiner, g.Items.Select(Format))})",
            PatternSequence g when g.MaxConditions == 1 => Format(g.Items.Single()),
            PatternSequence g => string.Join(g.ConditionJoiner, g.Items.Select(Format)),
            _ => throw new NotImplementedException()
        };
    }
}
