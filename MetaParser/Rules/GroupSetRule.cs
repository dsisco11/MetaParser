using MetaParser.Tokens;

using System.Buffers;
using System.Diagnostics;

namespace MetaParser.Rules
{
    /// <summary>
    /// Consumes contiguous sequences of a set of values.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [DebuggerDisplay("GroupSetRule :: {Definition} :: {Values.ToString()}")]
    public class GroupSetRule<TData, TValue> : TokenRule<TData, TValue> 
        where TData : struct, IEquatable<TData>
        where TValue : unmanaged, IEquatable<TValue>
    {
        public override RuleSpecificty Specificity => new RuleSpecificty(false, true);

        #region Properties
        public TValue[] Values { get; init; }
        #endregion

        #region Constructors
        public GroupSetRule(TData definition, TValue[] values) : base(definition)
        {
            Values = values;
        }

        public GroupSetRule(TData definition, ReadOnlySpan<TValue> values) : base(definition)
        {
            Values = values.ToArray();
        }

        public GroupSetRule(Token<TData, TValue> instance) : base(instance.Data)
        {
            Values = instance.Value;
        }
        #endregion

        protected override bool Consume(ITokenizer<TValue> Tokenizer, Token<TData, TValue>? Previous, out ReadOnlySequence<TValue> Consumed)
        {
            var rd = Tokenizer.GetReader();
            bool success = rd.AdvancePastAny(Values.AsSpan()) > 0;
            Consumed = Tokenizer.Consume(ref rd);
            return success;
        }
    }
}
