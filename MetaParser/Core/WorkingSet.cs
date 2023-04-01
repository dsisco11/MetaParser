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
    public TokenEntity[] Tokens { get; set; } = Array.Empty<TokenEntity>();

    public ConsumerEntity[] Consumers { get; set; } = Array.Empty<ConsumerEntity>();

    public PatternEntity[] Patterns { get; set; } = Array.Empty<PatternEntity>();
    #endregion

    #region Constructors
    public WorkingSet()
    {
    }

    public WorkingSet(IEnumerable<ConsumerEntity> consumers)
    {
        Consumers = consumers.ToArray();
        Patterns = consumers.SelectMany(static (x) => x.Patterns).Distinct().ToArray();
        Tokens = consumers.Select(static (x) => x.Token).Distinct().ToArray();
        Sort();
    }

    public WorkingSet(params ConsumerEntity[] consumers)
    {
        Consumers = consumers.ToArray();
        Patterns = consumers.SelectMany(static (x) => x.Patterns).Distinct().ToArray();
        Tokens = consumers.Select(static (x) => x.Token).Distinct().ToArray();
        Sort();
    }

    public WorkingSet(IEnumerable<TokenEntity> tokens)
    {
        Tokens = tokens.ToArray();
        Consumers = tokens.SelectMany(static (x) => x.GetConsumers()).Distinct().ToArray();
        Patterns = Consumers.SelectMany(static (x) => x.Patterns).Distinct().ToArray();
        Sort();
    }

    public WorkingSet(params TokenEntity[] tokens)
    {
        Tokens = tokens.ToArray();
        Consumers = tokens.SelectMany(static (x) => x.GetConsumers()).Distinct().ToArray();
        Patterns = Consumers.SelectMany(static (x) => x.Patterns).Distinct().ToArray();
        Sort();
    }

    public WorkingSet(TokenEntity token, ConsumerEntity consumer, params PatternEntity[] patterns)
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
