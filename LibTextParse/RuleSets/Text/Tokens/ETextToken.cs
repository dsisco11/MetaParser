﻿namespace MetaParser.Parsing
{
    /// <summary>
    /// Defines all of the different possible token types
    /// </summary>
    public enum ETextToken : long
    {
        /// <summary></summary>
        None,
        /// <summary>Any collection of 2 or more characters NOT within quotations</summary>
        Ident,
        /// <summary>' ', '\r', '\t', '\f'</summary>
        Whitespace,
        /// <summary>'\n'</summary>
        Newline,
        /// <summary>'|'</summary>
        Column,
        /// <summary>':'</summary>
        Colon,
        /// <summary>';'</summary>
        Semicolon,
        /// <summary>','</summary>
        Comma,
        /// <summary>'['</summary>
        SqBracketOpen,
        /// <summary>']'</summary>
        SqBracketClose,
        /// <summary>'('</summary>
        ParenthOpen,
        /// <summary>')'</summary>
        ParenthClose,
        /// <summary>'{'</summary>
        BracketOpen,
        /// <summary>'}'</summary>
        BracketClose,
        /// <summary>'<'</summary>
        LessThan,
        /// <summary>'>'</summary>
        GreaterThan,
        /// <summary>'-'</summary>
        HypenMinus,
        /// <summary>'='</summary>
        Equals,
        /// <summary>'+'</summary>
        Plus,

        /// <summary></summary>
        Number,
        /// <summary></summary>
        Percentage,
        /// <summary></summary>
        Dimension,

        /// <summary>an identifier followed immediately by an opening-parenthesis "FUNCTION_NAME("</summary>
        FunctionName,
        /// <summary>an identifier prefixed with an at-sign(@)</summary>
        At_Keyword,
        /// <summary>an identifier prefixed with a hashtag(#)</summary>
        Hash,

        /// <summary>A code-like comment text sequence of some sort</summary>
        Comment,
        /// <summary>A collection of characters contained within quotations ("")</summary>
        String,
        /// <summary></summary>
        Bad_String,
        /// <summary></summary>
        Url,
        /// <summary></summary>
        Bad_Url,

        /// <summary>'<!--'</summary>
        CDO,
        /// <summary>'-->'</summary>
        CDC,
    }
}
