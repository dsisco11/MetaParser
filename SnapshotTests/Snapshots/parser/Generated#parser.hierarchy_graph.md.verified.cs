//HintName: parser.hierarchy_graph.md.cs
/*
```mermaid
graph LR
None_0
Token_0["lexer_whitespace"]
Pattern_3
Pattern_7
Consumer_0
Token_1["lexer_newline"]
Pattern_10
Pattern_13
Consumer_1
Token_2["lexer_keyword_var"]
Pattern_17
Consumer_2
Token_3["lexer_keyword_vars"]
Pattern_22
Consumer_3
Token_4["lexer_keyword_function"]
Pattern_31
Consumer_4
Token_5["lexer_keyword_byte"]
Pattern_36
Consumer_5
Token_6["lexer_keyword_short"]
Pattern_42
Consumer_6
Token_7["lexer_keyword_int"]
Pattern_46
Consumer_7
Token_8["lexer_keyword_uint"]
Pattern_51
Consumer_8
Token_9["lexer_keyword_float"]
Pattern_57
Consumer_9
Token_10["lexer_char_open_bracket"]
Pattern_59
Consumer_10
Token_11["lexer_char_close_bracket"]
Pattern_61
Consumer_11
Token_12["lexer_char_open_sqbracket"]
Pattern_63
Consumer_12
Token_13["lexer_char_close_sqbracket"]
Pattern_65
Consumer_13
Token_14["lexer_char_open_parenthesis"]
Pattern_67
Consumer_14
Token_15["lexer_char_close_parenthesis"]
Pattern_69
Consumer_15
Token_16["lexer_char_colon"]
Pattern_71
Consumer_16
Token_17["lexer_char_semicolon"]
Pattern_73
Consumer_17
Token_18["lexer_char_asterisk"]
Pattern_75
Consumer_18
Token_19["lexer_char_at_symbol"]
Pattern_77
Consumer_19
Token_20["lexer_char_single_quote"]
Pattern_79
Consumer_20
Token_21["lexer_char_double_quote"]
Pattern_81
Consumer_21
Token_22["lexer_char_solidus"]
Pattern_83
Consumer_22
Token_23["lexer_char_reverse_solidus"]
Pattern_85
Consumer_23
Token_24["lexer_digits"]
Pattern_87
Pattern_89
Consumer_24
Token_25["lexer_identifier"]
Pattern_92
Pattern_98
Pattern_99
Pattern_105
Consumer_25
Token_26["lexer_comment"]
Pattern_108
Pattern_111
Pattern_113
Consumer_26
Pattern_116
Pattern_118
Pattern_120
Consumer_27
Token_27["syntax_typename"]
Pattern_122
Consumer_28
Pattern_124
Consumer_29
Pattern_126
Consumer_30
Pattern_128
Consumer_31
Pattern_130
Consumer_32
Pattern_132
Consumer_33
Pattern_134
Consumer_34
Token_28["syntax_string_single_line"]
Pattern_136
Pattern_138
Pattern_139
Pattern_141
Pattern_143
Consumer_35
Pattern_145
Pattern_147
Pattern_148
Pattern_150
Pattern_152
Consumer_36
Token_29["syntax_string_multi_line"]
Pattern_155
Pattern_157
Pattern_159
Consumer_37
Pattern_162
Pattern_164
Pattern_166
Consumer_38
Token_30["syntax_comment"]
Pattern_169
Pattern_171
Consumer_39
Pattern_174
Pattern_177
Pattern_179
Consumer_40
Token_31["syntax_declaration"]
Pattern_182
Pattern_184
Consumer_41
Token_32["syntax_codeblock"]
Pattern_186
Pattern_188
Pattern_190
Consumer_42
Token_33["syntax_program"]
Pattern_193
Pattern_204
Pattern_205
Pattern_216
Consumer_43
Pattern_0
Pattern_1
Pattern_2
Pattern_4
Pattern_5
Pattern_6
Pattern_8
Pattern_9
Pattern_11
Pattern_12
Pattern_14
Pattern_15
Pattern_16
Pattern_18
Pattern_19
Pattern_20
Pattern_21
Pattern_23
Pattern_24
Pattern_25
Pattern_26
Pattern_27
Pattern_28
Pattern_29
Pattern_30
Pattern_32
Pattern_33
Pattern_34
Pattern_35
Pattern_37
Pattern_38
Pattern_39
Pattern_40
Pattern_41
Pattern_43
Pattern_44
Pattern_45
Pattern_47
Pattern_48
Pattern_49
Pattern_50
Pattern_52
Pattern_53
Pattern_54
Pattern_55
Pattern_56
Pattern_58
Pattern_60
Pattern_62
Pattern_64
Pattern_66
Pattern_68
Pattern_70
Pattern_72
Pattern_74
Pattern_76
Pattern_78
Pattern_80
Pattern_82
Pattern_84
Pattern_86
Pattern_88
Pattern_90
Pattern_91
Pattern_93
Pattern_94
Pattern_95
Pattern_96
Pattern_97
Pattern_100
Pattern_101
Pattern_102
Pattern_103
Pattern_104
Pattern_106
Pattern_107
Pattern_109
Pattern_110
Pattern_112
Pattern_114
Pattern_115
Pattern_117
Pattern_119
Pattern_121
Pattern_123
Pattern_125
Pattern_127
Pattern_129
Pattern_131
Pattern_133
Pattern_135
Pattern_137
Pattern_140
Pattern_142
Pattern_144
Pattern_146
Pattern_149
Pattern_151
Pattern_153
Pattern_154
Pattern_156
Pattern_158
Pattern_160
Pattern_161
Pattern_163
Pattern_165
Pattern_167
Pattern_168
Pattern_170
Pattern_172
Pattern_173
Pattern_175
Pattern_176
Pattern_178
Pattern_180
Pattern_181
Pattern_183
Pattern_185
Pattern_187
Pattern_189
Pattern_191
Pattern_192
Pattern_194
Pattern_195
Pattern_196
Pattern_197
Pattern_198
Pattern_199
Pattern_200
Pattern_201
Pattern_202
Pattern_203
Pattern_206
Pattern_207
Pattern_208
Pattern_209
Pattern_210
Pattern_211
Pattern_212
Pattern_213
Pattern_214
Pattern_215
None_0 --> Token_0
None_0 --> Pattern_3
None_0 --> Pattern_7
None_0 --> Consumer_0
None_0 --> Token_1
None_0 --> Pattern_10
None_0 --> Pattern_13
None_0 --> Consumer_1
None_0 --> Token_2
None_0 --> Pattern_17
None_0 --> Consumer_2
None_0 --> Token_3
None_0 --> Pattern_22
None_0 --> Consumer_3
None_0 --> Token_4
None_0 --> Pattern_31
None_0 --> Consumer_4
None_0 --> Token_5
None_0 --> Pattern_36
None_0 --> Consumer_5
None_0 --> Token_6
None_0 --> Pattern_42
None_0 --> Consumer_6
None_0 --> Token_7
None_0 --> Pattern_46
None_0 --> Consumer_7
None_0 --> Token_8
None_0 --> Pattern_51
None_0 --> Consumer_8
None_0 --> Token_9
None_0 --> Pattern_57
None_0 --> Consumer_9
None_0 --> Token_10
None_0 --> Pattern_59
None_0 --> Consumer_10
None_0 --> Token_11
None_0 --> Pattern_61
None_0 --> Consumer_11
None_0 --> Token_12
None_0 --> Pattern_63
None_0 --> Consumer_12
None_0 --> Token_13
None_0 --> Pattern_65
None_0 --> Consumer_13
None_0 --> Token_14
None_0 --> Pattern_67
None_0 --> Consumer_14
None_0 --> Token_15
None_0 --> Pattern_69
None_0 --> Consumer_15
None_0 --> Token_16
None_0 --> Pattern_71
None_0 --> Consumer_16
None_0 --> Token_17
None_0 --> Pattern_73
None_0 --> Consumer_17
None_0 --> Token_18
None_0 --> Pattern_75
None_0 --> Consumer_18
None_0 --> Token_19
None_0 --> Pattern_77
None_0 --> Consumer_19
None_0 --> Token_20
None_0 --> Pattern_79
None_0 --> Consumer_20
None_0 --> Token_21
None_0 --> Pattern_81
None_0 --> Consumer_21
None_0 --> Token_22
None_0 --> Pattern_83
None_0 --> Consumer_22
None_0 --> Token_23
None_0 --> Pattern_85
None_0 --> Consumer_23
None_0 --> Token_24
None_0 --> Pattern_87
None_0 --> Pattern_89
None_0 --> Consumer_24
None_0 --> Token_25
None_0 --> Pattern_92
None_0 --> Pattern_98
None_0 --> Pattern_99
None_0 --> Pattern_105
None_0 --> Consumer_25
None_0 --> Token_26
None_0 --> Pattern_108
None_0 --> Pattern_111
None_0 --> Pattern_113
None_0 --> Consumer_26
None_0 --> Pattern_116
None_0 --> Pattern_118
None_0 --> Pattern_120
None_0 --> Consumer_27
None_0 --> Token_27
None_0 --> Pattern_122
None_0 --> Consumer_28
None_0 --> Pattern_124
None_0 --> Consumer_29
None_0 --> Pattern_126
None_0 --> Consumer_30
None_0 --> Pattern_128
None_0 --> Consumer_31
None_0 --> Pattern_130
None_0 --> Consumer_32
None_0 --> Pattern_132
None_0 --> Consumer_33
None_0 --> Pattern_134
None_0 --> Consumer_34
None_0 --> Token_28
None_0 --> Pattern_136
None_0 --> Pattern_138
None_0 --> Pattern_139
None_0 --> Pattern_141
None_0 --> Pattern_143
None_0 --> Consumer_35
None_0 --> Pattern_145
None_0 --> Pattern_147
None_0 --> Pattern_148
None_0 --> Pattern_150
None_0 --> Pattern_152
None_0 --> Consumer_36
None_0 --> Token_29
None_0 --> Pattern_155
None_0 --> Pattern_157
None_0 --> Pattern_159
None_0 --> Consumer_37
None_0 --> Pattern_162
None_0 --> Pattern_164
None_0 --> Pattern_166
None_0 --> Consumer_38
None_0 --> Token_30
None_0 --> Pattern_169
None_0 --> Pattern_171
None_0 --> Consumer_39
None_0 --> Pattern_174
None_0 --> Pattern_177
None_0 --> Pattern_179
None_0 --> Consumer_40
None_0 --> Token_31
None_0 --> Pattern_182
None_0 --> Pattern_184
None_0 --> Consumer_41
None_0 --> Token_32
None_0 --> Pattern_186
None_0 --> Pattern_188
None_0 --> Pattern_190
None_0 --> Consumer_42
None_0 --> Token_33
None_0 --> Pattern_193
None_0 --> Pattern_204
None_0 --> Pattern_205
None_0 --> Pattern_216
None_0 --> Consumer_43
Token_0 --> Consumer_0
Pattern_3 --> Pattern_0
Pattern_3 --> Pattern_1
Pattern_3 --> Pattern_2
Pattern_7 --> Pattern_4
Pattern_7 --> Pattern_5
Pattern_7 --> Pattern_6
Consumer_0 --> Pattern_3
Consumer_0 --> Pattern_7
Token_1 --> Consumer_1
Pattern_10 --> Pattern_8
Pattern_10 --> Pattern_9
Pattern_13 --> Pattern_11
Pattern_13 --> Pattern_12
Consumer_1 --> Pattern_10
Consumer_1 --> Pattern_13
Token_2 --> Consumer_2
Pattern_17 --> Pattern_14
Pattern_17 --> Pattern_15
Pattern_17 --> Pattern_16
Consumer_2 --> Pattern_17
Token_3 --> Consumer_3
Pattern_22 --> Pattern_18
Pattern_22 --> Pattern_19
Pattern_22 --> Pattern_20
Pattern_22 --> Pattern_21
Consumer_3 --> Pattern_22
Token_4 --> Consumer_4
Pattern_31 --> Pattern_23
Pattern_31 --> Pattern_24
Pattern_31 --> Pattern_25
Pattern_31 --> Pattern_26
Pattern_31 --> Pattern_27
Pattern_31 --> Pattern_28
Pattern_31 --> Pattern_29
Pattern_31 --> Pattern_30
Consumer_4 --> Pattern_31
Token_5 --> Consumer_5
Pattern_36 --> Pattern_32
Pattern_36 --> Pattern_33
Pattern_36 --> Pattern_34
Pattern_36 --> Pattern_35
Consumer_5 --> Pattern_36
Token_6 --> Consumer_6
Pattern_42 --> Pattern_37
Pattern_42 --> Pattern_38
Pattern_42 --> Pattern_39
Pattern_42 --> Pattern_40
Pattern_42 --> Pattern_41
Consumer_6 --> Pattern_42
Token_7 --> Consumer_7
Pattern_46 --> Pattern_43
Pattern_46 --> Pattern_44
Pattern_46 --> Pattern_45
Consumer_7 --> Pattern_46
Token_8 --> Consumer_8
Pattern_51 --> Pattern_47
Pattern_51 --> Pattern_48
Pattern_51 --> Pattern_49
Pattern_51 --> Pattern_50
Consumer_8 --> Pattern_51
Token_9 --> Consumer_9
Pattern_57 --> Pattern_52
Pattern_57 --> Pattern_53
Pattern_57 --> Pattern_54
Pattern_57 --> Pattern_55
Pattern_57 --> Pattern_56
Consumer_9 --> Pattern_57
Token_10 --> Consumer_10
Pattern_59 --> Pattern_58
Consumer_10 --> Pattern_59
Token_11 --> Consumer_11
Pattern_61 --> Pattern_60
Consumer_11 --> Pattern_61
Token_12 --> Consumer_12
Pattern_63 --> Pattern_62
Consumer_12 --> Pattern_63
Token_13 --> Consumer_13
Pattern_65 --> Pattern_64
Consumer_13 --> Pattern_65
Token_14 --> Consumer_14
Pattern_67 --> Pattern_66
Consumer_14 --> Pattern_67
Token_15 --> Consumer_15
Pattern_69 --> Pattern_68
Consumer_15 --> Pattern_69
Token_16 --> Consumer_16
Pattern_71 --> Pattern_70
Consumer_16 --> Pattern_71
Token_17 --> Consumer_17
Pattern_73 --> Pattern_72
Consumer_17 --> Pattern_73
Token_18 --> Consumer_18
Pattern_75 --> Pattern_74
Consumer_18 --> Pattern_75
Token_19 --> Consumer_19
Pattern_77 --> Pattern_76
Consumer_19 --> Pattern_77
Token_20 --> Consumer_20
Pattern_79 --> Pattern_78
Consumer_20 --> Pattern_79
Token_21 --> Consumer_21
Pattern_81 --> Pattern_80
Consumer_21 --> Pattern_81
Token_22 --> Consumer_22
Pattern_83 --> Pattern_82
Consumer_22 --> Pattern_83
Token_23 --> Consumer_23
Pattern_85 --> Pattern_84
Consumer_23 --> Pattern_85
Token_24 --> Consumer_24
Pattern_87 --> Pattern_86
Pattern_89 --> Pattern_88
Consumer_24 --> Pattern_87
Consumer_24 --> Pattern_89
Token_25 --> Consumer_25
Pattern_92 --> Pattern_90
Pattern_92 --> Pattern_91
Pattern_98 --> Pattern_93
Pattern_98 --> Pattern_94
Pattern_98 --> Pattern_95
Pattern_98 --> Pattern_96
Pattern_98 --> Pattern_97
Pattern_99 --> Pattern_92
Pattern_99 --> Pattern_98
Pattern_105 --> Pattern_100
Pattern_105 --> Pattern_101
Pattern_105 --> Pattern_102
Pattern_105 --> Pattern_103
Pattern_105 --> Pattern_104
Consumer_25 --> Pattern_99
Consumer_25 --> Pattern_105
Token_26 --> Consumer_26
Token_26 --> Consumer_27
Pattern_108 --> Pattern_106
Pattern_108 --> Pattern_107
Pattern_111 --> Pattern_109
Pattern_111 --> Pattern_110
Pattern_113 --> Pattern_112
Consumer_26 --> Pattern_108
Consumer_26 --> Pattern_111
Consumer_26 --> Pattern_113
Pattern_116 --> Pattern_114
Pattern_116 --> Pattern_115
Pattern_118 --> Pattern_117
Pattern_120 --> Pattern_119
Consumer_27 --> Pattern_116
Consumer_27 --> Pattern_118
Consumer_27 --> Pattern_120
Token_27 --> Consumer_28
Token_27 --> Consumer_29
Token_27 --> Consumer_30
Token_27 --> Consumer_31
Token_27 --> Consumer_32
Token_27 --> Consumer_33
Token_27 --> Consumer_34
Pattern_122 --> Pattern_121
Consumer_28 --> Pattern_122
Pattern_124 --> Pattern_123
Consumer_29 --> Pattern_124
Pattern_126 --> Pattern_125
Consumer_30 --> Pattern_126
Pattern_128 --> Pattern_127
Consumer_31 --> Pattern_128
Pattern_130 --> Pattern_129
Consumer_32 --> Pattern_130
Pattern_132 --> Pattern_131
Consumer_33 --> Pattern_132
Pattern_134 --> Pattern_133
Consumer_34 --> Pattern_134
Token_28 --> Consumer_35
Token_28 --> Consumer_36
Pattern_136 --> Pattern_135
Pattern_138 --> Pattern_137
Pattern_139 --> Pattern_138
Pattern_141 --> Pattern_140
Pattern_143 --> Pattern_142
Consumer_35 --> Pattern_136
Consumer_35 --> Pattern_139
Consumer_35 --> Pattern_141
Consumer_35 --> Pattern_143
Pattern_145 --> Pattern_144
Pattern_147 --> Pattern_146
Pattern_148 --> Pattern_147
Pattern_150 --> Pattern_149
Pattern_152 --> Pattern_151
Consumer_36 --> Pattern_145
Consumer_36 --> Pattern_148
Consumer_36 --> Pattern_150
Consumer_36 --> Pattern_152
Token_29 --> Consumer_37
Token_29 --> Consumer_38
Pattern_155 --> Pattern_153
Pattern_155 --> Pattern_154
Pattern_157 --> Pattern_156
Pattern_159 --> Pattern_158
Consumer_37 --> Pattern_155
Consumer_37 --> Pattern_157
Consumer_37 --> Pattern_159
Pattern_162 --> Pattern_160
Pattern_162 --> Pattern_161
Pattern_164 --> Pattern_163
Pattern_166 --> Pattern_165
Consumer_38 --> Pattern_162
Consumer_38 --> Pattern_164
Consumer_38 --> Pattern_166
Token_30 --> Consumer_39
Token_30 --> Consumer_40
Pattern_169 --> Pattern_167
Pattern_169 --> Pattern_168
Pattern_171 --> Pattern_170
Consumer_39 --> Pattern_169
Consumer_39 --> Pattern_171
Pattern_174 --> Pattern_172
Pattern_174 --> Pattern_173
Pattern_177 --> Pattern_175
Pattern_177 --> Pattern_176
Pattern_179 --> Pattern_178
Consumer_40 --> Pattern_174
Consumer_40 --> Pattern_177
Consumer_40 --> Pattern_179
Token_31 --> Consumer_41
Pattern_182 --> Pattern_180
Pattern_182 --> Pattern_181
Pattern_184 --> Pattern_183
Consumer_41 --> Pattern_182
Consumer_41 --> Pattern_184
Token_32 --> Consumer_42
Pattern_186 --> Pattern_185
Pattern_188 --> Pattern_187
Pattern_190 --> Pattern_189
Consumer_42 --> Pattern_186
Consumer_42 --> Pattern_188
Consumer_42 --> Pattern_190
Token_33 --> Consumer_43
Pattern_193 --> Pattern_191
Pattern_193 --> Pattern_192
Pattern_204 --> Pattern_194
Pattern_204 --> Pattern_195
Pattern_204 --> Pattern_196
Pattern_204 --> Pattern_197
Pattern_204 --> Pattern_198
Pattern_204 --> Pattern_199
Pattern_204 --> Pattern_200
Pattern_204 --> Pattern_201
Pattern_204 --> Pattern_202
Pattern_204 --> Pattern_203
Pattern_205 --> Pattern_193
Pattern_205 --> Pattern_204
Pattern_216 --> Pattern_206
Pattern_216 --> Pattern_207
Pattern_216 --> Pattern_208
Pattern_216 --> Pattern_209
Pattern_216 --> Pattern_210
Pattern_216 --> Pattern_211
Pattern_216 --> Pattern_212
Pattern_216 --> Pattern_213
Pattern_216 --> Pattern_214
Pattern_216 --> Pattern_215
Consumer_43 --> Pattern_205
Consumer_43 --> Pattern_216
```
*/
