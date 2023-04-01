using System.Collections.Generic;
using System.Collections.Immutable;

namespace MetaParser.Parsing.Constructs.Stages;

/// <summary>
/// A processing group is a collection of input values, output values, and consumers from which a single parser stage is constructed.
/// </summary>
internal sealed record ParsingStageContext
{
    #region Fields
    public readonly ImmutableHashSet<string> Inputs;
    public readonly ImmutableHashSet<string> Outputs;
    public readonly ImmutableArray<ConsumerEntity> Consumers;
    #endregion

    #region Constructors
    public ParsingStageContext(IEnumerable<string> inputs, IEnumerable<string> outputs, IEnumerable<ConsumerEntity> consumers)
    {
        Inputs = inputs.ToImmutableHashSet();
        Outputs = outputs.ToImmutableHashSet();
        Consumers = consumers.ToImmutableArray();
    }
    #endregion
}
