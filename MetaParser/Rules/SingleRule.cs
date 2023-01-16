using System.Buffers;
using System.Diagnostics;

namespace MetaParser.Rules
{
    [DebuggerDisplay("SingleRule :: {Definition} :: {Value.ToString()}")]
    public class SingleRule<TEnum, TValue> : ITokenRule<TEnum, TValue> 
        where TEnum : unmanaged
        where TValue : unmanaged, IEquatable<TValue>
    {
        public RuleSpecificty Specificity => new RuleSpecificty(false, false, 1);

        #region Properties
        private TEnum Definition { get; init; }
        public TValue Value { get; init; }
        #endregion

        #region Constructors
        public SingleRule(TEnum definition, TValue value)
        {
            Definition = definition;
            Value = value;
        }
        #endregion

        public bool TryConsume(ITokenizer<TValue> Tokenizer, TEnum? Previous, out TEnum TokenType, out long TokenLength)
        {
            var rd = Tokenizer.GetReader();
            bool success = rd.IsNext(Value, true);
            TokenLength = rd.Consumed;
            TokenType = Definition;
            return success;
        }
    }
}
