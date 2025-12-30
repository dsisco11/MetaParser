using MetaParser.Schema;

namespace MetaParser.Generation;

/// <summary>
/// Generates GreenTokenFactory with caching for keywords/operators.
/// </summary>
internal static class GreenTokenFactoryGenerator
{
    public static GeneratedFileBuilder Generate(SchemaDefinition schema)
    {
        var file = new GeneratedFileBuilder(schema.Namespace)
            .WithFileName("GreenTokenFactory.g.cs")
            .AddUsings("System", "System.Collections.Generic");

        var code = file.Code;

        GenerateFactory(code);

        return file;
    }

    private static void GenerateFactory(CodeBuilder code)
    {
        code.AppendSummary("Factory for creating green tokens with caching for keywords/operators.");
        code.AppendLine("internal sealed class GreenTokenFactory");
        code.OpenBlock();

        // Cache fields
        code.AppendLine("private readonly Dictionary<CacheKey, GreenTokenWithNoTrivia> _cache;");
        code.AppendLine("private readonly int _maxCacheSize;");
        code.AppendLine("private int _cacheSize;");
        code.AppendLine("private readonly object _lock = new();");
        code.AppendLine();

        // Constructor
        code.AppendSummaryLine("Creates a new token factory with the specified cache size.");
        code.AppendLine("public GreenTokenFactory(int maxCacheSize = 1024)");
        code.OpenBlock();
        code.AppendLine("_maxCacheSize = maxCacheSize;");
        code.AppendLine("_cache = new Dictionary<CacheKey, GreenTokenWithNoTrivia>(maxCacheSize);");
        code.CloseBlock();
        code.AppendLine();

        // CreateToken (string) - cached
        code.AppendSummaryLine("Creates or retrieves a cached token.");
        code.AppendLine("public GreenToken CreateToken(ushort kind, string text)");
        code.OpenBlock();
        code.AppendLine("if (text.Length <= 16)");
        code.OpenBlock();
        code.AppendLine("var key = new CacheKey(kind, text);");
        code.AppendLine("lock (_lock)");
        code.OpenBlock();
        code.AppendLine("if (_cache.TryGetValue(key, out var cached)) return cached;");
        code.AppendLine("if (_cacheSize < _maxCacheSize)");
        code.OpenBlock();
        code.AppendLine("var token = new GreenTokenWithNoTrivia(kind, text);");
        code.AppendLine("_cache[key] = token;");
        code.AppendLine("_cacheSize++;");
        code.AppendLine("return token;");
        code.CloseBlock();
        code.CloseBlock();
        code.CloseBlock();
        code.AppendLine("return new GreenTokenWithNoTrivia(kind, text);");
        code.CloseBlock();
        code.AppendLine();

        // CreateToken (memory) - not cached
        code.AppendSummaryLine("Creates a token backed by a memory slice (not cached).");
        code.AppendLine("public GreenToken CreateToken(ushort kind, ReadOnlyMemory<char> text)");
        code.OpenBlock();
        code.AppendLine("return new GreenTokenWithNoTrivia(kind, text);");
        code.CloseBlock();
        code.AppendLine();

        // CreateTokenWithLeadingTrivia
        code.AppendLine("public GreenToken CreateTokenWithLeadingTrivia(ushort kind, GreenNode leadingTrivia, string text)");
        code.AppendLine("    => new GreenTokenWithLeadingTrivia(kind, leadingTrivia, text);");
        code.AppendLine();
        code.AppendLine("public GreenToken CreateTokenWithLeadingTrivia(ushort kind, GreenNode leadingTrivia, ReadOnlyMemory<char> text)");
        code.AppendLine("    => new GreenTokenWithLeadingTrivia(kind, leadingTrivia, text);");
        code.AppendLine();

        // CreateTokenWithTrailingTrivia
        code.AppendLine("public GreenToken CreateTokenWithTrailingTrivia(ushort kind, string text, GreenNode trailingTrivia)");
        code.AppendLine("    => new GreenTokenWithTrailingTrivia(kind, text, trailingTrivia);");
        code.AppendLine();
        code.AppendLine("public GreenToken CreateTokenWithTrailingTrivia(ushort kind, ReadOnlyMemory<char> text, GreenNode trailingTrivia)");
        code.AppendLine("    => new GreenTokenWithTrailingTrivia(kind, text, trailingTrivia);");
        code.AppendLine();

        // CreateTokenWithTrivia
        code.AppendLine("public GreenToken CreateTokenWithTrivia(ushort kind, GreenNode leadingTrivia, string text, GreenNode trailingTrivia)");
        code.AppendLine("    => new GreenTokenWithTrivia(kind, leadingTrivia, text, trailingTrivia);");
        code.AppendLine();
        code.AppendLine("public GreenToken CreateTokenWithTrivia(ushort kind, GreenNode leadingTrivia, ReadOnlyMemory<char> text, GreenNode trailingTrivia)");
        code.AppendLine("    => new GreenTokenWithTrivia(kind, leadingTrivia, text, trailingTrivia);");
        code.AppendLine();

        // CreateTrivia
        code.AppendSummaryLine("Creates trivia from a memory slice.");
        code.AppendLine("public GreenTrivia CreateTrivia(ushort kind, ReadOnlyMemory<char> text)");
        code.AppendLine("    => new GreenTrivia(kind, text);");
        code.AppendLine();
        code.AppendSummaryLine("Creates trivia from a string.");
        code.AppendLine("public GreenTrivia CreateTrivia(ushort kind, string text)");
        code.AppendLine("    => new GreenTrivia(kind, text);");
        code.AppendLine();

        // CreateTriviaList
        code.AppendSummaryLine("Creates a trivia list.");
        code.AppendLine("public GreenTriviaList CreateTriviaList(ushort kind, params GreenTrivia[] trivia)");
        code.AppendLine("    => new GreenTriviaList(kind, trivia);");
        code.AppendLine();

        // ClearCache
        code.AppendSummaryLine("Clears the token cache.");
        code.AppendLine("public void ClearCache()");
        code.OpenBlock();
        code.AppendLine("lock (_lock) { _cache.Clear(); _cacheSize = 0; }");
        code.CloseBlock();
        code.AppendLine();

        // CacheCount
        code.AppendSummaryLine("Gets the current cache size.");
        code.AppendLine("public int CacheCount { get { lock (_lock) { return _cacheSize; } } }");
        code.AppendLine();

        // CacheKey struct
        code.AppendSummaryLine("Key for the token cache.");
        code.AppendLine("private readonly struct CacheKey : IEquatable<CacheKey>");
        code.OpenBlock();
        code.AppendLine("public readonly ushort Kind;");
        code.AppendLine("public readonly string Text;");
        code.AppendLine("private readonly int _hashCode;");
        code.AppendLine();
        code.AppendLine("public CacheKey(ushort kind, string text)");
        code.OpenBlock();
        code.AppendLine("Kind = kind;");
        code.AppendLine("Text = text;");
        code.AppendLine("unchecked { _hashCode = (kind * 397) ^ (text?.GetHashCode() ?? 0); }");
        code.CloseBlock();
        code.AppendLine();
        code.AppendLine("public bool Equals(CacheKey other) => Kind == other.Kind && Text == other.Text;");
        code.AppendLine("public override bool Equals(object? obj) => obj is CacheKey other && Equals(other);");
        code.AppendLine("public override int GetHashCode() => _hashCode;");
        code.CloseBlock();

        code.CloseBlock();
        code.AppendLine();

        // Default factory singleton
        code.AppendSummaryLine("Shared default factory instance.");
        code.AppendLine("internal static class DefaultGreenTokenFactory");
        code.OpenBlock();
        code.AppendLine("private static GreenTokenFactory? _instance;");
        code.AppendSummaryLine("Gets the shared default factory.");
        code.AppendLine("public static GreenTokenFactory Instance => _instance ??= new GreenTokenFactory();");
        code.CloseBlock();
    }
}
