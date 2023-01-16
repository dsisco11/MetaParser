namespace MetaParser.Parsing
{
    /// <summary>
    /// Defines all of the different possible token types
    /// </summary>
    public static class ETextToken
    {
        /// <summary></summary>
        public const byte None = 0;
        /// <summary>Any collection of 2 or more characters NOT within quotations</summary>
        public const byte Ident = 1;
        /// <summary>' ', '\r', '\t', '\f'</summary>
        public const byte Whitespace = 2;
        /// <summary>'\n'</summary>
        public const byte Newline = 3;
        /// <summary>'|'</summary>
        public const byte Column = 4;
        /// <summary>':'</summary>
        public const byte Colon = 5;
        /// <summary>';'</summary>
        public const byte Semicolon = 6;
        /// <summary>','</summary>
        public const byte Comma = 7;
        /// <summary>'['</summary>
        public const byte SqBracketOpen = 8;
        /// <summary>']'</summary>
        public const byte SqBracketClose = 9;
        /// <summary>'('</summary>
        public const byte ParenthOpen = 10;
        /// <summary>')'</summary>
        public const byte ParenthClose = 11;
        /// <summary>'{'</summary>
        public const byte BracketOpen = 12;
        /// <summary>'}'</summary>
        public const byte BracketClose = 13;
        /// <summary>'<'</summary>
        public const byte LessThan = 14;
        /// <summary>'>'</summary>
        public const byte GreaterThan = 15;
        /// <summary>'-'</summary>
        public const byte HypenMinus = 16;
        /// <summary>'='</summary>
        public const byte Equals = 17;
        /// <summary>'+'</summary>
        public const byte Plus = 18;
        /// <summary>'*'</summary>
        public const byte Asterisk = 19;
        /// <summary>'/'</summary>
        public const byte Solidus = 20;
        /// <summary>'\'</summary>
        public const byte ReverseSolidus = 21;

        /// <summary></summary>
        public const byte Number = 22;
        public const byte Bad_Number = 23;

        /// <summary>A code-like comment text sequence of some sort</summary>
        public const byte Comment = 24;
        public const byte Bad_Comment = 25;

        /// <summary>A collection of characters contained within quotations ("")</summary>
        public const byte String = 26;
        /// <summary></summary>
        public const byte Bad_String = 27;

        /// <summary></summary>
        public const byte Url = 28;
        /// <summary></summary>
        public const byte Bad_Url = 29;

    }
}
