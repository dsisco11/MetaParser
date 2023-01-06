﻿using System.Buffers;

namespace MetaParser
{
    public interface ITokenizer<T> : IReadOnlyTokenizer<T> where T : unmanaged, IEquatable<T>
    {
        ReadOnlySequence<T> Consume(SequenceReader<T> reader);
    }
}