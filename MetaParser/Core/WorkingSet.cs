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
    private SortedSet<Pattern> sortedPatterns = new SortedSet<Pattern>(PatternComparer.Instance);
    #endregion

    #region Properties
    public TokenInfo[] Tokens { get; set; } = Array.Empty<TokenInfo>();

    public Consumer[] Consumers { get; set; } = Array.Empty<Consumer>();

    [Obsolete("Try to use SortedPatterns instead")]
    public Pattern[] Patterns { get; set; } = Array.Empty<Pattern>();
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
        sortedPatterns = new SortedSet<Pattern>(Patterns, PatternComparer.Instance);
    }

    public WorkingSet(params Consumer[] consumers)
    {
        Consumers = consumers.ToArray();
        Patterns = consumers.SelectMany(static (x) => x.Patterns).ToArray();
        Tokens = consumers.Select(static (x) => x.Token).ToArray();
        sortedPatterns = new SortedSet<Pattern>(Patterns, PatternComparer.Instance);
    }

    public WorkingSet(IEnumerable<TokenInfo> tokens)
    {
        Tokens = tokens.ToArray();
        Consumers = tokens.SelectMany(static (x) => x.GetConsumers()).ToArray();
        Patterns = Consumers.SelectMany(static (x) => x.Patterns).ToArray();
        sortedPatterns = new SortedSet<Pattern>(Patterns, PatternComparer.Instance);
    }

    public WorkingSet(params TokenInfo[] tokens)
    {
        Tokens = tokens.ToArray();
        Consumers = tokens.SelectMany(static (x) => x.GetConsumers()).ToArray();
        Patterns = Consumers.SelectMany(static (x) => x.Patterns).ToArray();
        sortedPatterns = new SortedSet<Pattern>(Patterns, PatternComparer.Instance);
    }

    public WorkingSet(TokenInfo token, Consumer consumer, params Pattern[] patterns)
    {
        Patterns = patterns;
        Consumers = new[] { consumer };
        Tokens = new[] { token };
        sortedPatterns = new SortedSet<Pattern>(Patterns, PatternComparer.Instance);
    }
    #endregion
}
