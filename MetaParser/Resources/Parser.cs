public abstract class Parser<InputType, ValueType, EnumType>
    where InputType : unmanaged, System.IEquatable<InputType>
    where ValueType : unmanaged, System.IEquatable<ValueType>
    where EnumType : unmanaged, System.Enum
{
    #region Types
    public record Token(EnumType Id, ValueToken[] Values);

    public readonly struct ValueToken
    {
        public readonly ValueType Id;
        public readonly System.ReadOnlyMemory<InputType> Data;

        public ValueToken(ValueType Id, System.ReadOnlyMemory<InputType> Data)
        {
            this.Id = Id;
            this.Data = Data;
        }
    }
    #endregion

    public System.Collections.Generic.List<Token> Parse(System.ReadOnlyMemory<InputType> Input)
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

        var idSource = new System.Memory<ValueType>(new ValueType[valueTokens.Count]);
        for (int i = 0; i < valueTokens.Count; i++)
        {
            idSource.Span[i] = valueTokens[i].Id;
        }

        return parse_complex(idSource, valueTokens);
    }

    protected abstract System.Collections.Generic.List<Token> parse_complex(System.ReadOnlyMemory<ValueType> Source, System.Collections.Generic.List<ValueToken> Tokens);

    protected abstract bool try_consume_token_constant(System.ReadOnlyMemory<InputType> Source, out ValueType Id, out int Length);
    protected abstract bool try_consume_token_compound(System.ReadOnlyMemory<InputType> Source, out ValueType Id, out int Length);
    protected abstract bool try_consume_token_complex(System.ReadOnlyMemory<int> Source, out ValueType Id, out int Length);
}
