using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using System;
using System.Diagnostics;
using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class LogicExecuteConsumer : MetaCodeBuilder
{
    public static LogicExecuteConsumer Instance { get; private set; } = new LogicExecuteConsumer();

    protected override void Write(ParserContext context)
    {
        var writer = context.Writer ?? throw new InvalidOperationException("Writer is null");

        Debug.Assert(context.State.Targets.Consumers.Length == 1);
        var consumer = context.State.Targets.Consumers.Single();

        if (consumer.IsConstant)
        {
            writer.Write($"new {ConsumerResult} ({Format_Token_Id_Const_Ref(consumer.Token.Name)}, {consumer.Start!.Length})");
        }
        else
        {
            writer.Write($"{Format_Pattern_Consumer_Function_Name(consumer.Index)}({context.State.ActiveBufferName})");
        }
    }
}
