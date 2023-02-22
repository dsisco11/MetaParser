//HintName: MetaParser.MetaParser.constant_tokens.token.struct.g.cs
namespace Foo.Bar.Tokens;
public readonly record struct ValueToken(byte Id, global::System.ReadOnlyMemory<char> Data)
{
    public override string ToString()
    {
        return $"{(ETokenType)Id}({Data})";
    }
}

public sealed record Token(ETokenType Id, ValueToken[] Values)
{
    public override string ToString()
    {
        return $"{Id}[{String.Join(", ", Values.Select(o => o.ToString()))}]";
    }
}
