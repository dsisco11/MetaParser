using MetaParser.CodeGen.Core;
using MetaParser.Contexts;
using MetaParser.Structs;

namespace MetaParser.Builders.TokenLogic.Consumer
{
    internal class DetectTokensAndThen : IMetaCodeBuilder
    {
        public IMetaCodeBuilder Body { get; }

        public DetectTokensAndThen(IMetaCodeBuilder body)
        {
            Body = body;
        }

        public void WriteTo(MetaParserContext context)
        {
            var wr = context.writer;
            wr.WriteLine($"switch ({MetaParserContext.VarNameBufferMajor})");
            wr.WriteLine("{");
            wr.Indent++;

            var workTokens = context.Tokens with { WorkingSet = new PatternConsumer[1] };
            var workContext = context with { Tokens = workTokens };

            foreach (PatternConsumer consumer in context.Tokens.WorkingSet)
            {
                if (consumer.Start is null)
                {
                    continue;
                }
                workContext.Tokens.WorkingSet[0] = consumer;

                wr.Write("case ");
                InlineConsumerListPatternBuilder.WriteTo(workContext, consumer.Start);
                wr.WriteLine(":");
                wr.WriteLine("{");
                wr.Indent++;

                Body.WriteTo(workContext);

                wr.Indent--;
                wr.WriteLine("}");
            }
            wr.Indent--;
            wr.WriteLine("}");// end switch
        }
    }
}
