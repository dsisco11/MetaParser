//HintName: parser.dependency_graph.md.cs
/*
```mermaid
graph LR
Token_0["lexer_whitespace"]
Token_1["lexer_newline"]
Token_2["lexer_keyword_var"]
Token_3["lexer_keyword_vars"]
Token_4["lexer_keyword_function"]
Token_5["lexer_keyword_byte"]
Token_6["lexer_keyword_short"]
Token_7["lexer_keyword_int"]
Token_8["lexer_keyword_uint"]
Token_9["lexer_keyword_float"]
Token_10["lexer_char_open_bracket"]
Token_11["lexer_char_close_bracket"]
Token_12["lexer_char_open_sqbracket"]
Token_13["lexer_char_close_sqbracket"]
Token_14["lexer_char_open_parenthesis"]
Token_15["lexer_char_close_parenthesis"]
Token_16["lexer_char_colon"]
Token_17["lexer_char_semicolon"]
Token_18["lexer_char_asterisk"]
Token_19["lexer_char_at_symbol"]
Token_20["lexer_char_single_quote"]
Token_21["lexer_char_double_quote"]
Token_22["lexer_char_solidus"]
Token_23["lexer_char_reverse_solidus"]
Token_24["lexer_digits"]
Token_25["lexer_identifier"]
Token_26["lexer_comment"]
Token_27["syntax_typename"]
Token_28["syntax_string_single_line"]
Token_29["syntax_string_multi_line"]
Token_30["syntax_comment"]
Token_31["syntax_declaration"]
Token_32["syntax_codeblock"]
Token_33["syntax_program"]
Token_27 --> Token_2
Token_27 --> Token_3
Token_27 --> Token_5
Token_27 --> Token_6
Token_27 --> Token_7
Token_27 --> Token_8
Token_27 --> Token_9
Token_28 --> Token_20
Token_28 --> Token_1
Token_28 --> Token_23
Token_28 --> Token_21
Token_29 --> Token_19
Token_29 --> Token_20
Token_29 --> Token_23
Token_29 --> Token_21
Token_30 --> Token_22
Token_30 --> Token_1
Token_30 --> Token_18
Token_30 --> Token_23
Token_31 --> Token_25
Token_31 --> Token_16
Token_31 --> Token_17
Token_32 --> Token_10
Token_32 --> Token_31
Token_32 --> Token_11
Token_33 --> Token_2
Token_33 --> Token_4
Token_33 --> Token_0
Token_33 --> Token_25
Token_33 --> Token_14
Token_33 --> Token_15
Token_33 --> Token_10
Token_33 --> Token_11
```
*/
