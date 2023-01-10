using MetaParser.Tokens;

using System.Buffers;
using System.Diagnostics;

namespace MetaParser.Rules
{
    /// <summary>
    /// Consumes contiguous sequences of a single value
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [DebuggerDisplay("GroupSingleRule :: {Definition} :: {Value.ToString()}")]
    public class GroupSingleRule<TData, TValue> : TokenRule<TData, TValue> 
        where TData : struct, IEquatable<TData>
        where TValue : unmanaged, IEquatable<TValue>
    {
        public override RuleSpecificty Specificity => new RuleSpecificty(false, true, 1);

        #region Properties
        public TValue Value { get; init; }
        #endregion

        #region Constructors
        public GroupSingleRule(TData definition, TValue value) : base(definition)
        {
            Value = value;
        }

        public GroupSingleRule(Token<TData, TValue> instance) : base(instance.Data)
        {
            Value = instance.Value.Single();
        }
        #endregion

        protected override bool Consume(ITokenizer<TValue> Tokenizer, Token<TData, TValue>? Previous, out ReadOnlySequence<TValue> Consumed)
        {
            var rd = Tokenizer.GetReader();
            bool success = rd.AdvancePast(Value) > 0;
            Consumed = Tokenizer.Consume(ref rd);
            return success;
        }
    }
}
