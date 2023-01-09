using MetaParser.Tokens;

namespace MetaParser.Rules
{
    /// <summary>
    /// Static rules always return the same token instance and detect matches by comparing the current stream to said tokens value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <inheritdoc cref="ITokenRule{T}"/>
    public sealed class AutoRule<T> : ITokenRule<T>
        where T : IEquatable<T>
    {
        private readonly IStaticToken<T> Instance;

        public AutoRule(IStaticToken<T> instance)
        {
            Instance = instance;
        }

        public bool TryConsume(ITokenizer<T> Tokenizer, IToken<T> Previous, out IToken<T>? Token)
        {
            var rd = Tokenizer.GetReader();
            if(rd.IsNext(Instance.Value, advancePast: true))
            {
                Tokenizer.TryConsume(rd, out _);
                Token = Instance;
                return true;
            }

            Token = null;
            return false;
        }
    }
}
