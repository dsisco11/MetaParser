using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal sealed record LexingStageDefinition : ParsingStageDefinition<ValueConsumerDeclaration>
{
    [JsonIgnore]
    public override EParsingStage Type => EParsingStage.Lexer;

    #region Properties
    [JsonIgnore]
    public override string[]? Ignored
    {
        get => Array.Empty<string>();
        set => throw new NotSupportedException();
    }

    [JsonPropertyName("dropped")]
    public override ValueConsumerDeclaration[] Dropped { get; set; }


    [JsonPropertyName("consumers")]
    public override Dictionary<string, IEnumerable<ValueConsumerDeclaration>>? Consumers { get; set; }

    #endregion

    #region Constructors
    [JsonConstructor]
    public LexingStageDefinition(Dictionary<string, IEnumerable<ValueConsumerDeclaration>>? consumers, ValueConsumerDeclaration[]? dropped)
    {
        Consumers = consumers;
        Dropped = dropped ?? Array.Empty<ValueConsumerDeclaration>();
    }
    #endregion
}
