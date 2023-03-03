//HintName: MetaParser.MetaParser.complex_tokens.parser.class.g.cs
namespace UnitTestParser;
{
    public sealed partial class Parser
    {
        public Token[] Parse(global::System.ReadOnlyMemory<char> stream)
        {
            var tokensArray = Parse_Constant(stream);
            var tokensBuffer = new global::System.ReadOnlyMemory<ValueToken>( tokensArray );
            return Parse_Compound(tokensBuffer);
            
        }
        private static ValueToken[] Parse_Constant(global::System.ReadOnlyMemory<char> stream)
        {
            var buffer = stream;
            var reader = buffer.Span;
            var results = new global::System.Collections.Generic.List<ValueToken>();
            
            while (reader.Length > 0)
            {
                if (TryProcessConstant(reader, out var outId, out var outLength))
                {
                    if (buffer.Length != reader.Length)
                    {
                        var unk_content_size = buffer.Length - reader.Length;
                        var unk_content = buffer.Slice(0, unk_content_size);
                        results.Add(new ValueToken(TokenId.Unknown, unk_content));
                        buffer = buffer.Slice(unk_content_size);
                        reader = buffer.Span;
                    }
                    
                    var consumed = buffer.Slice(0, outLength);
                    results.Add( new ValueToken(outId, consumed) );
                    buffer = buffer.Slice(outLength);
                    reader = buffer.Span;
                }
                else
                {
                    reader = reader.Slice(1);
                }
            }
            
            if (buffer.Length != reader.Length)
            {
                var unk_content_size = buffer.Length - reader.Length;
                var unk_content = buffer.Slice(0, unk_content_size);
                results.Add(new ValueToken(TokenId.Unknown, unk_content));
                buffer = buffer.Slice(unk_content_size);
                reader = buffer.Span;
            }
            
            return results.ToArray();
        }
        private static Token[] Parse_Compound(global::System.ReadOnlyMemory<ValueToken> stream)
        {
            var idValues = new byte[stream.Length];
            for (int i = 0; i < stream.Length; i++)
            {
                idValues[i] = stream.Span[i].Id;
            }
            
            var buffer = new global::System.ReadOnlyMemory<byte>( idValues );
            var reader = buffer.Span;
            var results = new global::System.Collections.Generic.List<Token>();
            
            while (reader.Length > 0)
            {
                if (TryProcessCompound(reader, out var outId, out var outLength))
                {
                    var consumed = stream.Slice(0, outLength).ToArray();
                    results.Add(new Token((ETokenType) outId, consumed) );
                    
                    stream = stream.Slice(outLength);
                    buffer = buffer.Slice(outLength);
                    reader = buffer.Span;
                }
                else
                {
                    /* Proxy the current token as it has no special compound behavior */
                    var consumed = stream.Span[0];
                    results.Add(new Token((ETokenType) consumed.Id, new[] { consumed }));
                    stream = stream.Slice(1);
                    buffer = buffer.Slice(1);
                    reader = buffer.Span;
                }
            }
            
            return results.ToArray();
        }
        private static Token[] Parse_Complex(global::System.ReadOnlyMemory<Token> stream)
        {
            return Array.Empty<Token>();
        }
    }
}
