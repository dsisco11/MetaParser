using MetaParser.Structs;

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal sealed record TokenConsumerDeclaration : ConsumerDeclaration<TokenPattern>
{
    [JsonPropertyName("$type")]
    public override EConsumerType Type { get => EConsumerType.Token; set { } }

    [JsonConstructor]
    public TokenConsumerDeclaration(IEnumerable<TokenPattern>? start, IEnumerable<TokenPattern>? consume, IEnumerable<TokenPattern>? stop, IEnumerable<TokenPattern>? escape) : base(start, consume, stop, escape)
    {
    }
}

#if false
internal sealed record TokenConsumerDeclaration : IConsumerDeclaration
{
    #region Properties
    [JsonPropertyName("$type")]
    public EConsumerType Type => EConsumerType.Token;

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
    IEnumerable<PatternDeclaration> IConsumerDeclaration.Start => Start as IEnumerable<PatternDeclaration>;
    IEnumerable<PatternDeclaration> IConsumerDeclaration.Consume => Consume as IEnumerable<PatternDeclaration>;
    IEnumerable<PatternDeclaration> IConsumerDeclaration.Stop => Stop as IEnumerable<PatternDeclaration>;
    IEnumerable<PatternDeclaration> IConsumerDeclaration.Escape => Escape as IEnumerable<PatternDeclaration>;
    #endregion

    [JsonConstructor]
    public TokenConsumerDeclaration(IEnumerable<TokenPattern>? start, IEnumerable<TokenPattern>? consume, IEnumerable<TokenPattern>? stop, IEnumerable<TokenPattern>? escape)
    {
        Start = start ?? Array.Empty<TokenPattern>();
        Consume = consume ?? Array.Empty<TokenPattern>();
        Stop = stop ?? Array.Empty<TokenPattern>();
        Escape = escape ?? Array.Empty<TokenPattern>();
    }
}
#endif