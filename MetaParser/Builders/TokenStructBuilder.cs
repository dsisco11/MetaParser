using MetaParser.Builders.Interfaces;
using MetaParser.Core;

namespace MetaParser.Builders;
using static CodeCommon;

internal class TokenStructBuilder : MetaCodeBuilder
{
    protected override void Write(MetaParserContext context)
    {
        var writer = context.Writer;

        writer.WriteLine($"namespace {context.Config.Namespace};");
        writer.WriteLine($@"[System.Diagnostics.DebuggerDisplay(""{{Data}}"", Name = ""{{({TokenEnum})Id}}"")]");
        writer.WriteLine("[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]");
        writer.WriteLine($"public readonly record struct {TokenValueStructName}({context.Config.IdType} Id, {ReadOnlyMemory}<{context.Config.InputType}> Data)");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine("public override string ToString()");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine("return Data.ToString();");
        writer.Indent--;
        writer.WriteLine("}");
        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine();
        writer.WriteLine($@"[System.Diagnostics.DebuggerDisplay(""{{this.ToString()}}"", Name = ""{{({TokenEnum})Id}}"")]");
        writer.WriteLine($"public sealed record {TokenRecordTypeName}({TokenEnum} Id, {TokenValueStructName}[] Values)");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine("public override string ToString()");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine("var sb = new global::System.Text.StringBuilder();");
        writer.WriteLine("for (int i=0; i<Values.Length; i++)");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine("sb.Append(Values[i].Data.ToString());");
        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine();
        writer.WriteLine("return sb.ToString();");
        writer.Indent--;
        writer.WriteLine("}");
        writer.Indent--;
        writer.WriteLine("}");
    }
}
