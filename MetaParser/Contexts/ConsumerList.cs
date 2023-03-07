using MetaParser.Consumers;

using System;
using System.Collections.Immutable;

namespace MetaParser.Contexts
{
    internal sealed record ConsumerList
    {
        /// <summary>
        /// Complete list of all tokens defined
        /// </summary>
        public ImmutableArray<ConsumerInfo> CompleteSet { get; set; } = ImmutableArray<ConsumerInfo>.Empty;

        /// <summary>
        /// Set of tokens being targeted by the current action
        /// </summary>
        public ConsumerInfo[] WorkingSet { get; set; } = Array.Empty<ConsumerInfo>();
    }
}
