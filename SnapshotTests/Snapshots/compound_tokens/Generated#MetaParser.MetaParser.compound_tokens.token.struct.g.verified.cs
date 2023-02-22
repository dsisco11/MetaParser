//HintName: MetaParser.MetaParser.compound_tokens.token.struct.g.cs
namespace Foo.Bar.Tokens;
[System.Diagnostics.DebuggerDisplay("{Data}", Name = "{(ETokenType)Id}")]
public readonly record struct ValueToken(byte Id, global::System.ReadOnlyMemory<char> Data)
{
    public override string ToString()
    {
        return Data.ToString();
    }
}

[System.Diagnostics.DebuggerDisplay("{this.ToString()}", Name = "{(ETokenType)Id}")]
public sealed record Token(ETokenType Id, ValueToken[] Values)
{
    public override string ToString()
    {
        var sb = new global::System.Text.StringBuilder();
        for (int i=0; i<Values.Length; i++)
        {
            sb.Append(Values[i].Data.ToString());
        }
        
        return sb.ToString();
    }
}
