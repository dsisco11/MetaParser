using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal sealed record SyntaxStageDefinition : ParsingStageDefinition<TokenConsumerDeclaration>
{
    [JsonIgnore]
    public override EParsingStage Type => EParsingStage.Syntax;

    #region Properties


    [JsonPropertyName("consumers")]
    public override Dictionary<string, IEnumerable<TokenConsumerDeclaration>>? Consumers { get; set; }
    [JsonPropertyName("ignored")]
    public override string[] Ignored { get; set; }
    [JsonPropertyName("dropped")]
    public override TokenConsumerDeclaration[] Dropped { get; set; }
    #endregion

    #region Constructors
    [JsonConstructor]
    public SyntaxStageDefinition(Dictionary<string, IEnumerable<TokenConsumerDeclaration>>? consumers, string[]? ignored, TokenConsumerDeclaration[]? dropped)
    {
        Consumers = consumers;
        Ignored = ignored ?? Array.Empty<string>();
        Dropped = dropped ?? Array.Empty<TokenConsumerDeclaration>();
    }
    #endregion
}
