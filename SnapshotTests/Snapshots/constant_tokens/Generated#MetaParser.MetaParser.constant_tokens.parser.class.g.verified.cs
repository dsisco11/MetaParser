//HintName: MetaParser.MetaParser.constant_tokens.parser.class.g.cs
namespace Foo.Bar.Tokens;
public sealed partial class Parser
{
    public System.Collections.Generic.List<Token> Parse(global::System.ReadOnlyMemory<System.Char> Input)
    {
        System.Collections.Generic.List<ValueToken> valueTokens = new();
        var Source = Input;
        do
        {
            if (try_consume_token_constant(Source, out var constId, out var constLen))
            {
                valueTokens.Add(new ValueToken(constId, Source.Slice(0, constLen)));
                Source = Source.Slice(constLen);
            }
            else if (try_consume_token_compound(Source, out var compId, out var compLen))
            {
                valueTokens.Add(new ValueToken(compId, Source.Slice(0, compLen)));
                Source = Source.Slice(compLen);
            }
            else // Consume 'unknown' token
            {
                valueTokens.Add(new ValueToken(0, Source.Slice(0, 1)));
                Source = Source.Slice(1);
            }
        }
        while (Source.Length > 0);
        
        var idValues = new System.Int32[valueTokens.Count];
        for (int i = 0; i < valueTokens.Count; i++)
        {
            idValues[i] = valueTokens[i].Id;
        }
        var idSource = new global::System.ReadOnlyMemory<System.Int32>( idValues );
        
        System.Int32 offset = 0;
        var buffer = idSource;
        System.Collections.Generic.List<Token> results = new();
        do
        {
            if (try_consume_token_complex(buffer, out var outId, out var outLength))
            {
                var values = new ValueToken[outLength];
                valueTokens.CopyTo(offset, values, 0, outLength);
                results.Add(new Token((EToken) outId, values));
                
                offset += outLength;
                buffer = buffer.Slice(outLength);
                continue;
            }
            
            // Proxy the current value-token
            var proxyValue = valueTokens[offset];
            var token = new Token((EToken) proxyValue.Id, new[] { proxyValue });
            results.Add(token);
            offset += 1;
            buffer = buffer.Slice(1);
        }
        while (buffer.Length > 0);
        
        return results;
    }
}
