using System;
using System.Text;

namespace MetaParser.Generation;

/// <summary>
/// A StringBuilder wrapper that handles indentation for code generation.
/// </summary>
internal sealed class CodeBuilder
{
    private readonly StringBuilder _sb;
    private int _indentLevel;
    private readonly string _indentString;
    private bool _needsIndent;

    public CodeBuilder(string indentString = "    ")
    {
        _sb = new StringBuilder();
        _indentLevel = 0;
        _indentString = indentString;
        _needsIndent = true;
    }

    /// <summary>
    /// Gets the current indent level.
    /// </summary>
    public int IndentLevel => _indentLevel;

    /// <summary>
    /// Increases the indent level.
    /// </summary>
    public CodeBuilder Indent()
    {
        _indentLevel++;
        return this;
    }

    /// <summary>
    /// Decreases the indent level.
    /// </summary>
    public CodeBuilder Outdent()
    {
        if (_indentLevel > 0)
            _indentLevel--;
        return this;
    }

    /// <summary>
    /// Appends text without a newline.
    /// </summary>
    public CodeBuilder Append(string text)
    {
        WriteIndentIfNeeded();
        _sb.Append(text);
        return this;
    }

    /// <summary>
    /// Appends text followed by a newline.
    /// </summary>
    public CodeBuilder AppendLine(string text)
    {
        WriteIndentIfNeeded();
        _sb.AppendLine(text);
        _needsIndent = true;
        return this;
    }

    /// <summary>
    /// Appends an empty line.
    /// </summary>
    public CodeBuilder AppendLine()
    {
        _sb.AppendLine();
        _needsIndent = true;
        return this;
    }

    /// <summary>
    /// Opens a block with '{' and increases indent.
    /// </summary>
    public CodeBuilder OpenBlock()
    {
        AppendLine("{");
        Indent();
        return this;
    }

    /// <summary>
    /// Closes a block with '}' and decreases indent.
    /// </summary>
    public CodeBuilder CloseBlock()
    {
        Outdent();
        AppendLine("}");
        return this;
    }

    /// <summary>
    /// Closes a block with '}' followed by additional text.
    /// </summary>
    public CodeBuilder CloseBlock(string suffix)
    {
        Outdent();
        AppendLine("}" + suffix);
        return this;
    }

    /// <summary>
    /// Appends a summary XML doc comment.
    /// </summary>
    public CodeBuilder AppendSummary(string summary)
    {
        AppendLine("/// <summary>");
        foreach (var line in summary.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None))
        {
            AppendLine($"/// {EscapeXml(line)}");
        }
        AppendLine("/// </summary>");
        return this;
    }

    /// <summary>
    /// Appends a single-line summary XML doc comment.
    /// </summary>
    public CodeBuilder AppendSummaryLine(string summary)
    {
        AppendLine($"/// <summary>{EscapeXml(summary)}</summary>");
        return this;
    }

    /// <summary>
    /// Appends a param XML doc comment.
    /// </summary>
    public CodeBuilder AppendParam(string name, string description)
    {
        AppendLine($"/// <param name=\"{name}\">{EscapeXml(description)}</param>");
        return this;
    }

    /// <summary>
    /// Appends a returns XML doc comment.
    /// </summary>
    public CodeBuilder AppendReturns(string description)
    {
        AppendLine($"/// <returns>{EscapeXml(description)}</returns>");
        return this;
    }

    /// <summary>
    /// Appends a region directive.
    /// </summary>
    public CodeBuilder AppendRegion(string name)
    {
        AppendLine($"#region {name}");
        AppendLine();
        return this;
    }

    /// <summary>
    /// Appends an endregion directive.
    /// </summary>
    public CodeBuilder AppendEndRegion()
    {
        AppendLine();
        AppendLine("#endregion");
        return this;
    }

    /// <summary>
    /// Executes an action within an indented block.
    /// </summary>
    public CodeBuilder WithBlock(Action<CodeBuilder> action)
    {
        OpenBlock();
        action(this);
        CloseBlock();
        return this;
    }

    /// <summary>
    /// Executes an action within an indented block, with custom closing.
    /// </summary>
    public CodeBuilder WithBlock(Action<CodeBuilder> action, string closingSuffix)
    {
        OpenBlock();
        action(this);
        CloseBlock(closingSuffix);
        return this;
    }

    private void WriteIndentIfNeeded()
    {
        if (_needsIndent)
        {
            for (int i = 0; i < _indentLevel; i++)
            {
                _sb.Append(_indentString);
            }
            _needsIndent = false;
        }
    }

    private static string EscapeXml(string text)
    {
        return text
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;");
    }

    public override string ToString() => _sb.ToString();

    /// <summary>
    /// Gets the length of the generated code.
    /// </summary>
    public int Length => _sb.Length;

    /// <summary>
    /// Clears the builder.
    /// </summary>
    public CodeBuilder Clear()
    {
        _sb.Clear();
        _indentLevel = 0;
        _needsIndent = true;
        return this;
    }
}
