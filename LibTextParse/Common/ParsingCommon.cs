using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using static MetaParser.UnicodeCommon;

namespace MetaParser
{
    public static class ParsingCommon
    {
        #region Hexadecimal
        public static bool Try_Consume_Hexadecimal_Number(in SequenceReader<char> Stream, out ulong outValue)
        {
            ulong result = 0;
            Stream.IsNext(CHAR_HASH, true);
            while (Stream.TryRead(out char ch) && Is_Ascii_Hex_Digit(ch))
            {
                var v = (ulong)Ascii_Hex_To_Value(ch);
                //result = (16 * result) + v;
                result <<= 4;// mul 16
                result |= (v & 0xF);
            }

            outValue = result;
            return true;
        }
        #endregion

        #region Integer
        public static bool Parse_Integer(SequenceReader<char> Stream, out long outValue)
        {/* Docs: https://html.spec.whatwg.org/multipage/common-microsyntaxes.html#signed-integers */

            bool sign = true;
            /* Skip ASCII whitespace */
            Stream.AdvancePastAny(ASCII_WHITESPACE);

            if (Stream.End)
            {
                outValue = long.MaxValue;
                return false;
            }

            if (!Stream.IsNext(CHAR_PLUS_SIGN, true))
            {
                if (Stream.IsNext(CHAR_HYPHEN_MINUS, true))
                {
                    sign = false;
                }
            }


            /* Collect sequence of ASCII digit codepoints */
            _try_consume_ascii_digit_sequence(Stream, out ReadOnlySequence<char> digitSeq);

            if (!Stream.End && Stream.TryPeek(out char pk) && char.IsLetter(pk))
            {
                outValue = long.MaxValue;
                return false;
            }

            var n = _consume_subseq_base10(digitSeq);
            outValue = sign ? n :  -n;
            return true;
        }
        #endregion

        #region Decimal
        public static bool Parse_FloatingPoint(in SequenceReader<char> stream, out double outValue)
        {/* Docs: https://html.spec.whatwg.org/multipage/common-microsyntaxes.html#rules-for-parsing-floating-point-number-values */

            double value = 1;
            double divisor = 1;
            double exponent = 1;

            /* Skip ASCII whitespace */
            stream.AdvancePastAny(ASCII_WHITESPACE);

            if (stream.End)
            {
                outValue = double.NaN;
                return false;
            }

            // consume EITHER a plus sign OR a minus sign, but not both
            if (!stream.IsNext(CHAR_PLUS_SIGN, true))
            {
                if (stream.IsNext(CHAR_HYPHEN_MINUS, true))
                {
                    value = divisor = -1;
                }
            }

            if (stream.End)
            {
                outValue = double.NaN;
                return false;
            }

            /* 9) If the character indicated by position is a U+002E FULL STOP (.), 
             * and that is not the last character in input, 
             * and the character after the character indicated by position is an ASCII digit, 
             * then set value to zero and jump to the step labeled fraction. */
            if (!stream.IsNext(CHAR_FULL_STOP))
            {
                /* 11) Collect a sequence of code points that are ASCII digits from input given position, and interpret the resulting sequence as a base-ten integer. Multiply value by that integer. */
                if(!_try_consume_ascii_digit_sequence(stream, out ReadOnlySequence<char> digitSeq))
                {
                    outValue = double.NaN;
                    return false;
                }

                var n = _consume_subseq_base10(digitSeq);
                value *= n;
            }

            /* 12) If position is past the end of input, jump to the step labeled conversion. */
            if (stream.End)
            {
                return _seq_conversion(out outValue, value);
            }

            /* 13) Fraction: If the character indicated by position is a U+002E FULL STOP (.), run these substeps: */
            if (stream.IsNext(CHAR_FULL_STOP, true))
            {
                /* 2) If position is past the end of input, or if the character indicated by position is not an ASCII digit, U+0065 LATIN SMALL LETTER E (e), or U+0045 LATIN CAPITAL LETTER E (E), then jump to the step labeled conversion. */
                /* 3) If the character indicated by position is a U+0065 LATIN SMALL LETTER E character (e) or a U+0045 LATIN CAPITAL LETTER E character (E), skip the remainder of these substeps. */
                if (!stream.End && stream.TryPeek(out char pk) && char.IsDigit(pk))
                {
                    _parse_number_seq_fraction(stream, ref value, ref divisor);
                }
            }

            /* 14) If the character indicated by position is U+0065 (e) or a U+0045 (E), then: */
            if (stream.IsNext(CHAR_E_LOWER, true) || stream.IsNext(CHAR_E_UPPER, true))
            {
                /* 2) If position is past the end of input, then jump to the step labeled conversion. */
                /* 3) If the character indicated by position is a U+002D HYPHEN-MINUS character (-): */
                if (!stream.IsNext(CHAR_PLUS_SIGN, true))
                {
                    if (stream.IsNext(CHAR_HYPHEN_MINUS, true))
                    {
                        exponent = -1;
                    }
                }

                /* 4) If the character indicated by position is not an ASCII digit, then jump to the step labeled conversion. */
                /* 5) Collect a sequence of code points that are ASCII digits from input given position, and interpret the resulting sequence as a base-ten integer. Multiply exponent by that integer. */
                if(_try_consume_ascii_digit_sequence(stream, out ReadOnlySequence<char> digitSeq))
                {
                    var n = _consume_subseq_base10(digitSeq);
                    exponent *= n;
                    /* 6) Multiply value by ten raised to the exponentth power. */
                    value *= Math.Pow(10, exponent);
                }
            }

            return _seq_conversion(out outValue, value);

            static void _parse_number_seq_fraction(in SequenceReader<char> stream, ref double value, ref double divisor)
            {
                while (!stream.End && stream.TryPeek(out char pk) && char.IsDigit(pk))
                {
                    if (!stream.TryRead(out var ch))
                    {
                        break;
                    }
                    /* 4) Fraction loop: Multiply divisor by ten. */
                    divisor *= 10;
                    /* 5) Add the value of the character indicated by position, interpreted as a base-ten digit (0..9) and divided by divisor, to value. */
                    int i = (ch - CHAR_DIGIT_0); //char.GetNumericValue(ch);
                    Debug.Assert(i < 10);

                    double n = i;
                    n /= divisor;
                    value += n;
                    /* 6) Advance position to the next character. */
                    /* 7) If position is past the end of input, then jump to the step labeled conversion. */
                }
            }

            static bool _seq_conversion(out double outValue, double value)
            {
                /* 15) Conversion: Let S be the set of finite IEEE 754 double-precision floating-point values except −0, but with two special values added: 2^1024 and −2^1024. */
                /* 16) Let rounded-value be the number in S that is closest to value, 
                 * selecting the number with an even significand if there are two equally close values. 
                 * (The two special values 2^1024 and −2^1024 are considered to have even significands for this purpose.) */
                var roundedValue = value;
                if (roundedValue == -0D) roundedValue = -roundedValue;

                /* 17) If rounded-value is 2^1024 or −2^1024, return an error. */
                if (roundedValue == double.MinValue || roundedValue == double.MaxValue)
                {
                    outValue = double.NaN;
                    return false;
                }
                /* 18) Return rounded-value. */
                outValue = roundedValue;
                return true;
            }
        }
        #endregion

        #region Utility
        private static bool _try_consume_ascii_digit_sequence(in SequenceReader<char> stream, out ReadOnlySequence<char> consumed)
        {
            var startPos = stream.Position;
            var count = stream.AdvancePastAny(ASCII_DIGITS);
            if (count == 0)
            {
                consumed = ReadOnlySequence<char>.Empty;
                return false;
            }

            consumed = stream.Sequence.Slice(startPos, count);
            return true;
        }

        private static long _consume_subseq_base10(ReadOnlySequence<char> seq)
        {
            var rd = new SequenceReader<char>(seq);
            long accum = 0;
            long power = 1;
            while (rd.TryRead(out char ch))
            {
                long i = (ch - CHAR_DIGIT_0);
                Debug.Assert(i < 10);

                accum += power * i;
                power *= 10;
            }

            return accum;
        }
        #endregion

        #region Character Identification
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Is_Identifier_Start(SequenceReader<char> Stream)
        {
            if (Stream.IsNext(CHAR_HYPHEN_MINUS, true))
            {
                bool result = Is_Name_Start_Char(Stream) || Is_Valid_Escape(Stream);
                Stream.Rewind(1);
                return result;
            }
            else if (Is_Name_Start_Char(Stream))
            {
                return true;
            }
            else if (Stream.IsNext(CHAR_REVERSE_SOLIDUS))
            {
                return Is_Valid_Escape(Stream);
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Is_Number_Start(SequenceReader<char> Stream)
        {// We can see EITHER (+/-) AND . THEN we MUST see a digit
            var start = Stream.Consumed;
            if (!Stream.IsNext(CHAR_PLUS_SIGN, true))
            {
                Stream.IsNext(CHAR_HYPHEN_MINUS, true);
            }

            Stream.IsNext(CHAR_FULL_STOP, true);

            bool pk = Stream.TryPeek(out char ch);
            Stream.Rewind(Stream.Consumed - start);
            return pk && char.IsDigit(ch);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Is_Name_Char(SequenceReader<char> Stream)
        {
            if (Stream.TryPeek(out char ch))
            {
                if (ch >= CHAR_UNICODE_CONTROL) return true;
                if (ch == CHAR_UNDERSCORE) return true;
                if (ch == CHAR_HYPHEN_MINUS) return true;
                if (char.IsLetterOrDigit(ch)) return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Is_Name_Start_Char(SequenceReader<char> Stream)
        {
            if (Stream.TryPeek(out char ch))
            {
                if (ch >= CHAR_UNICODE_CONTROL) return true;
                if (ch == CHAR_UNDERSCORE) return true;
                if (char.IsLetter(ch)) return true;
                //if (token >= CHAR_A_UPPER && token <= 'Z' || token >= CHAR_A_LOWER && token <= 'z') return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Is_Valid_Escape(SequenceReader<char> Stream)
        {// SEE:  https://www.w3.org/TR/css-syntax-3/#check-if-two-code-points-are-a-valid-escape
            if (!Stream.IsNext(CHAR_REVERSE_SOLIDUS)) return false;
            else if (Stream.TryPeek(1, out var pk) && pk == CHAR_LINE_FEED) return false;

            return true;
        }

        #endregion
    }
}
