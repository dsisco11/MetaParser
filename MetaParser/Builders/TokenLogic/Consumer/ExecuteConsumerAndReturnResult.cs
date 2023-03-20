using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using System.Diagnostics;
using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class ExecuteConsumerAndReturnResult : MetaCodeBuilder
{
    protected override void Write(MetaParserContext context)
    {
        Debug.Assert(context.WorkingSet.Consumers.Length == 1);
        var writer = context.Writer;
        var consumer = context.WorkingSet.Consumers.Single();

        if (consumer.IsConstant)
        {
            writer.WriteLine($"return new {TypeConsumerProcessingResult} ({Format_Token_Id_Const_Ref(consumer.Token.Name)}, {consumer.Start!.Length});");
        }
        else if (consumer.IsDynamic)
        {
            var consumerId = consumer.Index;
            var consumerFunc = Format_Pattern_Consumer_Function_Name(consumerId);
            writer.WriteLine($"return {consumerFunc}({context.ActiveBufferName});");
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}
