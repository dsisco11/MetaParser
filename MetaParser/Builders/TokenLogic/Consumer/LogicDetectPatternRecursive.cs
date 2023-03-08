using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using MetaParser.Patternization;

using System.Diagnostics;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class LogicDetectPatternRecursive : MetaCodeBuilder
{
    protected override void Write(MetaParserContext context)
    {
        var writer = context.writer;

        writer.WriteLine($"return {VarNameBufferMajor} switch");
        writer.WriteLine("{");
        writer.Indent++;
        foreach (var consumer in context.Consumers.WorkingSet)
        {
            Debug.Assert(consumer.Type == Consumers.EConsumerType.Token);
            writer.Write("[");
            writer.Write(PatternFormatter.ToString(consumer.Start));
            writer.Write(", ");

            if (consumer.Consume is not null)
            {
                writer.Write($"var {VarNameBufferMinor}] when (");

                bool first = true;
                foreach (var pattern in consumer.Consume?.GetSubPatterns())
                {
                    Debug.Assert(pattern is PatternTokenRef);
                    if (!first)
                    {
                        writer.Write(" or ");
                    }
                    first = false;

                    if (pattern is PatternTokenRef tokenRef)
                    {
                        writer.Write($"{Format_Token_Start_Detection_Function_Name(tokenRef.TokenName)}({VarNameBufferMinor})");
                    }
                }
                writer.Write(")");
            }
            else
            {
                writer.Write("..]");
            }

            writer.WriteLine(" => true,");
        }
        writer.WriteLine("_ => false");
        writer.Indent--;
        writer.WriteLine("};");
    }
}
