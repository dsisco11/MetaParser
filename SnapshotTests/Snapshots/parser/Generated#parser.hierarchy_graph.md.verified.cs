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
Pattern_18
Consumer_2
Token_3["lexer_keyword_vars"]
Pattern_23
Pattern_24
Consumer_3
Token_4["lexer_keyword_function"]
Pattern_33
Pattern_34
Consumer_4
Token_5["lexer_keyword_byte"]
Pattern_39
Pattern_40
Consumer_5
Token_6["lexer_keyword_short"]
Pattern_46
Pattern_47
Consumer_6
Token_7["lexer_keyword_int"]
Pattern_51
Pattern_52
Consumer_7
Token_8["lexer_keyword_uint"]
Pattern_57
Pattern_58
Consumer_8
Token_9["lexer_keyword_float"]
Pattern_64
Pattern_65
Consumer_9
Token_10["lexer_char_open_bracket"]
Pattern_67
Consumer_10
Token_11["lexer_char_close_bracket"]
Pattern_69
Consumer_11
Token_12["lexer_char_open_sqbracket"]
Pattern_71
Consumer_12
Token_13["lexer_char_close_sqbracket"]
Pattern_73
Consumer_13
Token_14["lexer_char_open_parenthesis"]
Pattern_75
Consumer_14
Token_15["lexer_char_close_parenthesis"]
Pattern_77
Consumer_15
Token_16["lexer_char_colon"]
Pattern_79
Consumer_16
Token_17["lexer_char_semicolon"]
Pattern_81
Consumer_17
Token_18["lexer_char_asterisk"]
Pattern_83
Consumer_18
Token_19["lexer_char_solidus"]
Pattern_85
Consumer_19
Token_20["lexer_char_reverse_solidus"]
Pattern_87
Consumer_20
Token_21["lexer_digits"]
Pattern_89
Pattern_91
Consumer_21
Token_22["lexer_identifier"]
Pattern_94
Pattern_100
Pattern_101
Pattern_107
Consumer_22
Token_23["lexer_comment"]
Pattern_110
Pattern_111
Pattern_114
Pattern_115
Pattern_117
Consumer_23
Pattern_120
Pattern_121
Pattern_123
Pattern_125
Consumer_24
Token_24["syntax_typename"]
Pattern_127
Consumer_25
Pattern_129
Consumer_26
Pattern_131
Consumer_27
Pattern_133
Consumer_28
Pattern_135
Consumer_29
Pattern_137
Consumer_30
Pattern_139
Consumer_31
Token_25["syntax_comment"]
Pattern_142
Pattern_144
Consumer_32
Pattern_147
Pattern_150
Pattern_152
Consumer_33
Token_26["syntax_declaration"]
Pattern_155
Pattern_157
Consumer_34
Token_27["syntax_codeblock"]
Pattern_159
Pattern_161
Pattern_163
Consumer_35
Token_28["syntax_program"]
Pattern_166
Pattern_177
Pattern_178
Pattern_189
Consumer_36
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
Pattern_19
Pattern_20
Pattern_21
Pattern_22
Pattern_25
Pattern_26
Pattern_27
Pattern_28
Pattern_29
Pattern_30
Pattern_31
Pattern_32
Pattern_35
Pattern_36
Pattern_37
Pattern_38
Pattern_41
Pattern_42
Pattern_43
Pattern_44
Pattern_45
Pattern_48
Pattern_49
Pattern_50
Pattern_53
Pattern_54
Pattern_55
Pattern_56
Pattern_59
Pattern_60
Pattern_61
Pattern_62
Pattern_63
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
Pattern_92
Pattern_93
Pattern_95
Pattern_96
Pattern_97
Pattern_98
Pattern_99
Pattern_102
Pattern_103
Pattern_104
Pattern_105
Pattern_106
Pattern_108
Pattern_109
Pattern_112
Pattern_113
Pattern_116
Pattern_118
Pattern_119
Pattern_122
Pattern_124
Pattern_126
Pattern_128
Pattern_130
Pattern_132
Pattern_134
Pattern_136
Pattern_138
Pattern_140
Pattern_141
Pattern_143
Pattern_145
Pattern_146
Pattern_148
Pattern_149
Pattern_151
Pattern_153
Pattern_154
Pattern_156
Pattern_158
Pattern_160
Pattern_162
Pattern_164
Pattern_165
Pattern_167
Pattern_168
Pattern_169
Pattern_170
Pattern_171
Pattern_172
Pattern_173
Pattern_174
Pattern_175
Pattern_176
Pattern_179
Pattern_180
Pattern_181
Pattern_182
Pattern_183
Pattern_184
Pattern_185
Pattern_186
Pattern_187
Pattern_188
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
None_0 --> Pattern_18
None_0 --> Consumer_2
None_0 --> Token_3
None_0 --> Pattern_23
None_0 --> Pattern_24
None_0 --> Consumer_3
None_0 --> Token_4
None_0 --> Pattern_33
None_0 --> Pattern_34
None_0 --> Consumer_4
None_0 --> Token_5
None_0 --> Pattern_39
None_0 --> Pattern_40
None_0 --> Consumer_5
None_0 --> Token_6
None_0 --> Pattern_46
None_0 --> Pattern_47
None_0 --> Consumer_6
None_0 --> Token_7
None_0 --> Pattern_51
None_0 --> Pattern_52
None_0 --> Consumer_7
None_0 --> Token_8
None_0 --> Pattern_57
None_0 --> Pattern_58
None_0 --> Consumer_8
None_0 --> Token_9
None_0 --> Pattern_64
None_0 --> Pattern_65
None_0 --> Consumer_9
None_0 --> Token_10
None_0 --> Pattern_67
None_0 --> Consumer_10
None_0 --> Token_11
None_0 --> Pattern_69
None_0 --> Consumer_11
None_0 --> Token_12
None_0 --> Pattern_71
None_0 --> Consumer_12
None_0 --> Token_13
None_0 --> Pattern_73
None_0 --> Consumer_13
None_0 --> Token_14
None_0 --> Pattern_75
None_0 --> Consumer_14
None_0 --> Token_15
None_0 --> Pattern_77
None_0 --> Consumer_15
None_0 --> Token_16
None_0 --> Pattern_79
None_0 --> Consumer_16
None_0 --> Token_17
None_0 --> Pattern_81
None_0 --> Consumer_17
None_0 --> Token_18
None_0 --> Pattern_83
None_0 --> Consumer_18
None_0 --> Token_19
None_0 --> Pattern_85
None_0 --> Consumer_19
None_0 --> Token_20
None_0 --> Pattern_87
None_0 --> Consumer_20
None_0 --> Token_21
None_0 --> Pattern_89
None_0 --> Pattern_91
None_0 --> Consumer_21
None_0 --> Token_22
None_0 --> Pattern_94
None_0 --> Pattern_100
None_0 --> Pattern_101
None_0 --> Pattern_107
None_0 --> Consumer_22
None_0 --> Token_23
None_0 --> Pattern_110
None_0 --> Pattern_111
None_0 --> Pattern_114
None_0 --> Pattern_115
None_0 --> Pattern_117
None_0 --> Consumer_23
None_0 --> Pattern_120
None_0 --> Pattern_121
None_0 --> Pattern_123
None_0 --> Pattern_125
None_0 --> Consumer_24
None_0 --> Token_24
None_0 --> Pattern_127
None_0 --> Consumer_25
None_0 --> Pattern_129
None_0 --> Consumer_26
None_0 --> Pattern_131
None_0 --> Consumer_27
None_0 --> Pattern_133
None_0 --> Consumer_28
None_0 --> Pattern_135
None_0 --> Consumer_29
None_0 --> Pattern_137
None_0 --> Consumer_30
None_0 --> Pattern_139
None_0 --> Consumer_31
None_0 --> Token_25
None_0 --> Pattern_142
None_0 --> Pattern_144
None_0 --> Consumer_32
None_0 --> Pattern_147
None_0 --> Pattern_150
None_0 --> Pattern_152
None_0 --> Consumer_33
None_0 --> Token_26
None_0 --> Pattern_155
None_0 --> Pattern_157
None_0 --> Consumer_34
None_0 --> Token_27
None_0 --> Pattern_159
None_0 --> Pattern_161
None_0 --> Pattern_163
None_0 --> Consumer_35
None_0 --> Token_28
None_0 --> Pattern_166
None_0 --> Pattern_177
None_0 --> Pattern_178
None_0 --> Pattern_189
None_0 --> Consumer_36
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
Pattern_18 --> Pattern_17
Consumer_2 --> Pattern_18
Token_3 --> Consumer_3
Pattern_23 --> Pattern_19
Pattern_23 --> Pattern_20
Pattern_23 --> Pattern_21
Pattern_23 --> Pattern_22
Pattern_24 --> Pattern_23
Consumer_3 --> Pattern_24
Token_4 --> Consumer_4
Pattern_33 --> Pattern_25
Pattern_33 --> Pattern_26
Pattern_33 --> Pattern_27
Pattern_33 --> Pattern_28
Pattern_33 --> Pattern_29
Pattern_33 --> Pattern_30
Pattern_33 --> Pattern_31
Pattern_33 --> Pattern_32
Pattern_34 --> Pattern_33
Consumer_4 --> Pattern_34
Token_5 --> Consumer_5
Pattern_39 --> Pattern_35
Pattern_39 --> Pattern_36
Pattern_39 --> Pattern_37
Pattern_39 --> Pattern_38
Pattern_40 --> Pattern_39
Consumer_5 --> Pattern_40
Token_6 --> Consumer_6
Pattern_46 --> Pattern_41
Pattern_46 --> Pattern_42
Pattern_46 --> Pattern_43
Pattern_46 --> Pattern_44
Pattern_46 --> Pattern_45
Pattern_47 --> Pattern_46
Consumer_6 --> Pattern_47
Token_7 --> Consumer_7
Pattern_51 --> Pattern_48
Pattern_51 --> Pattern_49
Pattern_51 --> Pattern_50
Pattern_52 --> Pattern_51
Consumer_7 --> Pattern_52
Token_8 --> Consumer_8
Pattern_57 --> Pattern_53
Pattern_57 --> Pattern_54
Pattern_57 --> Pattern_55
Pattern_57 --> Pattern_56
Pattern_58 --> Pattern_57
Consumer_8 --> Pattern_58
Token_9 --> Consumer_9
Pattern_64 --> Pattern_59
Pattern_64 --> Pattern_60
Pattern_64 --> Pattern_61
Pattern_64 --> Pattern_62
Pattern_64 --> Pattern_63
Pattern_65 --> Pattern_64
Consumer_9 --> Pattern_65
Token_10 --> Consumer_10
Pattern_67 --> Pattern_66
Consumer_10 --> Pattern_67
Token_11 --> Consumer_11
Pattern_69 --> Pattern_68
Consumer_11 --> Pattern_69
Token_12 --> Consumer_12
Pattern_71 --> Pattern_70
Consumer_12 --> Pattern_71
Token_13 --> Consumer_13
Pattern_73 --> Pattern_72
Consumer_13 --> Pattern_73
Token_14 --> Consumer_14
Pattern_75 --> Pattern_74
Consumer_14 --> Pattern_75
Token_15 --> Consumer_15
Pattern_77 --> Pattern_76
Consumer_15 --> Pattern_77
Token_16 --> Consumer_16
Pattern_79 --> Pattern_78
Consumer_16 --> Pattern_79
Token_17 --> Consumer_17
Pattern_81 --> Pattern_80
Consumer_17 --> Pattern_81
Token_18 --> Consumer_18
Pattern_83 --> Pattern_82
Consumer_18 --> Pattern_83
Token_19 --> Consumer_19
Pattern_85 --> Pattern_84
Consumer_19 --> Pattern_85
Token_20 --> Consumer_20
Pattern_87 --> Pattern_86
Consumer_20 --> Pattern_87
Token_21 --> Consumer_21
Pattern_89 --> Pattern_88
Pattern_91 --> Pattern_90
Consumer_21 --> Pattern_89
Consumer_21 --> Pattern_91
Token_22 --> Consumer_22
Pattern_94 --> Pattern_92
Pattern_94 --> Pattern_93
Pattern_100 --> Pattern_95
Pattern_100 --> Pattern_96
Pattern_100 --> Pattern_97
Pattern_100 --> Pattern_98
Pattern_100 --> Pattern_99
Pattern_101 --> Pattern_94
Pattern_101 --> Pattern_100
Pattern_107 --> Pattern_102
Pattern_107 --> Pattern_103
Pattern_107 --> Pattern_104
Pattern_107 --> Pattern_105
Pattern_107 --> Pattern_106
Consumer_22 --> Pattern_101
Consumer_22 --> Pattern_107
Token_23 --> Consumer_23
Token_23 --> Consumer_24
Pattern_110 --> Pattern_108
Pattern_110 --> Pattern_109
Pattern_111 --> Pattern_110
Pattern_114 --> Pattern_112
Pattern_114 --> Pattern_113
Pattern_115 --> Pattern_114
Pattern_117 --> Pattern_116
Consumer_23 --> Pattern_111
Consumer_23 --> Pattern_115
Consumer_23 --> Pattern_117
Pattern_120 --> Pattern_118
Pattern_120 --> Pattern_119
Pattern_121 --> Pattern_120
Pattern_123 --> Pattern_122
Pattern_125 --> Pattern_124
Consumer_24 --> Pattern_121
Consumer_24 --> Pattern_123
Consumer_24 --> Pattern_125
Token_24 --> Consumer_25
Token_24 --> Consumer_26
Token_24 --> Consumer_27
Token_24 --> Consumer_28
Token_24 --> Consumer_29
Token_24 --> Consumer_30
Token_24 --> Consumer_31
Pattern_127 --> Pattern_126
Consumer_25 --> Pattern_127
Pattern_129 --> Pattern_128
Consumer_26 --> Pattern_129
Pattern_131 --> Pattern_130
Consumer_27 --> Pattern_131
Pattern_133 --> Pattern_132
Consumer_28 --> Pattern_133
Pattern_135 --> Pattern_134
Consumer_29 --> Pattern_135
Pattern_137 --> Pattern_136
Consumer_30 --> Pattern_137
Pattern_139 --> Pattern_138
Consumer_31 --> Pattern_139
Token_25 --> Consumer_32
Token_25 --> Consumer_33
Pattern_142 --> Pattern_140
Pattern_142 --> Pattern_141
Pattern_144 --> Pattern_143
Consumer_32 --> Pattern_142
Consumer_32 --> Pattern_144
Pattern_147 --> Pattern_145
Pattern_147 --> Pattern_146
Pattern_150 --> Pattern_148
Pattern_150 --> Pattern_149
Pattern_152 --> Pattern_151
Consumer_33 --> Pattern_147
Consumer_33 --> Pattern_150
Consumer_33 --> Pattern_152
Token_26 --> Consumer_34
Pattern_155 --> Pattern_153
Pattern_155 --> Pattern_154
Pattern_157 --> Pattern_156
Consumer_34 --> Pattern_155
Consumer_34 --> Pattern_157
Token_27 --> Consumer_35
Pattern_159 --> Pattern_158
Pattern_161 --> Pattern_160
Pattern_163 --> Pattern_162
Consumer_35 --> Pattern_159
Consumer_35 --> Pattern_161
Consumer_35 --> Pattern_163
Token_28 --> Consumer_36
Pattern_166 --> Pattern_164
Pattern_166 --> Pattern_165
Pattern_177 --> Pattern_167
Pattern_177 --> Pattern_168
Pattern_177 --> Pattern_169
Pattern_177 --> Pattern_170
Pattern_177 --> Pattern_171
Pattern_177 --> Pattern_172
Pattern_177 --> Pattern_173
Pattern_177 --> Pattern_174
Pattern_177 --> Pattern_175
Pattern_177 --> Pattern_176
Pattern_178 --> Pattern_166
Pattern_178 --> Pattern_177
Pattern_189 --> Pattern_179
Pattern_189 --> Pattern_180
Pattern_189 --> Pattern_181
Pattern_189 --> Pattern_182
Pattern_189 --> Pattern_183
Pattern_189 --> Pattern_184
Pattern_189 --> Pattern_185
Pattern_189 --> Pattern_186
Pattern_189 --> Pattern_187
Pattern_189 --> Pattern_188
Consumer_36 --> Pattern_178
Consumer_36 --> Pattern_189
```
*/
