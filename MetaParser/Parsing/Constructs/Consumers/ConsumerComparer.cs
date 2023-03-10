using System.Collections.Generic;

using MetaParser.Parsing.Constructs.Consumers;

namespace MetaParser.Consumers;

internal class ConsumerComparer : IComparer<TokenConsumer>
{
    public static ConsumerComparer Instance = new ConsumerComparer();

    public int Compare(TokenConsumer x, TokenConsumer y)
    {
        return x.Token.Index.CompareTo(y.Token.Index);
    }
}
