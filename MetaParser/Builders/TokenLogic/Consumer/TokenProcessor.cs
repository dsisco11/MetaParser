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
            /** STEPS
             * 1) Find token type via switch block pattern
             * 2) Jump to token specific consumer function
             * 3) Consume start block (elements can be optional)
             * 4) 
             */
            new DetectTokensAndThen(new ConsumeTokenAndReturn()).WriteTo(context);

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
                wr.WriteLine($"bool {MetaParserContext.Get_Token_Consumer_Function_Name(consumer.ConsumerIndex)}()");
                wr.WriteLine("{");
                wr.Indent++;
                ConsumeAndThen.Instance.WriteTo(workingContext);
                wr.Indent--;
                wr.WriteLine("}");
            }
        }
    }
}
