using MetaParser.Builders.Interfaces;
using MetaParser.Consumers;
using MetaParser.Core;

using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class DetectRecursiveTokensAndThen : MetaCodeBuilder
{
    protected override void Write(MetaParserContext context)
    {
        if (!context.Consumers.WorkingSet.Any())
        {
            return;
        }

        var writer = context.writer;
        var workContext = context with { Consumers = context.Consumers with { WorkingSet = new TokenConsumer[1] } };

#if DEBUG
        writer.WriteLine("// Recursive consumers");
#endif
        var sortedConsumers = context.Consumers.WorkingSet.OrderByDescending(static (c) => c.DependencyInfo.MaxDepth).ThenByDescending(static (c) => c.Start.Length);
        foreach (TokenConsumer consumer in sortedConsumers)
        {
            workContext.Consumers.WorkingSet[0] = consumer;

            writer.WriteLine($"if ({Format_Pattern_Start_Detection_Function_Name(consumer.Index)}({VarNameBufferMajor}))");
            writer.WriteLine("{");
            writer.Indent++;

            base.WriteContent(workContext);

            writer.Indent--;
            writer.WriteLine("}");
        }

        writer.WriteLine();
    }
}
