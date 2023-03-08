using MetaParser.Consumers;

using System;
using System.Collections.Immutable;

namespace MetaParser.Core;

internal sealed record ConsumerList
{
    /// <summary>
    /// Complete list of all tokens defined
    /// </summary>
    public ImmutableArray<TokenConsumer> CompleteSet { get; set; } = ImmutableArray<TokenConsumer>.Empty;

    /// <summary>
    /// Set of tokens being targeted by the current action
    /// </summary>
    public TokenConsumer[] WorkingSet { get; set; } = Array.Empty<TokenConsumer>();
}
