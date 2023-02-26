using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal record ConsumerDeclaration<T> : IConsumerDefinition
    where T : PatternDefinition
{
    #region Properties
    [JsonPropertyName("$type")]
    public string Type { get; set; }

    [JsonPropertyName("start")]
    public IEnumerable<T> Start { get; set; }

    [JsonPropertyName("consume")]
    public IEnumerable<T> Consume { get; set; }

    [JsonPropertyName("stop")]
    public IEnumerable<T> Stop { get; set; }

    [JsonPropertyName("escape")]
    public IEnumerable<T> Escape { get; set; }

    IEnumerable<PatternDefinition> IConsumerDefinition.Start => Start as IEnumerable<PatternDefinition>;
    IEnumerable<PatternDefinition> IConsumerDefinition.Consume => Consume as IEnumerable<PatternDefinition>;
    IEnumerable<PatternDefinition> IConsumerDefinition.Stop => Stop as IEnumerable<PatternDefinition>;
    IEnumerable<PatternDefinition> IConsumerDefinition.Escape => Escape as IEnumerable<PatternDefinition>;
    #endregion

    [JsonConstructor]
    protected ConsumerDeclaration(string type, IEnumerable<T>? start, IEnumerable<T>? consume, IEnumerable<T>? stop, IEnumerable<T>? escape)
    {
        Type = type;
        Start = start ?? Array.Empty<T>();
        Consume = consume ?? Array.Empty<T>();
        Stop = stop ?? Array.Empty<T>();
        Escape = escape ?? Array.Empty<T>();
    }
}

internal sealed record ValueConsumerDeclaration : IConsumerDefinition
{
    #region Properties
    [JsonPropertyName("$type")]
    public string Type { get; set; }

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
    public ValueConsumerDeclaration(string type, IEnumerable<ValuePattern>? start, IEnumerable<ValuePattern>? consume, IEnumerable<ValuePattern>? stop, IEnumerable<ValuePattern>? escape)
    {
        Type = type;
        Start = start ?? Array.Empty<ValuePattern>();
        Consume = consume ?? Array.Empty<ValuePattern>();
        Stop = stop ?? Array.Empty<ValuePattern>();
        Escape = escape ?? Array.Empty<ValuePattern>();
    }
}

internal sealed record TokenConsumerDeclaration : IConsumerDefinition
{
    #region Properties
    [JsonPropertyName("$type")]
    public string Type { get; set; }

    [JsonPropertyName("start")]
    public IEnumerable<TokenPattern> Start { get; set; }

    [JsonPropertyName("consume")]
    public IEnumerable<TokenPattern> Consume { get; set; }

    [JsonPropertyName("stop")]
    public IEnumerable<TokenPattern> Stop { get; set; }

    [JsonPropertyName("escape")]
    public IEnumerable<TokenPattern> Escape { get; set; }
    #endregion

    #region IConsumerDefinition
    IEnumerable<PatternDefinition> IConsumerDefinition.Start => Start as IEnumerable<PatternDefinition>;
    IEnumerable<PatternDefinition> IConsumerDefinition.Consume => Consume as IEnumerable<PatternDefinition>;
    IEnumerable<PatternDefinition> IConsumerDefinition.Stop => Stop as IEnumerable<PatternDefinition>;
    IEnumerable<PatternDefinition> IConsumerDefinition.Escape => Escape as IEnumerable<PatternDefinition>;
    #endregion

    [JsonConstructor]
    public TokenConsumerDeclaration(string type, IEnumerable<TokenPattern>? start, IEnumerable<TokenPattern>? consume, IEnumerable<TokenPattern>? stop, IEnumerable<TokenPattern>? escape)
    {
        Type = type;
        Start = start ?? Array.Empty<TokenPattern>();
        Consume = consume ?? Array.Empty<TokenPattern>();
        Stop = stop ?? Array.Empty<TokenPattern>();
        Escape = escape ?? Array.Empty<TokenPattern>();
    }
}
