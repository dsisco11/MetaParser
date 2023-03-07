using MetaParser.CodeGen.Interfaces;
using MetaParser.Consumers;
using MetaParser.Core;
using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;

internal class DetectTokensAndThen : IMetaCodeBuilder
{
    #region Properties
    public IMetaCodeBuilder Body { get; }
    #endregion

    #region Constructors
    public DetectTokensAndThen(IMetaCodeBuilder body)
    {
        Body = body;
    }
    #endregion




    public void WriteTo(MetaParserContext context)
    {
        // TODO: Detect cyclic tokens and generate different detection & consuming logic
        var workTokens = context.Consumers with { WorkingSet = new ConsumerInfo[1] };
        var workContext = context with { Consumers = workTokens };

        var linearConsumers = context.Consumers.WorkingSet.Where(static (x) => !x.DependencyInfo.IsRecursive);
        var recursiveConsumers = context.Consumers.WorkingSet.Where(static (x) => !x.DependencyInfo.IsRecursive);

        var sortedLinearConsumers = linearConsumers.OrderByDescending(static (c) => c.DependencyInfo.MaxDepth).ThenByDescending(static (c) => c.Start.Length);
        var sortedRecursiveConsumers = recursiveConsumers.OrderByDescending(static (c) => c.DependencyInfo.MaxDepth).ThenByDescending(static (c) => c.Start.Length);

        var writer = context.writer;
        writer.WriteLine($"switch ({MetaParserContext.VarNameBufferMajor})");
        writer.WriteLine("{");
        writer.Indent++;

        foreach (ConsumerInfo consumer in sortedLinearConsumers)
        {
            workContext.Consumers.WorkingSet[0] = consumer;

            writer.Write("case ");
            ConsumerPatternMatcher.WriteTo(context, consumer.Start);
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
