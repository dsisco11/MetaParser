using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using MetaParser.Parsing.Constructs;

using System;
using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class LogicPatternMatcher : MetaCodeBuilder
{
    protected override void Write(MetaParserContext context)
    {
        var writer = context.Writer;
        var pattern = context.WorkingSet.Patterns.Single();

        if (!pattern.IsInlinable && pattern.IsConditional && pattern.Length == 1)
        {// This is a single non-inlineable item, so we execute the function
            writer.Write(Format(pattern));
            writer.Write($"({context.ActiveBufferName})");
        }
        else if (!pattern.IsInlinable && pattern.IsSequence && pattern.Length > 1)
        {// wrap pattern detection condition in a local switch clause that returns true/false
            writer.Write(context.ActiveBufferName);
            writer.WriteLine(" switch");
            writer.WriteLine("{");
            writer.Indent++;
            foreach (var item in pattern)
            {
                writer.Write(Format(item));
                writer.WriteLine(" => true,");
            }
            writer.WriteLine("_ => false");
            writer.Indent--;
            writer.WriteLine("}");
        }
        else if (pattern.IsDeterministic && pattern.Length == 1)
        {// This is a single inlineable item, so do a length-1 buffer check
         // "buffer[0] == x"
            writer.Write($"{context.ActiveBufferName}[0] == {Format(pattern)}");
        }
        else if (pattern.IsDeterministic)
        {// "buffer.StartsWith(stackalloc []{ x, y, z }"
            writer.Write(context.ActiveBufferName);
            writer.Write(".StartsWith(stackalloc []{ ");
            writer.Write(Format(pattern));
            writer.Write(" })");
        }
        else
        {// "buffer is [x, y, z, ..]"
            writer.Write(context.ActiveBufferName);
            writer.Write(" ");
            writer.Write("is ");
            writer.Write($"[ {Format(pattern)}, ..]");
        }
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
            PatternGroup g when g.Condition == EPatternCondition.OneOf && g.Items.Length > 1 => $"({string.Join(g.ConditionJoiner, g.Items.Select(static o => Format(o)))})",
            PatternGroup g when g.MaxConditions == 1 => Format(g.Items.Single()),
            PatternGroup g => string.Join(g.ConditionJoiner, g.Items.Select(static o => Format(o))),
            _ => throw new NotImplementedException()
        };
    }
}
