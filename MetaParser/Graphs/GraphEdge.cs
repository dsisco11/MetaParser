namespace MetaParser.Graphs
{
    public sealed record GraphEdge<T>
    {
        public readonly T Source, Destination;
    }
}