using MetaParser.Parsing.Constructs.Patternization;

namespace MetaParser.Parsing.Constructs.Consumers;

internal record ConsumerClauseInfo
{
    public PatternGroup? Start { get; set; }
    public PatternGroup? Consume { get; set; }
    public PatternGroup? Stop { get; set; }
    public PatternGroup? Escape { get; set; }
}
