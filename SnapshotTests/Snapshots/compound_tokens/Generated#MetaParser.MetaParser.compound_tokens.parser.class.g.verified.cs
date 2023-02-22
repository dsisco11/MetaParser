//HintName: MetaParser.MetaParser.compound_tokens.parser.class.g.cs
namespace Foo.Bar.Tokens;
public sealed partial class Parser
{
    public System.Collections.Generic.List<Token> Parse(global::System.ReadOnlyMemory<char> Input)
    {
        var buffer = Input;
        var reader = buffer.Span;
        var valueTokens = new System.Collections.Generic.List<ValueToken>();
        
        do
        {
            if (TryConsume(reader, out var outId, out var outLen))
            {
                if (buffer.Length != reader.Length)
                {
                    var unk_content_size = buffer.Length - reader.Length;
                    var unk_content = buffer.Slice(0, unk_content_size);
                    valueTokens.Add(new ValueToken(TokenId.Unknown, unk_content));
                    buffer = buffer.Slice(unk_content_size);
                    reader = buffer.Span;
                }
                
                var consumed = buffer.Slice(0, outLen);
                valueTokens.Add( new ValueToken(outId, consumed) );
                buffer = buffer.Slice(outLen);
                reader = buffer.Span;
            }
            else
            {
                reader = reader.Slice(1);
            }
        }
        while (reader.Length > 0);
        
        if (buffer.Length != reader.Length)
        {
            var unk_content_size = buffer.Length - reader.Length;
            var unk_content = buffer.Slice(0, unk_content_size);
            valueTokens.Add(new ValueToken(TokenId.Unknown, unk_content));
            buffer = buffer.Slice(unk_content_size);
            reader = buffer.Span;
        }
        
        var idValues = new byte[valueTokens.Count];
        for (int i = 0; i < valueTokens.Count; i++)
        {
            idValues[i] = valueTokens[i].Id;
        }
        var idSource = new global::System.ReadOnlyMemory<byte>( idValues );
        
        int offset = 0;
        var idBuffer = idSource;
        System.Collections.Generic.List<Token> results = new();
        do
        {
            if (consume_complex_token(idBuffer.Span, out var outId, out var outLength))
            {
                var values = new ValueToken[outLength];
                valueTokens.CopyTo(offset, values, 0, outLength);
                results.Add(new Token((ETokenType) outId, values));
                
                offset += outLength;
                idBuffer = idBuffer.Slice(outLength);
                continue;
            }
            
            // Proxy the current value-token
            var proxyValue = valueTokens[offset];
            var token = new Token((ETokenType) proxyValue.Id, new[] { proxyValue });
            results.Add(token);
            offset += 1;
            idBuffer = idBuffer.Slice(1);
        }
        while (idBuffer.Length > 0);
        
        return results;
    }
    private static bool TryConsume(global::System.ReadOnlySpan<char> source, out byte Id, out int Length)
    {
        if (consume_constant_token(source, out var constId, out var constLen))
        {
            Id = constId;
            Length = constLen;
            return true;
        }
        else if (consume_compound_token(source, out var compId, out var compLen))
        {
            Id = compId;
            Length = compLen;
            return true;
        }
        
        Id = default;
        Length = default;
        return false;
    }
}
