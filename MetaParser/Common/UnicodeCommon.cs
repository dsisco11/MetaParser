using System.Buffers;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace MetaParser
{
    public static class UnicodeCommon
    {/* https://en.wikipedia.org/wiki/List_of_Unicode_characters */
        #region Constants

        #region /* Specials */
        public const int CHAR_UNICODE_MAX = 0x10FFFF;
        public const char CHAR_MAX = char.MaxValue;
        public const char EOF = '\u0000';
        public const char CHAR_NULL = '\u0000';
        public const char CHAR_REPLACEMENT = '\uFFFD';// U+FFFD
        public const char CHAR_UNICODE_CONTROL = '\u0080';
        #endregion

        #region /* Ascii whitespace */
        /* Docs: https://infra.spec.whatwg.org/#ascii-whitespace */
        public const char CHAR_TAB = '\u0009';
        /// <summary>
        /// Newline (\n)
        /// </summary>
        public const char CHAR_LINE_FEED = '\u000A';
        /// <summary>
        /// Form Feed
        /// </summary>
        public const char CHAR_FORM_FEED = '\u000C';
        /// <summary>
        /// Carriage Return (\r)
        /// </summary>
        public const char CHAR_CARRIAGE_RETURN = '\u000D';
        /// <summary>
        /// " "
        /// </summary>
        public const char CHAR_SPACE = '\u0020';

        public static readonly char[] ASCII_WHITESPACE = new[] { CHAR_SPACE, CHAR_LINE_FEED, CHAR_TAB, CHAR_FORM_FEED, CHAR_CARRIAGE_RETURN };
        public static readonly char[] ASCII_WHITESPACE_EXCLUDING_NEWLINE = new[] { CHAR_SPACE, CHAR_TAB, CHAR_FORM_FEED, CHAR_CARRIAGE_RETURN };
        public static readonly char[] SYMBOLS_QUOTATION_MARKS = new[] { CHAR_QUOTATION_MARK, CHAR_APOSTRAPHE };
        #endregion

        #region /* C0 Control codes */
        /* Docs: https://infra.spec.whatwg.org/#c0-control */
        /// <summary>
        /// C0 Control code: INFORMATION SEPARATOR ONE
        /// </summary>
        public const char CHAR_C0_INFO_SEPERATOR = '\u001F';
        public const char CHAR_C0_DELETE = '\u007F';
        public const char CHAR_C0_APPLICATION_PROGRAM_COMMAND = '\u009F';
        #endregion

        #region /* Ascii Symbols */

        /// <summary>
        /// !
        /// </summary>
        public const char CHAR_EXCLAMATION_POINT = '\u0021';
        /// <summary>
        /// ?
        /// </summary>
        public const char CHAR_QUESTION_MARK = '\u003F';
        /// <summary>
        /// @
        /// </summary>
        public const char CHAR_AT_SIGN = '\u0040';
        /// <summary>
        /// $
        /// </summary>
        public const char CHAR_DOLLAR_SIGN = '\u0024';
        /// <summary>
        /// &
        /// </summary>
        public const char CHAR_AMPERSAND = '\u0026';
        /// <summary>
        /// *
        /// </summary>
        public const char CHAR_ASTERISK = '\u002A';
        /// <summary>
        /// ^
        /// </summary>
        public const char CHAR_CARET = '\u005E';
        /// <summary>
        /// `
        /// </summary>
        public const char CHAR_BACKTICK = '\u0060';
        /// <summary>
        /// ~
        /// </summary>
        public const char CHAR_TILDE = '\u007E';
        /// <summary>
        /// |
        /// </summary>
        public const char CHAR_PIPE = '\u007C';
        /// <summary>
        /// =
        /// </summary>
        public const char CHAR_EQUALS = '\u003D';

        /// <summary>
        /// '<'
        /// </summary>
        public const char CHAR_LEFT_CHEVRON = '\u003C';
        /// <summary>
        /// '>'
        /// </summary>
        public const char CHAR_RIGHT_CHEVRON = '\u003E';

        /// <summary>
        /// {
        /// </summary>
        public const char CHAR_LEFT_CURLY_BRACKET = '\u007B';
        /// <summary>
        /// }
        /// </summary>
        public const char CHAR_RIGHT_CURLY_BRACKET = '\u007D';

        /// <summary>
        /// [
        /// </summary>
        public const char CHAR_LEFT_SQUARE_BRACKET = '\u005B';
        /// <summary>
        /// ]
        /// </summary>
        public const char CHAR_RIGHT_SQUARE_BRACKET = '\u005D';

        /// <summary>
        /// (
        /// </summary>
        public const char CHAR_LEFT_PARENTHESES = '\u0028';
        /// <summary>
        /// )
        /// </summary>
        public const char CHAR_RIGHT_PARENTHESES = '\u0029';
        #endregion

        #region /* Ascii digits */
        /* Docs: https://infra.spec.whatwg.org/#ascii-digit */
        public const char CHAR_DIGIT_0 = '\u0030';
        public const char CHAR_DIGIT_1 = '\u0031';
        public const char CHAR_DIGIT_2 = '\u0032';
        public const char CHAR_DIGIT_3 = '\u0033';
        public const char CHAR_DIGIT_4 = '\u0034';
        public const char CHAR_DIGIT_5 = '\u0035';
        public const char CHAR_DIGIT_6 = '\u0036';
        public const char CHAR_DIGIT_7 = '\u0037';
        public const char CHAR_DIGIT_8 = '\u0038';
        public const char CHAR_DIGIT_9 = '\u0039';

        public static readonly char[] ASCII_DIGITS = new[] { CHAR_DIGIT_0, CHAR_DIGIT_1, CHAR_DIGIT_2, CHAR_DIGIT_3, CHAR_DIGIT_4, CHAR_DIGIT_5, CHAR_DIGIT_6, CHAR_DIGIT_7, CHAR_DIGIT_8, CHAR_DIGIT_9 };
        #endregion

        #region Ascii Upper Alpha
        /* Docs: https://infra.spec.whatwg.org/#ascii-upper-alpha */
        public const char CHAR_A_UPPER = '\u0041';
        public const char CHAR_B_UPPER = '\u0042';
        public const char CHAR_C_UPPER = '\u0043';
        public const char CHAR_D_UPPER = '\u0044';
        public const char CHAR_E_UPPER = '\u0045';
        public const char CHAR_F_UPPER = '\u0046';
        public const char CHAR_G_UPPER = '\u0047';
        public const char CHAR_H_UPPER = '\u0048';
        public const char CHAR_M_UPPER = '\u004D';
        public const char CHAR_P_UPPER = '\u0050';
        public const char CHAR_S_UPPER = '\u0053';
        public const char CHAR_T_UPPER = '\u0054';
        public const char CHAR_U_UPPER = '\u0055';
        public const char CHAR_V_UPPER = '\u0056';
        public const char CHAR_W_UPPER = '\u0057';
        public const char CHAR_X_UPPER = '\u0058';
        public const char CHAR_Y_UPPER = '\u0059';
        public const char CHAR_Z_UPPER = '\u005A';
        #endregion

        #region Ascii Lower Alpha
        /* Docs: https://infra.spec.whatwg.org/#ascii-lower-alpha */
        public const char CHAR_A_LOWER = '\u0061';
        public const char CHAR_B_LOWER = '\u0062';
        public const char CHAR_C_LOWER = '\u0063';
        public const char CHAR_D_LOWER = '\u0064';
        public const char CHAR_E_LOWER = '\u0065';
        public const char CHAR_F_LOWER = '\u0066';
        public const char CHAR_G_LOWER = '\u0067';
        public const char CHAR_H_LOWER = '\u0068';
        public const char CHAR_M_LOWER = '\u006D';
        public const char CHAR_S_LOWER = '\u0073';
        public const char CHAR_T_LOWER = '\u0074';
        public const char CHAR_U_LOWER = '\u0075';
        public const char CHAR_V_LOWER = '\u0076';
        public const char CHAR_W_LOWER = '\u0077';
        public const char CHAR_X_LOWER = '\u0078';
        public const char CHAR_Y_LOWER = '\u0079';
        public const char CHAR_Z_LOWER = '\u007A';
        #endregion

        #region /* Common */
        /// <summary>
        /// "
        /// </summary>
        public const char CHAR_QUOTATION_MARK = '\u0022';
        /// <summary>
        /// '
        /// </summary>
        public const char CHAR_APOSTRAPHE = '\u0027';
        /// <summary>
        /// +
        /// </summary>
        public const char CHAR_PLUS_SIGN = '\u002B';
        /// <summary>
        /// %
        /// </summary>
        public const char CHAR_PERCENT = '\u0025';
        /// <summary>
        /// -
        /// </summary>
        public const char CHAR_HYPHEN_MINUS = '\u002D';
        /// <summary>
        /// _
        /// </summary>
        public const char CHAR_UNDERSCORE = '\u005F';
        /// <summary>
        /// .
        /// </summary>
        public const char CHAR_FULL_STOP = '\u002E';
        /// <summary>
        /// /
        /// </summary>
        public const char CHAR_SOLIDUS = '\u002F';
        /// <summary>
        /// \
        /// </summary>
        public const char CHAR_REVERSE_SOLIDUS = '\u005C';
        /// <summary>
        /// #
        /// </summary>
        public const char CHAR_HASH = '\u0023';

        /// <summary>
        /// ,
        /// </summary>
        public const char CHAR_COMMA = '\u002C';
        /// <summary>
        /// :
        /// </summary>
        public const char CHAR_COLON = '\u003A';
        /// <summary>
        /// ;
        /// </summary>
        public const char CHAR_SEMICOLON = '\u003B';

        /// <summary>
        /// &nbsp
        /// </summary>
        public const char CHAR_NBSP = '\u00A0';
        #endregion

        #endregion

        #region Modifier Keys
        /* These are the character representations for common keys */
        public const char KEY_CTRL_MODIFIER = '⌃';
        public const char KEY_ALT_MODIFIER = '⌥';
        public const char KEY_SHIFT_MODIFIER = '⇧';
        public const char KEY_META_MODIFIER = '⌘';

        public const char KEY_ESCAPE = '⎋';
        public const char KEY_BACKSPACE = '⌫';
        public const char KEY_CAPSLOCK = '⇪';
        public const char KEY_ENTER = '↵';
        public const char KEY_TAB = '⇥';
        public const char KEY_SPACE = ' ';

        public const char KEY_DELETE = '⌦';
        public const char KEY_END = '↘';
        public const char KEY_INSERT = '↖';
        public const char KEY_HOME = '↖';
        public const char KEY_PGDOWN = '⇟';
        public const char KEY_PGUP = '⇞';

        public const char KEY_UP = '↑';
        public const char KEY_RIGHT = '→';
        public const char KEY_DOWN = '↓';
        public const char KEY_LEFT = '←';

        #endregion

        #region Character Checks

        /// <summary>
        /// A surrogate is a code point that is in the range U+D800 to U+DFFF, inclusive.
        /// </summary>
        /// <param name="codePoint">Code point to check</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Is_Surrogate_Code_Point(char codePoint)
        {/* Docs: https://infra.spec.whatwg.org/#surrogate */
            return codePoint switch
            {
                var _ when (codePoint is >= '\uD800' and <= '\uDFFF') => true,
                _ => false,
            };
        }

        /// <summary>
        /// True if code point is an ASCII hex-digit character (0-9 | a-f | A-F)
        /// </summary>
        /// <param name="codePoint">Code point to check</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Is_Ascii_Hex_Digit(char codePoint)
        {/* Docs: https://infra.spec.whatwg.org/#ascii-digit
            Docs:  https://www.w3.org/TR/css-syntax-3/#hex-digit
            */
            switch (codePoint)
            {
                case var _ when (codePoint is >= CHAR_DIGIT_0 and <= CHAR_DIGIT_9):
                case var _ when (codePoint is >= CHAR_A_LOWER and <= CHAR_F_LOWER):
                case var _ when (codePoint is >= CHAR_A_UPPER and <= CHAR_F_UPPER):
                    return true;
                default:
                    return false;
            }
        }
        #endregion

        #region Hexadecimal
        /// <summary>
        /// Map of ASCII code points to their hex value.
        /// 0xFF is a placeholder.
        /// </summary>
        private static ReadOnlySpan<byte> HexLookupTable => new byte[]
        {
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0x0,  0x1,  0x2,  0x3,  0x4,  0x5,  0x6,  0x7,  0x8,  0x9,  0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xA,  0xB,  0xC,  0xD,  0xE,  0xF,  0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xa,  0xb,  0xc,  0xd,  0xe,  0xf
        };

        /// <summary>
        /// Converts an ASCII hexadecimal character to its numeric value
        /// </summary>
        /// <param name="c">Code point to convert</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Ascii_Hex_To_Value(char c)
        {
            if (c > HexLookupTable.Length)
                throw new ArgumentOutOfRangeException($"The given character('{c}') is not a valid hexadecimal specifier!");

            Contract.EndContractBlock();

            return HexLookupTable[c];
        }

        #endregion

        #region Encoding Sets
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Percent_Encode_Set_C0_Control(char c)
        {/* Docs: https://url.spec.whatwg.org/#c0-control-percent-encode-set */
            return c is (>= CHAR_C0_DELETE and <= CHAR_C0_APPLICATION_PROGRAM_COMMAND) or > CHAR_TILDE;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Percent_Encode_Set_Fragment(char c)
        {/* Docs: https://url.spec.whatwg.org/#fragment-percent-encode-set */
            return Percent_Encode_Set_C0_Control(c) || c switch
            {
                CHAR_SPACE or CHAR_QUOTATION_MARK or CHAR_LEFT_CHEVRON or CHAR_RIGHT_CHEVRON or CHAR_BACKTICK => true,
                _ => false,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Percent_Encode_Set_Path(char c)
        {/* Docs: https://url.spec.whatwg.org/#path-percent-encode-set */
            return Percent_Encode_Set_Fragment(c) || c switch
            {
                CHAR_HASH or CHAR_QUESTION_MARK or CHAR_LEFT_CURLY_BRACKET or CHAR_RIGHT_CURLY_BRACKET => true,
                _ => false,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Percent_Encode_Set_Userinfo(char c)
        {/* Docs: https://url.spec.whatwg.org/#userinfo-percent-encode-set */
            return Percent_Encode_Set_Path(c) || c switch
            {
                CHAR_SOLIDUS or CHAR_COLON or CHAR_SEMICOLON or CHAR_EQUALS or CHAR_AT_SIGN or CHAR_LEFT_SQUARE_BRACKET or CHAR_REVERSE_SOLIDUS or CHAR_RIGHT_SQUARE_BRACKET or CHAR_CARET or CHAR_PIPE => true,
                _ => false,
            };
        }
        #endregion


        /// <summary>
        /// Consumes an escaped character from the current reading position
        /// </summary>
        /// <returns></returns>
        public static char Consume_Escaped(SequenceReader<char> Stream)
        {// Docs:  https://www.w3.org/TR/css-syntax-3/#consume-escaped-code-point
            // Consume as many hex digits as possible but no more then 5 (for a total of 6)
            if (ParsingCommon.TryParseHexadecimal(Stream, out var hexNum))
            {
                Stream.AdvancePastAny(ASCII_WHITESPACE);
                return hexNum is 0 or > CHAR_UNICODE_MAX
                    ? CHAR_REPLACEMENT
                    : Is_Surrogate_Code_Point((char)hexNum) ? CHAR_REPLACEMENT : (char)hexNum;
            }

            return Stream.TryRead(out var ch) ? ch : CHAR_REPLACEMENT;
        }
    }
}
