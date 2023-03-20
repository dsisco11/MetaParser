using MetaParser.Builders.Interfaces;
using MetaParser.Core;

namespace MetaParser.Builders;
using static CodeCommon;

internal class ResultStructBuilder : MetaCodeBuilder
{
    protected override void Write(MetaParserContext context)
    {
        var writer = context.Writer;

        writer.WriteLine($"namespace {context.Config.Namespace};");
        writer.WriteLine("[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]");
        writer.WriteLine($"public readonly record struct {TypeConsumerProcessingResult}({context.Config.IdType} id, uint length);");
    }
}
