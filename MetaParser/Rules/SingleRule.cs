﻿using MetaParser.Tokens;

using System.Buffers;
using System.Diagnostics;

namespace MetaParser.Rules
{
    [DebuggerDisplay("SingleRule :: {Definition} :: {Value.ToString()}")]
    public class SingleRule<TData, TValue> : TokenRule<TData, TValue> 
        where TData : struct, IEquatable<TData>
        where TValue : unmanaged, IEquatable<TValue>
    {
        public override RuleSpecificty Specificity => new RuleSpecificty(false, false, 1);

        #region Properties
        public TValue Value { get; init; }

        #endregion

        #region Constructors
        public SingleRule(TData definition, TValue value) : base(definition)
        {
            Value = value;
        }

        public SingleRule(Token<TData, TValue> instance) : base(instance)
        {
            Value = instance.Value.Single();
        }
        #endregion

        protected override bool Consume(ITokenizer<TValue> Tokenizer, Token<TData, TValue>? Previous, out ReadOnlySequence<TValue> Consumed)
        {
            var rd = Tokenizer.GetReader();
            bool success = rd.IsNext(Value, true);
            Consumed = Tokenizer.Consume(ref rd);
            return success;
        }
    }
}
