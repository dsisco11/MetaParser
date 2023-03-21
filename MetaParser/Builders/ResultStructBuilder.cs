using MetaParser.Builders.Interfaces;
using MetaParser.Core;

namespace MetaParser.Builders;
using static CodeCommon;

internal class ResultStructBuilder : MetaCodeBuilder
{
    protected override void Write(ParserContext context)
    {
        var writer = context.Writer;

        writer.WriteLine($"namespace {context.Config.Namespace};");
        writer.WriteLine("[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]");
        writer.WriteLine($"public readonly record struct {TypeConsumerResult}({context.Config.IdType} id, int length);");
    }
}
