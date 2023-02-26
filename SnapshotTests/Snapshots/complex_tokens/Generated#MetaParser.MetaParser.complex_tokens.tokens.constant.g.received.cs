//HintName: MetaParser.MetaParser.complex_tokens.tokens.constant.g.cs
namespace Foo.Bar.Tokens;
public sealed partial class Parser
{
    private static bool consume_constant_token(global::System.ReadOnlySpan<char> source, out byte id, out int length)
    {
        switch (source)
        {
            case [ 'v', 'a', 'r', ..]:
            {
                id = TokenId.Keyword_Var;
                length = 3;
                return true;
            }
            case [ 'f', 'u', 'n', 'c', 't', 'i', 'o', 'n', ..]:
            {
                id = TokenId.Keyword_Function;
                length = 8;
                return true;
            }
            case [ '{', ..]:
            {
                id = TokenId.Char_Open_Bracket;
                length = 1;
                return true;
            }
            case [ '}', ..]:
            {
                id = TokenId.Char_Close_Bracket;
                length = 1;
                return true;
            }
            case [ '*', ..]:
            {
                id = TokenId.Char_Asterisk;
                length = 1;
                return true;
            }
            case [ '/', ..]:
            {
                id = TokenId.Char_Solidus;
                length = 1;
                return true;
            }
            case [ '\\', ..]:
            {
                id = TokenId.Char_Reverse_Solidus;
                length = 1;
                return true;
            }
            case [ ' ' or '\t' or '\f', ..]:
            {
                return consume_8();
            }
            case [ (>='0' and <='9'), ..]:
            {
                return consume_9();
            }
            case [ (>='a' and <='z') or (>='A' and <='Z'), ..]:
            {
                return consume_10();
            }
            case [ '\n', ..]:
            {
                return consume_11();
            }
        }
        id = default;
        length = default;
        return false;
        
        bool consume_8()
        {
            var start = source;
            id = TokenId.Whitespace;
            buffer = start.Slice(1);
            
            while (buffer.Length > 0)
            {
                if (buffer.StartsWith(stackalloc []{ ' ' or '\t' or '\f'}))
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
        bool consume_9()
        {
            var start = source;
            id = TokenId.Digits;
            buffer = start.Slice(1);
            
            while (buffer.Length > 0)
            {
                if (buffer is [ (>='0' and <='9'), ..])
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
        bool consume_10()
        {
            var start = source;
            id = TokenId.Letters;
            buffer = start.Slice(1);
            
            while (buffer.Length > 0)
            {
                if (buffer is [ (>='a' and <='z') or (>='A' and <='Z'), ..])
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
        bool consume_11()
        {
            var start = source;
            id = TokenId.Newline;
            buffer = start.Slice(1);
            
            while (buffer.Length > 0)
            {
                if (buffer.StartsWith(stackalloc []{ '\n'}))
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
    }
}
