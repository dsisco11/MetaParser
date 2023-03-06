using System.Collections.Generic;

namespace MetaParser.Consumers
{
    internal class ConsumerComparer : IComparer<ConsumerInfo>
    {
        public static ConsumerComparer Instance = new ConsumerComparer();

        public int Compare(ConsumerInfo x, ConsumerInfo y)
        {
            return x.Token.Index.CompareTo(y.Token.Index);
        }
    }
}
