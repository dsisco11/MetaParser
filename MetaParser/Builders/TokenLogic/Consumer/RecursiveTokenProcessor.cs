using MetaParser.CodeGen.Core;
using MetaParser.Consumers;
using MetaParser.Contexts;

namespace MetaParser.Builders.TokenLogic.Consumer;

internal class RecursiveTokenProcessor : IMetaCodeBuilder
{
    public static IMetaCodeBuilder Instance = new TokenProcessor();

    public void WriteTo(MetaParserContext context)
    {
        var wr = context.writer;
        new DetectTokensAndThen(ConsumeTokenAndReturn.Instance).WriteTo(context);

        // return failure
        wr.WriteLine("id = default;");
        wr.WriteLine("length = default;");
        wr.WriteLine("return false;");
        wr.WriteLine();

        var workingContext = context with { Consumers = context.Consumers with { WorkingSet = new PatternConsumer[1] } };
        foreach (var consumer in context.Consumers.WorkingSet)
        {
            workingContext.Consumers.WorkingSet[0] = consumer;
            if (!consumer.IsDynamic)
            {
                continue;// skip const patterns as they get an inline fast-path
            }

            // generate consumer functions
            var consumerFuncName = MetaParserContext.Format_Pattern_Consumer_Function_Name(consumer.ConsumerIndex);
            var consumeFunc = context.Get_Local_Token_Consumer_Function_Definition(consumer.TokenType, consumerFuncName, ConsumeAndThen.Instance);
            consumeFunc.WriteTo(workingContext);
        }
    }
}
