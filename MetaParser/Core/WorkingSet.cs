using MetaParser.Parsing.Constructs;

using System;

namespace MetaParser.Core;

/// <summary>
/// Holds a list of tokens, consumers, or patterns that are being targeted by the current action
/// </summary>
internal record WorkingSet
{
    public TokenInfo[] Tokens { get; set; } = Array.Empty<TokenInfo>();
    public Consumer[] Consumers { get; set; } = Array.Empty<Consumer>();
    public Pattern[] Patterns { get; set; } = Array.Empty<Pattern>();
}
