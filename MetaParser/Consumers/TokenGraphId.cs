namespace MetaParser.Consumers;

internal record TokenGraphId
{
    public readonly int TokenIndex;
    public readonly int ConsumerIndex;

    public TokenGraphId(int tokenIndex, int consumerIndex)
    {
        TokenIndex = tokenIndex;
        ConsumerIndex = consumerIndex;
    }
}
