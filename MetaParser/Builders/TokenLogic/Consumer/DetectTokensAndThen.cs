using MetaParser.Builders.Interfaces;
using MetaParser.Consumers;
using MetaParser.Core;
using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class DetectTokensAndThen : MetaCodeBuilder
{
    // TODO: Detect cyclic tokens and generate different detection & consuming logic
    protected override void Write(MetaParserContext context)
    {
        var workTokens = context.Consumers with { WorkingSet = new ConsumerInfo[1] };
        var workContext = context with { Consumers = workTokens };

        var sortedConsumers = context.Consumers.WorkingSet.OrderByDescending(static (c) => c.DependencyInfo.MaxDepth).ThenByDescending(static (c) => c.Start.Length);

        var writer = context.writer;
        writer.WriteLine($"switch ({VarNameBufferMajor})");
        writer.WriteLine("{");
        writer.Indent++;

        foreach (ConsumerInfo consumer in sortedConsumers)
        {
            workContext.Consumers.WorkingSet[0] = consumer;

            writer.Write("case ");
            ConsumerPatternFormatter.WriteTo(context, consumer.Start);
            writer.WriteLine(":");
            writer.WriteLine("{");
            writer.Indent++;

            base.WriteContent(workContext);

            writer.Indent--;
            writer.WriteLine("}");
        }

        writer.Indent--;
        writer.WriteLine("}");// end switch
    }
}
