using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

internal record ParsingStages
{
    [JsonPropertyName("lexer")]
    public LexingStageDefinition? LexingStage { get; set; }

    [JsonPropertyName("syntax")]
    public GrammarStageDefinition? GrammarStage { get; set; }
}
