//HintName: MetaParser.MetaParser.linear_parser.parser.class.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        public Token[] Parse(global::System.ReadOnlyMemory<char> input)
        {
            var tokensArray = Parse_Constant(input);
            var tokensBuffer = new global::System.ReadOnlyMemory<ValueToken>( tokensArray );
            return Parse_Compound(tokensBuffer);
            
        }
        private static ValueToken[] Parse_Constant(global::System.ReadOnlyMemory<char> input)
        {
            var buffer = input;
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
        private static Token[] Parse_Compound(global::System.ReadOnlyMemory<ValueToken> input)
        {
            var idValues = new byte[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                idValues[i] = input.Span[i].Id;
            }
            
            var buffer = new global::System.ReadOnlyMemory<byte>( idValues );
            var reader = buffer.Span;
            var results = new global::System.Collections.Generic.List<Token>();
            
            while (reader.Length > 0)
            {
                if (TryProcessCompound(reader, out var outId, out var outLength))
                {
                    var consumed = input.Slice(0, outLength).ToArray();
                    results.Add(new Token((ETokenType) outId, consumed) );
                    
                    input = input.Slice(outLength);
                    buffer = buffer.Slice(outLength);
                    reader = buffer.Span;
                }
                else
                {
                    /* Proxy the current token as it has no special compound behavior */
                    var consumed = input.Span[0];
                    results.Add(new Token((ETokenType) consumed.Id, new[] { consumed }));
                    input = input.Slice(1);
                    buffer = buffer.Slice(1);
                    reader = buffer.Span;
                }
            }
            
            return results.ToArray();
        }
        private static Token[] Parse_Complex(global::System.ReadOnlyMemory<Token> input)
        {
            return Array.Empty<Token>();
        }
    }
}
