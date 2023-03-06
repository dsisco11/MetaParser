using MetaParser.CodeGen.Core;
using MetaParser.Consumers;
using MetaParser.Contexts;

using System;
using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;

internal class RecursiveDetectTokensAndThen : IMetaCodeBuilder
{
    public IMetaCodeBuilder Body { get; }

    public RecursiveDetectTokensAndThen(IMetaCodeBuilder body)
    {
        Body = body;
    }

    public void WriteTo(MetaParserContext context)
    {
        var writer = context.writer;
        writer.WriteLine($"switch ({MetaParserContext.VarNameBufferMajor})");
        writer.WriteLine("{");
        writer.Indent++;

        var workTokens = context.Consumers with { WorkingSet = new ConsumerInfo[1] };
        var workContext = context with { Consumers = workTokens };

        foreach (ConsumerInfo consumer in context.Consumers.WorkingSet.OrderByDescending<ConsumerInfo, int>(static (c) => c.DependencyInfo.Order))
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
