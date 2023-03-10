using System.Collections.Generic;

using MetaParser.Parsing.Constructs.Consumers;

namespace MetaParser.Consumers;

internal class ConsumerComparer : IComparer<Consumer>
{
    public static ConsumerComparer Instance = new ConsumerComparer();

    public int Compare(Consumer x, Consumer y)
    {
        return x.Token.Index.CompareTo(y.Token.Index);
    }
}
