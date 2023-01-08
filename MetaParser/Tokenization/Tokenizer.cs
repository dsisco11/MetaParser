using System.Buffers;
using System.Runtime.CompilerServices;

namespace MetaParser
{
    /// <summary>
    /// Provides access to a genericized, consumable stream of data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Tokenizer<T> : ITokenizer<T>, IReadOnlyTokenizer<T> where T : unmanaged, IEquatable<T>
    {
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
        private Range Bounds => Range.StartAt(Position);

        public int Position { get; private set; } = 0;
        public readonly T EOF_ITEM = default;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new stream from a memory pointer
        /// </summary>
        /// <param name="Memory"></param>
        /// <param name="EOF_ITEM"></param>
        public Tokenizer(ReadOnlyMemory<T> Memory)
        {
            Data = Memory;
            DataSeq = new(Data);
            CurrentSeq = DataSeq;
        }

        /// <summary>
        /// Creates a new stream from a memory pointer
        /// </summary>
        /// <param name="Memory"></param>
        /// <param name="EOF_ITEM"></param>
        public Tokenizer(ReadOnlyMemory<T> Memory, T EOF_ITEM) : this(Memory)
        {
            this.EOF_ITEM = EOF_ITEM;
        }
        #endregion

        #region Accessors
        public long Length => DataSeq.Length;// Data.Length;
        public long Remaining => CurrentSeq.Length;//(Length - Position);
        /// <summary>
        /// Returns the next item to be consumed, equivalent to calling Peek(0)
        /// </summary>
        [Obsolete]
        public T Next => Peek(0);
        /// <summary>
        /// Returns the next item to be consumed, equivalent to calling Peek(1)
        /// </summary>
        [Obsolete]
        public T NextNext => Peek(1);
        /// <summary>
        /// Returns the next item to be consumed, equivalent to calling Peek(2)
        /// </summary>
        [Obsolete]
        public T NextNextNext => Peek(2);

        /// <summary>
        /// Returns whether the stream position is currently at the end of the stream
        /// </summary>
        public bool atEnd => CurrentSeq.IsEmpty;

        /// <summary>
        /// Returns whether the next character in the stream is the EOF character
        /// </summary>
        public bool atEOF => object.Equals(CurrentSeq.FirstSpan[0], EOF_ITEM);
        #endregion

        #region Reader
        public SequenceReader<T> Get_Reader()
        {
            return new SequenceReader<T>(CurrentSeq);
        }

        public SequenceReader<T> Get_Reader(Range range)
        {
            var r = range.GetOffsetAndLength((int)CurrentSeq.Length);
            return new SequenceReader<T>(CurrentSeq.Slice(r.Offset, r.Length));
        }

        public T Peek(Index index)
        {
            var offset = index.GetOffset((int)Remaining);
            return Get_Reader().TryPeek(offset, out T val) ? val : EOF_ITEM;
        }
        #endregion

        #region Consuming
        /// <summary>
        /// Consumes the given <paramref name="reader"/>, progressing the stream and returning the difference.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public ReadOnlySequence<T> Consume(ref SequenceReader<T> reader)
        {
            var consumed = CurrentSeq.Slice(0, reader.Position);
            CurrentSeq = CurrentSeq.Slice(reader.Position);
            return consumed;
        }

        public ReadOnlySequence<T> Consume(Index endIndex)
        {
            var index = endIndex.GetOffset((int)Remaining);
            var consumed = CurrentSeq.Slice(0, index);
            CurrentSeq = CurrentSeq.Slice(index);
            return consumed;
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

        #region Cloning
        /// <summary>
        /// Creates and returns a copy of this stream
        /// </summary>
        /// <returns></returns>
        public Tokenizer<T> Clone()
        {
            return new Tokenizer<T>(Data, EOF_ITEM);
        }
        #endregion

        #region Overrides
        public override string ToString() => Data[Bounds].ToString();
        #endregion
    }
}
