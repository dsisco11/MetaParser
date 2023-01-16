using MetaParser.Tokens;

using System.Buffers;
using System.Diagnostics;

namespace MetaParser.Rules
{
    /// <summary>
    /// Consumes a specific value sequence producing a single token.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [DebuggerDisplay("SequenceRule :: {Definition} :: {Sequence.ToString()}")]
    public class SequenceRule<TEnum, TValue> : ITokenRule<TEnum, TValue> 
        where TEnum : unmanaged
        where TValue : unmanaged, IEquatable<TValue>
    {
        public RuleSpecificty Specificity => new RuleSpecificty(false, false, (short)Sequence.Length);

        #region Properties
        private TEnum Definition { get; init; }
        public TValue[] Sequence { get; init; }
        #endregion

        #region Constructors
        public SequenceRule(TEnum definition, TValue[] sequence)
        {
            Definition = definition;
            Sequence = sequence;
        }

        public SequenceRule(TEnum definition, ReadOnlySpan<TValue> sequence)
        {
            Definition = definition;
            Sequence = sequence.ToArray();
        }
        #endregion

        public bool TryConsume(ITokenizer<TValue> Tokenizer, TEnum? Previous, out TEnum TokenType, out long TokenLength)
        {
            var rd = Tokenizer.GetReader();
            bool success = rd.IsNext(Sequence.AsSpan(), true);

            TokenType = Definition;
            TokenLength = rd.Consumed;
            return success;
        }
    }
}
