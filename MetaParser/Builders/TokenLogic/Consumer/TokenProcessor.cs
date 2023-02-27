using MetaParser.CodeGen.Core;
using MetaParser.Contexts;

namespace MetaParser.Builders.TokenLogic.Consumer
{
    internal class TokenProcessor : IMetaCodeBuilder
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

            var workingContext = context with { Tokens = context.Tokens with { WorkingSet = new Structs.PatternConsumer[1] } };
            foreach (var consumer in context.Tokens.WorkingSet)
            {
                workingContext.Tokens.WorkingSet[0] = consumer;
                if (consumer.IsConstantLength)
                {
                    continue;// skip constant length patterns as they get an inline fast-path
                }

                // generate consumer functions
                var consumerFuncName = MetaParserContext.Get_Token_Consumer_Function_Name(consumer.ConsumerIndex);
                var consumeFunc = context.Get_Local_Token_Consumer_Function_Definition(consumer.Type, consumerFuncName, ConsumeAndThen.Instance);
                consumeFunc.WriteTo(workingContext);
            }
        }
    }
}
