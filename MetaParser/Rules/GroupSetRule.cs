using MetaParser.Tokens;

using System.Buffers;
using System.Diagnostics;

namespace MetaParser.Rules
{
    /// <summary>
    /// Consumes contiguous sequences of a set of values.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [DebuggerDisplay("GroupSetRule :: {Definition} :: {Values.ToString()}")]
    public class GroupSetRule<TEnum, TValue> : ITokenRule<TEnum, TValue> 
        where TEnum : unmanaged
        where TValue : unmanaged, IEquatable<TValue>
    {
        public RuleSpecificty Specificity => new RuleSpecificty(false, true);

        #region Properties
        private TEnum definition;
        public TValue[] Values { get; init; }
        #endregion

        #region Constructors
        public GroupSetRule(TEnum definition, TValue[] values)
        {
            this.definition = definition;
            Values = values;
        }

        public GroupSetRule(TEnum definition, ReadOnlySpan<TValue> values)
        {
            this.definition = definition;
            Values = values.ToArray();
        }
        #endregion

        public bool TryConsume(ITokenizer<TValue> Tokenizer, TEnum? Previous, out TEnum TokenType, out long TokenLength)
        {
            var rd = Tokenizer.GetReader();
            bool success = rd.AdvancePastAny(Values.AsSpan()) > 0;

            TokenType = definition;
            TokenLength = rd.Consumed;
            return success;
        }
    }
}
