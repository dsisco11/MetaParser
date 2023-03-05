using MetaParser.CodeGen;
using MetaParser.CodeGen.Core;
using MetaParser.Contexts;

using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders
{
    internal class TokenStructBuilder : ICodeBuilder<MetaParserContext>
    {
        public void WriteTo(MetaParserContext context)
        {
            var wr = context.writer;

            wr.WriteLine($"namespace {context.Namespace};");
            wr.WriteLine($@"[System.Diagnostics.DebuggerDisplay(""{{Data}}"", Name = ""{{({MetaParserContext.TokenEnum})Id}}"")]");
            wr.WriteLine("[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]");
            wr.WriteLine($"public readonly record struct {MetaParserContext.TokenValueStructName}({context.IdType} Id, {CodeCommon.ReadOnlyMemory}<{context.InputType}> Data)");
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
            wr.WriteLine($@"[System.Diagnostics.DebuggerDisplay(""{{this.ToString()}}"", Name = ""{{({MetaParserContext.TokenEnum})Id}}"")]");
            wr.WriteLine($"public sealed record {MetaParserContext.TokenRecordTypeName}({MetaParserContext.TokenEnum} Id, {MetaParserContext.TokenValueStructName}[] Values)");
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
