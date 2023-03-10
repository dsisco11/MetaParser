using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using MetaParser.Parsing.Constructs.Consumers;
using MetaParser.Parsing.Constructs.Core;
using MetaParser.Parsing.Constructs.Patternization;

using System.Diagnostics;
using System.Linq;

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

        var allTokens = context.WorkingSet.Consumers.Select(static (c) => c.Token).Distinct();
        Debug.Assert(allTokens.Count() == 1);

        var targetToken = allTokens.Single();

        // Check if this token has any consumers which are non-recursive, if so then its possible for the token to appear in the stream already from a lower stage.
        bool hasEarlierStages = targetToken.GetConsumers().Any(static (c) => !c.DependencyInfo.IsRecursive);
        if (hasEarlierStages)
        {
            writer.WriteLine($"[{Format_Token_Id_Const_Ref(targetToken)}, ..] => true,");
        }

        var tokenConsumers = context.WorkingSet.Consumers.Where(static (c) => c.Type == EConsumerType.Token);
        foreach (var consumer in tokenConsumers)
        {
            writer.Write("[");
            writer.Write(PatternFormatter.ToString(consumer.Start));
            writer.Write(", ");

            if (consumer.Consume is not null)
            {
                writer.Write($"var {VarNameBufferMinor}] when (");

                bool first = true;
                foreach (var pattern in consumer.Consume)
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
