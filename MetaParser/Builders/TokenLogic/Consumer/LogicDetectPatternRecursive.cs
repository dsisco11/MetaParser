using MetaParser.Builders.Interfaces;
using MetaParser.Core;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class LogicDetectPatternRecursive : MetaCodeBuilder
{
    protected override void Write(MetaParserContext context)
    {
        var writer = context.writer;

        writer.WriteLine($"switch ({VarNameBufferMajor})");
        writer.WriteLine("{");
        writer.Indent++;
        foreach (var consumer in context.Consumers.WorkingSet)
        {
            writer.WriteLine("case ");
        }
        writer.Indent--;
        writer.WriteLine("}");        
    }
}
