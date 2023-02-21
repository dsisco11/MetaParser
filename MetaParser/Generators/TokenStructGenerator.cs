using System.CodeDom.Compiler;
using MetaParser.Contexts;

namespace MetaParser.Generators
{
    internal static class TokenStructGenerator
    {
        public static void Generate(IndentedTextWriter wr, MetaParserContext context)
        {
            wr.WriteLine($"namespace {context.Namespace};");
            //wr.WriteLine($@"[System.Diagnostics.DebuggerDisplay('Data = {{Data}}', Name = '{{Enum.GetName<{context.TokenEnum}>(Id)}}')]");
            wr.WriteLine($"public readonly record struct ValueToken({context.IdTypeName} Id, {CodeGen.FormatReadOnlyMemoryBuffer(context.InputType)} Data)");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine("public override string ToString()");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine($@"return $""{{({context.TokenEnum})Id}}({{Data}})"";");
            wr.Indent--;
            wr.WriteLine("}");
            wr.Indent--;
            wr.WriteLine("}");
            wr.WriteLine();
            //wr.WriteLine($@"[System.Diagnostics.DebuggerDisplay('Data = {{Data}}', Name = '{{Enum.GetName<{context.TokenEnum}>(Id)}}')]");
            wr.WriteLine($"public sealed record Token({context.TokenEnum} Id, ValueToken[] Values)");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine("public override string ToString()");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine($@"return $""{{Id}}[{{String.Join("", "", Values.Select(o => o.ToString()))}}]"";");
            wr.Indent--;
            wr.WriteLine("}");
            wr.Indent--;
            wr.WriteLine("}");
        }
    }
}
