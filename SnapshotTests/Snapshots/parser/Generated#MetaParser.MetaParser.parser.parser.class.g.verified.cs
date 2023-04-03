//HintName: MetaParser.MetaParser.parser.parser.class.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        public Token[] Parse(global::System.ReadOnlyMemory<char> buffer0)
        {
            var buffer1 = Execute_Parsing_Table_0(buffer0);
            var buffer2 = Execute_Parsing_Table_1(buffer1);
            var buffer3 = Execute_Parsing_Table_2(buffer2);
            return buffer3;
            
        }
        private static global::System.Collections.Generic.List<byte> Execute_Parsing_Table_0(global::System.ReadOnlyMemory<byte> buffer3)
        {
                var buffer4 = buffer3;
                var buffer5 = buffer4.Span;
                var results = new global::System.Collections.Generic.List<byte>();
                
                while (buffer5.Length > 0)
                {
                    var processed = process_parser_table_0(buffer5);
                    if (processed.length != default)
                    {
                        if (buffer4.Length != buffer5.Length)
                        {
                            var unk_content_size = buffer4.Length - buffer5.Length;
                            var unk_content = buffer4.Slice(0, unk_content_size);
                            results.Add(new ValueToken(TokenId.Unknown, unk_content));
                            buffer4 = buffer4.Slice(unk_content_size);
                            buffer5 = buffer4.Span;
                        }
                        
                        var consumed = buffer4.Slice(0, processed.length);
                        results.Add( new ValueToken(processed.id, consumed) );
                        buffer4 = buffer4.Slice(processed.length);
                        buffer5 = buffer4.Span;
                    }
                    else
                    {
                        buffer5 = buffer5.Slice(1);
                    }
                }
                
                if (buffer4.Length != buffer5.Length)
                {
                    var unk_content_size = buffer4.Length - buffer5.Length;
                    var unk_content = buffer4.Slice(0, unk_content_size);
                    results.Add(new ValueToken(TokenId.Unknown, unk_content));
                    buffer4 = buffer4.Slice(unk_content_size);
                    buffer5 = buffer4.Span;
                }
                
                return results.ToArray();
            }
            private static global::System.Collections.Generic.List<byte> Execute_Parsing_Table_1(global::System.ReadOnlyMemory<byte> buffer4)
            {
                    var buffer5 = buffer4;
                    var buffer6 = buffer5.Span;
                    var results = new global::System.Collections.Generic.List<byte>();
                    
                    while (buffer6.Length > 0)
                    {
                        var processed = process_parser_table_1(buffer6);
                        if (processed.length != default)
                        {
                            if (buffer5.Length != buffer6.Length)
                            {
                                var unk_content_size = buffer5.Length - buffer6.Length;
                                var unk_content = buffer5.Slice(0, unk_content_size);
                                results.Add(new ValueToken(TokenId.Unknown, unk_content));
                                buffer5 = buffer5.Slice(unk_content_size);
                                buffer6 = buffer5.Span;
                            }
                            
                            var consumed = buffer5.Slice(0, processed.length);
                            results.Add( new ValueToken(processed.id, consumed) );
                            buffer5 = buffer5.Slice(processed.length);
                            buffer6 = buffer5.Span;
                        }
                        else
                        {
                            buffer6 = buffer6.Slice(1);
                        }
                    }
                    
                    if (buffer5.Length != buffer6.Length)
                    {
                        var unk_content_size = buffer5.Length - buffer6.Length;
                        var unk_content = buffer5.Slice(0, unk_content_size);
                        results.Add(new ValueToken(TokenId.Unknown, unk_content));
                        buffer5 = buffer5.Slice(unk_content_size);
                        buffer6 = buffer5.Span;
                    }
                    
                    return results.ToArray();
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
                }
            }
