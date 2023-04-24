using MetaParser.Builders.Core;
using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using System;

namespace MetaParser.Builders;
using static CodeCommon;

internal class TokenStructBuilder : MetaCodeBuilder
{
    protected override void Write(ParserContext context)
    {
        var writer = context.Writer ?? throw new InvalidOperationException("Writer is null");
        NamespaceStatement.Instance.WriteTo(context);
        var idType = context.Config.IdType;

        writer.WriteLine("#nullable enable");
        writer.WriteLine(@$"

public sealed record {ValueTokenNodeType} : {TokenNodeType}
{{
    public {ReadOnlyMemory}<{context.Config.InputType}> Value {{ get; }}

    public {ValueTokenNodeType}({TokenEnum} id, {ReadOnlyMemory}<{context.Config.InputType}> value) : base(id, value.Length)
    {{
        Value = value;
    }}

    public {ValueTokenNodeType}({idType} id, {ReadOnlyMemory}<{context.Config.InputType}> value) : base(id, value.Length)
    {{
        Value = value;
    }}

    public override bool TryReplaceChild({TokenNodeType} oldChild, {TokenNodeType} newChild, out {TokenNodeType}? result)
    {{
        throw new InvalidOperationException(""Cannot replace a child of a lexer token"");
    }}
}}

public sealed record {SyntaxTokenNodeType} : {TokenNodeType}
{{
    public {TokenNodeType}[] Children {{ get; }}

    public {SyntaxTokenNodeType}({idType} id, {TokenNodeType}[] children) : base(id, children.Sum(child => child.Width))
    {{
        Children = children;
    }}

    public {SyntaxTokenNodeType}({TokenEnum} id, {TokenNodeType}[] children) : base(id, children.Sum(child => child.Width))
    {{
        Children = children;
    }}

    public override bool TryReplaceChild({TokenNodeType} oldChild, {TokenNodeType} newChild, out {TokenNodeType}? result)
    {{
        if (oldChild is null || newChild is null)
        {{
            result = null;
            return false;
        }}

        var index = Array.IndexOf(Children, oldChild);

        if (index >= 0)
        {{
            var newChildren = ({TokenNodeType}[])Children.Clone();
            newChildren[index] = newChild;

            result = new {SyntaxTokenNodeType}(Id, newChildren);
            return true;
        }}
        else
        {{
            result = null;
            return false;
        }}
    }}
}}
");

        //writer.WriteLine($@"[System.Diagnostics.DebuggerDisplay(""{{Data}}"", Name = ""{{({TokenEnum})Id}}"")]");
        //writer.WriteLine("[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]");
        //writer.WriteLine($"public readonly record struct {TokenValueStructName}({context.Config.IdType} Id, {ReadOnlyMemory}<{context.Config.InputType}> Data)");
        //writer.WriteLine("{");
        //writer.Indent++;
        //writer.WriteLine("public override string ToString()");
        //writer.WriteLine("{");
        //writer.Indent++;
        //writer.WriteLine("return Data.ToString();");
        //writer.Indent--;
        //writer.WriteLine("}");
        //writer.Indent--;
        //writer.WriteLine("}");
        //writer.WriteLine();
        //writer.WriteLine($@"[System.Diagnostics.DebuggerDisplay(""{{this.ToString()}}"", Name = ""{{({TokenEnum})Id}}"")]");
        //writer.WriteLine($"public sealed record {TokenRecordTypeName}({TokenEnum} Id, {TokenValueStructName}[] Values)");
        //writer.WriteLine("{");
        //writer.Indent++;
        //writer.WriteLine("public override string ToString()");
        //writer.WriteLine("{");
        //writer.Indent++;
        //writer.WriteLine("var sb = new global::System.Text.StringBuilder();");
        //writer.WriteLine("for (int i=0; i<Values.Length; i++)");
        //writer.WriteLine("{");
        //writer.Indent++;
        //writer.WriteLine("sb.Append(Values[i].Data.ToString());");
        //writer.Indent--;
        //writer.WriteLine("}");
        //writer.WriteLine();
        //writer.WriteLine("return sb.ToString();");
        //writer.Indent--;
        //writer.WriteLine("}");
        //writer.Indent--;
        //writer.WriteLine("}");
    }
}
