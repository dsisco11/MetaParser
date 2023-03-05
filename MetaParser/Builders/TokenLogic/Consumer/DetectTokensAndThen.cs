using MetaParser.CodeGen.Core;
using MetaParser.Consumers;
using MetaParser.Contexts;

using System;
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
            var writer = context.writer;
            writer.WriteLine($"switch ({MetaParserContext.VarNameBufferMajor})");
            writer.WriteLine("{");
            writer.Indent++;

            var workTokens = context.Consumers with { WorkingSet = new PatternConsumer[1] };
            var workContext = context with { Consumers = workTokens };

            foreach (PatternConsumer consumer in context.Consumers.WorkingSet.OrderByDescending(x => x.EffectiveStart.Length))
            {
                workContext.Consumers.WorkingSet[0] = consumer;

                writer.Write("case ");
                switch (consumer.IsOpen)
                {
                    case true when consumer.Start is null:
                        {// no start condition, only a CONSUME criteria
                            ConsumerPatternMatcher.WriteTo(context, consumer.Consume!);
                            break;
                        }
                    case true when consumer.Start is not null:
                        {// for open ended consumers it is implied that their consume criteria is part of their start condition
                            ConsumerPatternMatcher.WriteTo(context, consumer.Start.Combine(consumer.Consume!));
                            break;
                        }
                    case false when consumer.Start is not null:
                        {
                            ConsumerPatternMatcher.WriteTo(context, consumer.Start);
                            break;
                        }
                    default:
                        {
                            throw new NotImplementedException();
                        }
                }
                writer.WriteLine(":");
                writer.WriteLine("{");
                writer.Indent++;

                Body.WriteTo(workContext);

                writer.Indent--;
                writer.WriteLine("}");
            }
            writer.Indent--;
            writer.WriteLine("}");// end switch
        }
    }
}
