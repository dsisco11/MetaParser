using MetaParser.Parsing.Constructs;

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
    #region Properties
    public TokenInfo[] Tokens { get; set; } = Array.Empty<TokenInfo>();

    public Consumer[] Consumers { get; set; } = Array.Empty<Consumer>();

    public Pattern[] Patterns { get; set; } = Array.Empty<Pattern>();
    #endregion

    #region Constructors
    public WorkingSet()
    {
    }

    public WorkingSet(IEnumerable<Consumer> consumers)
    {
        Consumers = consumers.ToArray();
        Patterns = consumers.SelectMany(static (x) => x.Patterns).Distinct().ToArray();
        Tokens = consumers.Select(static (x) => x.Token).Distinct().ToArray();
        Sort();
    }

    public WorkingSet(params Consumer[] consumers)
    {
        Consumers = consumers.ToArray();
        Patterns = consumers.SelectMany(static (x) => x.Patterns).Distinct().ToArray();
        Tokens = consumers.Select(static (x) => x.Token).Distinct().ToArray();
        Sort();
    }

    public WorkingSet(IEnumerable<TokenInfo> tokens)
    {
        Tokens = tokens.ToArray();
        Consumers = tokens.SelectMany(static (x) => x.GetConsumers()).Distinct().ToArray();
        Patterns = Consumers.SelectMany(static (x) => x.Patterns).Distinct().ToArray();
        Sort();
    }

    public WorkingSet(params TokenInfo[] tokens)
    {
        Tokens = tokens.ToArray();
        Consumers = tokens.SelectMany(static (x) => x.GetConsumers()).Distinct().ToArray();
        Patterns = Consumers.SelectMany(static (x) => x.Patterns).Distinct().ToArray();
        Sort();
    }

    public WorkingSet(TokenInfo token, Consumer consumer, params Pattern[] patterns)
    {
        Patterns = patterns;
        Consumers = new[] { consumer };
        Tokens = new[] { token };
        Sort();
    }
    #endregion

    #region Methods
    // method to sort all arrays
    public void Sort()
    {
        Array.Sort(Tokens);
        Array.Sort(Consumers);
        Array.Sort(Patterns);
    }
    #endregion
}
