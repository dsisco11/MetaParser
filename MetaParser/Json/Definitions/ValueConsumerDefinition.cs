using MetaParser.Structs;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal sealed record ValueConsumerDefinition : IConsumerDefinition
{
    #region Properties
    [JsonPropertyName("$type")]
    public EConsumerType Type => EConsumerType.Data;

    [JsonPropertyName("start")]
    public IEnumerable<ValuePattern> Start { get; set; }

    [JsonPropertyName("consume")]
    public IEnumerable<ValuePattern> Consume { get; set; }

    [JsonPropertyName("stop")]
    public IEnumerable<ValuePattern> Stop { get; set; }

    [JsonPropertyName("escape")]
    public IEnumerable<ValuePattern> Escape { get; set; }
    #endregion

    #region IConsumerDefinition
    IEnumerable<PatternDefinition> IConsumerDefinition.Start => Start as IEnumerable<PatternDefinition>;
    IEnumerable<PatternDefinition> IConsumerDefinition.Consume => Consume as IEnumerable<PatternDefinition>;
    IEnumerable<PatternDefinition> IConsumerDefinition.Stop => Stop as IEnumerable<PatternDefinition>;
    IEnumerable<PatternDefinition> IConsumerDefinition.Escape => Escape as IEnumerable<PatternDefinition>;
    #endregion

    [JsonConstructor]
    public ValueConsumerDefinition(IEnumerable<ValuePattern>? start, IEnumerable<ValuePattern>? consume, IEnumerable<ValuePattern>? stop, IEnumerable<ValuePattern>? escape)
    {
        Start = start ?? Array.Empty<ValuePattern>();
        Consume = consume ?? Array.Empty<ValuePattern>();
        Stop = stop ?? Array.Empty<ValuePattern>();
        Escape = escape ?? Array.Empty<ValuePattern>();
    }
}
