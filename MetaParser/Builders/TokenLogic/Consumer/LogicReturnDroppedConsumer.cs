using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using System;
using System.Diagnostics;
using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class LogicReturnDroppedConsumer : MetaCodeBuilder
{
    public static LogicReturnDroppedConsumer Instance { get; } = new();

    protected override void Write(ParserContext context)
    {
        Debug.Assert(context.State.Targets.Consumers.Length == 1);
        var writer = context.Writer ?? throw new InvalidOperationException("Writer is null");
        var consumer = context.State.Targets.Consumers.Single();

        if (consumer.IsConstant)
        {
            writer.WriteLine($"return new (0, {consumer.Start!.Length}, false);");
        }
        else
        {
            writer.WriteLine($"return {Format_Pattern_Consumer_Function_Name(consumer.Index)}({context.State.ActiveBufferName});");
        }
    }
}
