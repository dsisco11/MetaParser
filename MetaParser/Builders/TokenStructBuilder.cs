using MetaParser.Builders.Interfaces;
using MetaParser.Core;

namespace MetaParser.Builders
{
    internal class TokenStructBuilder : ICodeBuilder<MetaParserContext>
    {
        public void WriteTo(MetaParserContext context)
        {
            var wr = context.writer;

            wr.WriteLine($"namespace {context.Config.Namespace};");
            wr.WriteLine($@"[System.Diagnostics.DebuggerDisplay(""{{Data}}"", Name = ""{{({CodeCommon.TokenEnum})Id}}"")]");
            wr.WriteLine("[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]");
            wr.WriteLine($"public readonly record struct {CodeCommon.TokenValueStructName}({context.Config.IdType} Id, {CodeCommon.ReadOnlyMemory}<{context.Config.InputType}> Data)");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine("public override string ToString()");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine("return Data.ToString();");
            wr.Indent--;
            wr.WriteLine("}");
            wr.Indent--;
            wr.WriteLine("}");
            wr.WriteLine();
            wr.WriteLine($@"[System.Diagnostics.DebuggerDisplay(""{{this.ToString()}}"", Name = ""{{({CodeCommon.TokenEnum})Id}}"")]");
            wr.WriteLine($"public sealed record {CodeCommon.TokenRecordTypeName}({CodeCommon.TokenEnum} Id, {CodeCommon.TokenValueStructName}[] Values)");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine("public override string ToString()");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine("var sb = new global::System.Text.StringBuilder();");
            wr.WriteLine("for (int i=0; i<Values.Length; i++)");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine("sb.Append(Values[i].Data.ToString());");
            wr.Indent--;
            wr.WriteLine("}");
            wr.WriteLine();
            wr.WriteLine("return sb.ToString();");
            wr.Indent--;
            wr.WriteLine("}");
            wr.Indent--;
            wr.WriteLine("}");
        }
    }
}
