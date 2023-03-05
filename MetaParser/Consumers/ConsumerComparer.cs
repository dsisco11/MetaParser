using MetaParser.Consumers;

using System.Collections.Generic;

namespace MetaParser.Consumers
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
