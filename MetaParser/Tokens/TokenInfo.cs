using MetaParser.Consumers;
using System.Collections.Generic;

namespace MetaParser.Tokens;

internal record TokenInfo
{
    #region Fields
    public readonly int Index;
    public readonly string Name;
    public readonly List<TokenConsumer> Consumers;
    public readonly TokenGraphId Identity;
    #endregion

    public TokenInfo(int index, string name)
    {
        Index = index;
        Name = name;
        Consumers = new();
        Identity = new TokenGraphId(Index, -1);
    }
}
