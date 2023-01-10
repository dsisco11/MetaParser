using MetaParser.Tokens;

using System.Buffers;
using System.Diagnostics;

namespace MetaParser.Rules
{
    [DebuggerDisplay("BlockRule :: {Definition} :: [{BlockStart.ToString()}] -> [{BlockEnd.ToString()}]")]
    public class BlockRule<TData, TValue> : TokenRule<TData, TValue> 
        where TData : struct, IEquatable<TData>
        where TValue : unmanaged, IEquatable<TValue>
    {
        public override RuleSpecificty Specificity => new RuleSpecificty(false, true);

        #region Properties
        public TValue[] BlockStart { get; init; }
        public TValue[] BlockEnd { get; init; }
        #endregion

        #region Constructors
        public BlockRule(TData definition, ReadOnlySpan<TValue> blockStart, ReadOnlySpan<TValue> blockEnd) : base(definition)
        {
            BlockStart = blockStart.ToArray();
            BlockEnd = blockEnd.ToArray();
        }
        #endregion

        protected override bool Consume(ITokenizer<TValue> Tokenizer, Token<TData, TValue>? Previous, out ReadOnlySequence<TValue> Consumed)
        {
            var rd = Tokenizer.GetReader();
            if (!rd.IsNext(BlockStart.AsSpan(), advancePast: true))
            {
                Consumed = ReadOnlySequence<TValue>.Empty;
                return false;
            }

            // Skip forward until we find the block end sequence or we reach the stream end
            if (!rd.TryReadTo(out ReadOnlySpan<TValue> _, BlockEnd.AsSpan(), advancePastDelimiter: true))
            {// Consume everything thats left if we couldnt find it
                rd.AdvanceToEnd();
            }

            Consumed = Tokenizer.Consume(ref rd);
            return true;
        }
    }
}
