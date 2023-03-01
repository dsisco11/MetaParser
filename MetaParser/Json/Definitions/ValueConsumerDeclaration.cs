using MetaParser.Structs;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal sealed record ValueConsumerDeclaration : ConsumerDeclaration<ValuePatternDeclaration>
{
    [JsonPropertyName("$type")]
    public override EConsumerType Type { get => EConsumerType.Data; set { } }

    [JsonConstructor]
    public ValueConsumerDeclaration(IEnumerable<ValuePatternDeclaration>? start, IEnumerable<ValuePatternDeclaration>? consume, IEnumerable<ValuePatternDeclaration>? stop, IEnumerable<ValuePatternDeclaration>? escape) : base(start, consume, stop, escape)
    {
    }
}

#if false
internal sealed record ValueConsumerDeclaration : IConsumerDeclaration
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
    IEnumerable<PatternDeclaration> IConsumerDeclaration.Start => Start as IEnumerable<PatternDeclaration>;
    IEnumerable<PatternDeclaration> IConsumerDeclaration.Consume => Consume as IEnumerable<PatternDeclaration>;
    IEnumerable<PatternDeclaration> IConsumerDeclaration.Stop => Stop as IEnumerable<PatternDeclaration>;
    IEnumerable<PatternDeclaration> IConsumerDeclaration.Escape => Escape as IEnumerable<PatternDeclaration>;
    #endregion

    [JsonConstructor]
    public ValueConsumerDeclaration(IEnumerable<ValuePattern>? start, IEnumerable<ValuePattern>? consume, IEnumerable<ValuePattern>? stop, IEnumerable<ValuePattern>? escape)
    {
        Start = start ?? Array.Empty<ValuePattern>();
        Consume = consume ?? Array.Empty<ValuePattern>();
        Stop = stop ?? Array.Empty<ValuePattern>();
        Escape = escape ?? Array.Empty<ValuePattern>();
    }
}
#endif