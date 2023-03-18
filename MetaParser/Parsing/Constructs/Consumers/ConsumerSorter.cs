using System.Collections.Generic;

namespace MetaParser.Parsing.Constructs.Consumers;

internal class ConsumerSorter : IComparer<Consumer>
{
    public readonly static ConsumerSorter Instance = new ConsumerSorter();

    public int Compare(Consumer x, Consumer y)
    {
        // compare tokens by max node depth
        var depthCompare = x.Token.DependencyInfo.NodeDepth.Max.CompareTo(y.Token.DependencyInfo.NodeDepth.Max);
        if (depthCompare != 0)
        {
            return depthCompare;
        }
        // compare each pattern of each consumer
        var startCompare = x.Start.CompareTo(y.Start);
        if (startCompare != 0)
        {
            return startCompare;
        }

        if (x.Consume is not null)
        {
            if (y.Consume is not null)
            {
                var consumeCompare = x.Consume.CompareTo(y.Consume);
                if (consumeCompare != 0)
                {
                    return consumeCompare;
                }
            }
            return -1;
        }

        if (x.Stop is not null)
        {
            if (y.Stop is not null)
            {
                var stopCompare = x.Stop.CompareTo(y.Stop);
                if (stopCompare != 0)
                {
                    return stopCompare;
                }
            }
            return -1;
        }

        if (x.Escape is not null)
        {
            if (y.Escape is not null)
            {
                var escapeCompare = x.Escape.CompareTo(y.Escape);
                return escapeCompare;
            }
            return -1;
        }

        return 0;
    }
}
