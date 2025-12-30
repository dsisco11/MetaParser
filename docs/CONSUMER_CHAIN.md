# MetaParser v2 - Consumer Chain Infrastructure

> **Document Created:** December 30, 2025  
> **Status:** Draft

---

## Table of Contents

1. [Overview](#overview)
2. [Design Goals](#design-goals)
3. [Core Interfaces](#core-interfaces)
4. [Input Handling](#input-handling)
5. [Consumer Base Classes](#consumer-base-classes)
6. [Token Priority](#token-priority)
7. [Consumer Chain Orchestration](#consumer-chain-orchestration)
8. [Trivia Handling](#trivia-handling)
9. [Error Handling](#error-handling)
10. [Generated Consumer Structure](#generated-consumer-structure)
11. [Performance Considerations](#performance-considerations)

---

## Overview

The consumer chain is the core parsing infrastructure that transforms input text into a green token tree. Each level in the chain consumes output from the level below:

```
Level 0: char[]        → GreenToken[]   (Lexer consumers)
Level 1: GreenToken[]  → GreenToken[]   (Parser consumers)
Level N: GreenToken[]  → GreenToken[]   (Higher-level constructs)
```

### Key Principles

- **Layered Processing** - Each level only sees tokens from the previous level
- **Priority-Based Selection** - Earlier-defined tokens have higher priority
- **Greedy Matching** - Consumers match as much input as possible
- **Immutable Output** - Consumers produce immutable green tokens
- **Trivia Attachment** - Whitespace/comments handled according to Roslyn rules

---

## Design Goals

1. **Deterministic Parsing** - Same input always produces same output
2. **Zero-Copy Where Possible** - Use `ReadOnlySpan<char>` and slices
3. **Streaming Support** - Handle large files without loading entirely into memory
4. **Generated Code Simplicity** - Easy to generate clean consumer implementations
5. **Debuggability** - Clear execution flow, easy to trace token matching
6. **SIMD Optimization** - Leverage `SearchValues<T>` and vectorized span methods

---

## Core Interfaces

### IConsumer<TInput>

The base interface for all consumers:

```csharp
namespace MetaParser.Parsing;

using System.Buffers;

/// <summary>
/// Base interface for token consumers at any level.
/// </summary>
/// <typeparam name="TInput">The input type (char for Level 0, GreenToken for Level 1+)</typeparam>
public interface IConsumer<TInput>
{
    /// <summary>
    /// The priority of this consumer (lower = higher priority).
    /// Determined by definition order in schema.
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// The token kind this consumer produces.
    /// </summary>
    ushort TokenKind { get; }

    /// <summary>
    /// Attempts to consume input and produce a green token.
    /// </summary>
    /// <param name="reader">The input reader (advanced on success)</param>
    /// <param name="token">The produced token (if successful)</param>
    /// <returns>True if consumption succeeded</returns>
    bool TryConsume(ref SequenceReader<TInput> reader, out GreenToken token);

    /// <summary>
    /// Quick check if this consumer could potentially match at current position.
    /// Used for fast filtering before full matching attempt.
    /// </summary>
    bool CanStartWith(TInput item);
}
```

### ILexerConsumer

Specialized interface for Level 0 (character) consumers:

```csharp
/// <summary>
/// Level 0 consumer: consumes characters, produces green tokens.
/// </summary>
public interface ILexerConsumer : IConsumer<char>
{
    /// <summary>
    /// Whether this consumer produces trivia tokens.
    /// </summary>
    bool IsTrivia { get; }
}
```

### IParserConsumer

Specialized interface for Level 1+ (token) consumers:

```csharp
/// <summary>
/// Level 1+ consumer: consumes green tokens, produces green tokens.
/// </summary>
public interface IParserConsumer : IConsumer<GreenToken>
{
    /// <summary>
    /// The token kinds this consumer can start with.
    /// Used for quick filtering.
    /// </summary>
    ReadOnlySpan<ushort> StarterKinds { get; }
}
```

---

## Input Handling

### Using SequenceReader<T>

We use `System.Buffers.SequenceReader<T>` for efficient input traversal. This provides:

- Built-in support for `ReadOnlySequence<T>` (enables streaming/chunked input)
- Optimized peek, advance, and read operations
- Position tracking via `SequencePosition`

```csharp
using System.Buffers;

// For contiguous input (most common case)
var sequence = new ReadOnlySequence<char>(inputMemory);
var reader = new SequenceReader<char>(sequence);

// For chunked/streaming input
var sequence = BuildChunkedSequence(chunks);
var reader = new SequenceReader<char>(sequence);
```

### SequenceReader<char> Key Operations

```csharp
// Peek without advancing
reader.TryPeek(out char c);
reader.TryPeek(offset, out char c);

// Read and advance
reader.TryRead(out char c);
reader.Advance(count);

// Position tracking
var position = reader.Position;
reader.Rewind(count);  // Go back

// Span access (for contiguous segments)
reader.UnreadSpan  // Remaining unread data in current segment
reader.Remaining   // Total remaining count
```

### SequenceReaderExtensions for Lexing

SIMD-optimized extensions using `SearchValues<char>`:

```csharp
using System.Buffers;

namespace MetaParser.Parsing;

/// <summary>
/// SIMD-optimized extensions for SequenceReader lexing operations.
/// </summary>
public static class SequenceReaderExtensions
{
    /// <summary>
    /// Try to consume an exact string match.
    /// Uses vectorized comparison when possible.
    /// </summary>
    public static bool TryReadExact(
        this ref SequenceReader<char> reader,
        ReadOnlySpan<char> expected)
    {
        if (reader.Remaining < expected.Length)
            return false;

        // Fast path for contiguous data
        if (reader.UnreadSpan.Length >= expected.Length)
        {
            if (reader.UnreadSpan.StartsWith(expected))
            {
                reader.Advance(expected.Length);
                return true;
            }
            return false;
        }

        // Slow path for segmented data
        return TryReadExactSlow(ref reader, expected);
    }

    /// <summary>
    /// Advance while characters match the SearchValues set.
    /// Uses SIMD-accelerated searching.
    /// </summary>
    public static int AdvanceWhileAny(
        this ref SequenceReader<char> reader,
        SearchValues<char> values)
    {
        var count = 0;

        while (!reader.End)
        {
            var span = reader.UnreadSpan;
            var idx = span.IndexOfAnyExcept(values);

            if (idx == 0)
                break;  // First char doesn't match

            if (idx < 0)
            {
                // Entire span matches
                count += span.Length;
                reader.Advance(span.Length);
            }
            else
            {
                // Partial match
                count += idx;
                reader.Advance(idx);
                break;
            }
        }

        return count;
    }

    /// <summary>
    /// Advance while characters are in the specified range.
    /// </summary>
    public static int AdvanceWhileInRange(
        this ref SequenceReader<char> reader,
        char min,
        char max)
    {
        var count = 0;

        while (reader.TryPeek(out var c) && c >= min && c <= max)
        {
            reader.Advance(1);
            count++;
        }

        return count;
    }

    /// <summary>
    /// Advance until any character in the SearchValues set is found.
    /// Returns the position of the found character, or -1 if not found.
    /// </summary>
    public static long AdvanceUntilAny(
        this ref SequenceReader<char> reader,
        SearchValues<char> values)
    {
        var startPosition = reader.Consumed;

        while (!reader.End)
        {
            var span = reader.UnreadSpan;
            var idx = span.IndexOfAny(values);

            if (idx >= 0)
            {
                reader.Advance(idx);
                return reader.Consumed - startPosition;
            }

            reader.Advance(span.Length);
        }

        return -1;  // Not found
    }

    /// <summary>
    /// Try to read a span of consumed characters.
    /// </summary>
    public static bool TryReadTo(
        this ref SequenceReader<char> reader,
        out ReadOnlySpan<char> span,
        char delimiter,
        bool advancePastDelimiter = true)
    {
        if (reader.TryReadTo(out ReadOnlySequence<char> sequence, delimiter, advancePastDelimiter))
        {
            // Fast path: contiguous
            if (sequence.IsSingleSegment)
            {
                span = sequence.FirstSpan;
                return true;
            }

            // Slow path: copy to contiguous buffer
            span = sequence.ToArray();
            return true;
        }

        span = default;
        return false;
    }
}
```

---

## Consumer Base Classes

### LexerConsumerBase

Base class for Level 0 consumers with common functionality:

```csharp
namespace MetaParser.Parsing;

using System.Buffers;

/// <summary>
/// Base class for lexer (Level 0) consumers.
/// </summary>
public abstract class LexerConsumerBase : ILexerConsumer
{
    public abstract int Priority { get; }
    public abstract ushort TokenKind { get; }
    public virtual bool IsTrivia => false;

    public abstract bool TryConsume(ref SequenceReader<char> reader, out GreenToken token);
    public abstract bool CanStartWith(char c);

    /// <summary>
    /// Helper to create a token from consumed text.
    /// </summary>
    protected GreenToken CreateToken(
        ReadOnlyMemory<char> text,
        GreenNode? leadingTrivia = null,
        GreenNode? trailingTrivia = null)
    {
        return GreenTokenFactory.Create(TokenKind, text, leadingTrivia, trailingTrivia);
    }
}
```

### ParserConsumerBase

Base class for Level 1+ consumers:

```csharp
/// <summary>
/// Base class for parser (Level 1+) consumers.
/// </summary>
public abstract class ParserConsumerBase : IParserConsumer
{
    public abstract int Priority { get; }
    public abstract ushort TokenKind { get; }
    public abstract ReadOnlySpan<ushort> StarterKinds { get; }

    public abstract bool TryConsume(ref SequenceReader<GreenToken> reader, out GreenToken token);

    public bool CanStartWith(GreenToken token)
    {
        var kinds = StarterKinds;
        for (int i = 0; i < kinds.Length; i++)
        {
            if (kinds[i] == token.RawKind)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Helper to create a composite token from children.
    /// </summary>
    protected GreenToken CreateToken(
        GreenNode children,
        GreenNode? leadingTrivia = null,
        GreenNode? trailingTrivia = null)
    {
        return GreenTokenFactory.Create(TokenKind, children, leadingTrivia, trailingTrivia);
    }
}
```

---

## Token Priority

### Priority Assignment

Tokens receive priority based on their definition order in the schema:

```json
{
  "tokens": {
    "keyword_if": { "consume": "if" }, // Priority 0
    "keyword_in": { "consume": "in" }, // Priority 1
    "identifier": { "consume": "..." } // Priority 2
  }
}
```

### Priority Resolution Algorithm

When multiple consumers could match at a position:

```csharp
/// <summary>
/// Selects the best consumer match at current position.
/// </summary>
private bool TrySelectConsumer<T>(
    ref SequenceReader<T> reader,
    ReadOnlySpan<IConsumer<T>> consumers,
    out GreenToken token)
{
    // Quick filter: get consumers that can start with current element
    if (!reader.TryPeek(out var current))
    {
        token = default!;
        return false;
    }

    // Try consumers in priority order (already sorted by Priority)
    foreach (var consumer in consumers)
    {
        if (!consumer.CanStartWith(current))
            continue;

        var startPosition = reader.Consumed;

        if (consumer.TryConsume(ref reader, out token))
        {
            return true;  // First successful match wins
        }

        // Rewind on failure
        reader.Rewind(reader.Consumed - startPosition);
    }

    token = default!;
    return false;
}
```

### Longest Match Consideration

For certain token types, longest match may be preferred over priority:

```csharp
/// <summary>
/// Alternative: Select longest match among successful consumers.
/// </summary>
private bool TrySelectLongestMatch<T>(
    ref SequenceReader<T> reader,
    ReadOnlySpan<IConsumer<T>> consumers,
    out GreenToken token)
{
    if (!reader.TryPeek(out var current))
    {
        token = default!;
        return false;
    }

    GreenToken? bestMatch = null;
    long bestLength = -1;
    int bestPriority = int.MaxValue;
    var startPosition = reader.Consumed;

    foreach (var consumer in consumers)
    {
        if (!consumer.CanStartWith(current))
            continue;

        var beforeConsume = reader.Consumed;

        if (consumer.TryConsume(ref reader, out var match))
        {
            var length = reader.Consumed - startPosition;

            // Prefer longer match, or same length with higher priority
            if (length > bestLength ||
                (length == bestLength && consumer.Priority < bestPriority))
            {
                bestMatch = match;
                bestLength = length;
                bestPriority = consumer.Priority;
            }
        }

        // Rewind to try next consumer
        reader.Rewind(reader.Consumed - startPosition);
    }

    if (bestMatch != null)
    {
        reader.Advance(bestLength);
        token = bestMatch;
        return true;
    }

    token = default!;
    return false;
}
```

---

## Consumer Chain Orchestration

### ConsumerChain Class

The main orchestrator that runs all consumer levels:

```csharp
namespace MetaParser.Parsing;

/// <summary>
/// Orchestrates the consumer chain to parse input into a green tree.
/// </summary>
public sealed class ConsumerChain
{
    private readonly ILexerConsumer[] _lexerConsumers;
    private readonly ILexerConsumer[] _triviaConsumers;
    private readonly IParserConsumer[][] _parserLevels;

    public ConsumerChain(
        IEnumerable<ILexerConsumer> lexerConsumers,
        IEnumerable<IEnumerable<IParserConsumer>> parserLevels)
    {
        // Sort by priority and separate trivia
        var sorted = lexerConsumers.OrderBy(c => c.Priority).ToArray();
        _lexerConsumers = sorted.Where(c => !c.IsTrivia).ToArray();
        _triviaConsumers = sorted.Where(c => c.IsTrivia).ToArray();

        _parserLevels = parserLevels
            .Select(level => level.OrderBy(c => c.Priority).ToArray())
            .ToArray();
    }

    /// <summary>
    /// Parse input text into a green syntax tree.
    /// </summary>
    public GreenNode Parse(ReadOnlyMemory<char> input)
    {
        // Level 0: Characters → Tokens
        var lexerTokens = RunLexer(input);

        // Level 1..N: Tokens → Tokens
        var currentTokens = lexerTokens;
        foreach (var parserLevel in _parserLevels)
        {
            currentTokens = RunParserLevel(currentTokens, parserLevel);
        }

        // Create root node
        return GreenTokenFactory.CreateRoot(currentTokens);
    }

    /// <summary>
    /// Run the lexer level (Level 0).
    /// </summary>
    private List<GreenToken> RunLexer(ReadOnlyMemory<char> input)
    {
        var tokens = new List<GreenToken>();
        var sequence = new ReadOnlySequence<char>(input);
        var reader = new SequenceReader<char>(sequence);
        GreenNode? pendingTrivia = null;

        while (!reader.End)
        {
            // First, collect any trivia
            pendingTrivia = CollectTrivia(ref reader, input, pendingTrivia);

            if (reader.End)
                break;

            // Try to match a token
            if (TryMatchLexerToken(ref reader, input, out var token))
            {
                // Attach pending trivia as leading
                if (pendingTrivia != null)
                {
                    token = token.WithLeadingTrivia(pendingTrivia);
                    pendingTrivia = null;
                }

                tokens.Add(token);
            }
            else
            {
                // No consumer matched - create error token
                var errorToken = CreateErrorToken(ref reader, input);
                if (pendingTrivia != null)
                {
                    errorToken = errorToken.WithLeadingTrivia(pendingTrivia);
                    pendingTrivia = null;
                }
                tokens.Add(errorToken);
            }
        }

        // Attach any remaining trivia to last token (or create EOF token)
        if (pendingTrivia != null && tokens.Count > 0)
        {
            var lastIndex = tokens.Count - 1;
            tokens[lastIndex] = tokens[lastIndex].WithTrailingTrivia(pendingTrivia);
        }

        return tokens;
    }

    /// <summary>
    /// Collect trivia tokens until a non-trivia position.
    /// </summary>
    private GreenNode? CollectTrivia(
        ref SequenceReader<char> reader,
        ReadOnlyMemory<char> input,
        GreenNode? existing)
    {
        var triviaList = existing != null
            ? new List<GreenNode> { existing }
            : new List<GreenNode>();

        while (!reader.End)
        {
            var matched = false;

            foreach (var consumer in _triviaConsumers)
            {
                if (!reader.TryPeek(out var c) || !consumer.CanStartWith(c))
                    continue;

                var startPos = reader.Consumed;

                if (consumer.TryConsume(ref reader, out var trivia))
                {
                    triviaList.Add(trivia);
                    matched = true;
                    break;
                }

                reader.Rewind(reader.Consumed - startPos);
            }

            if (!matched)
                break;
        }

        return triviaList.Count switch
        {
            0 => null,
            1 => triviaList[0],
            _ => GreenTokenFactory.CreateTriviaList(triviaList)
        };
    }

    /// <summary>
    /// Try to match a lexer token at current position.
    /// </summary>
    private bool TryMatchLexerToken(
        ref SequenceReader<char> reader,
        ReadOnlyMemory<char> input,
        out GreenToken token)
    {
        if (!reader.TryPeek(out var current))
        {
            token = default!;
            return false;
        }

        foreach (var consumer in _lexerConsumers)
        {
            if (!consumer.CanStartWith(current))
                continue;

            var startPos = reader.Consumed;

            if (consumer.TryConsume(ref reader, out token))
            {
                return true;
            }

            reader.Rewind(reader.Consumed - startPos);
        }

        token = default!;
        return false;
    }

    /// <summary>
    /// Run a parser level.
    /// </summary>
    private List<GreenToken> RunParserLevel(
        List<GreenToken> inputTokens,
        IParserConsumer[] consumers)
    {
        var output = new List<GreenToken>();
        var tokenArray = CollectionsMarshal.AsSpan(inputTokens).ToArray();
        var sequence = new ReadOnlySequence<GreenToken>(tokenArray);
        var reader = new SequenceReader<GreenToken>(sequence);

        while (!reader.End)
        {
            if (TryMatchParserToken(ref reader, consumers, out var token))
            {
                output.Add(token);
            }
            else
            {
                // Pass through unmatched token
                if (reader.TryRead(out var passthrough))
                {
                    output.Add(passthrough);
                }
            }
        }

        return output;
    }

    /// <summary>
    /// Try to match a parser token at current position.
    /// </summary>
    private bool TryMatchParserToken(
        ref SequenceReader<GreenToken> reader,
        IParserConsumer[] consumers,
        out GreenToken token)
    {
        if (!reader.TryPeek(out var current))
        {
            token = default!;
            return false;
        }

        foreach (var consumer in consumers)
        {
            if (!consumer.CanStartWith(current))
                continue;

            var startPos = reader.Consumed;

            if (consumer.TryConsume(ref reader, out token))
            {
                return true;
            }

            reader.Rewind(reader.Consumed - startPos);
        }

        token = default!;
        return false;
    }

    /// <summary>
    /// Create an error token for unrecognized input.
    /// </summary>
    private GreenToken CreateErrorToken(
        ref SequenceReader<char> reader,
        ReadOnlyMemory<char> input)
    {
        var start = (int)reader.Consumed;
        reader.Advance(1);  // Consume at least one character

        var text = input.Slice(start, (int)reader.Consumed - start);
        return GreenTokenFactory.CreateError(text);
    }
}
```

---

## Trivia Handling

### Trivia Attachment Strategy

Following Roslyn's rules, trivia attaches based on line boundaries:

```csharp
/// <summary>
/// Determines how to attach trivia between tokens.
/// </summary>
public static class TriviaAttachment
{
    /// <summary>
    /// Split trivia between trailing (current token) and leading (next token).
    /// </summary>
    public static (GreenNode? trailing, GreenNode? leading) SplitTrivia(
        IReadOnlyList<GreenNode> trivia)
    {
        if (trivia.Count == 0)
            return (null, null);

        // Find the split point: after last newline on same line
        var splitIndex = 0;

        for (int i = 0; i < trivia.Count; i++)
        {
            if (IsEndOfLine(trivia[i]))
            {
                // Include this EOL in trailing, next items are leading
                splitIndex = i + 1;
            }
        }

        if (splitIndex == 0)
        {
            // No newline found - all trivia is trailing (same line)
            return (CreateTriviaNode(trivia), null);
        }

        if (splitIndex >= trivia.Count)
        {
            // All trivia is trailing (ends with newline)
            return (CreateTriviaNode(trivia), null);
        }

        // Split the trivia
        var trailing = trivia.Take(splitIndex).ToList();
        var leading = trivia.Skip(splitIndex).ToList();

        return (CreateTriviaNode(trailing), CreateTriviaNode(leading));
    }

    private static bool IsEndOfLine(GreenNode trivia)
    {
        return trivia.RawKind == TriviaKind.EndOfLine ||
               trivia.RawKind == TriviaKind.Newline;
    }

    private static GreenNode? CreateTriviaNode(IReadOnlyList<GreenNode> trivia)
    {
        return trivia.Count switch
        {
            0 => null,
            1 => trivia[0],
            _ => GreenTokenFactory.CreateTriviaList(trivia)
        };
    }
}
```

---

## Error Handling

### Error Recovery Strategies

```csharp
/// <summary>
/// Error recovery options for the consumer chain.
/// </summary>
public enum ErrorRecovery
{
    /// <summary>Skip single character and continue.</summary>
    SkipChar,

    /// <summary>Skip to next whitespace.</summary>
    SkipToWhitespace,

    /// <summary>Skip to next newline.</summary>
    SkipToNewline,

    /// <summary>Skip to specific synchronization token.</summary>
    SkipToSync
}

/// <summary>
/// Error token representing unrecognized input.
/// </summary>
public sealed class GreenErrorToken : GreenToken
{
    public string ErrorMessage { get; }

    internal GreenErrorToken(ReadOnlyMemory<char> text, string? message = null)
        : base(TokenKind.Error, text)
    {
        ErrorMessage = message ?? "Unexpected character";
    }
}
```

### Diagnostic Collection

```csharp
/// <summary>
/// Collects diagnostics during parsing.
/// </summary>
public sealed class ParseDiagnostics
{
    private readonly List<Diagnostic> _diagnostics = new();

    public IReadOnlyList<Diagnostic> Diagnostics => _diagnostics;
    public bool HasErrors => _diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error);

    public void AddError(int position, int length, string message)
    {
        _diagnostics.Add(new Diagnostic(
            DiagnosticSeverity.Error,
            position,
            length,
            message));
    }

    public void AddWarning(int position, int length, string message)
    {
        _diagnostics.Add(new Diagnostic(
            DiagnosticSeverity.Warning,
            position,
            length,
            message));
    }
}

public readonly record struct Diagnostic(
    DiagnosticSeverity Severity,
    int Position,
    int Length,
    string Message);

public enum DiagnosticSeverity { Info, Warning, Error }
```

---

## Generated Consumer Structure

### Example: Generated Lexer Consumer (Fixed String)

For schema definition:

```json
{
  "tokens": {
    "keyword_if": { "consume": "if" }
  }
}
```

Generated:

```csharp
// Generated by MetaParser
namespace MyParser.Parsing.Consumers;

using System.Buffers;

internal sealed class KeywordIfConsumer : LexerConsumerBase
{
    public static readonly KeywordIfConsumer Instance = new();

    // SIMD-optimized set for identifier continuation chars
    private static readonly SearchValues<char> s_identifierChars =
        SearchValues.Create("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_");

    public override int Priority => 0;
    public override ushort TokenKind => (ushort)MyParser.TokenKind.KeywordIf;

    private static ReadOnlySpan<char> Text => "if";

    public override bool CanStartWith(char c) => c == 'i';

    public override bool TryConsume(ref SequenceReader<char> reader, out GreenToken token)
    {
        if (reader.TryReadExact(Text))
        {
            // Ensure not followed by identifier char (keyword boundary)
            if (!reader.TryPeek(out var next) || !s_identifierChars.Contains(next))
            {
                token = GreenKeywordIfToken.Instance;
                return true;
            }

            // Rollback - this is an identifier starting with "if"
            reader.Rewind(Text.Length);
        }

        token = default!;
        return false;
    }
}
```

### Example: Generated Whitespace Consumer (SIMD-Optimized)

For schema definition:

```json
{
  "tokens": {
    "whitespace": { "consume": [" ", "\t"] }
  }
}
```

Generated:

```csharp
// Generated by MetaParser
namespace MyParser.Parsing.Consumers;

using System.Buffers;

internal sealed class WhitespaceConsumer : LexerConsumerBase
{
    public static readonly WhitespaceConsumer Instance = new();

    // SIMD-accelerated character matching
    private static readonly SearchValues<char> s_whitespaceChars =
        SearchValues.Create(" \t");

    public override int Priority => 4;
    public override ushort TokenKind => (ushort)MyParser.TokenKind.Whitespace;
    public override bool IsTrivia => true;

    public override bool CanStartWith(char c) => s_whitespaceChars.Contains(c);

    public override bool TryConsume(ref SequenceReader<char> reader, out GreenToken token)
    {
        var startPos = reader.Consumed;

        // Use SIMD-optimized bulk consumption
        var count = reader.AdvanceWhileAny(s_whitespaceChars);

        if (count > 0)
        {
            // Create trivia token (uses ReadOnlyMemory<char>)
            token = GreenTokenFactory.CreateWhitespace(startPos, count);
            return true;
        }

        token = default!;
        return false;
    }
}
```

### Example: Generated Identifier Consumer (Range-Based)

For schema definition:

```json
{
  "tokens": {
    "identifier": {
      "start": [{ "range": ["a", "z"] }, { "range": ["A", "Z"] }, "_"],
      "consume": [
        { "range": ["a", "z"] },
        { "range": ["A", "Z"] },
        { "range": ["0", "9"] },
        "_"
      ]
    }
  }
}
```

Generated:

```csharp
// Generated by MetaParser
namespace MyParser.Parsing.Consumers;

using System.Buffers;

internal sealed class IdentifierConsumer : LexerConsumerBase
{
    public static readonly IdentifierConsumer Instance = new();

    // SIMD-optimized character sets
    private static readonly SearchValues<char> s_startChars =
        SearchValues.Create("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_");

    private static readonly SearchValues<char> s_continueChars =
        SearchValues.Create("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_");

    public override int Priority => 2;
    public override ushort TokenKind => (ushort)MyParser.TokenKind.Identifier;

    public override bool CanStartWith(char c) => s_startChars.Contains(c);

    public override bool TryConsume(ref SequenceReader<char> reader, out GreenToken token)
    {
        if (!reader.TryPeek(out var first) || !s_startChars.Contains(first))
        {
            token = default!;
            return false;
        }

        var startPos = reader.Consumed;
        reader.Advance(1);  // Consume start char

        // SIMD-accelerated consumption of remaining identifier chars
        reader.AdvanceWhileAny(s_continueChars);

        var length = (int)(reader.Consumed - startPos);
        token = GreenTokenFactory.CreateIdentifier(startPos, length);
        return true;
    }
}
```

### Example: Generated Parser Consumer

For schema definition:

```json
{
  "tokens": {
    "number": { "consume": [{ "$token": "digit" }] }
  }
}
```

Generated:

```csharp
// Generated by MetaParser
namespace MyParser.Parsing.Consumers;

using System.Buffers;

internal sealed class NumberConsumer : ParserConsumerBase
{
    public static readonly NumberConsumer Instance = new();

    public override int Priority => 1;
    public override ushort TokenKind => (ushort)MyParser.TokenKind.Number;

    private static readonly ushort[] _starterKinds = [(ushort)MyParser.TokenKind.Digit];
    public override ReadOnlySpan<ushort> StarterKinds => _starterKinds;

    public override bool TryConsume(ref SequenceReader<GreenToken> reader, out GreenToken token)
    {
        if (!reader.TryPeek(out var first) || first.RawKind != (ushort)MyParser.TokenKind.Digit)
        {
            token = default!;
            return false;
        }

        var children = new List<GreenToken>();

        while (reader.TryPeek(out var current) &&
               current.RawKind == (ushort)MyParser.TokenKind.Digit)
        {
            reader.TryRead(out var digit);
            children.Add(digit);
        }

        token = GreenTokenFactory.CreateNumber(
            GreenTokenFactory.CreateList(children));
        return true;
    }
}
```

### Generated Chain Factory

```csharp
// Generated by MetaParser
namespace MyParser.Parsing;

public static class ParserFactory
{
    public static ConsumerChain CreateChain()
    {
        var lexerConsumers = new ILexerConsumer[]
        {
            KeywordIfConsumer.Instance,      // Priority 0
            KeywordElseConsumer.Instance,    // Priority 1
            IdentifierConsumer.Instance,     // Priority 2
            DigitConsumer.Instance,          // Priority 3
            WhitespaceConsumer.Instance,     // Priority 4 (trivia)
            NewlineConsumer.Instance,        // Priority 5 (trivia)
        };

        var parserLevels = new IParserConsumer[][]
        {
            // Level 1
            [
                NumberConsumer.Instance,     // Priority 0
            ],
        };

        return new ConsumerChain(lexerConsumers, parserLevels);
    }
}
```

---

## Performance Considerations

### SIMD and SearchValues Optimizations

The generated consumers leverage `System.Buffers.SearchValues<T>` for SIMD-accelerated character matching:

```csharp
// SearchValues uses hardware intrinsics when available:
// - SSE2/AVX2 on x64
// - AdvSimd on ARM64
// - Scalar fallback on other platforms

// Single character check - O(1) with vectorization
private static readonly SearchValues<char> s_digits = SearchValues.Create("0123456789");
bool isDigit = s_digits.Contains(c);  // SIMD when checking many chars

// Bulk operations on spans - fully vectorized
int index = span.IndexOfAny(s_digits);           // Find first digit
int index = span.IndexOfAnyExcept(s_digits);     // Find first non-digit
bool contains = span.ContainsAny(s_digits);      // Check if any digit exists
```

### Span<T> SIMD Methods

Generated code prefers these vectorized `Span<T>` methods:

| Method                                | Use Case                  | SIMD Optimized |
| ------------------------------------- | ------------------------- | -------------- |
| `span.IndexOfAny(SearchValues)`       | Find character from set   | ✅ Yes         |
| `span.IndexOfAnyExcept(SearchValues)` | Find character NOT in set | ✅ Yes         |
| `span.ContainsAny(SearchValues)`      | Check if set chars exist  | ✅ Yes         |
| `span.StartsWith(ReadOnlySpan)`       | Keyword matching          | ✅ Yes         |
| `span.SequenceEqual(ReadOnlySpan)`    | Exact comparison          | ✅ Yes         |
| `span.IndexOf(char)`                  | Single char search        | ✅ Yes         |
| `span.IndexOf(ReadOnlySpan)`          | Substring search          | ✅ Yes         |

### Hot Path Optimizations

1. **CanStartWith Fast Path** - Quick rejection before full match attempt
2. **Priority Sorting** - Consumers pre-sorted by priority
3. **Span-Based Matching** - Zero-allocation string comparisons
4. **Pooled Collections** - Reuse token lists where possible

### Memory Allocation Strategy

```csharp
/// <summary>
/// Object pool for token lists during parsing.
/// </summary>
internal static class TokenListPool
{
    private static readonly ObjectPool<List<GreenToken>> s_pool =
        new DefaultObjectPool<List<GreenToken>>(
            new ListPolicy<GreenToken>(),
            maximumRetained: 16);

    public static List<GreenToken> Rent() => s_pool.Get();

    public static void Return(List<GreenToken> list)
    {
        list.Clear();
        s_pool.Return(list);
    }
}
```

### Benchmarking Targets

| Operation             | Target                        |
| --------------------- | ----------------------------- |
| Lexer throughput      | > 100 MB/s                    |
| Token allocation      | < 100 bytes per token average |
| Parser level overhead | < 10% per level               |

---

## Appendix: Consumer Pattern Catalog

### Fixed String Consumer

```csharp
// Pattern: "consume": "text"
public override bool TryConsume(ref SequenceReader<char> reader, out GreenToken token)
{
    if (reader.TryReadExact(Text))
    {
        token = CachedToken;
        return true;
    }
    token = default!;
    return false;
}
```

### Character Set Consumer (SIMD-Optimized)

```csharp
// Pattern: "consume": ["+", "-", "*", "/"]
private static readonly SearchValues<char> s_operators = SearchValues.Create("+-*/");

public override bool TryConsume(ref SequenceReader<char> reader, out GreenToken token)
{
    if (reader.TryPeek(out var c) && s_operators.Contains(c))
    {
        reader.Advance(1);
        token = CreateToken(c);
        return true;
    }
    token = default!;
    return false;
}
```

### Character Range Consumer (SIMD-Optimized)

```csharp
// Pattern: "consume": { "range": ["a", "z"] }
// Build SearchValues from range at static init time
private static readonly SearchValues<char> s_lowerLetters =
    SearchValues.Create("abcdefghijklmnopqrstuvwxyz");

public override bool TryConsume(ref SequenceReader<char> reader, out GreenToken token)
{
    var startPos = reader.Consumed;

    // SIMD-accelerated bulk consumption
    var count = reader.AdvanceWhileAny(s_lowerLetters);

    if (count > 0)
    {
        token = CreateToken(startPos, count);
        return true;
    }

    token = default!;
    return false;
}
```

### Multi-Character Sequence Consumer

```csharp
// Pattern: "consume": "==" (multi-char operator)
private static ReadOnlySpan<char> Text => "==";

public override bool TryConsume(ref SequenceReader<char> reader, out GreenToken token)
{
    // Uses vectorized span comparison
    if (reader.UnreadSpan.StartsWith(Text))
    {
        reader.Advance(Text.Length);
        token = GreenEqualsEqualsToken.Instance;
        return true;
    }
    token = default!;
    return false;
}
```

### String Literal Consumer (Delimited)

```csharp
// Pattern: "start": "\"", "consume": ..., "stop": "\"", "escape": "\\"
private static readonly SearchValues<char> s_stopChars = SearchValues.Create("\"\\");

public override bool TryConsume(ref SequenceReader<char> reader, out GreenToken token)
{
    if (!reader.TryPeek(out var c) || c != '"')
    {
        token = default!;
        return false;
    }

    var startPos = reader.Consumed;
    reader.Advance(1);  // Skip opening quote

    while (!reader.End)
    {
        // SIMD scan for quote or escape
        var idx = reader.UnreadSpan.IndexOfAny(s_stopChars);

        if (idx < 0)
        {
            // No terminator found - consume rest and error
            reader.AdvanceToEnd();
            break;
        }

        reader.Advance(idx);

        if (reader.TryPeek(out var found))
        {
            if (found == '"')
            {
                reader.Advance(1);  // Consume closing quote
                var length = (int)(reader.Consumed - startPos);
                token = CreateStringToken(startPos, length);
                return true;
            }
            else if (found == '\\')
            {
                reader.Advance(2);  // Skip escape + next char
            }
        }
    }

    // Unterminated string - still return token with error flag
    token = CreateErrorStringToken(startPos, (int)(reader.Consumed - startPos));
    return true;
}
```

### Token Reference Consumer

```csharp
// Pattern: "consume": [{ "$token": "digit" }]
public override bool TryConsume(ref SequenceReader<GreenToken> reader, out GreenToken token)
{
    var children = new List<GreenToken>();

    while (reader.TryPeek(out var t) && IsAcceptedKind(t.RawKind))
    {
        reader.TryRead(out var child);
        children.Add(child);
    }

    if (children.Count > 0)
    {
        token = CreateCompositeToken(children);
        return true;
    }

    token = default!;
    return false;
}
```

---

_This document defines the consumer chain infrastructure for MetaParser v2. The design emphasizes performance through SIMD optimization, determinism, and clean generated code._
