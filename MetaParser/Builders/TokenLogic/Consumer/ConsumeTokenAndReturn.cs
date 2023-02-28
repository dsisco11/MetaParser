using MetaParser.CodeGen.Core;
using MetaParser.Contexts;

using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;

internal class ConsumeTokenAndReturn : IMetaCodeBuilder
{
    public static ConsumeTokenAndReturn Instance = new ConsumeTokenAndReturn();

    public void WriteTo(MetaParserContext context)
    {
        var wr = context.writer;
        var consumer = context.Consumers.WorkingSet.Single();

        if (consumer.IsOpenEnded)
        {
            var consumerId = consumer.ConsumerIndex;
            var consumerFunc = MetaParserContext.Format_Pattern_Consumer_Function_Name(consumerId);
            wr.WriteLine($"id = {context.Get_TokenId_Ref(consumer.TokenName)};");
            wr.WriteLine($"return {consumerFunc}(stream, out length);");
        }
        else
        {
            wr.WriteLine($"id = {context.Get_TokenId_Ref(consumer.TokenName)};");
            wr.WriteLine($"length = {consumer.Start.Length};");
            wr.WriteLine($"return true;");
        }
    }
}
