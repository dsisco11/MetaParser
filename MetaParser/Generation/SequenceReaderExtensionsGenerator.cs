using MetaParser.Schema;

namespace MetaParser.Generation;

/// <summary>
/// Generates SequenceReader extension methods for lexer operations.
/// </summary>
internal static class SequenceReaderExtensionsGenerator
{
    public static GeneratedFileBuilder Generate(SchemaDefinition schema)
    {
        var file = new GeneratedFileBuilder(schema.Namespace)
            .WithFileName("LexerHelpers.g.cs")
            .AddUsings("System", "System.Buffers", "System.Runtime.CompilerServices");

        var code = file.Code;

        GenerateSequenceReaderExtensions(code);

        return file;
    }

    private static void GenerateSequenceReaderExtensions(CodeBuilder code)
    {
        code.AppendSummary("Extension methods for SequenceReader&lt;char&gt; to support lexer operations.");
        code.AppendLine("internal static class SequenceReaderExtensions");
        code.OpenBlock();

        // TryPeek
        code.AppendSummary("Peeks at the character at the specified offset without advancing.");
        code.AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        code.AppendLine("public static bool TryPeek(this ref SequenceReader<char> reader, int offset, out char value)");
        code.OpenBlock();
        code.AppendLine("if (reader.Remaining <= offset)");
        code.OpenBlock();
        code.AppendLine("value = default;");
        code.AppendLine("return false;");
        code.CloseBlock();
        code.AppendLine("var copy = reader;");
        code.AppendLine("copy.Advance(offset);");
        code.AppendLine("return copy.TryPeek(out value);");
        code.CloseBlock();
        code.AppendLine();

        // TryReadExact
        code.AppendSummary("Attempts to match and consume an exact string.");
        code.AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        code.AppendLine("public static bool TryReadExact(this ref SequenceReader<char> reader, ReadOnlySpan<char> expected)");
        code.OpenBlock();
        code.AppendLine("if (reader.Remaining < expected.Length)");
        code.Indent();
        code.AppendLine("return false;");
        code.Outdent();
        code.AppendLine();
        code.AppendLine("var copy = reader;");
        code.AppendLine("Span<char> buffer = stackalloc char[expected.Length];");
        code.AppendLine("if (!copy.TryCopyTo(buffer))");
        code.Indent();
        code.AppendLine("return false;");
        code.Outdent();
        code.AppendLine();
        code.AppendLine("if (!buffer.SequenceEqual(expected))");
        code.Indent();
        code.AppendLine("return false;");
        code.Outdent();
        code.AppendLine();
        code.AppendLine("reader.Advance(expected.Length);");
        code.AppendLine("return true;");
        code.CloseBlock();
        code.AppendLine();

        // TryReadExact with string
        code.AppendSummary("Attempts to match and consume an exact string.");
        code.AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        code.AppendLine("public static bool TryReadExact(this ref SequenceReader<char> reader, string expected)");
        code.OpenBlock();
        code.AppendLine("return TryReadExact(ref reader, expected.AsSpan());");
        code.CloseBlock();
        code.AppendLine();

        // IsNext
        code.AppendSummary("Checks if the next characters match without consuming.");
        code.AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        code.AppendLine("public static bool IsNext(this ref SequenceReader<char> reader, ReadOnlySpan<char> expected)");
        code.OpenBlock();
        code.AppendLine("if (reader.Remaining < expected.Length)");
        code.Indent();
        code.AppendLine("return false;");
        code.Outdent();
        code.AppendLine();
        code.AppendLine("Span<char> buffer = stackalloc char[expected.Length];");
        code.AppendLine("if (!reader.TryCopyTo(buffer))");
        code.Indent();
        code.AppendLine("return false;");
        code.Outdent();
        code.AppendLine();
        code.AppendLine("return buffer.SequenceEqual(expected);");
        code.CloseBlock();
        code.AppendLine();

        // AdvanceWhile with predicate
        code.AppendSummary("Advances the reader while the predicate is true, returning number of characters consumed.");
        code.AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        code.AppendLine("public static int AdvanceWhile(this ref SequenceReader<char> reader, Func<char, bool> predicate)");
        code.OpenBlock();
        code.AppendLine("int count = 0;");
        code.AppendLine("while (reader.TryPeek(out char c) && predicate(c))");
        code.OpenBlock();
        code.AppendLine("reader.Advance(1);");
        code.AppendLine("count++;");
        code.CloseBlock();
        code.AppendLine("return count;");
        code.CloseBlock();
        code.AppendLine();

        // AdvanceWhileInRange
        code.AppendSummary("Advances while characters are in the specified range [start, end].");
        code.AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        code.AppendLine("public static int AdvanceWhileInRange(this ref SequenceReader<char> reader, char start, char end)");
        code.OpenBlock();
        code.AppendLine("int count = 0;");
        code.AppendLine("while (reader.TryPeek(out char c) && c >= start && c <= end)");
        code.OpenBlock();
        code.AppendLine("reader.Advance(1);");
        code.AppendLine("count++;");
        code.CloseBlock();
        code.AppendLine("return count;");
        code.CloseBlock();
        code.AppendLine();

        // AdvanceWhileAnyOf with SearchValues
        code.AppendSummary("Advances while characters are in the SearchValues set (SIMD-optimized).");
        code.AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        code.AppendLine("public static int AdvanceWhileAnyOf(this ref SequenceReader<char> reader, SearchValues<char> values)");
        code.OpenBlock();
        code.AppendLine("int count = 0;");
        code.AppendLine("while (reader.TryPeek(out char c) && values.Contains(c))");
        code.OpenBlock();
        code.AppendLine("reader.Advance(1);");
        code.AppendLine("count++;");
        code.CloseBlock();
        code.AppendLine("return count;");
        code.CloseBlock();
        code.AppendLine();

        // AdvanceUntil
        code.AppendSummary("Advances until the terminator character is found.");
        code.AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        code.AppendLine("public static int AdvanceUntil(this ref SequenceReader<char> reader, char terminator)");
        code.OpenBlock();
        code.AppendLine("int count = 0;");
        code.AppendLine("while (reader.TryPeek(out char c) && c != terminator)");
        code.OpenBlock();
        code.AppendLine("reader.Advance(1);");
        code.AppendLine("count++;");
        code.CloseBlock();
        code.AppendLine("return count;");
        code.CloseBlock();
        code.AppendLine();

        // AdvanceUntilAny
        code.AppendSummary("Advances until any of the terminator characters is found.");
        code.AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        code.AppendLine("public static int AdvanceUntilAny(this ref SequenceReader<char> reader, SearchValues<char> terminators)");
        code.OpenBlock();
        code.AppendLine("int count = 0;");
        code.AppendLine("while (reader.TryPeek(out char c) && !terminators.Contains(c))");
        code.OpenBlock();
        code.AppendLine("reader.Advance(1);");
        code.AppendLine("count++;");
        code.CloseBlock();
        code.AppendLine("return count;");
        code.CloseBlock();
        code.AppendLine();

        // IsWhitespace (excluding newlines)
        code.AppendSummary("Returns true if the character is whitespace (excluding newlines).");
        code.AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        code.AppendLine("public static bool IsWhitespace(char c) => c == ' ' || c == '\\t' || c == '\\v' || c == '\\f';");
        code.AppendLine();

        // IsNewline
        code.AppendSummary("Returns true if the character starts a newline sequence.");
        code.AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        code.AppendLine("public static bool IsNewline(char c) => c == '\\n' || c == '\\r';");
        code.AppendLine();

        // AdvanceWhitespace
        code.AppendSummary("Advances past whitespace characters (excluding newlines).");
        code.AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        code.AppendLine("public static int AdvanceWhitespace(this ref SequenceReader<char> reader)");
        code.OpenBlock();
        code.AppendLine("int count = 0;");
        code.AppendLine("while (reader.TryPeek(out char c) && IsWhitespace(c))");
        code.OpenBlock();
        code.AppendLine("reader.Advance(1);");
        code.AppendLine("count++;");
        code.CloseBlock();
        code.AppendLine("return count;");
        code.CloseBlock();
        code.AppendLine();

        // AdvanceNewline
        code.AppendSummary("Advances past a single newline sequence (\\r\\n, \\n, or \\r).");
        code.AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        code.AppendLine("public static int AdvanceNewline(this ref SequenceReader<char> reader)");
        code.OpenBlock();
        code.AppendLine("if (!reader.TryPeek(out char c))");
        code.Indent();
        code.AppendLine("return 0;");
        code.Outdent();
        code.AppendLine();
        code.AppendLine("if (c == '\\r')");
        code.OpenBlock();
        code.AppendLine("reader.Advance(1);");
        code.AppendLine("if (reader.TryPeek(out char next) && next == '\\n')");
        code.OpenBlock();
        code.AppendLine("reader.Advance(1);");
        code.AppendLine("return 2;");
        code.CloseBlock();
        code.AppendLine("return 1;");
        code.CloseBlock();
        code.AppendLine();
        code.AppendLine("if (c == '\\n')");
        code.OpenBlock();
        code.AppendLine("reader.Advance(1);");
        code.AppendLine("return 1;");
        code.CloseBlock();
        code.AppendLine();
        code.AppendLine("return 0;");
        code.CloseBlock();
        code.AppendLine();

        // IsIdentifierStart
        code.AppendSummary("Returns true if the character can start an identifier.");
        code.AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        code.AppendLine("public static bool IsIdentifierStart(char c) => char.IsLetter(c) || c == '_';");
        code.AppendLine();

        // IsIdentifierPart
        code.AppendSummary("Returns true if the character can be part of an identifier.");
        code.AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        code.AppendLine("public static bool IsIdentifierPart(char c) => char.IsLetterOrDigit(c) || c == '_';");
        code.AppendLine();

        // AdvanceIdentifier
        code.AppendSummary("Advances past an identifier.");
        code.AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        code.AppendLine("public static int AdvanceIdentifier(this ref SequenceReader<char> reader)");
        code.OpenBlock();
        code.AppendLine("if (!reader.TryPeek(out char c) || !IsIdentifierStart(c))");
        code.Indent();
        code.AppendLine("return 0;");
        code.Outdent();
        code.AppendLine();
        code.AppendLine("reader.Advance(1);");
        code.AppendLine("int count = 1;");
        code.AppendLine("while (reader.TryPeek(out c) && IsIdentifierPart(c))");
        code.OpenBlock();
        code.AppendLine("reader.Advance(1);");
        code.AppendLine("count++;");
        code.CloseBlock();
        code.AppendLine("return count;");
        code.CloseBlock();
        code.AppendLine();

        // AdvanceDigits
        code.AppendSummary("Advances past decimal digits.");
        code.AppendLine("private static readonly SearchValues<char> s_digits = SearchValues.Create(\"0123456789\");");
        code.AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        code.AppendLine("public static int AdvanceDigits(this ref SequenceReader<char> reader)");
        code.OpenBlock();
        code.AppendLine("return reader.AdvanceWhileAnyOf(s_digits);");
        code.CloseBlock();
        code.AppendLine();

        // AdvanceHexDigits
        code.AppendSummary("Advances past hexadecimal digits.");
        code.AppendLine("private static readonly SearchValues<char> s_hexDigits = SearchValues.Create(\"0123456789abcdefABCDEF\");");
        code.AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        code.AppendLine("public static int AdvanceHexDigits(this ref SequenceReader<char> reader)");
        code.OpenBlock();
        code.AppendLine("return reader.AdvanceWhileAnyOf(s_hexDigits);");
        code.CloseBlock();

        code.CloseBlock();
    }
}
