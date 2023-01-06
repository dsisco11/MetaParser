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
            return Get_Reader(index..1).TryPeek(out T val) ? val : EOF_ITEM;
        }
        #endregion

        #region Consuming
        /// <summary>
        /// Consumes the given <paramref name="reader"/>, progressing the stream and returning the difference.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public ReadOnlySequence<T> Consume(SequenceReader<T> reader)
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

        //#region Peeking
        ///// <summary>
        ///// Returns the item at +<paramref name="Offset"/> from the current read position
        ///// </summary>
        ///// <param name="Offset">Distance from the current read position at which to peek</param>
        ///// <returns></returns>
        //public T Peek(Index Offset)
        //{
        //    var index = Position + Offset.GetOffset(Remaining);
        //    return index >= Data.Length ? EOF_ITEM : Data.Span[index];
        //}
        //#endregion

        //#region Scanning

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //private bool _scan(T subject, out long outOffset, Range searchRange, IEqualityComparer<T>? comparer)
        //{
        //    //var reader = Get_Reader();
        //    //if (reader.TryAdvanceTo(subject, true))
        //    //{
        //    //    outOffset = reader.Consumed;
        //    //    return true;
        //    //}

        //    var Comparator = comparer ?? EqualityComparer<T>.Default;
        //    var sub = Slice(searchRange);

        //    for (int i = 0; i < sub.Length; i++)
        //    {
        //        var current = sub.Span[0];
        //        if (Comparator.Equals(current, subject))
        //        {
        //            outOffset = i;
        //            return true;
        //        }
        //    }

        //    outOffset = 0;
        //    return false;
        //}
        //#endregion

        //#region Consume
        ///// <summary>
        ///// Returns the first unconsumed item from the stream and progresses the current reading position
        ///// </summary>
        //public T Consume()
        //{
        //    if (Position >= Data.Span.Length) return EOF_ITEM!;

        //    T retVal = Data.Span[Position];
        //    Position += 1;

        //    return retVal;
        //}
        ///// <summary>
        ///// Returns the first unconsumed item from the stream and progresses the current reading position
        ///// </summary>
        //public CastType Consume<CastType>() where CastType : T
        //{
        //    if (Position >= Data.Span.Length) return default(CastType)!;

        //    T retVal = Data.Span[Position];
        //    Position += 1;

        //    return (CastType)retVal!;
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //private ReadOnlyMemory<T> _consume(int Count)
        //{
        //    var startIndex = Position;
        //    var endIndex = (Position + Count);

        //    Position = endIndex;
        //    return Data[startIndex .. endIndex];
        //}

        ///// <summary>
        ///// Returns the specified number of items from the stream and progresses the current reading position by that number
        ///// </summary>
        ///// <param name="Count">Number of characters to consume</param>
        //public ReadOnlyMemory<T> Consume(int Count = 1)
        //{
        //    return _consume(Count);
        //}
        //#endregion

        //#region Consume While
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //private bool _consume_while(Predicate<T> Predicate)
        //{
        //    bool consumed = Predicate(Next);
        //    while (!atEnd && Predicate(Next)) { Consume(); }

        //    return consumed;
        //}

        ///// <summary>
        ///// Consumes items until reaching the first one that does not match the given predicate, then returns all matched items and progresses the current reading position by that number
        ///// </summary>
        ///// <param name="Predicate"></param>
        ///// <returns>True if atleast a single item was consumed</returns>
        //[Obsolete(null, error: true)]
        //public bool Consume_While(Predicate<T> Predicate)
        //{
        //    return _consume_while(Predicate);
        //}

        //// ================================

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //private bool _consume_while(Predicate<T> Predicate, int Limit)
        //{
        //    bool consumed = Predicate(Next);
        //    var limit = Limit;
        //    while (!atEnd && Predicate(Next) && limit-- >= 0) { Consume(); }

        //    return consumed;
        //}

        ///// <summary>
        ///// Consumes items until reaching the first one that does not match the given predicate, then returns all matched items and progresses the current reading position by that number
        ///// </summary>
        ///// <param name="Predicate"></param>
        ///// <returns>True if atleast a single item was consumed</returns>
        //[Obsolete(null, error: true)]
        //public bool Consume_While(Predicate<T> Predicate, int Limit)
        //{
        //    return _consume_while(Predicate, (int)Limit);
        //}
        //// ================================

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //private bool _consume_while(Predicate<T> Predicate, out int outStart, out int outEnd)
        //{
        //    outStart = Position;
        //    bool consumed = Predicate(Next);
        //    while (!atEnd && Predicate(Next)) { Consume(); }

        //    outEnd = Position;
        //    return consumed;
        //}
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //private bool _consume_while(Predicate<T> Predicate, int Limit, out int outStart, out int outEnd)
        //{
        //    outStart = Position;
        //    bool consumed = Predicate(Next);
        //    var limit = Limit;
        //    while (!atEnd && Predicate(Next) && limit-- >= 0) { Consume(); }

        //    outEnd = Position;
        //    return consumed;
        //}

        ///// <summary>
        ///// Consumes items until reaching the first one that does not match the given predicate, then returns all matched items and progresses the current reading position by that number
        ///// </summary>
        ///// <param name="Predicate"></param>
        ///// <returns>True if atleast a single item was consumed</returns>
        //[Obsolete(null, error: true)]
        //public bool Consume_While(Predicate<T> Predicate, out ReadOnlyMemory<T> outConsumed)
        //{
        //    bool RetVal = _consume_while(Predicate, out var outStart, out var outEnd);
        //    var Count = outEnd - outStart;
        //    outConsumed = Data.Slice(outStart, Count);
        //    return RetVal;
        //}
        ///// <summary>
        ///// Consumes items until reaching the first one that does not match the given predicate, then returns all matched items and progresses the current reading position by that number
        ///// </summary>
        ///// <param name="Predicate"></param>
        ///// <returns>True if atleast a single item was consumed</returns>
        //[Obsolete(null, error: true)]
        //public bool Consume_While(Predicate<T> Predicate, out ReadOnlySpan<T> outConsumed)
        //{
        //    bool RetVal = _consume_while(Predicate, out var outStart, out var outEnd);
        //    var Count = outEnd - outStart;
        //    outConsumed = Data.Span.Slice(outStart, Count);
        //    return RetVal;
        //}


        ///// <summary>
        ///// Consumes items until reaching the first one that does not match the given predicate, then returns all matched items and progresses the current reading position by that number
        ///// </summary>
        ///// <param name="Predicate"></param>
        ///// <returns>True if atleast a single item was consumed</returns>
        //[Obsolete(null, error: true)]
        //public bool Consume_While(Predicate<T> Predicate, out ReadOnlyMemory<T> outConsumed, Index? Limit = null)
        //{
        //    bool RetVal = _consume_while(Predicate, Limit, out var outStart, out var outEnd);
        //    var Count = outEnd - outStart;
        //    outConsumed = Data.Slice(outStart, Count);
        //    return RetVal;
        //}

        ///// <summary>
        ///// Consumes items until reaching the first one that does not match the given predicate, then returns all matched items and progresses the current reading position by that number
        ///// </summary>
        ///// <param name="Predicate"></param>
        ///// <returns>True if atleast a single item was consumed</returns>
        //[Obsolete(null, error: true)]
        //public bool Consume_While(Predicate<T> Predicate, out ReadOnlySpan<T> outConsumed, Index? Limit = null)
        //{
        //    bool RetVal = _consume_while(Predicate, Limit, out var outStart, out var outEnd);
        //    var Count = outEnd - outStart;
        //    outConsumed = Data.Span.Slice(outStart, Count);
        //    return RetVal;
        //}
        //#endregion

        //#region Consume_Until
        //private bool _consume_until(T Subject, out ReadOnlyMemory<T> Consumed, Index? Limit)
        //{
        //    var rd = Get_Reader(Limit ?? Index.End);
        //    rd.
        //    if (rd.TryAdvanceTo(Subject, true))
        //    {

        //    }

        //    if (_scan(Subject, out var offset, Range.EndAt(Limit ?? Index.End), EqualityComparer<T>.Default))
        //    {
        //        Consumed = Consume(offset);
        //        return true;
        //    }

        //    Consumed = ReadOnlyMemory<T>.Empty;
        //    return false;
        //}

        //public bool Consume_Until(T Subject, Index? Limit)
        //{
        //    return _consume_until(Subject, out _, Limit);
        //}

        //public bool Consume_Until(T Subject, out ReadOnlyMemory<T> Consumed, Index? Limit)
        //{
        //    return _consume_until(Subject, out Consumed, Limit);
        //}

        //public bool Consume_Until(T Subject, out ReadOnlySpan<T> Consumed, Index? Limit)
        //{
        //    if( _consume_until(Subject, out var mem, Limit))
        //    {
        //        Consumed = mem.Span;
        //        return true;
        //    }

        //    Consumed = ReadOnlySpan<T>.Empty;
        //    return false;
        //}

        //private bool _consume_until(ReadOnlySpan<T> Sequence, out ReadOnlyMemory<T> Consumed, Index? EndIndex)
        //{
        //    if (_scan(, out var offset, Range.EndAt(EndIndex ?? Index.End), EqualityComparer<T>.Default))
        //    {
        //        Consumed = Consume(offset);
        //        return true;
        //    }

        //    Consumed = ReadOnlyMemory<T>.Empty;
        //    return false;
        //}
        //#endregion

        //#region Reconsume
        ///// <summary>
        ///// Pushes the given number of items back onto the front of the stream
        ///// </summary>
        ///// <param name="Count"></param>
        //[Obsolete]
        //public void Reconsume(int Count = 1)
        //{
        //    if (Count > Position) throw new ArgumentOutOfRangeException($"{nameof(Count)} exceeds the number of items consumed.");
        //    Position -= Count;
        //}
        //#endregion

        //#region SubStream
        //public ITokenizer<T> Substream(Range range)
        //{
        //    return new Tokenizer<T>(Data[Bounds][range], EOF_ITEM);
        //}

        ///// <summary>
        ///// Consumes items until reaching the first one that does not match the given <paramref name="Predicate"/>, progressing this streams reading position by that number and then returning all matched items as new stream
        ///// </summary>
        ///// <param name="Predicate"></param>
        ///// <returns></returns>
        //[Obsolete]
        //public Tokenizer<T> Substream(Predicate<T> Predicate)
        //{
        //    var startIndex = Position;

        //    while (!atEnd && Predicate(Next)) { Consume(); }

        //    var count = Position - startIndex;
        //    var consumed = Data.Slice(startIndex, count);

        //    return new Tokenizer<T>(consumed, EOF_ITEM);
        //}
        //#endregion

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
