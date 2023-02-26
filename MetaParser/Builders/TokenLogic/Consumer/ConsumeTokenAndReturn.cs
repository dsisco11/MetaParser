using MetaParser.CodeGen.Core;
using MetaParser.Contexts;

using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;

internal class ConsumeTokenAndReturn : IMetaCodeBuilder
{
    public void WriteTo(MetaParserContext context)
    {
        var wr = context.writer;
        var consumer = context.Tokens.WorkingSet.Single();

        if (consumer.IsConstantLength)
        {
            wr.WriteLine($"id = {context.Get_TokenId_Ref(consumer.IdName)};");
            wr.WriteLine($"length = {consumer.Start.Length};");
            wr.WriteLine($"return true;");
        }
        else
        {
            var consumerId = consumer.ConsumerIndex;
            var consumerFunc = MetaParserContext.Get_Token_Consumer_Function_Name(consumerId);
            wr.WriteLine($"return {consumerFunc}();");
        }
    }
}
