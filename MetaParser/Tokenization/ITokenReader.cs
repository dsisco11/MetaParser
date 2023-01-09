using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace MetaParser
{

    /// <summary>
    /// A reader for an <see cref="ITokenizer{T}"/>, a temporary window onto the data being parsed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITokenReader<T> where T : IEquatable<T>
    {
        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.End"/>
        bool End { get; }

        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.Length"/>
        long Length { get; }

        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.Consumed"/>
        long Consumed { get; internal set; }

        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.Remaining"/>
        long Remaining { get; }

        #region Peeking
        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.TryPeek(out T)"/>
        bool TryPeek(out T value);

        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.TryPeek(long, out T)"/>
        bool TryPeek(long index, out T value);
        #endregion

        #region IsNext
        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.IsNext(T, bool)"/>
        bool IsNext(T next, bool advancePast = false)
        {
            if(TryPeek(out T value))
            {
                bool match = next.Equals(value);
                if (advancePast && match)
                {
                    Advance(1);
                }

                return match;
            }

            return false;
        }

        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.IsNext(ReadOnlySpan{T}, bool)"/>
        bool IsNext(ReadOnlySpan<T> next, bool advancePast = false)
        {
            for (int i = 0; i < next.Length; i++)
            {
                var current = next[i];
                if (TryPeek(i, out T value))
                {
                    if (!current.Equals(value))
                    {
                        return false;
                    }
                }
            }

            if (advancePast)
            {
                Advance(next.Length);
            }
            return true;
        }
        #endregion

        #region Advancing
        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.Advance(long)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Advance(long count)
        {
            if ((ulong)count > (ulong)Remaining)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            Consumed += count;
        }

        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.TryAdvanceTo(T, bool)"/>
        bool TryAdvanceTo(T delimiter, bool advancePastDelimiter = false);

        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.TryAdvanceToAny(ReadOnlySpan{T}, bool)"/>
        bool TryAdvanceToAny(ReadOnlySpan<T> delimiters, bool advancePastDelimiter = false);

        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.AdvancePast(T)"/>
        long AdvancePast(T value);

        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.AdvancePastAny(ReadOnlySpan{T})"/>
        long AdvancePastAny(ReadOnlySpan<T> values);

        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.AdvancePastAny(T, T)"/>
        long AdvancePastAny(T value0, T value1);

        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.AdvancePastAny(T, T, T)"/>
        long AdvancePastAny(T value0, T value1, T value2);

        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.AdvancePastAny(T, T, T, T)"/>
        long AdvancePastAny(T value0, T value1, T value2, T value3);

        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.AdvanceToEnd"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void AdvanceToEnd()
        {
            Consumed = Length;
        }
        #endregion

        #region Rewind

        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.Rewind(long)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Rewind(long count)
        {
            if ((ulong)count > (ulong)Consumed)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            Consumed -= count;
        }
        #endregion

        #region Reading
        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.TryRead(out T)"/>
        bool TryRead(out T value)
        {
            bool success = TryPeek(out value);
            if (success)
            {
                Advance(1);
                return false;
            }

            return false;
        }

        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.TryReadTo(out ReadOnlySpan{T}, T, bool)"/>
        bool TryReadTo(out ReadOnlySpan<T> value, T delimiter, bool advancePastDelimiter = true)
        {
            var index = IndexOf(delimiter);
            if (index > -1)
            {
                if (advancePastDelimiter)
                {
                    index += 1;
                }

                value = Slice(0, index);
                return true;
            }

            value = ReadOnlySpan<T>.Empty;
            return false;
        }

        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.TryReadTo(out ReadOnlySpan{T}, T, T, bool)"/>
        bool TryReadTo(out ReadOnlySpan<T> value, T delimiter, T delimiterEscape, bool advancePastDelimiter = true);

        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.TryReadTo(out ReadOnlySpan{T}, ReadOnlySpan{T}, bool)"/>
        bool TryReadTo(out ReadOnlySpan<T> value, ReadOnlySpan<T> delimiter, bool advancePastDelimiter = true)
        {
            var index = IndexOf(delimiter);
            if (index > -1)
            {
                if (advancePastDelimiter)
                {
                    index += delimiter.Length;
                }

                value = Slice(0, index);
                return true;
            }

            value = ReadOnlySpan<T>.Empty;
            return false;
        }

        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.TryReadTo(out System.Buffers.ReadOnlySequence{T}, T, bool)"/>
        bool TryReadTo(out ReadOnlySpan<T> value, ReadOnlySpan<T> delimiters, T delimiterEscape, bool advancePastDelimiter = true);

        /// <inheritdoc cref="System.Buffers.SequenceReader{T}.TryReadToAny(out ReadOnlySpan{T}, ReadOnlySpan{T}, bool)"/>
        bool TryReadToAny(out ReadOnlySpan<T> value, ReadOnlySpan<T> delimiters, bool advancePastDelimiter = true)
        {
            var index = IndexOf(delimiters);
            if (index > -1)
            {
                if (advancePastDelimiter)
                {
                    index += 1;
                }

                value = Slice(0, index);
                return true;
            }

            value = ReadOnlySpan<T>.Empty;
            return false;
        }

        #endregion

        #region Searching
        /// <summary>
        /// Attempts to locate the next occurance of the given value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns>True if the given value was found</returns>
        long IndexOf(T value);

        /// <summary>
        /// Attempts to locate the next occurance of the given sequence of values.
        /// </summary>
        /// <param name="value">Value sequence to search for</param>
        /// <param name="index">Index of value sequence</param>
        /// <returns>True if the given sequence was found</returns>
        long IndexOf(ReadOnlySpan<T> value);

        /// <summary>
        /// Attempts to locate the next occurance of any of the given values
        /// </summary>
        /// <param name="values"></param>
        /// <param name="index"></param>
        /// <returns>True if any of the values were found</returns>
        long IndexOfAny(ReadOnlySpan<T> values);
        #endregion

        #region Slicing
        ReadOnlySpan<T> Slice(long index, long count);
        ReadOnlySpan<T> Slice(Index index);
        ReadOnlySpan<T> Slice(Range range);
        #endregion

        #region ToString
        string ToString();
        #endregion
    }
}