using MetaParser.Builders.Interfaces;
using MetaParser.Consumers;
using MetaParser.Core;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class TokenProcessor : IMetaCodeBuilder
{
    public static IMetaCodeBuilder Instance = new TokenProcessor();

    public void WriteTo(MetaParserContext context)
    {
        var writer = context.writer;
        new DetectTokensAndThen(ConsumeTokenAndReturn.Instance).WriteTo(context);

        // return failure
        writer.WriteLine("id = default;");
        writer.WriteLine("length = default;");
        writer.WriteLine("return false;");
        writer.WriteLine();

        var workingContext = context with { Consumers = context.Consumers with { WorkingSet = new ConsumerInfo[1] } };
        foreach (var consumer in context.Consumers.WorkingSet)
        {
            workingContext.Consumers.WorkingSet[0] = consumer;
            if (consumer.IsConstant)
            {
                continue;// skip const patterns as they get an inline fast-path
            }

            // generate consumer functions
            var consumerFuncName = Format_Pattern_Consumer_Function_Name(consumer.Index);
            var consumeFunc = Get_Local_Token_Consumer_Function_Definition(context.Config, consumer.Type, consumerFuncName, ConsumeAndThen.Instance);
            consumeFunc.WriteTo(workingContext);
        }
    }
}
