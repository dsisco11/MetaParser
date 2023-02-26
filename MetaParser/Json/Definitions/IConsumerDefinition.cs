namespace MetaParser.Json.Definitions;

internal interface IConsumerDefinition
{
    ETokenType Type { get; }
    public PatternDefinition[] Start { get; }
    public PatternDefinition[] Consume { get; }
    public PatternDefinition[] Stop { get; }
    public PatternDefinition[] Escape { get; }
}