//HintName: MetaParser.MetaParser.constant_tokens.token.struct.g.cs
namespace Foo.Bar.Tokens;
public readonly record struct ValueToken(System.Int32 Id, global::System.ReadOnlyMemory<System.Char> Data)
{
    public override string ToString()
    {
        return $"{(EToken)Id}({Data})";
    }
}

public sealed record Token(EToken Id, ValueToken[] Values)
{
    public override string ToString()
    {
        return $"{Id}[{String.Join(", ", Values.Select(o => o.ToString()))}]";
    }
}
