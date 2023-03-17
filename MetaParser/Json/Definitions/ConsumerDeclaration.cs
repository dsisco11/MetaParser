using MetaParser.Parsing.Constructs;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal abstract record ConsumerDeclaration<T> : IConsumerDeclaration
    where T : IPatternDeclaration
{
    #region Properties
    public abstract EConsumerType Type { get; }

    [JsonPropertyName("start")]
    public IEnumerable<T> Start { get; set; }

    [JsonPropertyName("consume")]
    public IEnumerable<T> Consume { get; set; }

    [JsonPropertyName("stop")]
    public IEnumerable<T> Stop { get; set; }

    [JsonPropertyName("escape")]
    public IEnumerable<T> Escape { get; set; }

    IEnumerable<IPatternDeclaration> IConsumerDeclaration.Start => (IEnumerable<IPatternDeclaration>) Start;
    IEnumerable<IPatternDeclaration> IConsumerDeclaration.Consume => (IEnumerable<IPatternDeclaration>) Consume;
    IEnumerable<IPatternDeclaration> IConsumerDeclaration.Stop => (IEnumerable<IPatternDeclaration>) Stop;
    IEnumerable<IPatternDeclaration> IConsumerDeclaration.Escape => (IEnumerable<IPatternDeclaration>) Escape;
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
