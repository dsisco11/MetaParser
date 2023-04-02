//HintName: MetaParser.MetaParser.parser.parser.class.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        public Token[] Parse(global::System.ReadOnlyMemory<char> buffer0)
        {
            var buffer1 = Execute_Parsing_Table_2(buffer0);
            var buffer2 = Execute_Parsing_Table_3(buffer1);
            var buffer3 = Execute_Parsing_Table_6(buffer2);
            var buffer4 = Execute_Parsing_Table_7(buffer3);
            var buffer5 = Execute_Parsing_Table_8(buffer4);
            return buffer5;
            
        }
        private static global::System.Collections.Generic.List<byte> Execute_Parsing_Table_2(global::System.ReadOnlyMemory<byte> buffer5)
        {
                var buffer6 = buffer5;
                var buffer7 = buffer6.Span;
                var results = new global::System.Collections.Generic.List<byte>();
                
                while (buffer7.Length > 0)
                {
                    var processed = process_parser_table_2(buffer7);
                    if (processed.length != default)
                    {
                        if (buffer6.Length != buffer7.Length)
                        {
                            var unk_content_size = buffer6.Length - buffer7.Length;
                            var unk_content = buffer6.Slice(0, unk_content_size);
                            results.Add(new ValueToken(TokenId.Unknown, unk_content));
                            buffer6 = buffer6.Slice(unk_content_size);
                            buffer7 = buffer6.Span;
                        }
                        
                        var consumed = buffer6.Slice(0, processed.length);
                        results.Add( new ValueToken(processed.id, consumed) );
                        buffer6 = buffer6.Slice(processed.length);
                        buffer7 = buffer6.Span;
                    }
                    else
                    {
                        buffer7 = buffer7.Slice(1);
                    }
                }
                
                if (buffer6.Length != buffer7.Length)
                {
                    var unk_content_size = buffer6.Length - buffer7.Length;
                    var unk_content = buffer6.Slice(0, unk_content_size);
                    results.Add(new ValueToken(TokenId.Unknown, unk_content));
                    buffer6 = buffer6.Slice(unk_content_size);
                    buffer7 = buffer6.Span;
                }
                
                return results.ToArray();
            }
            private static global::System.Collections.Generic.List<byte> Execute_Parsing_Table_3(global::System.ReadOnlyMemory<byte> buffer6)
            {
                    var buffer7 = buffer6;
                    var buffer8 = buffer7.Span;
                    var results = new global::System.Collections.Generic.List<byte>();
                    
                    while (buffer8.Length > 0)
                    {
                        var processed = process_parser_table_3(buffer8);
                        if (processed.length != default)
                        {
                            if (buffer7.Length != buffer8.Length)
                            {
                                var unk_content_size = buffer7.Length - buffer8.Length;
                                var unk_content = buffer7.Slice(0, unk_content_size);
                                results.Add(new ValueToken(TokenId.Unknown, unk_content));
                                buffer7 = buffer7.Slice(unk_content_size);
                                buffer8 = buffer7.Span;
                            }
                            
                            var consumed = buffer7.Slice(0, processed.length);
                            results.Add( new ValueToken(processed.id, consumed) );
                            buffer7 = buffer7.Slice(processed.length);
                            buffer8 = buffer7.Span;
                        }
                        else
                        {
                            buffer8 = buffer8.Slice(1);
                        }
                    }
                    
                    if (buffer7.Length != buffer8.Length)
                    {
                        var unk_content_size = buffer7.Length - buffer8.Length;
                        var unk_content = buffer7.Slice(0, unk_content_size);
                        results.Add(new ValueToken(TokenId.Unknown, unk_content));
                        buffer7 = buffer7.Slice(unk_content_size);
                        buffer8 = buffer7.Span;
                    }
                    
                    return results.ToArray();
                }
                private static global::System.Collections.Generic.List<byte> Execute_Parsing_Table_6(global::System.ReadOnlyMemory<byte> buffer7)
                {
                        var buffer8 = buffer7;
                        var buffer9 = buffer8.Span;
                        var results = new global::System.Collections.Generic.List<byte>();
                        
                        while (buffer9.Length > 0)
                        {
                            var processed = process_parser_table_6(buffer9);
                            if (processed.length != default)
                            {
                                if (buffer8.Length != buffer9.Length)
                                {
                                    var unk_content_size = buffer8.Length - buffer9.Length;
                                    var unk_content = buffer8.Slice(0, unk_content_size);
                                    results.Add(new ValueToken(TokenId.Unknown, unk_content));
                                    buffer8 = buffer8.Slice(unk_content_size);
                                    buffer9 = buffer8.Span;
                                }
                                
                                var consumed = buffer8.Slice(0, processed.length);
                                results.Add( new ValueToken(processed.id, consumed) );
                                buffer8 = buffer8.Slice(processed.length);
                                buffer9 = buffer8.Span;
                            }
                            else
                            {
                                buffer9 = buffer9.Slice(1);
                            }
                        }
                        
                        if (buffer8.Length != buffer9.Length)
                        {
                            var unk_content_size = buffer8.Length - buffer9.Length;
                            var unk_content = buffer8.Slice(0, unk_content_size);
                            results.Add(new ValueToken(TokenId.Unknown, unk_content));
                            buffer8 = buffer8.Slice(unk_content_size);
                            buffer9 = buffer8.Span;
                        }
                        
                        return results.ToArray();
                    }
                    private static global::System.Collections.Generic.List<byte> Execute_Parsing_Table_7(global::System.ReadOnlyMemory<byte> buffer8)
                    {
                            var buffer9 = buffer8;
                            var buffer10 = buffer9.Span;
                            var results = new global::System.Collections.Generic.List<byte>();
                            
                            while (buffer10.Length > 0)
                            {
                                var processed = process_parser_table_7(buffer10);
                                if (processed.length != default)
                                {
                                    if (buffer9.Length != buffer10.Length)
                                    {
                                        var unk_content_size = buffer9.Length - buffer10.Length;
                                        var unk_content = buffer9.Slice(0, unk_content_size);
                                        results.Add(new ValueToken(TokenId.Unknown, unk_content));
                                        buffer9 = buffer9.Slice(unk_content_size);
                                        buffer10 = buffer9.Span;
                                    }
                                    
                                    var consumed = buffer9.Slice(0, processed.length);
                                    results.Add( new ValueToken(processed.id, consumed) );
                                    buffer9 = buffer9.Slice(processed.length);
                                    buffer10 = buffer9.Span;
                                }
                                else
                                {
                                    buffer10 = buffer10.Slice(1);
                                }
                            }
                            
                            if (buffer9.Length != buffer10.Length)
                            {
                                var unk_content_size = buffer9.Length - buffer10.Length;
                                var unk_content = buffer9.Slice(0, unk_content_size);
                                results.Add(new ValueToken(TokenId.Unknown, unk_content));
                                buffer9 = buffer9.Slice(unk_content_size);
                                buffer10 = buffer9.Span;
                            }
                            
                            return results.ToArray();
                        }
                        private static global::System.Collections.Generic.List<byte> Execute_Parsing_Table_8(global::System.ReadOnlyMemory<byte> buffer9)
                        {
                                var buffer10 = buffer9;
                                var buffer11 = buffer10.Span;
                                var results = new global::System.Collections.Generic.List<byte>();
                                
                                while (buffer11.Length > 0)
                                {
                                    var processed = process_parser_table_8(buffer11);
                                    if (processed.length != default)
                                    {
                                        if (buffer10.Length != buffer11.Length)
                                        {
                                            var unk_content_size = buffer10.Length - buffer11.Length;
                                            var unk_content = buffer10.Slice(0, unk_content_size);
                                            results.Add(new ValueToken(TokenId.Unknown, unk_content));
                                            buffer10 = buffer10.Slice(unk_content_size);
                                            buffer11 = buffer10.Span;
                                        }
                                        
                                        var consumed = buffer10.Slice(0, processed.length);
                                        results.Add( new ValueToken(processed.id, consumed) );
                                        buffer10 = buffer10.Slice(processed.length);
                                        buffer11 = buffer10.Span;
                                    }
                                    else
                                    {
                                        buffer11 = buffer11.Slice(1);
                                    }
                                }
                                
                                if (buffer10.Length != buffer11.Length)
                                {
                                    var unk_content_size = buffer10.Length - buffer11.Length;
                                    var unk_content = buffer10.Slice(0, unk_content_size);
                                    results.Add(new ValueToken(TokenId.Unknown, unk_content));
                                    buffer10 = buffer10.Slice(unk_content_size);
                                    buffer11 = buffer10.Span;
                                }
                                
                                return results.ToArray();
                            }
                        }
                    }
