using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using MetaParser.Parsing.Constructs;

using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class DetectRecursiveTokensAndThen : MetaCodeBuilder
{
    protected override void Write(MetaParserContext context)
    {
        if (!context.WorkingSet.Consumers.Any())
        {
            return;
        }

        var writer = context.writer;
#if DEBUG
        writer.WriteLine("// Recursive consumers");
#endif
        var orderedItems = context.WorkingSet.Consumers.Select(static (x) => x.Token)
                                                          .Distinct()
                                                          .OrderByDescending(static (t) => t.GetConsumers().Max(static (c) => c.DependencyInfo.MaxDepth))
                                                          .ThenByDescending(static (t) => t.GetConsumers().Max(static (c) => c.Start.Length));
        foreach (TokenInfo token in orderedItems)
        {
            writer.WriteLine($"if ({Format_Token_Start_Detection_Function_Name(token.Name)}({VarNameBufferMajor}))");
            writer.WriteLine("{");
            writer.Indent++;

            base.WriteContent(context);

            writer.Indent--;
            writer.WriteLine("}");
        }

        writer.WriteLine();
    }
}
