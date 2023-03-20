using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using System.Diagnostics;
using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
internal class LogicConsumerSwitchBlock : MetaCodeBuilder
{
    protected override void Write(MetaParserContext context)
    {
        if (!context.WorkingSet.Consumers.Any())
        {
            return;
        }

        // assert that all consumers are of the same type
        Debug.Assert(context.WorkingSet.Consumers.All(c => c.Type == context.WorkingSet.Consumers[0].Type));

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
