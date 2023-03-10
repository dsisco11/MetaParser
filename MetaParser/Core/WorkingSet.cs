using MetaParser.Consumers;
using MetaParser.Patternization;
using MetaParser.Tokens;

using System;

namespace MetaParser.Core;

/// <summary>
/// Holds a list of tokens, consumers, or patterns that are being targeted by the current action
/// </summary>
internal record WorkingSet
{
    public TokenInfo[] Tokens { get; set; } = Array.Empty<TokenInfo>();
    public TokenConsumer[] Consumers { get; set; } = Array.Empty<TokenConsumer>();
    public Pattern[] Patterns { get; set; } = Array.Empty<Pattern>();
}
