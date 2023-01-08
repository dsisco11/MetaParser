using System.Buffers;

namespace MetaParser
{
    public interface IReadOnlyTokenizer<T> where T : unmanaged, IEquatable<T>
    {
        bool AtEnd { get; }
        bool AtEOF { get; }
        long Length { get; }
        int Position { get; }
        long Remaining { get; }

        Tokenizer<T> Clone();
        SequenceReader<T> GetReader();
        SequenceReader<T> GetReader(Range range);
        T Peek(Index index);
        ReadOnlyMemory<T> Slice(Index index = default);
        ReadOnlyMemory<T> Slice(Range range);
        string ToString();
    }
}