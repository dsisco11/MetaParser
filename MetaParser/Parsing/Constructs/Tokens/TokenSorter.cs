using System.Collections.Generic;

namespace MetaParser.Parsing.Constructs.Consumers;

internal class TokenSorter : IComparer<TokenEntity>
{
    public readonly static TokenSorter Instance = new TokenSorter();

    public int Compare(TokenEntity left, TokenEntity right)
    {
        // compare tokens by max node depth
        var depthCompare = left.GraphInfo.Depth.CompareTo(right.GraphInfo.Depth);
        if (depthCompare != 0)
        {
            return depthCompare > 0 ? 1 : -1;
        }

        // compare token names
        var nameCompare = string.CompareOrdinal(left.ID, right.ID);
        if (nameCompare != 0)
        {
            return nameCompare > 0 ? 1 : -1;
        }

        return 0;
    }
}
