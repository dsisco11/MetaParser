//HintName: MetaParser.MetaParser.parser.parser.class.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        public Token[] Parse(global::System.ReadOnlyMemory<char> buffer0)
        {
            var buffer1 = Execute_Parsing_Table_0(buffer0);
            return buffer1;
            
        }
        private static global::System.Collections.Generic.List<byte> Execute_Parsing_Table_0(global::System.ReadOnlyMemory<char> buffer1)
        {
                var buffer2 = buffer1;
                var buffer3 = buffer2.Span;
                var results = new global::System.Collections.Generic.List<byte>();
                
                while (buffer3.Length > 0)
                {
                    var processed = process_parser_table_0(buffer3);
                    if (processed.length != default)
                    {
                        if (buffer2.Length != buffer3.Length)
                        {
                            var unk_content_size = buffer2.Length - buffer3.Length;
                            var unk_content = buffer2.Slice(0, unk_content_size);
                            results.Add(new ValueToken(TokenId.Unknown, unk_content));
                            buffer2 = buffer2.Slice(unk_content_size);
                            buffer3 = buffer2.Span;
                        }
                        
                        var consumed = buffer2.Slice(0, processed.length);
                        results.Add( new ValueToken(processed.id, consumed) );
                        buffer2 = buffer2.Slice(processed.length);
                        buffer3 = buffer2.Span;
                    }
                    else
                    {
                        buffer3 = buffer3.Slice(1);
                    }
                }
                
                if (buffer2.Length != buffer3.Length)
                {
                    var unk_content_size = buffer2.Length - buffer3.Length;
                    var unk_content = buffer2.Slice(0, unk_content_size);
                    results.Add(new ValueToken(TokenId.Unknown, unk_content));
                    buffer2 = buffer2.Slice(unk_content_size);
                    buffer3 = buffer2.Span;
                }
                
                return results.ToArray();
            }
        }
    }
