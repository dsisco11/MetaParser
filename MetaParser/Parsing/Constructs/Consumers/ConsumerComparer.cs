using System.Collections.Generic;

namespace MetaParser.Parsing.Constructs;

internal class ConsumerComparer : IComparer<Consumer>
{
    public static ConsumerComparer Instance = new ConsumerComparer();

    public int Compare(Consumer x, Consumer y)
    {
        return x.Token.Index.CompareTo(y.Token.Index);
    }
}
