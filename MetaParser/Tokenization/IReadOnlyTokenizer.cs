using System.Buffers;

namespace MetaParser
{
    public interface IReadOnlyTokenizer<T> where T : unmanaged, IEquatable<T>
    {
        bool atEnd { get; }
        bool atEOF { get; }
        long Length { get; }
        T Next { get; }
        T NextNext { get; }
        T NextNextNext { get; }
        int Position { get; }
        long Remaining { get; }

        Tokenizer<T> Clone();
        SequenceReader<T> Get_Reader();
        SequenceReader<T> Get_Reader(Range range);
        T Peek(Index index);
        ReadOnlyMemory<T> Slice(Index index = default);
        ReadOnlyMemory<T> Slice(Range range);
        string ToString();
    }
}