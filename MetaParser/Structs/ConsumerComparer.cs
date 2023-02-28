using System.Collections.Generic;

namespace MetaParser.Structs
{
    internal class ConsumerComparer : IComparer<PatternConsumer>
    {
        public static ConsumerComparer Instance = new ConsumerComparer();

        public int Compare(PatternConsumer x, PatternConsumer y)
        {
            return x.TokenIndex.CompareTo(y.TokenIndex);
        }
    }
}
