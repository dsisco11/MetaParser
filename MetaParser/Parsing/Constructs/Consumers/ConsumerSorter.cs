using MetaParser.Parsing.Constructs.Patterns;

using System.Collections.Generic;

namespace MetaParser.Parsing.Constructs.Consumers;

internal class ConsumerSorter : IComparer<ConsumerEntity>
{
    public readonly static ConsumerSorter Instance = new ConsumerSorter();

    public int Compare(ConsumerEntity left, ConsumerEntity right)
    {
        // compare each pattern of each consumer
        var startCompare = PatternSorter.Instance.Compare(left.Start, right.Start);
        if (startCompare != 0)
        {
            return startCompare;
        }

        if (left.Consume is not null)
        {
            if (right.Consume is not null)
            {
                var consumeCompare = PatternSorter.Instance.Compare(left.Consume, right.Consume);
                if (consumeCompare != 0)
                {
                    return consumeCompare;
                }
            }
            return -1;
        }

        if (left.Stop is not null)
        {
            if (right.Stop is not null)
            {
                var stopCompare = PatternSorter.Instance.Compare(left.Stop, right.Stop);
                if (stopCompare != 0)
                {
                    return stopCompare;
                }
            }
            return -1;
        }

        if (left.Escape is not null)
        {
            if (right.Escape is not null)
            {
                var escapeCompare = PatternSorter.Instance.Compare(left.Escape, right.Escape);
                return escapeCompare;
            }
            return -1;
        }

        return 0;
    }
}
