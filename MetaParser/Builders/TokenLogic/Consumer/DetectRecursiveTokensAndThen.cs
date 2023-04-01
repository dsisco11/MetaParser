using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using MetaParser.Parsing.Constructs;

using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class DetectRecursiveTokensAndThen : MetaCodeBuilder
{
    protected override void Write(ParserContext context)
    {
        if (!context.WorkingSet.Consumers.Any())
        {
            return;
        }

        var writer = context.Writer;
#if DEBUG
        writer.WriteLine("// Recursive tokens");
#endif
        for (int i = 0; i < context.WorkingSet.Tokens.Length; i++)
        {
            TokenEntity? token = context.WorkingSet.Tokens[i];
            writer.WriteLine($"if ({Format_Token_Start_Detection_Function_Name(token.Name)}({context.State.ActiveBufferName}))");
            writer.WriteLine("{");
            writer.Indent++;

            base.WriteContent(context);

            writer.Indent--;
            writer.WriteLine("}");
        }

        writer.WriteLine();
    }
}
