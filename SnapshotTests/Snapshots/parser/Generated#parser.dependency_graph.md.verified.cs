//HintName: parser.dependency_graph.md.cs
/*
```mermaid
graph LR
Token_0["lexer_keyword_var"]
Token_1["lexer_keyword_function"]
Token_2["lexer_keyword_byte"]
Token_3["lexer_keyword_short"]
Token_4["lexer_keyword_int"]
Token_5["lexer_keyword_float"]
Token_6["lexer_char_open_bracket"]
Token_7["lexer_char_close_bracket"]
Token_8["lexer_char_open_sqbracket"]
Token_9["lexer_char_close_sqbracket"]
Token_10["lexer_char_open_parenthesis"]
Token_11["lexer_char_close_parenthesis"]
Token_12["lexer_char_colon"]
Token_13["lexer_char_semicolon"]
Token_14["lexer_char_asterisk"]
Token_15["lexer_char_solidus"]
Token_16["lexer_char_reverse_solidus"]
Token_17["lexer_whitespace"]
Token_18["lexer_digits"]
Token_19["lexer_newline"]
Token_20["lexer_identifier"]
Token_21["lexer_comment"]
Token_22["syntax_typename"]
Token_23["syntax_comment"]
Token_24["syntax_declaration"]
Token_25["syntax_codeblock"]
Token_26["syntax_program"]
Token_22 --> Token_0
Token_22 --> Token_2
Token_22 --> Token_3
Token_22 --> Token_4
Token_22 --> Token_5
Token_23 --> Token_15
Token_23 --> Token_19
Token_23 --> Token_14
Token_23 --> Token_16
Token_24 --> Token_20
Token_24 --> Token_12
Token_24 --> Token_13
Token_25 --> Token_6
Token_25 --> Token_24
Token_25 --> Token_7
Token_26 --> Token_0
Token_26 --> Token_1
Token_26 --> Token_17
Token_26 --> Token_20
Token_26 --> Token_10
Token_26 --> Token_11
Token_26 --> Token_6
Token_26 --> Token_7
```
*/
