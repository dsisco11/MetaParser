using System.Collections.Generic;

namespace MetaParser.Tokens;

internal class TokenInfoComparer : IComparer<TokenInfo>
{
    public static TokenInfoComparer Instance = new TokenInfoComparer();

    public int Compare(TokenInfo x, TokenInfo y)
    {
        return x.Index.CompareTo(y.Index);
    }
}
