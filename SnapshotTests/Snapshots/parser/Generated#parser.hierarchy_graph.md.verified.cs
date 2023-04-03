//HintName: parser.hierarchy_graph.md.cs
/*
```mermaid
graph LR
None_0
Token_0["lexer_keyword_var"]
Pattern_3
Pattern_4
Consumer_0
Token_1["lexer_keyword_function"]
Pattern_13
Pattern_14
Consumer_1
Token_2["lexer_keyword_byte"]
Pattern_19
Pattern_20
Consumer_2
Token_3["lexer_keyword_short"]
Pattern_26
Pattern_27
Consumer_3
Token_4["lexer_keyword_int"]
Pattern_31
Pattern_32
Consumer_4
Token_5["lexer_keyword_float"]
Pattern_38
Pattern_39
Consumer_5
Token_6["lexer_char_open_bracket"]
Pattern_41
Consumer_6
Token_7["lexer_char_close_bracket"]
Pattern_43
Consumer_7
Token_8["lexer_char_open_sqbracket"]
Pattern_45
Consumer_8
Token_9["lexer_char_close_sqbracket"]
Pattern_47
Consumer_9
Token_10["lexer_char_open_parenthesis"]
Pattern_49
Consumer_10
Token_11["lexer_char_close_parenthesis"]
Pattern_51
Consumer_11
Token_12["lexer_char_colon"]
Pattern_53
Consumer_12
Token_13["lexer_char_semicolon"]
Pattern_55
Consumer_13
Token_14["lexer_char_asterisk"]
Pattern_57
Consumer_14
Token_15["lexer_char_solidus"]
Pattern_59
Consumer_15
Token_16["lexer_char_reverse_solidus"]
Pattern_61
Consumer_16
Token_17["lexer_whitespace"]
Pattern_65
Pattern_69
Consumer_17
Token_18["lexer_digits"]
Pattern_71
Pattern_73
Consumer_18
Token_19["lexer_newline"]
Pattern_76
Pattern_79
Consumer_19
Token_20["lexer_identifier"]
Pattern_82
Pattern_88
Pattern_89
Pattern_95
Consumer_20
Token_21["lexer_comment"]
Pattern_98
Pattern_99
Pattern_102
Pattern_103
Pattern_105
Consumer_21
Pattern_108
Pattern_109
Pattern_111
Pattern_113
Consumer_22
Token_22["syntax_typename"]
Pattern_115
Consumer_23
Pattern_117
Consumer_24
Pattern_119
Consumer_25
Pattern_121
Consumer_26
Pattern_123
Consumer_27
Token_23["syntax_comment"]
Pattern_126
Pattern_128
Consumer_28
Pattern_131
Pattern_134
Pattern_136
Consumer_29
Token_24["syntax_declaration"]
Pattern_139
Pattern_141
Consumer_30
Token_25["syntax_codeblock"]
Pattern_143
Pattern_145
Pattern_147
Consumer_31
Token_26["syntax_program"]
Pattern_150
Pattern_161
Pattern_162
Pattern_173
Consumer_32
Pattern_0
Pattern_1
Pattern_2
Pattern_5
Pattern_6
Pattern_7
Pattern_8
Pattern_9
Pattern_10
Pattern_11
Pattern_12
Pattern_15
Pattern_16
Pattern_17
Pattern_18
Pattern_21
Pattern_22
Pattern_23
Pattern_24
Pattern_25
Pattern_28
Pattern_29
Pattern_30
Pattern_33
Pattern_34
Pattern_35
Pattern_36
Pattern_37
Pattern_40
Pattern_42
Pattern_44
Pattern_46
Pattern_48
Pattern_50
Pattern_52
Pattern_54
Pattern_56
Pattern_58
Pattern_60
Pattern_62
Pattern_63
Pattern_64
Pattern_66
Pattern_67
Pattern_68
Pattern_70
Pattern_72
Pattern_74
Pattern_75
Pattern_77
Pattern_78
Pattern_80
Pattern_81
Pattern_83
Pattern_84
Pattern_85
Pattern_86
Pattern_87
Pattern_90
Pattern_91
Pattern_92
Pattern_93
Pattern_94
Pattern_96
Pattern_97
Pattern_100
Pattern_101
Pattern_104
Pattern_106
Pattern_107
Pattern_110
Pattern_112
Pattern_114
Pattern_116
Pattern_118
Pattern_120
Pattern_122
Pattern_124
Pattern_125
Pattern_127
Pattern_129
Pattern_130
Pattern_132
Pattern_133
Pattern_135
Pattern_137
Pattern_138
Pattern_140
Pattern_142
Pattern_144
Pattern_146
Pattern_148
Pattern_149
Pattern_151
Pattern_152
Pattern_153
Pattern_154
Pattern_155
Pattern_156
Pattern_157
Pattern_158
Pattern_159
Pattern_160
Pattern_163
Pattern_164
Pattern_165
Pattern_166
Pattern_167
Pattern_168
Pattern_169
Pattern_170
Pattern_171
Pattern_172
None_0 --> Token_0
None_0 --> Pattern_3
None_0 --> Pattern_4
None_0 --> Consumer_0
None_0 --> Token_1
None_0 --> Pattern_13
None_0 --> Pattern_14
None_0 --> Consumer_1
None_0 --> Token_2
None_0 --> Pattern_19
None_0 --> Pattern_20
None_0 --> Consumer_2
None_0 --> Token_3
None_0 --> Pattern_26
None_0 --> Pattern_27
None_0 --> Consumer_3
None_0 --> Token_4
None_0 --> Pattern_31
None_0 --> Pattern_32
None_0 --> Consumer_4
None_0 --> Token_5
None_0 --> Pattern_38
None_0 --> Pattern_39
None_0 --> Consumer_5
None_0 --> Token_6
None_0 --> Pattern_41
None_0 --> Consumer_6
None_0 --> Token_7
None_0 --> Pattern_43
None_0 --> Consumer_7
None_0 --> Token_8
None_0 --> Pattern_45
None_0 --> Consumer_8
None_0 --> Token_9
None_0 --> Pattern_47
None_0 --> Consumer_9
None_0 --> Token_10
None_0 --> Pattern_49
None_0 --> Consumer_10
None_0 --> Token_11
None_0 --> Pattern_51
None_0 --> Consumer_11
None_0 --> Token_12
None_0 --> Pattern_53
None_0 --> Consumer_12
None_0 --> Token_13
None_0 --> Pattern_55
None_0 --> Consumer_13
None_0 --> Token_14
None_0 --> Pattern_57
None_0 --> Consumer_14
None_0 --> Token_15
None_0 --> Pattern_59
None_0 --> Consumer_15
None_0 --> Token_16
None_0 --> Pattern_61
None_0 --> Consumer_16
None_0 --> Token_17
None_0 --> Pattern_65
None_0 --> Pattern_69
None_0 --> Consumer_17
None_0 --> Token_18
None_0 --> Pattern_71
None_0 --> Pattern_73
None_0 --> Consumer_18
None_0 --> Token_19
None_0 --> Pattern_76
None_0 --> Pattern_79
None_0 --> Consumer_19
None_0 --> Token_20
None_0 --> Pattern_82
None_0 --> Pattern_88
None_0 --> Pattern_89
None_0 --> Pattern_95
None_0 --> Consumer_20
None_0 --> Token_21
None_0 --> Pattern_98
None_0 --> Pattern_99
None_0 --> Pattern_102
None_0 --> Pattern_103
None_0 --> Pattern_105
None_0 --> Consumer_21
None_0 --> Pattern_108
None_0 --> Pattern_109
None_0 --> Pattern_111
None_0 --> Pattern_113
None_0 --> Consumer_22
None_0 --> Token_22
None_0 --> Pattern_115
None_0 --> Consumer_23
None_0 --> Pattern_117
None_0 --> Consumer_24
None_0 --> Pattern_119
None_0 --> Consumer_25
None_0 --> Pattern_121
None_0 --> Consumer_26
None_0 --> Pattern_123
None_0 --> Consumer_27
None_0 --> Token_23
None_0 --> Pattern_126
None_0 --> Pattern_128
None_0 --> Consumer_28
None_0 --> Pattern_131
None_0 --> Pattern_134
None_0 --> Pattern_136
None_0 --> Consumer_29
None_0 --> Token_24
None_0 --> Pattern_139
None_0 --> Pattern_141
None_0 --> Consumer_30
None_0 --> Token_25
None_0 --> Pattern_143
None_0 --> Pattern_145
None_0 --> Pattern_147
None_0 --> Consumer_31
None_0 --> Token_26
None_0 --> Pattern_150
None_0 --> Pattern_161
None_0 --> Pattern_162
None_0 --> Pattern_173
None_0 --> Consumer_32
Token_0 --> Consumer_0
Pattern_3 --> Pattern_0
Pattern_3 --> Pattern_1
Pattern_3 --> Pattern_2
Pattern_4 --> Pattern_3
Consumer_0 --> Pattern_4
Token_1 --> Consumer_1
Pattern_13 --> Pattern_5
Pattern_13 --> Pattern_6
Pattern_13 --> Pattern_7
Pattern_13 --> Pattern_8
Pattern_13 --> Pattern_9
Pattern_13 --> Pattern_10
Pattern_13 --> Pattern_11
Pattern_13 --> Pattern_12
Pattern_14 --> Pattern_13
Consumer_1 --> Pattern_14
Token_2 --> Consumer_2
Pattern_19 --> Pattern_15
Pattern_19 --> Pattern_16
Pattern_19 --> Pattern_17
Pattern_19 --> Pattern_18
Pattern_20 --> Pattern_19
Consumer_2 --> Pattern_20
Token_3 --> Consumer_3
Pattern_26 --> Pattern_21
Pattern_26 --> Pattern_22
Pattern_26 --> Pattern_23
Pattern_26 --> Pattern_24
Pattern_26 --> Pattern_25
Pattern_27 --> Pattern_26
Consumer_3 --> Pattern_27
Token_4 --> Consumer_4
Pattern_31 --> Pattern_28
Pattern_31 --> Pattern_29
Pattern_31 --> Pattern_30
Pattern_32 --> Pattern_31
Consumer_4 --> Pattern_32
Token_5 --> Consumer_5
Pattern_38 --> Pattern_33
Pattern_38 --> Pattern_34
Pattern_38 --> Pattern_35
Pattern_38 --> Pattern_36
Pattern_38 --> Pattern_37
Pattern_39 --> Pattern_38
Consumer_5 --> Pattern_39
Token_6 --> Consumer_6
Pattern_41 --> Pattern_40
Consumer_6 --> Pattern_41
Token_7 --> Consumer_7
Pattern_43 --> Pattern_42
Consumer_7 --> Pattern_43
Token_8 --> Consumer_8
Pattern_45 --> Pattern_44
Consumer_8 --> Pattern_45
Token_9 --> Consumer_9
Pattern_47 --> Pattern_46
Consumer_9 --> Pattern_47
Token_10 --> Consumer_10
Pattern_49 --> Pattern_48
Consumer_10 --> Pattern_49
Token_11 --> Consumer_11
Pattern_51 --> Pattern_50
Consumer_11 --> Pattern_51
Token_12 --> Consumer_12
Pattern_53 --> Pattern_52
Consumer_12 --> Pattern_53
Token_13 --> Consumer_13
Pattern_55 --> Pattern_54
Consumer_13 --> Pattern_55
Token_14 --> Consumer_14
Pattern_57 --> Pattern_56
Consumer_14 --> Pattern_57
Token_15 --> Consumer_15
Pattern_59 --> Pattern_58
Consumer_15 --> Pattern_59
Token_16 --> Consumer_16
Pattern_61 --> Pattern_60
Consumer_16 --> Pattern_61
Token_17 --> Consumer_17
Pattern_65 --> Pattern_62
Pattern_65 --> Pattern_63
Pattern_65 --> Pattern_64
Pattern_69 --> Pattern_66
Pattern_69 --> Pattern_67
Pattern_69 --> Pattern_68
Consumer_17 --> Pattern_65
Consumer_17 --> Pattern_69
Token_18 --> Consumer_18
Pattern_71 --> Pattern_70
Pattern_73 --> Pattern_72
Consumer_18 --> Pattern_71
Consumer_18 --> Pattern_73
Token_19 --> Consumer_19
Pattern_76 --> Pattern_74
Pattern_76 --> Pattern_75
Pattern_79 --> Pattern_77
Pattern_79 --> Pattern_78
Consumer_19 --> Pattern_76
Consumer_19 --> Pattern_79
Token_20 --> Consumer_20
Pattern_82 --> Pattern_80
Pattern_82 --> Pattern_81
Pattern_88 --> Pattern_83
Pattern_88 --> Pattern_84
Pattern_88 --> Pattern_85
Pattern_88 --> Pattern_86
Pattern_88 --> Pattern_87
Pattern_89 --> Pattern_82
Pattern_89 --> Pattern_88
Pattern_95 --> Pattern_90
Pattern_95 --> Pattern_91
Pattern_95 --> Pattern_92
Pattern_95 --> Pattern_93
Pattern_95 --> Pattern_94
Consumer_20 --> Pattern_89
Consumer_20 --> Pattern_95
Token_21 --> Consumer_21
Token_21 --> Consumer_22
Pattern_98 --> Pattern_96
Pattern_98 --> Pattern_97
Pattern_99 --> Pattern_98
Pattern_102 --> Pattern_100
Pattern_102 --> Pattern_101
Pattern_103 --> Pattern_102
Pattern_105 --> Pattern_104
Consumer_21 --> Pattern_99
Consumer_21 --> Pattern_103
Consumer_21 --> Pattern_105
Pattern_108 --> Pattern_106
Pattern_108 --> Pattern_107
Pattern_109 --> Pattern_108
Pattern_111 --> Pattern_110
Pattern_113 --> Pattern_112
Consumer_22 --> Pattern_109
Consumer_22 --> Pattern_111
Consumer_22 --> Pattern_113
Token_22 --> Consumer_23
Token_22 --> Consumer_24
Token_22 --> Consumer_25
Token_22 --> Consumer_26
Token_22 --> Consumer_27
Pattern_115 --> Pattern_114
Consumer_23 --> Pattern_115
Pattern_117 --> Pattern_116
Consumer_24 --> Pattern_117
Pattern_119 --> Pattern_118
Consumer_25 --> Pattern_119
Pattern_121 --> Pattern_120
Consumer_26 --> Pattern_121
Pattern_123 --> Pattern_122
Consumer_27 --> Pattern_123
Token_23 --> Consumer_28
Token_23 --> Consumer_29
Pattern_126 --> Pattern_124
Pattern_126 --> Pattern_125
Pattern_128 --> Pattern_127
Consumer_28 --> Pattern_126
Consumer_28 --> Pattern_128
Pattern_131 --> Pattern_129
Pattern_131 --> Pattern_130
Pattern_134 --> Pattern_132
Pattern_134 --> Pattern_133
Pattern_136 --> Pattern_135
Consumer_29 --> Pattern_131
Consumer_29 --> Pattern_134
Consumer_29 --> Pattern_136
Token_24 --> Consumer_30
Pattern_139 --> Pattern_137
Pattern_139 --> Pattern_138
Pattern_141 --> Pattern_140
Consumer_30 --> Pattern_139
Consumer_30 --> Pattern_141
Token_25 --> Consumer_31
Pattern_143 --> Pattern_142
Pattern_145 --> Pattern_144
Pattern_147 --> Pattern_146
Consumer_31 --> Pattern_143
Consumer_31 --> Pattern_145
Consumer_31 --> Pattern_147
Token_26 --> Consumer_32
Pattern_150 --> Pattern_148
Pattern_150 --> Pattern_149
Pattern_161 --> Pattern_151
Pattern_161 --> Pattern_152
Pattern_161 --> Pattern_153
Pattern_161 --> Pattern_154
Pattern_161 --> Pattern_155
Pattern_161 --> Pattern_156
Pattern_161 --> Pattern_157
Pattern_161 --> Pattern_158
Pattern_161 --> Pattern_159
Pattern_161 --> Pattern_160
Pattern_162 --> Pattern_150
Pattern_162 --> Pattern_161
Pattern_173 --> Pattern_163
Pattern_173 --> Pattern_164
Pattern_173 --> Pattern_165
Pattern_173 --> Pattern_166
Pattern_173 --> Pattern_167
Pattern_173 --> Pattern_168
Pattern_173 --> Pattern_169
Pattern_173 --> Pattern_170
Pattern_173 --> Pattern_171
Pattern_173 --> Pattern_172
Consumer_32 --> Pattern_162
Consumer_32 --> Pattern_173
```
*/
