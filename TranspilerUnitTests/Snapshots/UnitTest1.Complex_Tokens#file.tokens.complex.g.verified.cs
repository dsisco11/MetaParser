//HintName: file.tokens.complex.g.cs
namespace Foo.Bar.Tokens;
public partial class Parser : MetaParser.Parser<char>
{
    private static bool try_consume_complex_token (System.Memory.ReadOnlyMemory<byte> source, out byte id, out int length)
    {
        switch (source)
        {
            case [TokenId.LETTERS]:
            {
                id = TokenId.IDENTIFIER;
                return _consume_identifier_token(source, out length);
            }
        }
        
        id = default;
        length = default;
        return false;
        
        static bool _consume_identifier_token (System.Memory.ReadOnlySpan<byte> start, out int consumed)
        {
            #if DEBUG
                Debug.Assert(buffer.StartsWith(stackalloc[] { TokenId.LETTERS }));
            #endif
            buffer = source.Slice(1)
            
            while (buffer.Length > 0)
            {
                switch (buffer)
                {
                    case TokenId.LETTERS:
                    case TokenId.DIGITS:
                        buffer = buffer.Slice(1);
                        continue;
                }
                
                break;
            }
            
            consumed = source.Length - buffer.Length;
            return true;// end consumption
        }
    }
}
