using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class ExecuteConsumerAndReturnResult : MetaCodeBuilder
{
    protected override void Write(MetaParserContext context)
    {
        var writer = context.writer;
        var consumer = context.Consumers.WorkingSet.Single();

        if (consumer.IsConstant)
        {
            writer.WriteLine($"id = {Get_TokenId_Ref(consumer.Token.Name)};");
            writer.WriteLine($"length = {consumer.Start!.Length};");
            writer.WriteLine($"return true;");
        }
        else if (consumer.IsDynamic)
        {
            var consumerId = consumer.Index;
            var consumerFunc = Format_Pattern_Consumer_Function_Name(consumerId);
            writer.WriteLine($"id = {Get_TokenId_Ref(consumer.Token.Name)};");
            writer.WriteLine($"return {consumerFunc}({VarNameBufferMajor}, out length);");
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}
