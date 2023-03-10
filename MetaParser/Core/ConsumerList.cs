using MetaParser.Parsing.Constructs;

using System;
using System.Collections.Immutable;

namespace MetaParser.Core;

internal sealed record ConsumerList
{
    /// <summary>
    /// Complete list of all tokens defined
    /// </summary>
    public ImmutableArray<Consumer> CompleteSet { get; set; } = ImmutableArray<Consumer>.Empty;

    /// <summary>
    /// Set of tokens being targeted by the current action
    /// </summary>
    public Consumer[] Working { get; set; } = Array.Empty<Consumer>();
}
