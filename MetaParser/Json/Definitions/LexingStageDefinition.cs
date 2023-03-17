using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal sealed class LexingStageDefinition : ParsingStageDefinition<ValueConsumerDeclaration>
{
    [JsonIgnore]
    public override EParsingStage Stage => EParsingStage.Lexing;

    #region Properties
    [JsonPropertyName("consumers")]
    public override Dictionary<string, IEnumerable<ValueConsumerDeclaration>>? Consumers { get; }
    #endregion
}
