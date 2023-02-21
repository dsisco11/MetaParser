//HintName: MetaParser.MetaParser.complex_tokens.tokens.complex.g.cs
namespace Foo.Bar.Tokens;
public sealed partial class Parser
{
    private bool try_consume_token_complex (global::System.ReadOnlyMemory<System.Int32> source, out System.Int32 id, out System.Int32 length)
    {
        switch (source.Span)
        {
            case [TokenId.Char_Solidus, TokenId.Char_Asterisk, ..]:
            {
                id = TokenId.Comment;
                return consume_comment_token(source.Span, out length);
            }
            case [TokenId.Letters, ..]:
            {
                id = TokenId.Identifier;
                return consume_identifier_token(source.Span, out length);
            }
        }
        
        id = default;
        length = default;
        return false;
        
        
        static bool consume_comment_token (global::System.ReadOnlySpan<System.Int32> start, out System.Int32 consumed)
        {
            #if DEBUG
                System.Diagnostics.Debug.Assert(start.StartsWith(stackalloc[] { TokenId.Char_Solidus, TokenId.Char_Asterisk }));
            #endif
            var buffer = start.Slice(2);
            Span<System.Int32> seqEndTerminator = stackalloc[] { TokenId.Char_Asterisk, TokenId.Char_Solidus };
            
            while (buffer.Length > 0)
            {
                if (buffer.StartsWith(seqEndTerminator))
                {
                    break;// end consumption
                }
                
                buffer = buffer.Slice(1);
            }
            
            if (buffer.StartsWith(seqEndTerminator))
            {
                consumed = 2 + (start.Length - buffer.Length);
                return true;
            }
            
            consumed = default;
            return false;
        }
        
        static bool consume_identifier_token (global::System.ReadOnlySpan<System.Int32> start, out System.Int32 consumed)
        {
            #if DEBUG
                System.Diagnostics.Debug.Assert(start.StartsWith(stackalloc[] { TokenId.Letters }));
            #endif
            var buffer = start.Slice(1);
            
            while (buffer.Length > 0)
            {
                switch (buffer[0])
                {
                    case TokenId.Letters:
                    case TokenId.Digits:
                        buffer = buffer.Slice(1);
                        continue;
                }
                
                break;
            }
            
            consumed = start.Length - buffer.Length;
            return true;
        }
    }
}
