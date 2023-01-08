﻿using MetaParser.Tokens;

using System.Buffers;

namespace MetaParser.Rules
{
    public class GreedyRule<T> : TokenRule<T> where T : unmanaged, IEquatable<T>
    {
        public T Value { get; init; }

        public GreedyRule(T value, TokenFactory<T>? tokenFactory = null) : base(tokenFactory)
        {
            Value = value;
        }

        protected override bool Consume(ITokenizer<T> Tokenizer, IToken<T> Previous, out ReadOnlySequence<T>? outConsumed)
        {
            var rd = Tokenizer.GetReader();
            bool success = rd.AdvancePast(Value) > 0;
            outConsumed = success ? Tokenizer.Consume(ref rd) : null;
            return success;
        }
    }
}
