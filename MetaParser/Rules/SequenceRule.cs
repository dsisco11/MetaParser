using MetaParser.Tokens;

using System.Buffers;
using System.Diagnostics;

namespace MetaParser.Rules
{
    /// <summary>
    /// Consumes a specific value sequence producing a single token.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [DebuggerDisplay("SequenceRule :: {Definition} :: {Sequence.ToString()}")]
    public class SequenceRule<TData, TValue> : TokenRule<TData, TValue> 
        where TData : struct, IEquatable<TData>
        where TValue : unmanaged, IEquatable<TValue>
    {
        public override RuleSpecificty Specificity => new RuleSpecificty(false, false, (short)Sequence.Length);

        #region Properties
        public TValue[] Sequence { get; init; }
        #endregion

        #region Constructors
        public SequenceRule(TData definition, TValue[] sequence) : base(definition)
        {
            Sequence = sequence;
        }

        public SequenceRule(TData definition, ReadOnlySpan<TValue> sequence) : base(definition)
        {
            Sequence = sequence.ToArray();
        }

        public SequenceRule(Token<TData, TValue> instance) : base(instance.Data)
        {
            Sequence = instance.Value;
        }
        #endregion

        protected override bool Consume(ITokenizer<TValue> Tokenizer, Token<TData, TValue>? Previous, out ReadOnlySequence<TValue> Consumed)
        {
            var rd = Tokenizer.GetReader();
            bool success = rd.IsNext(Sequence.AsSpan(), true);
            Consumed = Tokenizer.Consume(ref rd);
            return success;
        }
    }
}
