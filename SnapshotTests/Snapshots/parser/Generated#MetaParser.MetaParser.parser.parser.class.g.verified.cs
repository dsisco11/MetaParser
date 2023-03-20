//HintName: MetaParser.MetaParser.parser.parser.class.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        public Token[] Parse(global::System.ReadOnlyMemory<char> buffer0)
        {
            var tokensArray = Parse_Constant(buffer0);
            var tokensBuffer = new global::System.ReadOnlyMemory<ValueToken>( tokensArray );
            return Parse_Compound(tokensBuffer);
            
        }
        private static ValueToken[] Parse_Constant(global::System.ReadOnlyMemory<char> buffer0)
        {
            var buffer1 = buffer0;
            var buffer2 = buffer1.Span;
            var results = new global::System.Collections.Generic.List<ValueToken>();
            
            while (buffer2.Length > 0)
            {
                var processed = TryProcessingLexerToken(buffer2);
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
        private static Token[] Parse_Compound(global::System.ReadOnlyMemory<ValueToken> buffer1)
        {
            var idValues = new byte[buffer1.Length];
            for (int i = 0; i < buffer1.Length; i++)
            {
                idValues[i] = buffer1.Span[i].Id;
            }
            
            var buffer2 = new global::System.ReadOnlyMemory<byte>( idValues );
            var buffer3 = buffer2.Span;
            var results = new global::System.Collections.Generic.List<Token>();
            
            while (buffer3.Length > 0)
            {
                var processed = TryProcessingSyntaxToken(buffer3);
                if (processed.length != default)
                {
                    var consumed = buffer1.Slice(0, processed.length).ToArray();
                    results.Add(new Token((ETokenType) processed.id, consumed) );
                    
                    buffer1 = buffer1.Slice(processed.length);
                    buffer2 = buffer2.Slice(processed.length);
                    buffer3 = buffer2.Span;
                }
                else
                {
                    /* Forward the token on to the next stage */
                    var consumed = buffer1.Span[0];
                    results.Add(new Token((ETokenType) consumed.Id, new[] { consumed }));
                    buffer1 = buffer1.Slice(1);
                    buffer2 = buffer2.Slice(1);
                    buffer3 = buffer2.Span;
                }
            }
            
            return results.ToArray();
        }
        private static Token[] Parse_Complex(global::System.ReadOnlyMemory<Token> buffer2)
        {
            return Array.Empty<Token>();
        }
    }
}
