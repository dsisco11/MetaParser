using MetaParser.Builders.Interfaces;
using MetaParser.Core;

namespace MetaParser.Builders.Parser;
using static CodeCommon;

internal class UnknownTokenPusher : MetaCodeBuilder
{
    public static IMetaCodeBuilder Instance = new UnknownTokenPusher();

    protected override void Write(MetaParserContext context)
    {
        var wr = context.writer;
        wr.WriteLine($"if ({VarNameBufferMinor}.Length != {VarNameBufferLocal}.Length)");
        wr.WriteLine("{");
        wr.Indent++;
        wr.WriteLine($"var unk_content_size = {VarNameBufferMinor}.Length - {VarNameBufferLocal}.Length;");
        wr.WriteLine($"var unk_content = {VarNameBufferMinor}.Slice(0, unk_content_size);");
        wr.WriteLine($"results.Add(new {TokenValueStructName}({Format_Token_Id_Const_Ref(UnknownToken)}, unk_content));");
        wr.WriteLine($"{VarNameBufferMinor} = {VarNameBufferMinor}.Slice(unk_content_size);");
        wr.WriteLine($"{VarNameBufferLocal} = {VarNameBufferMinor}.Span;");
        wr.Indent--;
        wr.WriteLine("}");
    }
}
