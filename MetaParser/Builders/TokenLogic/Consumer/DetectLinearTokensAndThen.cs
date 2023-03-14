using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using MetaParser.Parsing.Constructs;

using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class DetectLinearTokensAndThen : MetaCodeBuilder
{
    protected override void Write(MetaParserContext context)
    {
        if (!context.WorkingSet.Consumers.Any())
        {
            return;
        }

        var writer = context.writer;
        var workContext = context with {};

#if DEBUG
        writer.WriteLine("// Linear consumers");
#endif
        var sortedConsumers = context.WorkingSet.Consumers.OrderByDescending(static (c) => c.Token.DependencyInfo.NodeDepth.Max).ThenByDescending(static (c) => c.Start.Length);
        writer.WriteLine($"switch ({VarNameBufferMajor})");
        writer.WriteLine("{");
        writer.Indent++;

        foreach (Parsing.Constructs.Consumer consumer in sortedConsumers)
        {
            workContext.WorkingSet = new (consumer);

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
