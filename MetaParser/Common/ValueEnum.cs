namespace MetaParser.Common
{
    public record struct ValueEnum<T> (T Value): IEquatable<T>
        where T : struct, Enum
    {
        public bool Equals(T other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other);
        }

        public bool Equals(ValueEnum<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }
    }
}
