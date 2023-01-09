using System.Buffers;

namespace MetaParser
{
    public interface ITokenizer<T> where T : IEquatable<T>
    {
        bool End { get; }
        long Length { get; }
        long Remaining { get; }

        ITokenReader<T> GetReader();

        bool TryConsume(ITokenReader<T> Reader, out ReadOnlySequence<T> Consumed);

        #region ToString
        string ToString();
        #endregion
    }
}