using System.Buffers;
using System.Diagnostics;

namespace MetaParser.Rules
{
    [DebuggerDisplay("PredicateRule :: {Definition}")]
    public class PredicateRule<TEnum, TValue> : ITokenRule<TEnum, TValue>
        where TEnum : unmanaged, IEquatable<TEnum>
        where TValue : unmanaged, IEquatable<TValue>
    {
        public RuleSpecificty Specificity => new RuleSpecificty(true, true);

        #region Properties
        private TEnum Definition { get; init; }
        public Predicate<TValue> Predicate { get; init; }
        public Predicate<TValue>? Qualifier { get; init; }

        #endregion

        #region Constructors
        public PredicateRule(TEnum definition, Predicate<TValue> predicate)
        {
            Definition = definition;
            Predicate = predicate;
        }
        public PredicateRule(TEnum definition, Predicate<TValue> predicate, Predicate<TValue> qualifier)
        {
            Definition = definition;
            Predicate = predicate;
            Qualifier = qualifier;
        }
        #endregion

        public bool TryConsume(ITokenizer<TValue> Tokenizer, TEnum? Previous, out TEnum TokenType, out long TokenLength)
        {
            var rd = Tokenizer.GetReader();
            if (Qualifier is not null)
            {
                if (!rd.TryPeek(out var pk) || !Qualifier(pk))
                {
                    TokenType = default;
                    TokenLength = default;
                    return false;
                }
            }

            do
            {
                if (!rd.TryPeek(out var ch))
                    break;

                if (!Predicate(ch))
                    break;

                rd.Advance(1);
            }
            while (!rd.End);

            TokenType = Definition;
            TokenLength = rd.Consumed;
            return rd.Consumed > 0;
        }
    }
}
