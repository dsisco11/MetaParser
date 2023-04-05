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
Token_19["lexer_char_solidus"]
Token_20["lexer_char_reverse_solidus"]
Token_21["lexer_digits"]
Token_22["lexer_identifier"]
Token_23["lexer_comment"]
Token_24["syntax_typename"]
Token_25["syntax_comment"]
Token_26["syntax_declaration"]
Token_27["syntax_codeblock"]
Token_28["syntax_program"]
Token_24 --> Token_2
Token_24 --> Token_3
Token_24 --> Token_5
Token_24 --> Token_6
Token_24 --> Token_7
Token_24 --> Token_8
Token_24 --> Token_9
Token_25 --> Token_19
Token_25 --> Token_1
Token_25 --> Token_18
Token_25 --> Token_20
Token_26 --> Token_22
Token_26 --> Token_16
Token_26 --> Token_17
Token_27 --> Token_10
Token_27 --> Token_26
Token_27 --> Token_11
Token_28 --> Token_2
Token_28 --> Token_4
Token_28 --> Token_0
Token_28 --> Token_22
Token_28 --> Token_14
Token_28 --> Token_15
Token_28 --> Token_10
Token_28 --> Token_11
```
*/
