using MetaParser.CodeGen.Core;
using MetaParser.Contexts;

namespace MetaParser.Builders
{
    internal class UnknownTokenPusher : IMetaCodeBuilder
    {
        public static IMetaCodeBuilder Instance = new UnknownTokenPusher();

        public void WriteTo(MetaParserContext context)
        {
            var wr = context.writer;
            wr.WriteLine($"if ({MetaParserContext.VarNameBufferMinor}.Length != {MetaParserContext.VarNameBufferLocal}.Length)");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine($"var unk_content_size = {MetaParserContext.VarNameBufferMinor}.Length - {MetaParserContext.VarNameBufferLocal}.Length;");
            wr.WriteLine($"var unk_content = {MetaParserContext.VarNameBufferMinor}.Slice(0, unk_content_size);");
            wr.WriteLine($"results.Add(new {MetaParserContext.TokenValueStructName}({MetaParserContext.Get_TokenId_Ref(MetaParserContext.UnknownToken)}, unk_content));");
            wr.WriteLine($"{MetaParserContext.VarNameBufferMinor} = {MetaParserContext.VarNameBufferMinor}.Slice(unk_content_size);");
            wr.WriteLine($"{MetaParserContext.VarNameBufferLocal} = {MetaParserContext.VarNameBufferMinor}.Span;");
            wr.Indent--;
            wr.WriteLine("}");
        }
    }
}
