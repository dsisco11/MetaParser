using MetaParser.Schema;
using System.Linq;

namespace MetaParser.Generation;

/// <summary>
/// Generates the TokenKind enum from the schema definition.
/// </summary>
internal static class TokenKindGenerator
{
    public static GeneratedFileBuilder Generate(SchemaDefinition schema)
    {
        var file = new GeneratedFileBuilder(schema.Namespace)
            .WithFileName("TokenKind.g.cs");

        var code = file.Code;

        GenerateTokenKindEnum(code, schema);

        return file;
    }

    private static void GenerateTokenKindEnum(CodeBuilder code, SchemaDefinition schema)
    {
        code.AppendSummary("Token kinds for the generated lexer.");
        code.AppendLine("internal enum TokenKind : ushort");
        code.OpenBlock();

        // Always include special tokens first
        code.AppendSummaryLine("End of file marker.");
        code.AppendLine("EndOfFile = 0,");
        code.AppendLine();

        code.AppendSummaryLine("Invalid/unknown token.");
        code.AppendLine("Bad = 1,");
        code.AppendLine();

        code.AppendSummaryLine("Whitespace trivia.");
        code.AppendLine("Whitespace = 2,");
        code.AppendLine();

        code.AppendSummaryLine("End of line trivia.");
        code.AppendLine("EndOfLine = 3,");
        code.AppendLine();

        // Generate token kinds from schema (starting at 10 to leave room for built-ins)
        ushort kindValue = 10;
        foreach (var kvp in schema.Tokens)
        {
            var name = kvp.Key;
            var def = kvp.Value;

            // Generate summary from definition
            var patternDesc = GetPatternDescription(def.Start ?? def.Consume);
            if (patternDesc is not null)
            {
                code.AppendSummaryLine($"Token: {EscapeForXml(patternDesc)}");
            }

            code.AppendLine($"{SanitizeIdentifier(name)} = {kindValue},");
            code.AppendLine();
            kindValue++;
        }

        code.CloseBlock();
        code.AppendLine();

        // Generate helper extension class
        GenerateTokenKindExtensions(code, schema);
    }

    private static void GenerateTokenKindExtensions(CodeBuilder code, SchemaDefinition schema)
    {
        code.AppendSummary("Extension methods for TokenKind.");
        code.AppendLine("internal static class TokenKindExtensions");
        code.OpenBlock();

        // IsTrivia method
        code.AppendSummaryLine("Returns true if this token kind is trivia.");
        code.AppendLine("public static bool IsTrivia(this TokenKind kind)");
        code.OpenBlock();
        code.AppendLine("return kind switch");
        code.OpenBlock();
        code.AppendLine("TokenKind.Whitespace => true,");
        code.AppendLine("TokenKind.EndOfLine => true,");

        // Add user-defined trivia
        foreach (var triviaName in schema.Trivia)
        {
            code.AppendLine($"TokenKind.{SanitizeIdentifier(triviaName)} => true,");
        }

        code.AppendLine("_ => false");
        code.CloseBlock(";");
        code.CloseBlock();
        code.AppendLine();

        // GetText method for fixed tokens
        code.AppendSummaryLine("Returns the fixed text for tokens with known text, or null for variable tokens.");
        code.AppendLine("public static string? GetText(this TokenKind kind)");
        code.OpenBlock();
        code.AppendLine("return kind switch");
        code.OpenBlock();

        foreach (var kvp in schema.Tokens)
        {
            var name = kvp.Key;
            var def = kvp.Value;

            // Only include tokens with fixed text (literal Start and no Consume pattern that varies)
            var fixedText = GetFixedText(def);
            if (fixedText is not null)
            {
                code.AppendLine($"TokenKind.{SanitizeIdentifier(name)} => \"{EscapeForString(fixedText)}\",");
            }
        }

        code.AppendLine("_ => null");
        code.CloseBlock(";");
        code.CloseBlock();

        code.CloseBlock();
    }

    /// <summary>
    /// Gets a human-readable description of a pattern for XML docs.
    /// </summary>
    private static string? GetPatternDescription(PatternDefinition? pattern)
    {
        return pattern switch
        {
            LiteralPattern lit => lit.Value,
            RangePattern range => $"[{range.Start}-{range.End}]",
            TokenReferencePattern refPat => $"${refPat.TokenName}",
            OneOfPattern oneOf => string.Join(" | ", oneOf.Patterns.Select(GetPatternDescription).Where(p => p is not null)),
            _ => null
        };
    }

    /// <summary>
    /// Gets the fixed text for a token if it's a simple literal without consume/stop patterns.
    /// </summary>
    private static string? GetFixedText(TokenDefinition def)
    {
        // Token must have only a literal Start and no Consume/Stop patterns
        if (def.Start is LiteralPattern literal && def.Consume is null && def.Stop is null)
        {
            return literal.Value;
        }
        return null;
    }

    private static string SanitizeIdentifier(string name)
    {
        // Convert to PascalCase and remove invalid characters
        if (string.IsNullOrEmpty(name))
            return "Unknown";

        var result = new System.Text.StringBuilder();
        bool capitalizeNext = true;

        foreach (var c in name)
        {
            if (char.IsLetterOrDigit(c))
            {
                result.Append(capitalizeNext ? char.ToUpperInvariant(c) : c);
                capitalizeNext = false;
            }
            else if (c == '_' || c == '-' || c == ' ')
            {
                capitalizeNext = true;
            }
        }

        // Ensure it starts with a letter
        if (result.Length > 0 && char.IsDigit(result[0]))
        {
            result.Insert(0, '_');
        }

        return result.Length > 0 ? result.ToString() : "Unknown";
    }

    private static string EscapeForXml(string text)
    {
        return text
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;");
    }

    private static string EscapeForString(string text)
    {
        return text
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\r", "\\r")
            .Replace("\n", "\\n")
            .Replace("\t", "\\t");
    }
}
