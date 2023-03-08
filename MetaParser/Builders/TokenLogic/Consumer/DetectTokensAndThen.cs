using MetaParser.Builders.Interfaces;
using MetaParser.Consumers;
using MetaParser.Core;
using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class DetectTokensAndThen : MetaCodeBuilder
{
    // TODO: Detect cyclic tokens and generate different detection & consuming logic
    protected override void Write(MetaParserContext context)
    {
        var writer = context.writer;
        var workTokens = context.Consumers with { WorkingSet = new TokenConsumer[1] };
        var workContext = context with { Consumers = workTokens };

        #region Recursive Consumers
        var consumersRecursive = context.Consumers.WorkingSet.Where(static c => c.DependencyInfo.IsRecursive);
        if (consumersRecursive.Any())
        {
            var sortedConsumersRecursive = consumersRecursive.OrderByDescending(static (c) => c.DependencyInfo.MaxDepth).ThenByDescending(static (c) => c.Start.Length);

        }
        #endregion

        #region Non-Recursive Consumers
        var consumersLinear = context.Consumers.WorkingSet.Where(static c => !c.DependencyInfo.IsRecursive);
        if (consumersLinear.Any())
        {
            var sortedConsumersLinear = consumersLinear.OrderByDescending(static (c) => c.DependencyInfo.MaxDepth).ThenByDescending(static (c) => c.Start.Length);
            writer.WriteLine($"switch ({VarNameBufferMajor})");
            writer.WriteLine("{");
            writer.Indent++;

            foreach (TokenConsumer consumer in sortedConsumersLinear)
            {
                workContext.Consumers.WorkingSet[0] = consumer;

                writer.Write("case ");
                ConsumerPatternFormatter.WriteTo(context, consumer.Start);
                writer.WriteLine(":");
                writer.WriteLine("{");
                writer.Indent++;

                base.WriteContent(workContext);

                writer.Indent--;
                writer.WriteLine("}");
            }

            writer.Indent--;
            writer.WriteLine("}");// end switch
        }
        #endregion
    }
}
