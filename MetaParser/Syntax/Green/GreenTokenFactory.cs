using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MetaParser.Syntax.Green;

/// <summary>
/// Factory for creating green tokens with caching for keywords and operators.
/// Frequently-used tokens (keywords, operators, punctuation) are cached to reduce allocations.
/// </summary>
public sealed class GreenTokenFactory
{
    /// <summary>
    /// Cache for keyword/operator tokens (keyed by kind + text).
    /// </summary>
    private readonly Dictionary<CacheKey, GreenTokenWithNoTrivia> _cache;
    
    /// <summary>
    /// Maximum number of cached tokens.
    /// </summary>
    private readonly int _maxCacheSize;
    
    /// <summary>
    /// Current cache size.
    /// </summary>
    private int _cacheSize;
    
    /// <summary>
    /// Lock for thread-safe cache access.
    /// </summary>
    private readonly object _lock = new();
    
    /// <summary>
    /// Creates a new token factory with the specified cache size.
    /// </summary>
    public GreenTokenFactory(int maxCacheSize = 1024)
    {
        _maxCacheSize = maxCacheSize;
        _cache = new Dictionary<CacheKey, GreenTokenWithNoTrivia>(maxCacheSize);
    }
    
    /// <summary>
    /// Creates or retrieves a cached token with the specified kind and text.
    /// For cacheable tokens (keywords, operators), returns a cached instance.
    /// </summary>
    public GreenToken CreateToken(ushort kind, string text)
    {
        // Only cache short tokens (keywords, operators)
        if (text.Length <= 16)
        {
            var key = new CacheKey(kind, text);
            
            lock (_lock)
            {
                if (_cache.TryGetValue(key, out var cached))
                    return cached;
                
                if (_cacheSize < _maxCacheSize)
                {
                    var token = new GreenTokenWithNoTrivia(kind, text);
                    _cache[key] = token;
                    _cacheSize++;
                    return token;
                }
            }
        }
        
        // Not cached - create new token
        return new GreenTokenWithNoTrivia(kind, text);
    }
    
    /// <summary>
    /// Creates a token backed by a memory slice (not cached, zero-copy).
    /// Use this for identifiers, literals, and other variable-content tokens.
    /// </summary>
    public GreenToken CreateToken(ushort kind, ReadOnlyMemory<char> text)
    {
        return new GreenTokenWithNoTrivia(kind, text);
    }
    
    /// <summary>
    /// Creates a token with leading trivia.
    /// </summary>
    public GreenToken CreateTokenWithLeadingTrivia(ushort kind, GreenNode leadingTrivia, string text)
    {
        return new GreenTokenWithLeadingTrivia(kind, leadingTrivia, text);
    }
    
    /// <summary>
    /// Creates a token with leading trivia (memory-backed).
    /// </summary>
    public GreenToken CreateTokenWithLeadingTrivia(ushort kind, GreenNode leadingTrivia, ReadOnlyMemory<char> text)
    {
        return new GreenTokenWithLeadingTrivia(kind, leadingTrivia, text);
    }
    
    /// <summary>
    /// Creates a token with trailing trivia.
    /// </summary>
    public GreenToken CreateTokenWithTrailingTrivia(ushort kind, string text, GreenNode trailingTrivia)
    {
        return new GreenTokenWithTrailingTrivia(kind, text, trailingTrivia);
    }
    
    /// <summary>
    /// Creates a token with trailing trivia (memory-backed).
    /// </summary>
    public GreenToken CreateTokenWithTrailingTrivia(ushort kind, ReadOnlyMemory<char> text, GreenNode trailingTrivia)
    {
        return new GreenTokenWithTrailingTrivia(kind, text, trailingTrivia);
    }
    
    /// <summary>
    /// Creates a token with both leading and trailing trivia.
    /// </summary>
    public GreenToken CreateTokenWithTrivia(ushort kind, GreenNode leadingTrivia, string text, GreenNode trailingTrivia)
    {
        return new GreenTokenWithTrivia(kind, leadingTrivia, text, trailingTrivia);
    }
    
    /// <summary>
    /// Creates a token with both leading and trailing trivia (memory-backed).
    /// </summary>
    public GreenToken CreateTokenWithTrivia(ushort kind, GreenNode leadingTrivia, ReadOnlyMemory<char> text, GreenNode trailingTrivia)
    {
        return new GreenTokenWithTrivia(kind, leadingTrivia, text, trailingTrivia);
    }
    
    /// <summary>
    /// Creates trivia from a memory slice.
    /// </summary>
    public GreenTrivia CreateTrivia(ushort kind, ReadOnlyMemory<char> text)
    {
        return new GreenTrivia(kind, text);
    }
    
    /// <summary>
    /// Creates trivia from a string.
    /// </summary>
    public GreenTrivia CreateTrivia(ushort kind, string text)
    {
        return new GreenTrivia(kind, text);
    }
    
    /// <summary>
    /// Creates a trivia list from multiple trivia items.
    /// </summary>
    public GreenTriviaList CreateTriviaList(ushort kind, params GreenTrivia[] trivia)
    {
        return new GreenTriviaList(kind, trivia);
    }
    
    /// <summary>
    /// Clears the token cache.
    /// </summary>
    public void ClearCache()
    {
        lock (_lock)
        {
            _cache.Clear();
            _cacheSize = 0;
        }
    }
    
    /// <summary>
    /// Gets the current cache size.
    /// </summary>
    public int CacheCount
    {
        get
        {
            lock (_lock)
            {
                return _cacheSize;
            }
        }
    }
    
    /// <summary>
    /// Key for the token cache.
    /// </summary>
    private readonly struct CacheKey : IEquatable<CacheKey>
    {
        public readonly ushort Kind;
        public readonly string Text;
        private readonly int _hashCode;
        
        public CacheKey(ushort kind, string text)
        {
            Kind = kind;
            Text = text;
            unchecked
            {
                _hashCode = (kind * 397) ^ (text?.GetHashCode() ?? 0);
            }
        }
        
        public bool Equals(CacheKey other)
        {
            return Kind == other.Kind && Text == other.Text;
        }
        
        public override bool Equals(object? obj)
        {
            return obj is CacheKey other && Equals(other);
        }
        
        public override int GetHashCode() => _hashCode;
    }
}

/// <summary>
/// A shared default factory instance for convenience.
/// </summary>
public static class DefaultGreenTokenFactory
{
    private static GreenTokenFactory? _instance;
    
    /// <summary>
    /// Gets the shared default factory instance.
    /// </summary>
    public static GreenTokenFactory Instance => _instance ??= new GreenTokenFactory();
}
