using System.Diagnostics;

namespace MetaParser.Rules
{
    /// <summary>
    /// Consumes contiguous sequences of a single value
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [DebuggerDisplay("GroupSingleRule :: {Definition} :: {Value.ToString()}")]
    public class GroupSingleRule<TEnum, TValue> : ITokenRule<TEnum, TValue> 
        where TEnum : unmanaged
        where TValue : unmanaged, IEquatable<TValue>
    {
        public RuleSpecificty Specificity => new RuleSpecificty(false, true, 1);

        #region Properties
        private TEnum Definition { get; init; }
        public TValue Value { get; init; }
        #endregion

        #region Constructors
        public GroupSingleRule(TEnum definition, TValue value)
        {
            Definition = definition;
            Value = value;
        }
        #endregion

        public bool TryConsume(ITokenizer<TValue> Tokenizer, TEnum? Previous, out TEnum TokenType, out long TokenLength)
        {
            var rd = Tokenizer.GetReader();
            bool success = rd.AdvancePast(Value) > 0;

            TokenType = Definition;
            TokenLength = rd.Consumed;
            return success;
        }
    }
}
