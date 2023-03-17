using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
internal class DetectLinearTokensAndThen : MetaCodeBuilder
{
    protected override void Write(MetaParserContext context)
    {
        if (!context.WorkingSet.Consumers.Any())
        {
            return;
        }

        var writer = context.Writer;
        var workContext = context with {};

#if DEBUG
        writer.WriteLine("// Linear consumers");
#endif
        var sortedConsumers = context.WorkingSet.Consumers.OrderByDescending(static (c) => c.Token.DependencyInfo.NodeDepth.Max).ThenByDescending(static (c) => c.Start.MaxLogicalLength);
        writer.WriteLine($"switch ({context.ActiveBufferName})");
        writer.WriteLine("{");
        writer.Indent++;

        foreach (Parsing.Constructs.Consumer consumer in sortedConsumers)
        {
            workContext.WorkingSet = new WorkingSet(consumer.Token, consumer, consumer.Start);

            writer.Write("case [");
            context.Config.CodeFactory.Get_Pattern_Writer().WriteTo(workContext);
            writer.WriteLine(", ..]:");
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
