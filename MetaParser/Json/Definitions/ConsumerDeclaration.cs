using MetaParser.Parsing.Constructs.Consumers;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal abstract record ConsumerDeclaration<T> : IConsumerDeclaration
    where T : IPatternDeclaration
{
    #region Properties
    [JsonPropertyName("$type")]
    public abstract EConsumerType Type { get; set; }

    [JsonPropertyName("start")]
    public IEnumerable<T> Start { get; set; }

    [JsonPropertyName("consume")]
    public IEnumerable<T> Consume { get; set; }

    [JsonPropertyName("stop")]
    public IEnumerable<T> Stop { get; set; }

    [JsonPropertyName("escape")]
    public IEnumerable<T> Escape { get; set; }

    IEnumerable<IPatternDeclaration> IConsumerDeclaration.Start => Start as IEnumerable<IPatternDeclaration>;
    IEnumerable<IPatternDeclaration> IConsumerDeclaration.Consume => Consume as IEnumerable<IPatternDeclaration>;
    IEnumerable<IPatternDeclaration> IConsumerDeclaration.Stop => Stop as IEnumerable<IPatternDeclaration>;
    IEnumerable<IPatternDeclaration> IConsumerDeclaration.Escape => Escape as IEnumerable<IPatternDeclaration>;
    #endregion

    [JsonConstructor]
    protected ConsumerDeclaration(IEnumerable<T>? start, IEnumerable<T>? consume, IEnumerable<T>? stop, IEnumerable<T>? escape)
    {
        Start = start ?? Array.Empty<T>();
        Consume = consume ?? Array.Empty<T>();
        Stop = stop ?? Array.Empty<T>();
        Escape = escape ?? Array.Empty<T>();
    }
}
