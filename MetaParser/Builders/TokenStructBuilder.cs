using MetaParser.CodeGen;
using MetaParser.CodeGen.Core;
using MetaParser.Contexts;

namespace MetaParser.Builders
{
    internal class TokenStructBuilder : ICodeBuilder<MetaParserContext>
    {
        public void WriteTo(MetaParserContext context)
        {
            var wr = context.writer;

            wr.WriteLine($"namespace {context.Namespace};");
            wr.WriteLine($@"[System.Diagnostics.DebuggerDisplay(""{{Data}}"", Name = ""{{({context.TokenEnum})Id}}"")]");
            wr.WriteLine($"public readonly record struct ValueToken({context.IdTypeName} Id, {CodeCommon.FormatReadOnlyMemoryBuffer(context.InputType)} Data)");
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
            wr.WriteLine($@"[System.Diagnostics.DebuggerDisplay(""{{this.ToString()}}"", Name = ""{{({context.TokenEnum})Id}}"")]");
            wr.WriteLine($"public sealed record Token({context.TokenEnum} Id, ValueToken[] Values)");
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
