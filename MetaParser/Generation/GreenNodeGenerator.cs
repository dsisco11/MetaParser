using MetaParser.Schema;

namespace MetaParser.Generation;

/// <summary>
/// Generates the GreenNode base class and related infrastructure.
/// </summary>
internal static class GreenNodeGenerator
{
    /// <summary>
    /// Generates the GreenNode.g.cs file.
    /// </summary>
    public static GeneratedFileBuilder Generate(SchemaDefinition schema)
    {
        var file = new GeneratedFileBuilder(schema.Namespace)
            .WithFileName("GreenNode.g.cs")
            .AddUsings("System", "System.IO", "System.Text");

        var code = file.Code;

        GenerateGreenNodeFlags(code);
        code.AppendLine();
        GenerateGreenNodeFormatConstants(code);
        code.AppendLine();
        GenerateGreenNodeClass(code);

        return file;
    }

    private static void GenerateGreenNodeFlags(CodeBuilder code)
    {
        code.AppendSummary("Flags for green nodes, packed into a single byte.");
        code.AppendLine("[Flags]");
        code.AppendLine("internal enum GreenNodeFlags : byte");
        code.OpenBlock();
        code.AppendLine("None = 0,");
        code.AppendLine();
        code.AppendSummaryLine("Node contains trivia.");
        code.AppendLine("ContainsTrivia = 1 << 0,");
        code.AppendLine();
        code.AppendSummaryLine("Node contains diagnostics.");
        code.AppendLine("ContainsDiagnostics = 1 << 1,");
        code.AppendLine();
        code.AppendSummaryLine("Node contains skipped text.");
        code.AppendLine("ContainsSkippedText = 1 << 2,");
        code.AppendLine();
        code.AppendSummaryLine("Node is a token (leaf node).");
        code.AppendLine("IsToken = 1 << 3,");
        code.AppendLine();
        code.AppendSummaryLine("Node is trivia.");
        code.AppendLine("IsTrivia = 1 << 4,");
        code.CloseBlock();
    }

    private static void GenerateGreenNodeFormatConstants(CodeBuilder code)
    {
        code.AppendSummary("Format specifiers for GreenNode.ToString(format).");
        code.AppendLine("internal static class GreenNodeFormat");
        code.OpenBlock();
        code.AppendSummaryLine("Full text including trivia (default).");
        code.AppendLine("public const string Text = \"T\";");
        code.AppendLine();
        code.AppendSummaryLine("Core text excluding trivia.");
        code.AppendLine("public const string Core = \"C\";");
        code.AppendLine();
        code.AppendSummaryLine("Mermaid flowchart diagram.");
        code.AppendLine("public const string Mermaid = \"M\";");
        code.AppendLine();
        code.AppendSummaryLine("Debug tree representation.");
        code.AppendLine("public const string DebugTree = \"D\";");
        code.AppendLine();
        code.AppendSummaryLine("JSON representation.");
        code.AppendLine("public const string Json = \"J\";");
        code.AppendLine();
        code.AppendSummaryLine("Single-line debug string.");
        code.AppendLine("public const string Debug = \"G\";");
        code.CloseBlock();
    }

    private static void GenerateGreenNodeClass(CodeBuilder code)
    {
        code.AppendSummary(@"Base class for all green (immutable) syntax nodes.
Implements IFormattable with format specifiers: T, C, M, D, J, G.");
        code.AppendLine("internal abstract class GreenNode : IFormattable, ISpanFormattable");
        code.OpenBlock();

        // Fields
        code.AppendRegion("Fields");
        code.AppendLine("private readonly ushort _kind;");
        code.AppendLine("private readonly GreenNodeFlags _flags;");
        code.AppendLine("private readonly byte _slotCount;");
        code.AppendLine("private readonly int _fullWidth;");
        code.AppendEndRegion();
        code.AppendLine();

        // Properties
        code.AppendRegion("Properties");
        code.AppendSummaryLine("Gets the syntax kind of this node.");
        code.AppendLine("public ushort RawKind => _kind;");
        code.AppendLine();
        code.AppendSummaryLine("Gets the node flags.");
        code.AppendLine("public GreenNodeFlags Flags => _flags;");
        code.AppendLine();
        code.AppendSummaryLine("Gets the number of child slots.");
        code.AppendLine("public int SlotCount => _slotCount;");
        code.AppendLine();
        code.AppendSummaryLine("Gets the full width including trivia.");
        code.AppendLine("public int FullWidth => _fullWidth;");
        code.AppendLine();
        code.AppendSummaryLine("Gets the width excluding leading/trailing trivia.");
        code.AppendLine("public virtual int Width => _fullWidth;");
        code.AppendLine();
        code.AppendSummaryLine("Gets whether this is a token.");
        code.AppendLine("public bool IsToken => (_flags & GreenNodeFlags.IsToken) != 0;");
        code.AppendLine();
        code.AppendSummaryLine("Gets whether this is trivia.");
        code.AppendLine("public bool IsTrivia => (_flags & GreenNodeFlags.IsTrivia) != 0;");
        code.AppendLine();
        code.AppendSummaryLine("Gets whether this contains trivia.");
        code.AppendLine("public bool ContainsTrivia => (_flags & GreenNodeFlags.ContainsTrivia) != 0;");
        code.AppendLine();
        code.AppendSummaryLine("Gets whether this contains diagnostics.");
        code.AppendLine("public bool ContainsDiagnostics => (_flags & GreenNodeFlags.ContainsDiagnostics) != 0;");
        code.AppendEndRegion();
        code.AppendLine();

        // Constructor
        code.AppendRegion("Constructor");
        code.AppendLine("protected GreenNode(ushort kind, int fullWidth, GreenNodeFlags flags = GreenNodeFlags.None, byte slotCount = 0)");
        code.OpenBlock();
        code.AppendLine("_kind = kind;");
        code.AppendLine("_fullWidth = fullWidth;");
        code.AppendLine("_flags = flags;");
        code.AppendLine("_slotCount = slotCount;");
        code.CloseBlock();
        code.AppendEndRegion();
        code.AppendLine();

        // Abstract members
        code.AppendRegion("Abstract Members");
        code.AppendSummaryLine("Gets the child at the specified slot index.");
        code.AppendLine("public abstract GreenNode? GetSlot(int index);");
        code.AppendLine();
        code.AppendSummaryLine("Writes the full text to the writer.");
        code.AppendLine("public abstract void WriteTo(TextWriter writer);");
        code.AppendEndRegion();
        code.AppendLine();

        // Virtual members
        code.AppendRegion("Virtual Members");
        code.AppendSummaryLine("Gets the leading trivia width.");
        code.AppendLine("public virtual int LeadingTriviaWidth => 0;");
        code.AppendLine();
        code.AppendSummaryLine("Gets the trailing trivia width.");
        code.AppendLine("public virtual int TrailingTriviaWidth => 0;");
        code.AppendLine();
        code.AppendSummaryLine("Gets the leading trivia.");
        code.AppendLine("public virtual GreenNode? LeadingTrivia => null;");
        code.AppendLine();
        code.AppendSummaryLine("Gets the trailing trivia.");
        code.AppendLine("public virtual GreenNode? TrailingTrivia => null;");
        code.AppendLine();
        code.AppendSummaryLine("Creates an owned copy of this node.");
        code.AppendLine("public virtual GreenNode ToOwned() => this;");
        code.AppendLine();
        code.AppendSummaryLine("Writes core text without trivia.");
        code.AppendLine("protected virtual void WriteCoreTo(TextWriter writer) => WriteTo(writer);");
        code.AppendLine();
        code.AppendSummaryLine("Gets a debug string representation.");
        code.AppendLine("protected virtual string FormatDebugString() => $\"{GetType().Name}[Kind={_kind}, Width={_fullWidth}]\";");
        code.AppendEndRegion();
        code.AppendLine();

        // Text methods
        code.AppendRegion("Text Methods");
        code.AppendSummaryLine("Gets the full text including trivia.");
        code.AppendLine("public string ToFullString()");
        code.OpenBlock();
        code.AppendLine("using var writer = new StringWriter();");
        code.AppendLine("WriteTo(writer);");
        code.AppendLine("return writer.ToString();");
        code.CloseBlock();
        code.AppendLine();
        code.AppendSummaryLine("Gets the core text excluding trivia.");
        code.AppendLine("public string ToCoreString()");
        code.OpenBlock();
        code.AppendLine("using var writer = new StringWriter();");
        code.AppendLine("WriteCoreTo(writer);");
        code.AppendLine("return writer.ToString();");
        code.CloseBlock();
        code.AppendEndRegion();
        code.AppendLine();

        // IFormattable
        code.AppendRegion("IFormattable");
        GenerateIFormattableImplementation(code);
        code.AppendEndRegion();
        code.AppendLine();

        // Format methods
        code.AppendRegion("Format Methods");
        GenerateFormatMethods(code);
        code.AppendEndRegion();
        code.AppendLine();

        // Helper methods
        code.AppendRegion("Helper Methods");
        code.AppendLine("protected static GreenNodeFlags CombineFlags(params GreenNode?[] children)");
        code.OpenBlock();
        code.AppendLine("var flags = GreenNodeFlags.None;");
        code.AppendLine("foreach (var child in children)");
        code.OpenBlock();
        code.AppendLine("if (child is null) continue;");
        code.AppendLine("if (child.ContainsTrivia || child.IsTrivia) flags |= GreenNodeFlags.ContainsTrivia;");
        code.AppendLine("if (child.ContainsDiagnostics) flags |= GreenNodeFlags.ContainsDiagnostics;");
        code.CloseBlock();
        code.AppendLine("return flags;");
        code.CloseBlock();
        code.AppendLine();
        code.AppendLine("protected static int ComputeWidth(params GreenNode?[] children)");
        code.OpenBlock();
        code.AppendLine("int width = 0;");
        code.AppendLine("foreach (var child in children)");
        code.OpenBlock();
        code.AppendLine("if (child is not null) width += child.FullWidth;");
        code.CloseBlock();
        code.AppendLine("return width;");
        code.CloseBlock();
        code.AppendEndRegion();

        code.CloseBlock();
    }

    private static void GenerateIFormattableImplementation(CodeBuilder code)
    {
        code.AppendSummary("Formats using the specified format specifier.");
        code.AppendLine("public string ToString(string? format, IFormatProvider? formatProvider)");
        code.OpenBlock();
        code.AppendLine("return format?.ToUpperInvariant() switch");
        code.OpenBlock();
        code.AppendLine("null or \"\" or \"T\" => ToFullString(),");
        code.AppendLine("\"C\" => ToCoreString(),");
        code.AppendLine("\"M\" => FormatMermaid(),");
        code.AppendLine("\"D\" => FormatDebugTree(),");
        code.AppendLine("\"J\" => FormatJson(),");
        code.AppendLine("\"G\" => FormatDebugString(),");
        code.AppendLine("_ => throw new FormatException($\"Unknown format: {format}\")");
        code.CloseBlock(";");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("public override string ToString() => ToFullString();");
        code.AppendLine();

        code.AppendSummaryLine("Tries to format into the provided span.");
        code.AppendLine("public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)");
        code.OpenBlock();
        code.AppendLine("var result = ToString(format.IsEmpty ? null : format.ToString(), provider);");
        code.AppendLine("if (result.Length <= destination.Length)");
        code.OpenBlock();
        code.AppendLine("result.AsSpan().CopyTo(destination);");
        code.AppendLine("charsWritten = result.Length;");
        code.AppendLine("return true;");
        code.CloseBlock();
        code.AppendLine("charsWritten = 0;");
        code.AppendLine("return false;");
        code.CloseBlock();
    }

    private static void GenerateFormatMethods(CodeBuilder code)
    {
        // FormatMermaid
        code.AppendLine("private string FormatMermaid()");
        code.OpenBlock();
        code.AppendLine("var sb = new StringBuilder();");
        code.AppendLine("sb.AppendLine(\"```mermaid\");");
        code.AppendLine("sb.AppendLine(\"flowchart TD\");");
        code.AppendLine("int nodeId = 0;");
        code.AppendLine("WriteMermaidNode(sb, this, ref nodeId, \"    \");");
        code.AppendLine("sb.AppendLine(\"```\");");
        code.AppendLine("return sb.ToString();");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("private static int WriteMermaidNode(StringBuilder sb, GreenNode node, ref int nextId, string indent)");
        code.OpenBlock();
        code.AppendLine("int myId = nextId++;");
        code.AppendLine("string label = GetMermaidLabel(node);");
        code.AppendLine("if (node.IsToken) sb.AppendLine($\"{indent}N{myId}[[\\\"{label}\\\"]]\");");
        code.AppendLine("else if (node.IsTrivia) sb.AppendLine($\"{indent}N{myId}(\\\"{label}\\\")\");");
        code.AppendLine("else sb.AppendLine($\"{indent}N{myId}[\\\"{label}\\\"]\");");
        code.AppendLine();
        code.AppendLine("if (node is GreenToken token)");
        code.OpenBlock();
        code.AppendLine("if (token.LeadingTrivia is not null)");
        code.OpenBlock();
        code.AppendLine("int tid = WriteMermaidNode(sb, token.LeadingTrivia, ref nextId, indent);");
        code.AppendLine("sb.AppendLine($\"{indent}N{tid} -.->|leading| N{myId}\");");
        code.CloseBlock();
        code.AppendLine("if (token.TrailingTrivia is not null)");
        code.OpenBlock();
        code.AppendLine("int tid = WriteMermaidNode(sb, token.TrailingTrivia, ref nextId, indent);");
        code.AppendLine("sb.AppendLine($\"{indent}N{myId} -.->|trailing| N{tid}\");");
        code.CloseBlock();
        code.CloseBlock();
        code.AppendLine();
        code.AppendLine("for (int i = 0; i < node.SlotCount; i++)");
        code.OpenBlock();
        code.AppendLine("var child = node.GetSlot(i);");
        code.AppendLine("if (child is not null)");
        code.OpenBlock();
        code.AppendLine("int cid = WriteMermaidNode(sb, child, ref nextId, indent);");
        code.AppendLine("sb.AppendLine($\"{indent}N{myId} --> N{cid}\");");
        code.CloseBlock();
        code.CloseBlock();
        code.AppendLine("return myId;");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("private static string GetMermaidLabel(GreenNode node)");
        code.OpenBlock();
        code.AppendLine("if (node is GreenToken t)");
        code.OpenBlock();
        code.AppendLine("var text = t.GetText();");
        code.AppendLine("if (text.Length > 15) text = text.Substring(0, 12) + \"...\";");
        code.AppendLine("return $\"Kind:{t.RawKind}<br/>{EscapeMermaid(text)}\";");
        code.CloseBlock();
        code.AppendLine("if (node is GreenTrivia tr) return $\"Trivia<br/>{EscapeMermaid(tr.GetText())}\";");
        code.AppendLine("if (node is GreenTriviaList tl) return $\"TriviaList<br/>Count:{tl.Count}\";");
        code.AppendLine("return $\"Node<br/>Kind:{node.RawKind}\";");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("private static string EscapeMermaid(string text) => text");
        code.Indent();
        code.AppendLine(".Replace(\"\\\\\", \"\\\\\\\\\").Replace(\"\\\"\", \"#quot;\")");
        code.AppendLine(".Replace(\"\\r\", \"\\\\r\").Replace(\"\\n\", \"\\\\n\").Replace(\"\\t\", \"\\\\t\")");
        code.AppendLine(".Replace(\"<\", \"#lt;\").Replace(\">\", \"#gt;\")");
        code.AppendLine(".Replace(\"{\", \"#lbrace;\").Replace(\"}\", \"#rbrace;\")");
        code.AppendLine(".Replace(\"[\", \"#lbrack;\").Replace(\"]\", \"#rbrack;\");");
        code.Outdent();
        code.AppendLine();

        // FormatDebugTree
        code.AppendLine("private string FormatDebugTree()");
        code.OpenBlock();
        code.AppendLine("var sb = new StringBuilder();");
        code.AppendLine("WriteDebugTree(sb, this, \"\", true);");
        code.AppendLine("return sb.ToString();");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("private static void WriteDebugTree(StringBuilder sb, GreenNode node, string indent, bool isLast)");
        code.OpenBlock();
        code.AppendLine("sb.Append(indent).Append(isLast ? \"└── \" : \"├── \").AppendLine(node.FormatDebugString());");
        code.AppendLine("var newIndent = indent + (isLast ? \"    \" : \"│   \");");
        code.AppendLine("if (node is GreenToken token)");
        code.OpenBlock();
        code.AppendLine("bool hasLeading = token.LeadingTrivia is not null;");
        code.AppendLine("bool hasTrailing = token.TrailingTrivia is not null;");
        code.AppendLine("if (hasLeading) WriteDebugTree(sb, token.LeadingTrivia!, newIndent, !hasTrailing);");
        code.AppendLine("if (hasTrailing) WriteDebugTree(sb, token.TrailingTrivia!, newIndent, true);");
        code.CloseBlock();
        code.AppendLine("else");
        code.OpenBlock();
        code.AppendLine("for (int i = 0; i < node.SlotCount; i++)");
        code.OpenBlock();
        code.AppendLine("var child = node.GetSlot(i);");
        code.AppendLine("if (child is null) continue;");
        code.AppendLine("bool last = true;");
        code.AppendLine("for (int j = i + 1; j < node.SlotCount; j++) if (node.GetSlot(j) is not null) { last = false; break; }");
        code.AppendLine("WriteDebugTree(sb, child, newIndent, last);");
        code.CloseBlock();
        code.CloseBlock();
        code.CloseBlock();
        code.AppendLine();

        // FormatJson
        code.AppendLine("private string FormatJson()");
        code.OpenBlock();
        code.AppendLine("var sb = new StringBuilder();");
        code.AppendLine("WriteJson(sb, this, 0);");
        code.AppendLine("return sb.ToString();");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("private static void WriteJson(StringBuilder sb, GreenNode node, int indent)");
        code.OpenBlock();
        code.AppendLine("var ind = new string(' ', indent * 2);");
        code.AppendLine("sb.Append(ind).AppendLine(\"{\");");
        code.AppendLine("sb.Append(ind).Append($\"  \\\"type\\\": \\\"{node.GetType().Name}\\\",\\n\");");
        code.AppendLine("sb.Append(ind).Append($\"  \\\"kind\\\": {node.RawKind},\\n\");");
        code.AppendLine("sb.Append(ind).Append($\"  \\\"fullWidth\\\": {node.FullWidth},\\n\");");
        code.AppendLine("sb.Append(ind).Append($\"  \\\"width\\\": {node.Width}\");");
        code.AppendLine("if (node is GreenToken t)");
        code.OpenBlock();
        code.AppendLine("sb.Append(\",\\n\").Append(ind).Append($\"  \\\"text\\\": \\\"{EscapeJson(t.GetText())}\\\"\");");
        code.AppendLine("if (t.LeadingTrivia is not null) { sb.Append(\",\\n\").Append(ind).Append(\"  \\\"leadingTrivia\\\": \"); WriteJson(sb, t.LeadingTrivia, indent + 1); }");
        code.AppendLine("if (t.TrailingTrivia is not null) { sb.Append(\",\\n\").Append(ind).Append(\"  \\\"trailingTrivia\\\": \"); WriteJson(sb, t.TrailingTrivia, indent + 1); }");
        code.CloseBlock();
        code.AppendLine("else if (node is GreenTrivia tr)");
        code.OpenBlock();
        code.AppendLine("sb.Append(\",\\n\").Append(ind).Append($\"  \\\"text\\\": \\\"{EscapeJson(tr.GetText())}\\\"\");");
        code.CloseBlock();
        code.AppendLine("sb.Append('\\n').Append(ind).Append('}');");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("private static string EscapeJson(string text) => text");
        code.Indent();
        code.AppendLine(".Replace(\"\\\\\", \"\\\\\\\\\").Replace(\"\\\"\", \"\\\\\\\"\")");
        code.AppendLine(".Replace(\"\\r\", \"\\\\r\").Replace(\"\\n\", \"\\\\n\").Replace(\"\\t\", \"\\\\t\");");
        code.Outdent();
    }
}
