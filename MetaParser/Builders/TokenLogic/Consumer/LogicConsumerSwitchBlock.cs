using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
internal class LogicConsumerSwitchBlock : MetaCodeBuilder
{
    protected override void Write(ParserContext context)
    {
        if (!context.State.Targets.Consumers.Any())
        {
            return;
        }

        // assert that all consumers are of the same type
        //Debug.Assert(context.State.Targets.Consumers.All(c => c.Type == context.State.Targets.Consumers[0].Type));

        var writer = context.Writer;
        var workContext = context with {};

        writer.WriteLine($"return {context.State.ActiveBufferName} switch");
        writer.WriteLine("{");
        writer.Indent++;

        for (int i = 0; i < context.State.Targets.Consumers.Length; i++)
        {
            Parsing.Constructs.ConsumerEntity? consumer = context.State.Targets.Consumers[i];
            workContext.State.Targets = new WorkingSet(consumer.Token, consumer, consumer.Start);

            writer.Write("[");
            context.Config.CodeFactory.Get_Pattern_Writer().WriteTo(workContext);
            writer.Write(", ..] => ");
            WriteContent(workContext);
            writer.WriteLine(",");
        }

        writer.WriteLine("_ => new (default, default)");
        writer.Indent--;
        writer.WriteLine("};");// end switch
    }
}
