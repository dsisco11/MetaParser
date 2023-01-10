using System.Buffers;

namespace MetaParser.Tokens
{
    public struct TokenStruct<TData, TValue>
        where TData : struct, IEquatable<TData>
        where TValue : unmanaged, IEquatable<TValue>
    {
        #region Fields
        public readonly TData data;
        public readonly ReadOnlySequence<TValue> value;
        #endregion
    }


    public record UntypedToken<TData>(ReadOnlySequence<TData> data)
        where TData : unmanaged, IEquatable<TData>
    { }
}
