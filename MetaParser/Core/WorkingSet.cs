using MetaParser.Parsing.Constructs;
using MetaParser.Parsing.Constructs.Patterns;

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Core;

/// <summary>
/// Holds a list of tokens, consumers, or patterns that are being targeted by the current action
/// </summary>
internal record WorkingSet
{
    #region Fields
    private TokenInfo[] tokens = Array.Empty<TokenInfo>();
    private Consumer[] consumers = Array.Empty<Consumer>();
    private Pattern[] patterns = Array.Empty<Pattern>();

    private SortedSet<TokenInfo> sortedTokens = new SortedSet<TokenInfo>();
    private SortedSet<Consumer> sortedConsumers = new SortedSet<Consumer>();
    private SortedSet<Pattern> sortedPatterns = new SortedSet<Pattern>(PatternComparer.Instance);
    #endregion

    #region Properties
    [Obsolete("Use SortedTokens instead")]
    public TokenInfo[] Tokens
    {
        get => tokens;
        set
        {
            tokens = value;
            sortedTokens = new SortedSet<TokenInfo>();
        }
    }

    [Obsolete("Use SortedConsumers instead")]
    public Consumer[] Consumers
    {
        get => consumers; 
        set
        {
            consumers = value;
            sortedConsumers = new SortedSet<Consumer>();
        }
    }

    [Obsolete("Use SortedPatterns instead")]
    public Pattern[] Patterns
    {
        get => patterns; 
        set
        {
            patterns = value;
            sortedPatterns = new SortedSet<Pattern>(PatternComparer.Instance);
        }
    }

    public SortedSet<TokenInfo> SortedTokens => sortedTokens;
    public SortedSet<Consumer> SortedConsumers => sortedConsumers; 
    public SortedSet<Pattern> SortedPatterns => sortedPatterns;
    #endregion

    #region Constructors
    public WorkingSet()
    {
    }

    public WorkingSet (IEnumerable<Consumer> consumers)
    {
        Consumers = consumers.ToArray();
        Patterns = consumers.SelectMany(static (x) => x.Patterns).ToArray();
        Tokens = consumers.Select(static (x) => x.Token).ToArray();
    }

    public WorkingSet(params Consumer[] consumers)
    {
        Consumers = consumers.ToArray();
        Patterns = consumers.SelectMany(static (x) => x.Patterns).ToArray();
        Tokens = consumers.Select(static (x) => x.Token).ToArray();
    }

    public WorkingSet(IEnumerable<TokenInfo> tokens)
    {
        Tokens = tokens.ToArray();
        Consumers = tokens.SelectMany(static (x) => x.GetConsumers()).ToArray();
        Patterns = Consumers.SelectMany(static (x) => x.Patterns).ToArray();
    }

    public WorkingSet(params TokenInfo[] tokens)
    {
        Tokens = tokens.ToArray();
        Consumers = tokens.SelectMany(static (x) => x.GetConsumers()).ToArray();
        Patterns = Consumers.SelectMany(static (x) => x.Patterns).ToArray();
    }

    public WorkingSet(TokenInfo token, Consumer consumer, params Pattern[] patterns)
    {
        Patterns = patterns;
        Consumers = new[] { consumer };
        Tokens = new[] { token };
    }
    #endregion
}
