using MetaParser.CodeGen.Core;
using MetaParser.Consumers;
using MetaParser.Contexts;

using System.Linq;

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

            var workTokens = context.Consumers with { WorkingSet = new PatternConsumer[1] };
            var workContext = context with { Consumers = workTokens };

            foreach (PatternConsumer consumer in context.Consumers.WorkingSet.OrderByDescending(x => x.Start.Length))
            {
                if (consumer.Start is null)
                {
                    continue;
                }
                workContext.Consumers.WorkingSet[0] = consumer;

                wr.Write("case ");
                ConsumerPatternMatcher.WriteTo(context, consumer.Start);
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
