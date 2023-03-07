using MetaParser.Builders.Interfaces;
using MetaParser.Core;

namespace MetaParser.Builders
{
    internal class UnknownTokenPusher : IMetaCodeBuilder
    {
        public static IMetaCodeBuilder Instance = new UnknownTokenPusher();

        public void WriteTo(MetaParserContext context)
        {
            var wr = context.writer;
            wr.WriteLine($"if ({CodeCommon.VarNameBufferMinor}.Length != {CodeCommon.VarNameBufferLocal}.Length)");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine($"var unk_content_size = {CodeCommon.VarNameBufferMinor}.Length - {CodeCommon.VarNameBufferLocal}.Length;");
            wr.WriteLine($"var unk_content = {CodeCommon.VarNameBufferMinor}.Slice(0, unk_content_size);");
            wr.WriteLine($"results.Add(new {CodeCommon.TokenValueStructName}({CodeCommon.Get_TokenId_Ref(CodeCommon.UnknownToken)}, unk_content));");
            wr.WriteLine($"{CodeCommon.VarNameBufferMinor} = {CodeCommon.VarNameBufferMinor}.Slice(unk_content_size);");
            wr.WriteLine($"{CodeCommon.VarNameBufferLocal} = {CodeCommon.VarNameBufferMinor}.Span;");
            wr.Indent--;
            wr.WriteLine("}");
        }
    }
}
