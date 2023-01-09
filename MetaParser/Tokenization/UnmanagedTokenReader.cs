using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MetaParser
{
    /// <inheritdoc cref="ITokenReader{T}"/>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("{ToString(),raw}")]
    public sealed class UnmanagedTokenReader<T> : ITokenReader<T> where T : unmanaged, IEquatable<T>
    {
        #region Fields
        /// <summary>
        /// Our stream of tokens
        /// </summary>
        private ReadOnlySequence<T> Data { get; init; }
        public long Consumed { get; private set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new stream from a memory pointer
        /// </summary>
        /// <param name="Sequence"></param>
        public UnmanagedTokenReader(ReadOnlySequence<T> Sequence)
        {
            Data = Sequence;
        }
        #endregion

        #region Accessors
        public bool End => Data.IsEmpty;
        public long Length => Data.Length;
        public long Remaining => Length - Consumed;
        #endregion

        #region Reader
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SequenceReader<T> GetReader()
        {
            return new(Data);
        }
        #endregion

        #region TryPeek
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryPeek(out T value)
        {
            return GetReader().TryPeek(out value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryPeek(Index index, out T value)
        {
            var offset = index.GetOffset((int)Remaining);
            return GetReader().TryPeek(offset, out value);
        }
        #endregion

        #region IsNext

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsNext(T next, bool advancePast)
        {
            var result = GetReader().IsNext(next, advancePast);
            if (result && advancePast)
            {
                Advance(1);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsNext(ReadOnlySpan<T> next, bool advancePast)
        {
            var result = GetReader().IsNext(next, advancePast);
            if (result && advancePast)
            {
                Advance(next.Length);
            }
            return result;
        }
        #endregion

        #region TryAdvanceTo

        public bool TryAdvanceTo(T delimiter, bool advancePastDelimiter = false)
        {
            var rd = GetReader();
            var success = rd.TryAdvanceTo(delimiter, advancePastDelimiter);
            if (success)
            {
                Advance(rd.Consumed);
            }
            return success;
        }


        public bool TryAdvanceToAny(ReadOnlySpan<T> delimiters, bool advancePastDelimiter = false)
        {
            var rd = GetReader();
            var success = rd.TryAdvanceToAny(delimiters, advancePastDelimiter);
            if (success)
            {
                Advance(rd.Consumed);
            }
            return success;
        }
        #endregion

        #region AdvancePast

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long AdvancePast(T value)
        {
            var count = GetReader().AdvancePast(value);
            Advance(count);
            return count;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long AdvancePastAny(ReadOnlySpan<T> values)
        {
            var count = GetReader().AdvancePastAny(values);
            Advance(count);
            return count;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long AdvancePastAny(T value0, T value1)
        {
            var count = GetReader().AdvancePastAny(value0, value1);
            Advance(count);
            return count;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long AdvancePastAny(T value0, T value1, T value2)
        {
            var count = GetReader().AdvancePastAny(value0, value1, value2);
            Advance(count);
            return count;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long AdvancePastAny(T value0, T value1, T value2, T value3)
        {
            var count = GetReader().AdvancePastAny(value0, value1, value2, value3);
            Advance(count);
            return count;
        }
        #endregion



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AdvanceToEnd()
        {
            Advance(Remaining);
        }

        #region Overrides
        public override string ToString()
        {
            return Data.ToString();
        }
        #endregion
    }
}
