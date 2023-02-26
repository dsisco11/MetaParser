//HintName: MetaParser.MetaParser.complex_tokens.tokens.compound.g.cs
namespace Foo.Bar.Tokens;
public sealed partial class Parser
{
    private static bool consume_compound_token(global::System.ReadOnlySpan<char> source, out byte id, out int length)
    {
        switch (source)
        {
            case [ TokenId.Letters, ..]:
            {
                return consume_12();
            }
            case [ TokenId.Char_Solidus, TokenId.Char_Asterisk, ..]:
            {
                return consume_13();
            }
        }
        id = default;
        length = default;
        return false;
        
        bool consume_12()
        {
            var start = source;
            id = TokenId.Identifier;
            buffer = start.Slice(1);
            
            while (buffer.Length > 0)
            {
                if (buffer.StartsWith(stackalloc []{ TokenId.Letters or TokenId.Digits}))
                /* If we have a set of valid consume targets, then try and consume as many as possible (the stop seq should be mutually exclusive with the set of consumables) */
                {
                    buffer = buffer.Slice(1);
                    // consumer forces moving on to next loop
                    continue;
                }
                
                // otherwise, default behaviour is to stop looping
                break;
            }
            
            length = start.Length - buffer.Length;
            return true;
        }
        bool consume_13()
        {
            var start = source;
            id = TokenId.Comment;
            buffer = start.Slice(2);
            while (buffer.Length > 0)
            {
                if (buffer.StartsWith(stackalloc []{ TokenId.Char_Asterisk, TokenId.Char_Solidus}))
                /* If we have a stop sequence, check for it */
                {
                    /* end consumption */
                    break;
                }
                
                // Token doesn't specify any consumables, thus ALL items are considered valid consumables
                buffer = buffer.Slice(1);
            }
            
            if (buffer .StartsWith(stackalloc []{ TokenId.Char_Asterisk, TokenId.Char_Solidus}))
            {
                length = 2 + (start.Length - buffer.Length);
                return true;
            }
            
            length = default;
            return false;
        }
    }
}
