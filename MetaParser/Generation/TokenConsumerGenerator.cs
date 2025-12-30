using MetaParser.Schema;
using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Generation;

/// <summary>
/// Generates consumer classes for each token type using SequenceReader&lt;char&gt;.
/// </summary>
internal static class TokenConsumerGenerator
{
    public static GeneratedFileBuilder Generate(SchemaDefinition schema)
    {
        var file = new GeneratedFileBuilder(schema.Namespace)
            .WithFileName("TokenConsumers.g.cs")
            .AddUsings("System", "System.Buffers", "System.Runtime.CompilerServices");

        var code = file.Code;

        // Generate built-in consumers first
        GenerateWhitespaceConsumer(code);
        code.AppendLine();
        GenerateEndOfLineConsumer(code);
        code.AppendLine();

        // Generate consumer for each token in schema
        int priority = 10;
        foreach (var kvp in schema.Tokens)
        {
            var name = kvp.Key;
            var def = kvp.Value;
            var isTrivia = schema.Trivia.Contains(name);

            GenerateTokenConsumer(code, name, def, priority++, isTrivia);
            code.AppendLine();
        }

        return file;
    }

    private static void GenerateWhitespaceConsumer(CodeBuilder code)
    {
        code.AppendSummary("Consumer for whitespace (spaces, tabs, etc. but not newlines).");
        code.AppendLine("internal sealed class WhitespaceConsumer : ITokenConsumer");
        code.OpenBlock();
        code.AppendLine("public static readonly WhitespaceConsumer Instance = new();");
        code.AppendLine("public int Priority => 1;");
        code.AppendLine("public bool IsTrivia => true;");
        code.AppendLine();
        code.AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        code.AppendLine("public LexerResult TryConsume(ref SequenceReader<char> reader)");
        code.OpenBlock();
        code.AppendLine("int count = reader.AdvanceWhitespace();");
        code.AppendLine("return count > 0 ? new LexerResult(TokenKind.Whitespace, count) : LexerResult.Fail;");
        code.CloseBlock();
        code.CloseBlock();
    }

    private static void GenerateEndOfLineConsumer(CodeBuilder code)
    {
        code.AppendSummary("Consumer for end-of-line sequences.");
        code.AppendLine("internal sealed class EndOfLineConsumer : ITokenConsumer");
        code.OpenBlock();
        code.AppendLine("public static readonly EndOfLineConsumer Instance = new();");
        code.AppendLine("public int Priority => 2;");
        code.AppendLine("public bool IsTrivia => true;");
        code.AppendLine();
        code.AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        code.AppendLine("public LexerResult TryConsume(ref SequenceReader<char> reader)");
        code.OpenBlock();
        code.AppendLine("int count = reader.AdvanceNewline();");
        code.AppendLine("return count > 0 ? new LexerResult(TokenKind.EndOfLine, count) : LexerResult.Fail;");
        code.CloseBlock();
        code.CloseBlock();
    }

    private static void GenerateTokenConsumer(CodeBuilder code, string name, TokenDefinition def, int priority, bool isTrivia)
    {
        var className = SanitizeIdentifier(name) + "Consumer";
        var kindName = SanitizeIdentifier(name);

        code.AppendSummary($"Consumer for {name} tokens.");
        code.AppendLine($"internal sealed class {className} : ITokenConsumer");
        code.OpenBlock();

        code.AppendLine($"public static readonly {className} Instance = new();");
        code.AppendLine($"public int Priority => {priority};");
        code.AppendLine($"public bool IsTrivia => {(isTrivia ? "true" : "false")};");
        code.AppendLine();

        // Generate SearchValues fields if needed
        GenerateSearchValuesFields(code, def);

        code.AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        code.AppendLine("public LexerResult TryConsume(ref SequenceReader<char> reader)");
        code.OpenBlock();

        // Generate the consumption logic
        GenerateConsumptionLogic(code, name, def, kindName);

        code.CloseBlock();
        code.CloseBlock();
    }

    private static void GenerateSearchValuesFields(CodeBuilder code, TokenDefinition def)
    {
        // Collect SearchValues needed for OneOf patterns with single-char literals
        var searchValuesPatterns = new List<(string name, string chars)>();
        
        if (def.Consume is OneOfPattern oneOf)
        {
            var allSingleChars = oneOf.Patterns.All(p => p is LiteralPattern lp && lp.Value.Length == 1);
            if (allSingleChars)
            {
                var chars = string.Concat(oneOf.Patterns.OfType<LiteralPattern>().Select(p => p.Value));
                searchValuesPatterns.Add(("s_consume", chars));
            }
        }

        foreach (var (fieldName, chars) in searchValuesPatterns)
        {
            code.AppendLine($"private static readonly SearchValues<char> {fieldName} = SearchValues.Create(\"{EscapeForString(chars)}\");");
        }

        if (searchValuesPatterns.Count > 0)
        {
            code.AppendLine();
        }
    }

    private static void GenerateConsumptionLogic(CodeBuilder code, string name, TokenDefinition def, string kindName)
    {
        code.AppendLine("if (reader.End) return LexerResult.Fail;");
        code.AppendLine();
        code.AppendLine("long startConsumed = reader.Consumed;");
        code.AppendLine();

        // Handle different token patterns
        if (def.Start is LiteralPattern startLiteral && def.Consume is null)
        {
            // Simple fixed token (keyword, operator)
            GenerateFixedTokenLogic(code, startLiteral, kindName);
        }
        else if (def.Start is not null && def.Consume is not null)
        {
            // Token with start + consume patterns (like identifier)
            GenerateStartConsumeLogic(code, def, kindName);
        }
        else if (def.Consume is not null)
        {
            // Token with only consume pattern
            GenerateConsumeOnlyLogic(code, def, kindName);
        }
        else
        {
            // Fallback - shouldn't happen with valid schema
            code.AppendLine("return LexerResult.Fail;");
        }
    }

    private static void GenerateFixedTokenLogic(CodeBuilder code, LiteralPattern literal, string kindName)
    {
        code.AppendLine($"if (reader.TryReadExact(\"{EscapeForString(literal.Value)}\"))");
        code.OpenBlock();
        code.AppendLine($"return new LexerResult(TokenKind.{kindName}, {literal.Value.Length});");
        code.CloseBlock();
        code.AppendLine("return LexerResult.Fail;");
    }

    private static void GenerateStartConsumeLogic(CodeBuilder code, TokenDefinition def, string kindName)
    {
        // Check start pattern
        GenerateStartPatternCheck(code, def.Start!);
        code.AppendLine();

        // Consume additional characters
        GenerateConsumePattern(code, def.Consume!);
        code.AppendLine();

        code.AppendLine("int length = (int)(reader.Consumed - startConsumed);");
        code.AppendLine($"return new LexerResult(TokenKind.{kindName}, length);");
    }

    private static void GenerateConsumeOnlyLogic(CodeBuilder code, TokenDefinition def, string kindName)
    {
        GenerateConsumePattern(code, def.Consume!);
        code.AppendLine();
        code.AppendLine("int length = (int)(reader.Consumed - startConsumed);");
        code.AppendLine("if (length == 0)");
        code.Indent();
        code.AppendLine("return LexerResult.Fail;");
        code.Outdent();
        code.AppendLine($"return new LexerResult(TokenKind.{kindName}, length);");
    }

    private static void GenerateStartPatternCheck(CodeBuilder code, PatternDefinition start)
    {
        switch (start)
        {
            case LiteralPattern literal:
                code.AppendLine($"if (!reader.TryReadExact(\"{EscapeForString(literal.Value)}\"))");
                code.Indent();
                code.AppendLine("return LexerResult.Fail;");
                code.Outdent();
                break;

            case RangePattern range:
                code.AppendLine("if (!reader.TryPeek(out char startChar))");
                code.Indent();
                code.AppendLine("return LexerResult.Fail;");
                code.Outdent();
                code.AppendLine($"if (startChar < '{EscapeChar(range.Start)}' || startChar > '{EscapeChar(range.End)}')");
                code.Indent();
                code.AppendLine("return LexerResult.Fail;");
                code.Outdent();
                code.AppendLine("reader.Advance(1);");
                break;

            case OneOfPattern oneOf:
                GenerateOneOfStartCheck(code, oneOf);
                break;
        }
    }

    private static void GenerateOneOfStartCheck(CodeBuilder code, OneOfPattern oneOf)
    {
        code.AppendLine("if (!reader.TryPeek(out char startChar))");
        code.Indent();
        code.AppendLine("return LexerResult.Fail;");
        code.Outdent();
        code.AppendLine();

        // Build condition
        var conditions = new List<string>();
        foreach (var pattern in oneOf.Patterns)
        {
            if (pattern is RangePattern rp)
            {
                conditions.Add($"(startChar >= '{EscapeChar(rp.Start)}' && startChar <= '{EscapeChar(rp.End)}')");
            }
            else if (pattern is LiteralPattern lp && lp.Value.Length == 1)
            {
                conditions.Add($"startChar == '{EscapeChar(lp.Value[0])}'");
            }
        }

        if (conditions.Count > 0)
        {
            code.AppendLine($"if (!({string.Join(" || ", conditions)}))");
            code.Indent();
            code.AppendLine("return LexerResult.Fail;");
            code.Outdent();
            code.AppendLine("reader.Advance(1);");
        }
    }

    private static void GenerateConsumePattern(CodeBuilder code, PatternDefinition consume)
    {
        switch (consume)
        {
            case LiteralPattern literal:
                code.AppendLine($"reader.TryReadExact(\"{EscapeForString(literal.Value)}\");");
                break;

            case RangePattern range:
                code.AppendLine($"reader.AdvanceWhileInRange('{EscapeChar(range.Start)}', '{EscapeChar(range.End)}');");
                break;

            case OneOfPattern oneOf:
                GenerateOneOfConsume(code, oneOf);
                break;
        }
    }

    private static void GenerateOneOfConsume(CodeBuilder code, OneOfPattern oneOf)
    {
        var allSingleChars = oneOf.Patterns.All(p => p is LiteralPattern lp && lp.Value.Length == 1);
        var hasRanges = oneOf.Patterns.Any(p => p is RangePattern);
        
        if (allSingleChars)
        {
            code.AppendLine("reader.AdvanceWhileAnyOf(s_consume);");
        }
        else if (hasRanges)
        {
            // Build inline condition for the loop
            var conditions = new List<string>();
            foreach (var pattern in oneOf.Patterns)
            {
                if (pattern is RangePattern rp)
                {
                    conditions.Add($"(ch >= '{EscapeChar(rp.Start)}' && ch <= '{EscapeChar(rp.End)}')");
                }
                else if (pattern is LiteralPattern lp && lp.Value.Length == 1)
                {
                    conditions.Add($"ch == '{EscapeChar(lp.Value[0])}'");
                }
            }
            
            code.AppendLine("while (reader.TryPeek(out char ch))");
            code.OpenBlock();
            code.AppendLine($"if (!({string.Join(" || ", conditions)}))");
            code.Indent();
            code.AppendLine("break;");
            code.Outdent();
            code.AppendLine("reader.Advance(1);");
            code.CloseBlock();
        }
        else
        {
            // Multi-char literals (rare case)
            code.AppendLine("while (!reader.End)");
            code.OpenBlock();
            code.AppendLine("bool consumed = false;");
            foreach (var pattern in oneOf.Patterns)
            {
                if (pattern is LiteralPattern lp)
                {
                    code.AppendLine($"if (!consumed && reader.TryReadExact(\"{EscapeForString(lp.Value)}\")) consumed = true;");
                }
            }
            code.AppendLine("if (!consumed) break;");
            code.CloseBlock();
        }
    }

    private static string SanitizeIdentifier(string name)
    {
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

        if (result.Length > 0 && char.IsDigit(result[0]))
        {
            result.Insert(0, '_');
        }

        return result.Length > 0 ? result.ToString() : "Unknown";
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

    private static string EscapeChar(char c)
    {
        return c switch
        {
            '\'' => "\\'",
            '\\' => "\\\\",
            '\r' => "\\r",
            '\n' => "\\n",
            '\t' => "\\t",
            _ => c.ToString()
        };
    }
}
