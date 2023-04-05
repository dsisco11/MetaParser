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
        private static global::System.Collections.Generic.List<byte> Execute_Parsing_Table_0(global::System.ReadOnlyMemory<byte> buffer0)
        {
                var buffer1 = buffer0;
                var buffer2 = buffer1.Span;
                var results = new global::System.Collections.Generic.List<byte>();
                
                while (buffer2.Length > 0)
                {
                    var processed = process_parser_table_0(buffer2);
                    if (processed.length != default)
                    {
                        if (buffer1.Length != buffer2.Length)
                        {
                            var unk_content_size = buffer1.Length - buffer2.Length;
                            var unk_content = buffer1.Slice(0, unk_content_size);
                            results.Add(new ValueToken(TokenId.Unknown, unk_content));
                            buffer1 = buffer1.Slice(unk_content_size);
                            buffer2 = buffer1.Span;
                        }
                        
                        var consumed = buffer1.Slice(0, processed.length);
                        results.Add( new ValueToken(processed.id, consumed) );
                        buffer1 = buffer1.Slice(processed.length);
                        buffer2 = buffer1.Span;
                    }
                    else
                    {
                        buffer2 = buffer2.Slice(1);
                    }
                }
                
                if (buffer1.Length != buffer2.Length)
                {
                    var unk_content_size = buffer1.Length - buffer2.Length;
                    var unk_content = buffer1.Slice(0, unk_content_size);
                    results.Add(new ValueToken(TokenId.Unknown, unk_content));
                    buffer1 = buffer1.Slice(unk_content_size);
                    buffer2 = buffer1.Span;
                }
                
                return results.ToArray();
            }
            private static global::System.Collections.Generic.List<byte> Execute_Parsing_Table_1(global::System.ReadOnlyMemory<byte> buffer0)
            {
                    var buffer1 = buffer0;
                    var buffer2 = buffer1.Span;
                    var results = new global::System.Collections.Generic.List<byte>();
                    
                    while (buffer2.Length > 0)
                    {
                        var processed = process_parser_table_1(buffer2);
                        if (processed.length != default)
                        {
                            if (buffer1.Length != buffer2.Length)
                            {
                                var unk_content_size = buffer1.Length - buffer2.Length;
                                var unk_content = buffer1.Slice(0, unk_content_size);
                                results.Add(new ValueToken(TokenId.Unknown, unk_content));
                                buffer1 = buffer1.Slice(unk_content_size);
                                buffer2 = buffer1.Span;
                            }
                            
                            var consumed = buffer1.Slice(0, processed.length);
                            results.Add( new ValueToken(processed.id, consumed) );
                            buffer1 = buffer1.Slice(processed.length);
                            buffer2 = buffer1.Span;
                        }
                        else
                        {
                            buffer2 = buffer2.Slice(1);
                        }
                    }
                    
                    if (buffer1.Length != buffer2.Length)
                    {
                        var unk_content_size = buffer1.Length - buffer2.Length;
                        var unk_content = buffer1.Slice(0, unk_content_size);
                        results.Add(new ValueToken(TokenId.Unknown, unk_content));
                        buffer1 = buffer1.Slice(unk_content_size);
                        buffer2 = buffer1.Span;
                    }
                    
                    return results.ToArray();
                }
                private static global::System.Collections.Generic.List<byte> Execute_Parsing_Table_2(global::System.ReadOnlyMemory<byte> buffer0)
                {
                        var buffer1 = buffer0;
                        var buffer2 = buffer1.Span;
                        var results = new global::System.Collections.Generic.List<byte>();
                        
                        while (buffer2.Length > 0)
                        {
                            var processed = process_parser_table_2(buffer2);
                            if (processed.length != default)
                            {
                                if (buffer1.Length != buffer2.Length)
                                {
                                    var unk_content_size = buffer1.Length - buffer2.Length;
                                    var unk_content = buffer1.Slice(0, unk_content_size);
                                    results.Add(new ValueToken(TokenId.Unknown, unk_content));
                                    buffer1 = buffer1.Slice(unk_content_size);
                                    buffer2 = buffer1.Span;
                                }
                                
                                var consumed = buffer1.Slice(0, processed.length);
                                results.Add( new ValueToken(processed.id, consumed) );
                                buffer1 = buffer1.Slice(processed.length);
                                buffer2 = buffer1.Span;
                            }
                            else
                            {
                                buffer2 = buffer2.Slice(1);
                            }
                        }
                        
                        if (buffer1.Length != buffer2.Length)
                        {
                            var unk_content_size = buffer1.Length - buffer2.Length;
                            var unk_content = buffer1.Slice(0, unk_content_size);
                            results.Add(new ValueToken(TokenId.Unknown, unk_content));
                            buffer1 = buffer1.Slice(unk_content_size);
                            buffer2 = buffer1.Span;
                        }
                        
                        return results.ToArray();
                    }
                }
            }
