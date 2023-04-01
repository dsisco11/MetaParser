using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using MetaParser.Parsing.Constructs;

using System;
using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class LogicPatternMatcher : MetaCodeBuilder
{
    protected override void Write(ParserContext context)
    {
        var writer = context.Writer;
        var pattern = context.WorkingSet.Patterns.Single();

        if (!pattern.IsInlinable && pattern.IsConditional && pattern.Length == 1)
        {// This is a single non-inlineable item, so we execute the function
            //writer.Write(Format(pattern));
            context.Config.CodeFactory.Get_Pattern_Writer().WriteTo(context);
            writer.Write($"({context.State.ActiveBufferName})");
        }
        else if (!pattern.IsInlinable && pattern.IsSequence && pattern.Length > 1)
        {// wrap pattern detection condition in a local switch clause that returns true/false
            writer.Write(context.State.ActiveBufferName);
            writer.WriteLine(" switch");
            writer.WriteLine("{");
            writer.Indent++;
            foreach (var item in pattern)
            {
                //writer.Write(Format(item));
                context.Config.CodeFactory.Get_Pattern_Writer()
                    .WriteTo(context with { WorkingSet = context.WorkingSet with { Patterns = new[] { item } } });
                writer.WriteLine(" => true,");
            }
            writer.WriteLine("_ => false");
            writer.Indent--;
            writer.WriteLine("}");
        }
        else if (pattern.IsDeterministic && pattern.Length == 1)
        {// This is a single inlineable item, so do a length-1 buffer check
         // "buffer[0] == x"
            //writer.Write($"{context.State.ActiveBufferName}[0] == {Format(pattern)}");
            writer.Write($"{context.State.ActiveBufferName}[0] == ");
            context.Config.CodeFactory.Get_Pattern_Writer().WriteTo(context);
        }
        else if (pattern.IsDeterministic)
        {// "buffer.StartsWith(stackalloc []{ x, y, z }"
            writer.Write(context.State.ActiveBufferName);
            writer.Write(".StartsWith(stackalloc []{ ");
            //writer.Write(Format(pattern));
            context.Config.CodeFactory.Get_Pattern_Writer().WriteTo(context);
            writer.Write(" })");
        }
        else
        {// "buffer is [x, y, z, ..]"
            writer.Write(context.State.ActiveBufferName);
            writer.Write(" ");
            writer.Write("is ");
            //writer.Write($"[ {Format(pattern)}, ..]");
            writer.Write($"[ ");
            context.Config.CodeFactory.Get_Pattern_Writer().WriteTo(context);
            writer.Write($", ..]");
        }
    }
}
