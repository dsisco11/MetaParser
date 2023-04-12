//HintName: parser.registry.debug.cs
/*
```
Token_0 | Name: lexer_whitespace | DependencyInfo: NodeData { Order = 0, Depth = 0, IsRecursive = False }
Token_1 | Name: lexer_newline | DependencyInfo: NodeData { Order = 1, Depth = 0, IsRecursive = False }
Token_2 | Name: lexer_keyword_var | DependencyInfo: NodeData { Order = 2, Depth = 0, IsRecursive = False }
Token_3 | Name: lexer_keyword_vars | DependencyInfo: NodeData { Order = 3, Depth = 0, IsRecursive = False }
Token_4 | Name: lexer_keyword_function | DependencyInfo: NodeData { Order = 4, Depth = 0, IsRecursive = False }
Token_5 | Name: lexer_keyword_byte | DependencyInfo: NodeData { Order = 5, Depth = 0, IsRecursive = False }
Token_6 | Name: lexer_keyword_short | DependencyInfo: NodeData { Order = 6, Depth = 0, IsRecursive = False }
Token_7 | Name: lexer_keyword_int | DependencyInfo: NodeData { Order = 7, Depth = 0, IsRecursive = False }
Token_8 | Name: lexer_keyword_uint | DependencyInfo: NodeData { Order = 8, Depth = 0, IsRecursive = False }
Token_9 | Name: lexer_keyword_float | DependencyInfo: NodeData { Order = 9, Depth = 0, IsRecursive = False }
Token_10 | Name: lexer_char_open_bracket | DependencyInfo: NodeData { Order = 10, Depth = 0, IsRecursive = False }
Token_11 | Name: lexer_char_close_bracket | DependencyInfo: NodeData { Order = 11, Depth = 0, IsRecursive = False }
Token_12 | Name: lexer_char_open_sqbracket | DependencyInfo: NodeData { Order = 12, Depth = 0, IsRecursive = False }
Token_13 | Name: lexer_char_close_sqbracket | DependencyInfo: NodeData { Order = 13, Depth = 0, IsRecursive = False }
Token_14 | Name: lexer_char_open_parenthesis | DependencyInfo: NodeData { Order = 14, Depth = 0, IsRecursive = False }
Token_15 | Name: lexer_char_close_parenthesis | DependencyInfo: NodeData { Order = 15, Depth = 0, IsRecursive = False }
Token_16 | Name: lexer_char_colon | DependencyInfo: NodeData { Order = 16, Depth = 0, IsRecursive = False }
Token_17 | Name: lexer_char_semicolon | DependencyInfo: NodeData { Order = 17, Depth = 0, IsRecursive = False }
Token_18 | Name: lexer_char_asterisk | DependencyInfo: NodeData { Order = 18, Depth = 0, IsRecursive = False }
Token_19 | Name: lexer_char_at_symbol | DependencyInfo: NodeData { Order = 19, Depth = 0, IsRecursive = False }
Token_20 | Name: lexer_char_single_quote | DependencyInfo: NodeData { Order = 20, Depth = 0, IsRecursive = False }
Token_21 | Name: lexer_char_double_quote | DependencyInfo: NodeData { Order = 21, Depth = 0, IsRecursive = False }
Token_22 | Name: lexer_char_solidus | DependencyInfo: NodeData { Order = 22, Depth = 0, IsRecursive = False }
Token_23 | Name: lexer_char_reverse_solidus | DependencyInfo: NodeData { Order = 23, Depth = 0, IsRecursive = False }
Token_24 | Name: lexer_digits | DependencyInfo: NodeData { Order = 24, Depth = 0, IsRecursive = False }
Token_25 | Name: lexer_identifier | DependencyInfo: NodeData { Order = 25, Depth = 0, IsRecursive = False }
Token_26 | Name: lexer_comment | DependencyInfo: NodeData { Order = 26, Depth = 0, IsRecursive = False }
Token_27 | Name: syntax_typename | DependencyInfo: NodeData { Order = 27, Depth = 1, IsRecursive = False }
Token_28 | Name: syntax_string_single_line | DependencyInfo: NodeData { Order = 28, Depth = 1, IsRecursive = False }
Token_29 | Name: syntax_string_multi_line | DependencyInfo: NodeData { Order = 29, Depth = 1, IsRecursive = False }
Token_30 | Name: syntax_comment | DependencyInfo: NodeData { Order = 30, Depth = 1, IsRecursive = False }
Token_31 | Name: syntax_declaration | DependencyInfo: NodeData { Order = 31, Depth = 1, IsRecursive = False }
Token_32 | Name: syntax_codeblock | DependencyInfo: NodeData { Order = 33, Depth = 2, IsRecursive = False }
Token_33 | Name: syntax_program | DependencyInfo: NodeData { Order = 32, Depth = 1, IsRecursive = False }

NodeData { Order = 0, Depth = 0, IsRecursive = False }
NodeData { Order = 1, Depth = 0, IsRecursive = False }
NodeData { Order = 2, Depth = 0, IsRecursive = False }
NodeData { Order = 3, Depth = 0, IsRecursive = False }
NodeData { Order = 4, Depth = 0, IsRecursive = False }
NodeData { Order = 5, Depth = 0, IsRecursive = False }
NodeData { Order = 6, Depth = 0, IsRecursive = False }
NodeData { Order = 7, Depth = 0, IsRecursive = False }
NodeData { Order = 8, Depth = 0, IsRecursive = False }
NodeData { Order = 9, Depth = 0, IsRecursive = False }
NodeData { Order = 10, Depth = 0, IsRecursive = False }
NodeData { Order = 11, Depth = 0, IsRecursive = False }
NodeData { Order = 12, Depth = 0, IsRecursive = False }
NodeData { Order = 13, Depth = 0, IsRecursive = False }
NodeData { Order = 14, Depth = 0, IsRecursive = False }
NodeData { Order = 15, Depth = 0, IsRecursive = False }
NodeData { Order = 16, Depth = 0, IsRecursive = False }
NodeData { Order = 17, Depth = 0, IsRecursive = False }
NodeData { Order = 18, Depth = 0, IsRecursive = False }
NodeData { Order = 19, Depth = 0, IsRecursive = False }
NodeData { Order = 20, Depth = 0, IsRecursive = False }
NodeData { Order = 21, Depth = 0, IsRecursive = False }
NodeData { Order = 22, Depth = 0, IsRecursive = False }
NodeData { Order = 23, Depth = 0, IsRecursive = False }
NodeData { Order = 24, Depth = 0, IsRecursive = False }
NodeData { Order = 25, Depth = 0, IsRecursive = False }
NodeData { Order = 26, Depth = 0, IsRecursive = False }
NodeData { Order = 27, Depth = 0, IsRecursive = False }
NodeData { Order = 28, Depth = 0, IsRecursive = False }
NodeData { Order = 29, Depth = 0, IsRecursive = False }
NodeData { Order = 30, Depth = 0, IsRecursive = False }
NodeData { Order = 31, Depth = 0, IsRecursive = False }
NodeData { Order = 32, Depth = 0, IsRecursive = False }
NodeData { Order = 33, Depth = 0, IsRecursive = False }
NodeData { Order = 34, Depth = 0, IsRecursive = False }
NodeData { Order = 35, Depth = 0, IsRecursive = False }
NodeData { Order = 36, Depth = 0, IsRecursive = False }
NodeData { Order = 37, Depth = 0, IsRecursive = False }
NodeData { Order = 38, Depth = 0, IsRecursive = False }
NodeData { Order = 39, Depth = 0, IsRecursive = False }
NodeData { Order = 40, Depth = 0, IsRecursive = False }
NodeData { Order = 41, Depth = 0, IsRecursive = False }
NodeData { Order = 42, Depth = 0, IsRecursive = False }
NodeData { Order = 43, Depth = 0, IsRecursive = False }

NodeData { Order = 0, Depth = 0, IsRecursive = False }
NodeData { Order = 1, Depth = 0, IsRecursive = False }
NodeData { Order = 2, Depth = 0, IsRecursive = False }
NodeData { Order = 142, Depth = 1, IsRecursive = False }
NodeData { Order = 3, Depth = 0, IsRecursive = False }
NodeData { Order = 4, Depth = 0, IsRecursive = False }
NodeData { Order = 5, Depth = 0, IsRecursive = False }
NodeData { Order = 143, Depth = 1, IsRecursive = False }
NodeData { Order = 6, Depth = 0, IsRecursive = False }
NodeData { Order = 7, Depth = 0, IsRecursive = False }
NodeData { Order = 144, Depth = 1, IsRecursive = False }
NodeData { Order = 8, Depth = 0, IsRecursive = False }
NodeData { Order = 9, Depth = 0, IsRecursive = False }
NodeData { Order = 145, Depth = 1, IsRecursive = False }
NodeData { Order = 10, Depth = 0, IsRecursive = False }
NodeData { Order = 11, Depth = 0, IsRecursive = False }
NodeData { Order = 12, Depth = 0, IsRecursive = False }
NodeData { Order = 146, Depth = 1, IsRecursive = False }
NodeData { Order = 13, Depth = 0, IsRecursive = False }
NodeData { Order = 14, Depth = 0, IsRecursive = False }
NodeData { Order = 15, Depth = 0, IsRecursive = False }
NodeData { Order = 16, Depth = 0, IsRecursive = False }
NodeData { Order = 147, Depth = 1, IsRecursive = False }
NodeData { Order = 17, Depth = 0, IsRecursive = False }
NodeData { Order = 18, Depth = 0, IsRecursive = False }
NodeData { Order = 19, Depth = 0, IsRecursive = False }
NodeData { Order = 20, Depth = 0, IsRecursive = False }
NodeData { Order = 21, Depth = 0, IsRecursive = False }
NodeData { Order = 22, Depth = 0, IsRecursive = False }
NodeData { Order = 23, Depth = 0, IsRecursive = False }
NodeData { Order = 24, Depth = 0, IsRecursive = False }
NodeData { Order = 148, Depth = 1, IsRecursive = False }
NodeData { Order = 25, Depth = 0, IsRecursive = False }
NodeData { Order = 26, Depth = 0, IsRecursive = False }
NodeData { Order = 27, Depth = 0, IsRecursive = False }
NodeData { Order = 28, Depth = 0, IsRecursive = False }
NodeData { Order = 149, Depth = 1, IsRecursive = False }
NodeData { Order = 29, Depth = 0, IsRecursive = False }
NodeData { Order = 30, Depth = 0, IsRecursive = False }
NodeData { Order = 31, Depth = 0, IsRecursive = False }
NodeData { Order = 32, Depth = 0, IsRecursive = False }
NodeData { Order = 33, Depth = 0, IsRecursive = False }
NodeData { Order = 150, Depth = 1, IsRecursive = False }
NodeData { Order = 34, Depth = 0, IsRecursive = False }
NodeData { Order = 35, Depth = 0, IsRecursive = False }
NodeData { Order = 36, Depth = 0, IsRecursive = False }
NodeData { Order = 151, Depth = 1, IsRecursive = False }
NodeData { Order = 37, Depth = 0, IsRecursive = False }
NodeData { Order = 38, Depth = 0, IsRecursive = False }
NodeData { Order = 39, Depth = 0, IsRecursive = False }
NodeData { Order = 40, Depth = 0, IsRecursive = False }
NodeData { Order = 152, Depth = 1, IsRecursive = False }
NodeData { Order = 41, Depth = 0, IsRecursive = False }
NodeData { Order = 42, Depth = 0, IsRecursive = False }
NodeData { Order = 43, Depth = 0, IsRecursive = False }
NodeData { Order = 44, Depth = 0, IsRecursive = False }
NodeData { Order = 45, Depth = 0, IsRecursive = False }
NodeData { Order = 153, Depth = 1, IsRecursive = False }
NodeData { Order = 46, Depth = 0, IsRecursive = False }
NodeData { Order = 154, Depth = 1, IsRecursive = False }
NodeData { Order = 47, Depth = 0, IsRecursive = False }
NodeData { Order = 155, Depth = 1, IsRecursive = False }
NodeData { Order = 48, Depth = 0, IsRecursive = False }
NodeData { Order = 156, Depth = 1, IsRecursive = False }
NodeData { Order = 49, Depth = 0, IsRecursive = False }
NodeData { Order = 157, Depth = 1, IsRecursive = False }
NodeData { Order = 50, Depth = 0, IsRecursive = False }
NodeData { Order = 158, Depth = 1, IsRecursive = False }
NodeData { Order = 51, Depth = 0, IsRecursive = False }
NodeData { Order = 159, Depth = 1, IsRecursive = False }
NodeData { Order = 52, Depth = 0, IsRecursive = False }
NodeData { Order = 160, Depth = 1, IsRecursive = False }
NodeData { Order = 53, Depth = 0, IsRecursive = False }
NodeData { Order = 161, Depth = 1, IsRecursive = False }
NodeData { Order = 54, Depth = 0, IsRecursive = False }
NodeData { Order = 162, Depth = 1, IsRecursive = False }
NodeData { Order = 55, Depth = 0, IsRecursive = False }
NodeData { Order = 163, Depth = 1, IsRecursive = False }
NodeData { Order = 56, Depth = 0, IsRecursive = False }
NodeData { Order = 164, Depth = 1, IsRecursive = False }
NodeData { Order = 57, Depth = 0, IsRecursive = False }
NodeData { Order = 165, Depth = 1, IsRecursive = False }
NodeData { Order = 58, Depth = 0, IsRecursive = False }
NodeData { Order = 166, Depth = 1, IsRecursive = False }
NodeData { Order = 59, Depth = 0, IsRecursive = False }
NodeData { Order = 167, Depth = 1, IsRecursive = False }
NodeData { Order = 60, Depth = 0, IsRecursive = False }
NodeData { Order = 168, Depth = 1, IsRecursive = False }
NodeData { Order = 61, Depth = 0, IsRecursive = False }
NodeData { Order = 169, Depth = 1, IsRecursive = False }
NodeData { Order = 62, Depth = 0, IsRecursive = False }
NodeData { Order = 63, Depth = 0, IsRecursive = False }
NodeData { Order = 170, Depth = 1, IsRecursive = False }
NodeData { Order = 64, Depth = 0, IsRecursive = False }
NodeData { Order = 65, Depth = 0, IsRecursive = False }
NodeData { Order = 66, Depth = 0, IsRecursive = False }
NodeData { Order = 67, Depth = 0, IsRecursive = False }
NodeData { Order = 68, Depth = 0, IsRecursive = False }
NodeData { Order = 171, Depth = 1, IsRecursive = False }
NodeData { Order = 213, Depth = 2, IsRecursive = False }
NodeData { Order = 69, Depth = 0, IsRecursive = False }
NodeData { Order = 70, Depth = 0, IsRecursive = False }
NodeData { Order = 71, Depth = 0, IsRecursive = False }
NodeData { Order = 72, Depth = 0, IsRecursive = False }
NodeData { Order = 73, Depth = 0, IsRecursive = False }
NodeData { Order = 172, Depth = 1, IsRecursive = False }
NodeData { Order = 74, Depth = 0, IsRecursive = False }
NodeData { Order = 75, Depth = 0, IsRecursive = False }
NodeData { Order = 173, Depth = 1, IsRecursive = False }
NodeData { Order = 76, Depth = 0, IsRecursive = False }
NodeData { Order = 77, Depth = 0, IsRecursive = False }
NodeData { Order = 174, Depth = 1, IsRecursive = False }
NodeData { Order = 78, Depth = 0, IsRecursive = False }
NodeData { Order = 175, Depth = 1, IsRecursive = False }
NodeData { Order = 79, Depth = 0, IsRecursive = False }
NodeData { Order = 80, Depth = 0, IsRecursive = False }
NodeData { Order = 176, Depth = 1, IsRecursive = False }
NodeData { Order = 81, Depth = 0, IsRecursive = False }
NodeData { Order = 177, Depth = 1, IsRecursive = False }
NodeData { Order = 82, Depth = 0, IsRecursive = False }
NodeData { Order = 178, Depth = 1, IsRecursive = False }
NodeData { Order = 83, Depth = 0, IsRecursive = False }
NodeData { Order = 179, Depth = 1, IsRecursive = False }
NodeData { Order = 84, Depth = 0, IsRecursive = False }
NodeData { Order = 180, Depth = 1, IsRecursive = False }
NodeData { Order = 85, Depth = 0, IsRecursive = False }
NodeData { Order = 181, Depth = 1, IsRecursive = False }
NodeData { Order = 86, Depth = 0, IsRecursive = False }
NodeData { Order = 182, Depth = 1, IsRecursive = False }
NodeData { Order = 87, Depth = 0, IsRecursive = False }
NodeData { Order = 183, Depth = 1, IsRecursive = False }
NodeData { Order = 88, Depth = 0, IsRecursive = False }
NodeData { Order = 184, Depth = 1, IsRecursive = False }
NodeData { Order = 89, Depth = 0, IsRecursive = False }
NodeData { Order = 185, Depth = 1, IsRecursive = False }
NodeData { Order = 90, Depth = 0, IsRecursive = False }
NodeData { Order = 186, Depth = 1, IsRecursive = False }
NodeData { Order = 91, Depth = 0, IsRecursive = False }
NodeData { Order = 187, Depth = 1, IsRecursive = False }
NodeData { Order = 214, Depth = 2, IsRecursive = False }
NodeData { Order = 92, Depth = 0, IsRecursive = False }
NodeData { Order = 188, Depth = 1, IsRecursive = False }
NodeData { Order = 93, Depth = 0, IsRecursive = False }
NodeData { Order = 189, Depth = 1, IsRecursive = False }
NodeData { Order = 94, Depth = 0, IsRecursive = False }
NodeData { Order = 190, Depth = 1, IsRecursive = False }
NodeData { Order = 95, Depth = 0, IsRecursive = False }
NodeData { Order = 191, Depth = 1, IsRecursive = False }
NodeData { Order = 215, Depth = 2, IsRecursive = False }
NodeData { Order = 96, Depth = 0, IsRecursive = False }
NodeData { Order = 192, Depth = 1, IsRecursive = False }
NodeData { Order = 97, Depth = 0, IsRecursive = False }
NodeData { Order = 193, Depth = 1, IsRecursive = False }
NodeData { Order = 98, Depth = 0, IsRecursive = False }
NodeData { Order = 99, Depth = 0, IsRecursive = False }
NodeData { Order = 194, Depth = 1, IsRecursive = False }
NodeData { Order = 100, Depth = 0, IsRecursive = False }
NodeData { Order = 195, Depth = 1, IsRecursive = False }
NodeData { Order = 101, Depth = 0, IsRecursive = False }
NodeData { Order = 196, Depth = 1, IsRecursive = False }
NodeData { Order = 102, Depth = 0, IsRecursive = False }
NodeData { Order = 103, Depth = 0, IsRecursive = False }
NodeData { Order = 197, Depth = 1, IsRecursive = False }
NodeData { Order = 104, Depth = 0, IsRecursive = False }
NodeData { Order = 198, Depth = 1, IsRecursive = False }
NodeData { Order = 105, Depth = 0, IsRecursive = False }
NodeData { Order = 199, Depth = 1, IsRecursive = False }
NodeData { Order = 106, Depth = 0, IsRecursive = False }
NodeData { Order = 107, Depth = 0, IsRecursive = False }
NodeData { Order = 200, Depth = 1, IsRecursive = False }
NodeData { Order = 108, Depth = 0, IsRecursive = False }
NodeData { Order = 201, Depth = 1, IsRecursive = False }
NodeData { Order = 109, Depth = 0, IsRecursive = False }
NodeData { Order = 110, Depth = 0, IsRecursive = False }
NodeData { Order = 202, Depth = 1, IsRecursive = False }
NodeData { Order = 111, Depth = 0, IsRecursive = False }
NodeData { Order = 112, Depth = 0, IsRecursive = False }
NodeData { Order = 203, Depth = 1, IsRecursive = False }
NodeData { Order = 113, Depth = 0, IsRecursive = False }
NodeData { Order = 204, Depth = 1, IsRecursive = False }
NodeData { Order = 114, Depth = 0, IsRecursive = False }
NodeData { Order = 115, Depth = 0, IsRecursive = False }
NodeData { Order = 205, Depth = 1, IsRecursive = False }
NodeData { Order = 116, Depth = 0, IsRecursive = False }
NodeData { Order = 206, Depth = 1, IsRecursive = False }
NodeData { Order = 117, Depth = 0, IsRecursive = False }
NodeData { Order = 207, Depth = 1, IsRecursive = False }
NodeData { Order = 118, Depth = 0, IsRecursive = False }
NodeData { Order = 208, Depth = 1, IsRecursive = False }
NodeData { Order = 119, Depth = 0, IsRecursive = False }
NodeData { Order = 209, Depth = 1, IsRecursive = False }
NodeData { Order = 120, Depth = 0, IsRecursive = False }
NodeData { Order = 121, Depth = 0, IsRecursive = False }
NodeData { Order = 210, Depth = 1, IsRecursive = False }
NodeData { Order = 122, Depth = 0, IsRecursive = False }
NodeData { Order = 123, Depth = 0, IsRecursive = False }
NodeData { Order = 124, Depth = 0, IsRecursive = False }
NodeData { Order = 125, Depth = 0, IsRecursive = False }
NodeData { Order = 126, Depth = 0, IsRecursive = False }
NodeData { Order = 127, Depth = 0, IsRecursive = False }
NodeData { Order = 128, Depth = 0, IsRecursive = False }
NodeData { Order = 129, Depth = 0, IsRecursive = False }
NodeData { Order = 130, Depth = 0, IsRecursive = False }
NodeData { Order = 131, Depth = 0, IsRecursive = False }
NodeData { Order = 211, Depth = 1, IsRecursive = False }
NodeData { Order = 216, Depth = 2, IsRecursive = False }
NodeData { Order = 132, Depth = 0, IsRecursive = False }
NodeData { Order = 133, Depth = 0, IsRecursive = False }
NodeData { Order = 134, Depth = 0, IsRecursive = False }
NodeData { Order = 135, Depth = 0, IsRecursive = False }
NodeData { Order = 136, Depth = 0, IsRecursive = False }
NodeData { Order = 137, Depth = 0, IsRecursive = False }
NodeData { Order = 138, Depth = 0, IsRecursive = False }
NodeData { Order = 139, Depth = 0, IsRecursive = False }
NodeData { Order = 140, Depth = 0, IsRecursive = False }
NodeData { Order = 141, Depth = 0, IsRecursive = False }
NodeData { Order = 212, Depth = 1, IsRecursive = False }
```
*/
