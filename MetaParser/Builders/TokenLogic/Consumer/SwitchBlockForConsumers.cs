using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
internal class SwitchBlockForConsumers : MetaCodeBuilder
{
    protected override void Write(MetaParserContext context)
    {
        if (!context.WorkingSet.Consumers.Any())
        {
            return;
        }

        var writer = context.Writer;
        var workContext = context with {};

        writer.WriteLine($"switch ({context.ActiveBufferName})");
        writer.WriteLine("{");
        writer.Indent++;

        for (int i = 0; i < context.WorkingSet.Consumers.Length; i++)
        {
            Parsing.Constructs.Consumer? consumer = context.WorkingSet.Consumers[i];
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
