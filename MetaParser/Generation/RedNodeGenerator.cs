using MetaParser.Schema;

namespace MetaParser.Generation;

/// <summary>
/// Generates the RedNode base class that wraps GreenNode with parent/position information.
/// 
/// Red nodes are the "facade" layer in Roslyn's red-green tree architecture:
/// - They wrap immutable green nodes
/// - They provide parent references (green nodes don't have parents)
/// - They track absolute positions in the source text
/// - They are created lazily on demand
/// </summary>
internal static class RedNodeGenerator
{
    public static GeneratedFileBuilder Generate(SchemaDefinition schema)
    {
        var file = new GeneratedFileBuilder(schema.Namespace)
            .WithFileName("SyntaxNode.g.cs")
            .AddUsings("System", "System.Collections.Generic", "System.Diagnostics");

        var code = file.Code;

        GenerateSyntaxNodeClass(code);

        return file;
    }

    private static void GenerateSyntaxNodeClass(CodeBuilder code)
    {
        code.AppendSummary(@"Base class for syntax nodes.

Syntax nodes wrap immutable green nodes and provide:
- Parent references for tree navigation
- Absolute positions in the source text
- Lazy child node creation

Syntax nodes are created on-demand as you navigate the tree.");
        code.AppendLine("internal abstract class SyntaxNode");
        code.OpenBlock();

        // Fields
        code.AppendLine("private readonly GreenNode _green;");
        code.AppendLine("private readonly SyntaxNode? _parent;");
        code.AppendLine("private readonly int _position;");
        code.AppendLine("private readonly int _slotIndex;");
        code.AppendLine();

        // Constructor
        code.AppendSummary("Creates a new syntax node wrapping the specified green node.");
        code.AppendParam("green", "The underlying green node.");
        code.AppendParam("parent", "The parent syntax node, or null for root.");
        code.AppendParam("position", "The absolute position in the source text.");
        code.AppendParam("slotIndex", "The index of this node within its parent's slots, or -1 for root.");
        code.AppendLine("protected SyntaxNode(GreenNode green, SyntaxNode? parent, int position, int slotIndex = -1)");
        code.OpenBlock();
        code.AppendLine("_green = green;");
        code.AppendLine("_parent = parent;");
        code.AppendLine("_position = position;");
        code.AppendLine("_slotIndex = slotIndex;");
        code.CloseBlock();
        code.AppendLine();

        // Properties
        code.AppendSummary("Gets the underlying green node.");
        code.AppendLine("public GreenNode Green => _green;");
        code.AppendLine();

        code.AppendSummary("Gets the parent node, or null if this is the root.");
        code.AppendLine("public SyntaxNode? Parent => _parent;");
        code.AppendLine();

        code.AppendSummary("Gets the absolute position of this node in the source text.");
        code.AppendLine("public int Position => _position;");
        code.AppendLine();

        code.AppendSummary("Gets the absolute position of the end of this node.");
        code.AppendLine("public int EndPosition => _position + _green.FullWidth;");
        code.AppendLine();

        code.AppendSummary("Gets the width of this node including trivia.");
        code.AppendLine("public int FullWidth => _green.FullWidth;");
        code.AppendLine();

        code.AppendSummary("Gets the width of this node excluding trivia.");
        code.AppendLine("public int Width => _green.Width;");
        code.AppendLine();

        code.AppendSummary("Gets the raw token/node kind.");
        code.AppendLine("public ushort RawKind => _green.RawKind;");
        code.AppendLine();

        code.AppendSummary("Gets the slot index of this node within its parent, or -1 for root.");
        code.AppendLine("public int SlotIndex => _slotIndex;");
        code.AppendLine();

        code.AppendSummary("Gets the span of this node (position and length excluding trivia).");
        code.AppendLine("public TextSpan Span => new TextSpan(_position + _green.LeadingTriviaWidth, _green.Width);");
        code.AppendLine();

        code.AppendSummary("Gets the full span of this node (position and length including trivia).");
        code.AppendLine("public TextSpan FullSpan => new TextSpan(_position, _green.FullWidth);");
        code.AppendLine();

        // Navigation methods
        code.AppendSummary("Gets the root node of the tree.");
        code.AppendLine("public SyntaxNode Root");
        code.OpenBlock();
        code.AppendLine("get");
        code.OpenBlock();
        code.AppendLine("var node = this;");
        code.AppendLine("while (node._parent is not null)");
        code.Indent();
        code.AppendLine("node = node._parent;");
        code.Outdent();
        code.AppendLine("return node;");
        code.CloseBlock();
        code.CloseBlock();
        code.AppendLine();

        code.AppendSummary("Gets all ancestor nodes from this node to the root.");
        code.AppendLine("public IEnumerable<SyntaxNode> Ancestors()");
        code.OpenBlock();
        code.AppendLine("var node = _parent;");
        code.AppendLine("while (node is not null)");
        code.OpenBlock();
        code.AppendLine("yield return node;");
        code.AppendLine("node = node._parent;");
        code.CloseBlock();
        code.CloseBlock();
        code.AppendLine();

        code.AppendSummary("Gets this node and all ancestor nodes.");
        code.AppendLine("public IEnumerable<SyntaxNode> AncestorsAndSelf()");
        code.OpenBlock();
        code.AppendLine("yield return this;");
        code.AppendLine("foreach (var ancestor in Ancestors())");
        code.Indent();
        code.AppendLine("yield return ancestor;");
        code.Outdent();
        code.CloseBlock();
        code.AppendLine();

        // Sibling navigation
        code.AppendSummary("Gets the next sibling node, or null if this is the last child.");
        code.AppendLine("public SyntaxNode? GetNextSibling()");
        code.OpenBlock();
        code.AppendLine("if (_parent is null || _slotIndex < 0) return null;");
        code.AppendLine();
        code.AppendLine("int nextIndex = _slotIndex + 1;");
        code.AppendLine("if (nextIndex >= _parent._green.SlotCount) return null;");
        code.AppendLine();
        code.AppendLine("var sibling = _parent._green.GetSlot(nextIndex);");
        code.AppendLine("if (sibling is null) return null;");
        code.AppendLine();
        code.AppendLine("int siblingPosition = _position + _green.FullWidth;");
        code.AppendLine("return sibling.CreateRed(_parent, siblingPosition, nextIndex);");
        code.CloseBlock();
        code.AppendLine();

        code.AppendSummary("Gets the previous sibling node, or null if this is the first child.");
        code.AppendLine("public SyntaxNode? GetPreviousSibling()");
        code.OpenBlock();
        code.AppendLine("if (_parent is null || _slotIndex <= 0) return null;");
        code.AppendLine();
        code.AppendLine("int prevIndex = _slotIndex - 1;");
        code.AppendLine("var sibling = _parent._green.GetSlot(prevIndex);");
        code.AppendLine("if (sibling is null) return null;");
        code.AppendLine();
        code.AppendLine("int siblingPosition = _position - sibling.FullWidth;");
        code.AppendLine("return sibling.CreateRed(_parent, siblingPosition, prevIndex);");
        code.CloseBlock();
        code.AppendLine();

        // Text access
        code.AppendSummary("Gets the source text of this node including trivia.");
        code.AppendLine("public string ToFullString() => _green.ToFullString();");
        code.AppendLine();

        code.AppendSummary("Gets the source text of this node excluding trivia.");
        code.AppendLine("public override string ToString() => _green.ToString();");

        code.CloseBlock();
        code.AppendLine();

        // Generate TextSpan struct
        GenerateTextSpanStruct(code);
    }

    private static void GenerateTextSpanStruct(CodeBuilder code)
    {
        code.AppendSummary("Represents a span of text in the source.");
        code.AppendLine("internal readonly struct TextSpan : IEquatable<TextSpan>");
        code.OpenBlock();

        code.AppendSummary("Gets the start position of the span.");
        code.AppendLine("public int Start { get; }");
        code.AppendLine();

        code.AppendSummary("Gets the length of the span.");
        code.AppendLine("public int Length { get; }");
        code.AppendLine();

        code.AppendSummary("Gets the end position of the span (exclusive).");
        code.AppendLine("public int End => Start + Length;");
        code.AppendLine();

        code.AppendSummary("Gets whether the span is empty.");
        code.AppendLine("public bool IsEmpty => Length == 0;");
        code.AppendLine();

        code.AppendSummary("Creates a new text span.");
        code.AppendLine("public TextSpan(int start, int length)");
        code.OpenBlock();
        code.AppendLine("Start = start;");
        code.AppendLine("Length = length;");
        code.CloseBlock();
        code.AppendLine();

        code.AppendSummary("Checks if this span contains the specified position.");
        code.AppendLine("public bool Contains(int position) => position >= Start && position < End;");
        code.AppendLine();

        code.AppendSummary("Checks if this span contains the specified span.");
        code.AppendLine("public bool Contains(TextSpan span) => span.Start >= Start && span.End <= End;");
        code.AppendLine();

        code.AppendSummary("Checks if this span overlaps with the specified span.");
        code.AppendLine("public bool OverlapsWith(TextSpan span) => Start < span.End && span.Start < End;");
        code.AppendLine();

        code.AppendLine("public bool Equals(TextSpan other) => Start == other.Start && Length == other.Length;");
        code.AppendLine("public override bool Equals(object? obj) => obj is TextSpan span && Equals(span);");
        code.AppendLine("public override int GetHashCode() => HashCode.Combine(Start, Length);");
        code.AppendLine("public static bool operator ==(TextSpan left, TextSpan right) => left.Equals(right);");
        code.AppendLine("public static bool operator !=(TextSpan left, TextSpan right) => !left.Equals(right);");
        code.AppendLine("public override string ToString() => $\"[{Start}..{End})\";");

        code.CloseBlock();
    }
}
