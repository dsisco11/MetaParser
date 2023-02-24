using MetaParser.CodeGen.Core;
using MetaParser.Contexts;

namespace MetaParser.Builders
{
    internal class UnknownTokenPusher : ICodeBuilder<MetaParserContext>
    {
        const string VarBuffer = "buffer";
        const string VarReader = "reader";

        public void WriteTo(MetaParserContext context)
        {
            var wr = context.writer;
            wr.WriteLine($"if ({VarBuffer}.Length != {VarReader}.Length)");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine($"var unk_content_size = {VarBuffer}.Length - {VarReader}.Length;");
            wr.WriteLine($"var unk_content = {VarBuffer}.Slice(0, unk_content_size);");
            wr.WriteLine($"valueTokens.Add(new ValueToken({context.Get_TokenId_Ref(context.UnknownToken)}, unk_content));");
            wr.WriteLine($"{VarBuffer} = {VarBuffer}.Slice(unk_content_size);");
            wr.WriteLine($"{VarReader} = {VarBuffer}.Span;");
            wr.Indent--;
            wr.WriteLine("}");
        }
    }
}
