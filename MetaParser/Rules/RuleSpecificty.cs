using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MetaParser.Rules
{
    [StructLayout(LayoutKind.Explicit)]
    [DebuggerDisplay("RuleSpecificity <{SpecificityValue}>")]
    public readonly struct RuleSpecificty
    {
        [FieldOffset(0)]
        public readonly int SpecificityValue;

        /// <summary>The length of the rules predefined sequence</summary>
        [FieldOffset(0)]
        public readonly short SequenceLength;

        /// <summary>TRUE if the rule consumes contiguous matching values as a single token</summary>
        [FieldOffset(2)]
        public readonly bool IsGreedy;

        /// <summary>TRUE if the rule involves complex logic beyond just consuming am predefined set of values.</summary>
        [FieldOffset(3)]
        public readonly bool IsComplex;

        public RuleSpecificty(bool isComplex, bool isGreedy, short sequenceLength = short.MaxValue) : this()
        {
            IsComplex = isComplex;
            IsGreedy = isGreedy;
            SequenceLength = sequenceLength;
        }
    }
}
