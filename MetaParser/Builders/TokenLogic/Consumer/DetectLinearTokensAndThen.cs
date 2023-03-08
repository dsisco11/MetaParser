using MetaParser.Builders.Interfaces;
using MetaParser.Consumers;
using MetaParser.Core;
using MetaParser.Patternization;

using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class DetectLinearTokensAndThen : MetaCodeBuilder
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
        writer.WriteLine("// Linear consumers");
#endif
        var sortedConsumers = context.Consumers.WorkingSet.OrderByDescending(static (c) => c.DependencyInfo.MaxDepth).ThenByDescending(static (c) => c.Start.Length);
        writer.WriteLine($"switch ({VarNameBufferMajor})");
        writer.WriteLine("{");
        writer.Indent++;

        foreach (TokenConsumer consumer in sortedConsumers)
        {
            workContext.Consumers.WorkingSet[0] = consumer;

            writer.Write("case ");
            PatternFormatter.WriteTo(context, consumer.Start);
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
