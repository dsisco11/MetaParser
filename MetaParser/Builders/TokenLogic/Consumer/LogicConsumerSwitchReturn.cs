using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using System;
using System.Collections.Immutable;

namespace MetaParser.Builders.TokenLogic.Consumer;
internal class LogicConsumerSwitchReturn : MetaCodeBuilder
{
    protected override void Write(ParserContext context)
    {
        var writer = context.Writer ?? throw new InvalidOperationException("Writer is null");

        writer.WriteLine($"return {context.State.ActiveBufferName} switch");
        writer.WriteLine("{");
        writer.Indent++;

        Parsing.Constructs.ConsumerEntity[] consumers = context.State.Targets.Consumers;
        var patternConsumerMap = consumers.ToImmutableDictionary(static (k) => k.Start, static (c) => c);
        var patterns = patternConsumerMap.Keys.ToImmutableArray().Sort();
        foreach (var pattern in patterns)
        {
            var consumer = patternConsumerMap[pattern];

            writer.Write("[");
            context.Config.CodeFactory.Get_Pattern_Writer().WriteTo(context with { State = context.State with { Targets = new WorkingSet(pattern) } });
            writer.Write(", ..] => ");
            WriteContent(context with { State = context.State with { Targets = new WorkingSet(consumer) } });
            writer.WriteLine(",");
        }

        writer.WriteLine("_ => new (0, 1)");
        writer.Indent--;
        writer.WriteLine("};");// end switch
    }
}
