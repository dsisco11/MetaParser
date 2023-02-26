using MetaParser.Json.JsonTypeConverters;

using System;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(ValueConsumer), typeDiscriminator: "constant")]
[JsonDerivedType(typeof(TokenConsumer), typeDiscriminator: "compound")]
internal abstract record ConsumerDefinition<T> : IConsumerDefinition
    where T : PatternDefinition
{
    #region Properties
    [JsonPropertyName("$type")]
    public ETokenType Type { get; set; }

    [JsonPropertyName("start")]
    [JsonConverter(typeof(JsonOneOrManyConverter))]
    public T[] Start { get; set; }

    [JsonPropertyName("consume")]
    [JsonConverter(typeof(JsonOneOrManyConverter))]
    public T[] Consume { get; set; }

    [JsonPropertyName("stop")]
    [JsonConverter(typeof(JsonOneOrManyConverter))]
    public T[] Stop { get; set; }

    [JsonPropertyName("escape")]
    [JsonConverter(typeof(JsonOneOrManyConverter))]
    public T[] Escape { get; set; }

    PatternDefinition[] IConsumerDefinition.Start => throw new NotImplementedException();

    PatternDefinition[] IConsumerDefinition.Consume => throw new NotImplementedException();

    PatternDefinition[] IConsumerDefinition.Stop => throw new NotImplementedException();

    PatternDefinition[] IConsumerDefinition.Escape => throw new NotImplementedException();
    #endregion

    [JsonConstructor]
    protected ConsumerDefinition(ETokenType type, T[]? start, T[]? consume, T[]? stop, T[]? escape)
    {
        Type = type;
        Start = start ?? Array.Empty<T>();
        Consume = consume ?? Array.Empty<T>();
        Stop = stop ?? Array.Empty<T>();
        Escape = escape ?? Array.Empty<T>();
    }
}

internal sealed record ValueConsumer : ConsumerDefinition<ValuePattern>
{
    [JsonConstructor]
    public ValueConsumer(ETokenType type, ValuePattern[]? start, ValuePattern[]? consume, ValuePattern[]? stop, ValuePattern[]? escape) : base(type, start, consume, stop, escape)
    {
    }
}

internal sealed record TokenConsumer : ConsumerDefinition<TokenPattern>
{
    [JsonConstructor]
    public TokenConsumer(ETokenType type, TokenPattern[]? start, TokenPattern[]? consume, TokenPattern[]? stop, TokenPattern[]? escape) : base(type, start, consume, stop, escape)
    {
    }
}
