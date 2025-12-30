using System;
using System.IO;
using System.Text;

namespace MetaParser.Syntax.Green;

/// <summary>
/// Flags for green nodes, packed into a single byte.
/// </summary>
[Flags]
public enum GreenNodeFlags : byte
{
    None = 0,
    
    /// <summary>
    /// Node contains trivia (whitespace, comments, etc.).
    /// </summary>
    ContainsTrivia = 1 << 0,
    
    /// <summary>
    /// Node contains diagnostic information.
    /// </summary>
    ContainsDiagnostics = 1 << 1,
    
    /// <summary>
    /// Node contains skipped text due to parse errors.
    /// </summary>
    ContainsSkippedText = 1 << 2,
    
    /// <summary>
    /// Node is a token (leaf node).
    /// </summary>
    IsToken = 1 << 3,
    
    /// <summary>
    /// Node is trivia.
    /// </summary>
    IsTrivia = 1 << 4,
}

/// <summary>
/// Format specifiers for GreenNode formatting.
/// </summary>
/// <remarks>
/// Usage: <c>$"{node:T}"</c> for text, <c>$"{node:M}"</c> for Mermaid, etc.
/// </remarks>
public static class GreenNodeFormat
{
    /// <summary>Full text including trivia (default).</summary>
    public const string Text = "T";
    
    /// <summary>Core text excluding trivia.</summary>
    public const string Core = "C";
    
    /// <summary>Mermaid flowchart diagram.</summary>
    public const string Mermaid = "M";
    
    /// <summary>Debug tree representation.</summary>
    public const string DebugTree = "D";
    
    /// <summary>JSON representation.</summary>
    public const string Json = "J";
    
    /// <summary>Single-line debug string.</summary>
    public const string Debug = "G";
}

/// <summary>
/// Base class for all green (immutable) syntax nodes.
/// Green nodes are the backbone of the Red-Green tree pattern:
/// - Immutable and thread-safe
/// - Don't know their parent or absolute position
/// - Can be shared/reused across the tree
/// - Store actual data (text, width, children)
/// 
/// Implements IFormattable with format specifiers:
/// - "T" or null: Full text including trivia (default)
/// - "C": Core text excluding trivia
/// - "M": Mermaid flowchart diagram
/// - "D": Debug tree representation
/// - "J": JSON representation
/// - "G": Single-line debug string
/// </summary>
public abstract class GreenNode : IFormattable
#if NET8_0_OR_GREATER
    , ISpanFormattable
#endif
{
    #region Fields
    
    private readonly ushort _kind;
    private readonly GreenNodeFlags _flags;
    private readonly byte _slotCount;
    private readonly int _fullWidth;
    
    #endregion
    
    #region Properties
    
    /// <summary>Gets the syntax kind of this node.</summary>
    public ushort RawKind => _kind;
    
    /// <summary>Gets the node flags.</summary>
    public GreenNodeFlags Flags => _flags;
    
    /// <summary>Gets the number of child slots.</summary>
    public int SlotCount => _slotCount;
    
    /// <summary>Gets the full width of this node including all trivia.</summary>
    public int FullWidth => _fullWidth;
    
    /// <summary>Gets the width of this node excluding leading/trailing trivia.</summary>
    public virtual int Width => _fullWidth;
    
    /// <summary>Gets whether this node is a token (leaf node).</summary>
    public bool IsToken => (_flags & GreenNodeFlags.IsToken) != 0;
    
    /// <summary>Gets whether this node is trivia.</summary>
    public bool IsTrivia => (_flags & GreenNodeFlags.IsTrivia) != 0;
    
    /// <summary>Gets whether this node or any descendant contains trivia.</summary>
    public bool ContainsTrivia => (_flags & GreenNodeFlags.ContainsTrivia) != 0;
    
    /// <summary>Gets whether this node or any descendant contains diagnostics.</summary>
    public bool ContainsDiagnostics => (_flags & GreenNodeFlags.ContainsDiagnostics) != 0;
    
    #endregion
    
    #region Constructors
    
    protected GreenNode(ushort kind, int fullWidth, GreenNodeFlags flags = GreenNodeFlags.None, byte slotCount = 0)
    {
        _kind = kind;
        _fullWidth = fullWidth;
        _flags = flags;
        _slotCount = slotCount;
    }
    
    #endregion
    
    #region Abstract Members
    
    /// <summary>Gets the child at the specified slot index.</summary>
    public abstract GreenNode? GetSlot(int index);
    
    /// <summary>Writes the full text of this node to the writer.</summary>
    public abstract void WriteTo(TextWriter writer);
    
    #endregion
    
    #region Virtual Members
    
    /// <summary>Gets the leading trivia width (0 for non-tokens).</summary>
    public virtual int LeadingTriviaWidth => 0;
    
    /// <summary>Gets the trailing trivia width (0 for non-tokens).</summary>
    public virtual int TrailingTriviaWidth => 0;
    
    /// <summary>Gets the leading trivia (null for non-tokens or tokens without leading trivia).</summary>
    public virtual GreenNode? LeadingTrivia => null;
    
    /// <summary>Gets the trailing trivia (null for non-tokens or tokens without trailing trivia).</summary>
    public virtual GreenNode? TrailingTrivia => null;
    
    /// <summary>Creates a copy of this node that owns its data.</summary>
    public virtual GreenNode ToOwned() => this;
    
    /// <summary>Writes the core text (without leading/trailing trivia) to the writer.</summary>
    protected virtual void WriteCoreTo(TextWriter writer) => WriteTo(writer);
    
    #endregion
    
    #region Text Methods
    
    /// <summary>Gets the full text of this node including all trivia.</summary>
    public string ToFullString()
    {
        using var writer = new StringWriter();
        WriteTo(writer);
        return writer.ToString();
    }
    
    /// <summary>Gets the core text of this node excluding trivia.</summary>
    public string ToCoreString()
    {
        using var writer = new StringWriter();
        WriteCoreTo(writer);
        return writer.ToString();
    }
    
    #endregion
    
    #region IFormattable Implementation
    
    /// <summary>
    /// Formats this node using the specified format.
    /// </summary>
    /// <param name="format">
    /// Format specifier:
    /// - "T" or null: Full text including trivia (default)
    /// - "C": Core text excluding trivia
    /// - "M": Mermaid flowchart diagram
    /// - "D": Debug tree representation
    /// - "J": JSON representation
    /// - "G": Single-line debug string
    /// </param>
    /// <param name="formatProvider">Ignored.</param>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return format?.ToUpperInvariant() switch
        {
            null or "" or "T" => ToFullString(),
            "C" => ToCoreString(),
            "M" => FormatMermaid(),
            "D" => FormatDebugTree(),
            "J" => FormatJson(),
            "G" => FormatDebugString(),
            _ => throw new FormatException($"Unknown format specifier: {format}")
        };
    }
    
    public override string ToString() => ToFullString();
    
#if NET8_0_OR_GREATER
    /// <summary>
    /// Tries to format this node into the provided span.
    /// </summary>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var result = ToString(format.IsEmpty ? null : format.ToString(), provider);
        if (result.Length <= destination.Length)
        {
            result.AsSpan().CopyTo(destination);
            charsWritten = result.Length;
            return true;
        }
        charsWritten = 0;
        return false;
    }
#endif
    
    #endregion
    
    #region Format Methods
    
    /// <summary>Gets a single-line debug representation.</summary>
    protected virtual string FormatDebugString()
    {
        return $"{GetType().Name}[Kind={_kind}, Width={_fullWidth}]";
    }
    
    /// <summary>Generates a Mermaid flowchart diagram.</summary>
    private string FormatMermaid()
    {
        var sb = new StringBuilder();
        sb.AppendLine("```mermaid");
        sb.AppendLine("flowchart TD");
        
        int nodeId = 0;
        WriteMermaidNode(sb, this, ref nodeId, "    ");
        
        sb.AppendLine("```");
        return sb.ToString();
    }
    
    private static int WriteMermaidNode(StringBuilder sb, GreenNode node, ref int nextId, string indent)
    {
        int myId = nextId++;
        string nodeLabel = GetMermaidLabel(node);
        
        if (node.IsToken)
            sb.AppendLine($"{indent}N{myId}[[\"{nodeLabel}\"]]");
        else if (node.IsTrivia)
            sb.AppendLine($"{indent}N{myId}(\"{nodeLabel}\")");
        else
            sb.AppendLine($"{indent}N{myId}[\"{nodeLabel}\"]");
        
        if (node is GreenToken token)
        {
            if (token.LeadingTrivia is not null)
            {
                int triviaId = WriteMermaidNode(sb, token.LeadingTrivia, ref nextId, indent);
                sb.AppendLine($"{indent}N{triviaId} -.->|leading| N{myId}");
            }
            if (token.TrailingTrivia is not null)
            {
                int triviaId = WriteMermaidNode(sb, token.TrailingTrivia, ref nextId, indent);
                sb.AppendLine($"{indent}N{myId} -.->|trailing| N{triviaId}");
            }
        }
        
        for (int i = 0; i < node.SlotCount; i++)
        {
            var child = node.GetSlot(i);
            if (child is not null)
            {
                int childId = WriteMermaidNode(sb, child, ref nextId, indent);
                sb.AppendLine($"{indent}N{myId} --> N{childId}");
            }
        }
        
        return myId;
    }
    
    private static string GetMermaidLabel(GreenNode node)
    {
        if (node is GreenToken token)
        {
            var text = token.GetText();
            if (text.Length > 15)
                text = text.Substring(0, 12) + "...";
            return $"Kind:{token.RawKind}<br/>{EscapeMermaid(text)}";
        }
        else if (node is GreenTrivia trivia)
        {
            var text = trivia.GetText();
            if (text.Length > 10)
                text = text.Substring(0, 7) + "...";
            return $"Trivia<br/>{EscapeMermaid(text)}";
        }
        else if (node is GreenTriviaList list)
        {
            return $"TriviaList<br/>Count:{list.Count}";
        }
        return $"Node<br/>Kind:{node.RawKind}";
    }
    
    private static string EscapeMermaid(string text)
    {
        return text
            .Replace("\\", "\\\\")
            .Replace("\"", "#quot;")
            .Replace("\r", "\\r")
            .Replace("\n", "\\n")
            .Replace("\t", "\\t")
            .Replace("<", "#lt;")
            .Replace(">", "#gt;")
            .Replace("{", "#lbrace;")
            .Replace("}", "#rbrace;")
            .Replace("[", "#lbrack;")
            .Replace("]", "#rbrack;");
    }
    
    /// <summary>Generates a debug tree representation.</summary>
    private string FormatDebugTree()
    {
        var sb = new StringBuilder();
        WriteDebugTree(sb, this, "", true);
        return sb.ToString();
    }
    
    private static void WriteDebugTree(StringBuilder sb, GreenNode node, string indent, bool isLast)
    {
        var marker = isLast ? "└── " : "├── ";
        sb.Append(indent);
        sb.Append(marker);
        sb.AppendLine(node.FormatDebugString());
        
        var newIndent = indent + (isLast ? "    " : "│   ");
        
        if (node is GreenToken token)
        {
            var hasLeading = token.LeadingTrivia is not null;
            var hasTrailing = token.TrailingTrivia is not null;
            
            if (hasLeading)
                WriteDebugTree(sb, token.LeadingTrivia!, newIndent, !hasTrailing);
            if (hasTrailing)
                WriteDebugTree(sb, token.TrailingTrivia!, newIndent, true);
        }
        else
        {
            for (int i = 0; i < node.SlotCount; i++)
            {
                var child = node.GetSlot(i);
                if (child is not null)
                {
                    bool lastChild = true;
                    for (int j = i + 1; j < node.SlotCount; j++)
                    {
                        if (node.GetSlot(j) is not null)
                        {
                            lastChild = false;
                            break;
                        }
                    }
                    WriteDebugTree(sb, child, newIndent, lastChild);
                }
            }
        }
    }
    
    /// <summary>Generates a JSON representation.</summary>
    private string FormatJson()
    {
        var sb = new StringBuilder();
        WriteJson(sb, this, 0);
        return sb.ToString();
    }
    
    private static void WriteJson(StringBuilder sb, GreenNode node, int indent)
    {
        var ind = new string(' ', indent * 2);
        
        sb.Append(ind);
        sb.AppendLine("{");
        
        sb.Append(ind);
        sb.Append($"  \"type\": \"{node.GetType().Name}\",\n");
        sb.Append(ind);
        sb.Append($"  \"kind\": {node.RawKind},\n");
        sb.Append(ind);
        sb.Append($"  \"fullWidth\": {node.FullWidth},\n");
        sb.Append(ind);
        sb.Append($"  \"width\": {node.Width}");
        
        if (node is GreenToken token)
        {
            sb.Append(",\n");
            sb.Append(ind);
            sb.Append($"  \"text\": \"{EscapeJson(token.GetText())}\"");
            
            if (token.LeadingTrivia is not null)
            {
                sb.Append(",\n");
                sb.Append(ind);
                sb.Append("  \"leadingTrivia\": ");
                WriteJson(sb, token.LeadingTrivia, indent + 1);
            }
            if (token.TrailingTrivia is not null)
            {
                sb.Append(",\n");
                sb.Append(ind);
                sb.Append("  \"trailingTrivia\": ");
                WriteJson(sb, token.TrailingTrivia, indent + 1);
            }
        }
        else if (node is GreenTrivia trivia)
        {
            sb.Append(",\n");
            sb.Append(ind);
            sb.Append($"  \"text\": \"{EscapeJson(trivia.GetText())}\"");
        }
        else if (node.SlotCount > 0)
        {
            sb.Append(",\n");
            sb.Append(ind);
            sb.Append("  \"children\": [\n");
            
            bool first = true;
            for (int i = 0; i < node.SlotCount; i++)
            {
                var child = node.GetSlot(i);
                if (child is not null)
                {
                    if (!first) sb.Append(",\n");
                    first = false;
                    WriteJson(sb, child, indent + 2);
                }
            }
            
            sb.Append('\n');
            sb.Append(ind);
            sb.Append("  ]");
        }
        
        sb.Append('\n');
        sb.Append(ind);
        sb.Append('}');
    }
    
    private static string EscapeJson(string text)
    {
        return text
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\r", "\\r")
            .Replace("\n", "\\n")
            .Replace("\t", "\\t");
    }
    
    #endregion
    
    #region Helper Methods
    
    protected static GreenNodeFlags CombineFlags(params GreenNode?[] children)
    {
        var flags = GreenNodeFlags.None;
        foreach (var child in children)
        {
            if (child is null) continue;
            if (child.ContainsTrivia || child.IsTrivia)
                flags |= GreenNodeFlags.ContainsTrivia;
            if (child.ContainsDiagnostics)
                flags |= GreenNodeFlags.ContainsDiagnostics;
        }
        return flags;
    }
    
    protected static int ComputeWidth(params GreenNode?[] children)
    {
        int width = 0;
        foreach (var child in children)
        {
            if (child is not null)
                width += child.FullWidth;
        }
        return width;
    }
    
    #endregion
}
