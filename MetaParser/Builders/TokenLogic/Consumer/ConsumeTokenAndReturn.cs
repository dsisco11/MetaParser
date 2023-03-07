using MetaParser.CodeGen.Interfaces;
using MetaParser.Core;

using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;

internal class ConsumeTokenAndReturn : IMetaCodeBuilder
{
    public static ConsumeTokenAndReturn Instance = new ConsumeTokenAndReturn();

    public void WriteTo(MetaParserContext context)
    {
        var writer = context.writer;
        var consumer = context.Consumers.WorkingSet.Single();

        if (consumer.IsConstant)
        {
            writer.WriteLine($"id = {MetaParserContext.Get_TokenId_Ref(consumer.Token.Name)};");
            writer.WriteLine($"length = {consumer.Start!.Length};");
            writer.WriteLine($"return true;");
        }
        else if (consumer.IsDynamic)
        {
            var consumerId = consumer.Index;
            var consumerFunc = MetaParserContext.Format_Pattern_Consumer_Function_Name(consumerId);
            writer.WriteLine($"id = {MetaParserContext.Get_TokenId_Ref(consumer.Token.Name)};");
            writer.WriteLine($"return {consumerFunc}({MetaParserContext.VarNameBufferMajor}, out length);");
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}
