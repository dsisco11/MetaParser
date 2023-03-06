using MetaParser.Consumers;

using System;
using System.Collections.Generic;

namespace MetaParser.Tokens;

internal record TokenInfo
{
    #region Properties
    public readonly int Index;
    public readonly string Name;
    public readonly List<ConsumerInfo> Consumers;
    #endregion

    public TokenInfo(int index, string name)
    {
        Index = index;
        Name = name;
        Consumers = new();
    }
}
