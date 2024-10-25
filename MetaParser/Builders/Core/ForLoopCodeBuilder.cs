using MetaParser.Core;

namespace MetaParser.Builders.Core;

internal class ForLoopCodeBuilder : CodeBuilder
{
    private readonly int _start;
    private readonly int _end;

    public ForLoopCodeBuilder(int start, int end)
    {
        _start = start;
        _end = end;
    }

    protected override void WriteTo(CodeGenContext context)
    {
        context.Writer.WriteLine($"for (int i = {_start}; i < {_end}; i++)");
        context.Writer.WriteLine("{");
        context.Writer.Indent++;

        GenerateInnerItems(context);

        context.Writer.Indent--;
        context.Writer.WriteLine("}");
    }
}
