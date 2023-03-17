using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal sealed record LexingStageDefinition : ParsingStageDefinition<ValueConsumerDeclaration>
{
    [JsonIgnore]
    public override EParsingStage Type => EParsingStage.Lexer;

    #region Properties
    [JsonPropertyName("consumers")]
    public override Dictionary<string, IEnumerable<ValueConsumerDeclaration>>? Consumers { get; set; }

    #endregion

    #region Constructors
    [JsonConstructor]
    public LexingStageDefinition(Dictionary<string, IEnumerable<ValueConsumerDeclaration>>? consumers)
    {
        Consumers = consumers;
    }
    #endregion
}
