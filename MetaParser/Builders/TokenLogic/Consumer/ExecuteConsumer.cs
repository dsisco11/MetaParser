using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using System.Diagnostics;
using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class ExecuteConsumer : MetaCodeBuilder
{
    protected override void Write(ParserContext context)
    {
        Debug.Assert(context.WorkingSet.Consumers.Length == 1);
        var writer = context.Writer;
        var consumer = context.WorkingSet.Consumers.Single();

        if (consumer.IsConstant)
        {
            writer.Write($"new {TypeConsumerResult} ({Format_Token_Id_Const_Ref(consumer.Token.Name)}, {consumer.Start!.Length})");
        }
        else
        {
            writer.Write($"{Format_Pattern_Consumer_Function_Name(consumer.Index)}({context.ActiveBufferName})");
        }
    }
}
