using MetaParser.Parsing.Tokens;

using System.Buffers;

namespace MetaParser.Parsing.Definitions
{
    /// <summary>
    /// Static rules always return the same token instance and detect matches by comparing the current stream to said tokens value.
    /// </summary>
    /// <typeparam name="Ty"></typeparam>
    /// <inheritdoc cref="ITokenRule{Ty}"/>
    public sealed record AutoRule<Ty> : ITokenRule<Ty>
        where Ty : unmanaged, IEquatable<Ty>
    {
        private readonly IToken<Ty> Instance;

        public AutoRule(IToken<Ty> instance)
        {
            Instance = instance;
            if (!Instance.Value.IsSingleSegment)
            {
                throw new Exception($"{nameof(AutoRule<Ty>)} can only accept constant tokens whose value is wholly contained within a contiguous block of memory.");
            }
        }

        public bool Check(IReadOnlyTokenizer<Ty> Tokenizer, IToken<Ty> Previous)
        {
            return Tokenizer.Get_Reader().IsNext(Instance.Value.FirstSpan);
        }

        public IToken<Ty>? Consume(ITokenizer<Ty> Tokenizer, IToken<Ty> Previous)
        {
            var rd = Tokenizer.Get_Reader();
            rd.Advance(1);
            Tokenizer.Consume(ref rd);
            return Instance;
        }
    }
}
