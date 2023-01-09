using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MetaParser
{
    /// <inheritdoc cref="ITokenizer{T}"/>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("{ToString(),raw}")]
    public sealed class UnmanagedTokenSequencer<T> : ITokenizer<T> where T : unmanaged, IEquatable<T>
    {
        #region Fields
        private readonly T EOF_ITEM;
        #endregion

        #region Properties
        /// <summary>
        /// Our stream of tokens
        /// </summary>
        private ReadOnlyMemory<T> Data { get; init; }
        private ReadOnlySequence<T> DataSeq { get; init; }
        private ReadOnlySequence<T> CurrentSeq;

        /// <summary>
        /// The current working range of the stream, that is, the range of unconsumed data.
        /// </summary>
        private Range Bounds => Range.StartAt(Consumed);

        public int Consumed { get; private set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new stream from a memory pointer
        /// </summary>
        /// <param name="Memory"></param>
        /// <param name="EOF_ITEM"></param>
        public UnmanagedTokenSequencer(ReadOnlyMemory<T> Memory)
        {
            Data = Memory;
            DataSeq = new(Data);
            CurrentSeq = DataSeq;
        }

        /// <summary>
        /// Creates a new stream from a memory pointer
        /// </summary>
        /// <param name="Memory"></param>
        /// <param name="EOF"></param>
        public UnmanagedTokenSequencer(ReadOnlyMemory<T> Memory, T EOF) : this(Memory)
        {
            this.EOF_ITEM = EOF;
        }
        #endregion

        #region Accessors
        public bool End => CurrentSeq.IsEmpty;
        public long Length => DataSeq.Length;
        public long Remaining => CurrentSeq.Length;
        #endregion

        #region Reader
        //public SequenceReader<T> GetReader()
        //{
        //    return new SequenceReader<T>(CurrentSeq);
        //}

        //public SequenceReader<T> GetReader(Range range)
        //{
        //    var r = range.GetOffsetAndLength((int)CurrentSeq.Length);
        //    return new SequenceReader<T>(CurrentSeq.Slice(r.Offset, r.Length));
        //}

        ITokenReader<T> ITokenizer<T>.GetReader()
        {
            return new UnmanagedTokenReader<T>(CurrentSeq);
        }
        #endregion

        #region Consuming
        /// <summary>
        /// Consumes the given <paramref name="Reader"/>, progressing the stream and returning the difference.
        /// </summary>
        /// <param name="Reader"></param>
        /// <returns></returns>
        public bool TryConsume(ITokenReader<T> Reader, out ReadOnlySequence<T> Consumed)
        {
            Consumed = CurrentSeq.Slice(0, Reader.Consumed);
            CurrentSeq = CurrentSeq.Slice(Reader.Consumed);
            return !Consumed.IsEmpty;
        }
        #endregion

        #region Slicing

        /// <summary>
        /// Returns a slice of this streams memory beginning at the streams current position
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyMemory<T> Slice(Index index = default)
        {
            return Data[Bounds][index..];
        }

        /// <summary>
        /// Returns a slice of this streams memory beginning at the streams current position
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyMemory<T> Slice(Range range)
        {
            return Data[Bounds][range];
        }
        #endregion

        #region Overrides
        public override string ToString() => Data[Bounds].ToString();
        #endregion
    }
}
