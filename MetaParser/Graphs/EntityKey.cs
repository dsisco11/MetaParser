using System;

namespace MetaParser.Graphs;

internal sealed record EntityKey : IComparable<EntityKey>
{
    #region Statics
    public static readonly EntityKey Default = new(NodeType.None, 0);
    #endregion

    #region Fields
    public readonly NodeType Type;
    public readonly int Index;
    #endregion

    #region Constructors
    public EntityKey(NodeType type, int index)
    {
        Type = type;
        Index = index;
    }
    #endregion

    public int CompareTo(EntityKey other)
    {
        if (Type != other.Type)
        {
            return Type.CompareTo(other.Type);
        }

        return Index.CompareTo(other.Index);
    }

    public static bool operator >(EntityKey left, EntityKey right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(EntityKey left, EntityKey right)
    {
        return left.CompareTo(right) >= 0;
    }

    public static bool operator <(EntityKey left, EntityKey right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(EntityKey left, EntityKey right)
    {
        return left.CompareTo(right) <= 0;
    }

    public override string ToString()
    {
        return $"{Type}_{Index}";
    }
}
