using System;
using System.Collections;
using System.Collections.Generic;

namespace MetaParser.DependencyGraph;

// seperate tokens that form circular references
// othert tokens should be put into sorted list 
//  - Sort tokens by ensuring that all the tokens which are referenced by a given token come after it in the list. Then reverse the list. This should guarantee that tokens are sorted according to reference depth.
internal class DependencyNode<T> : ISet<DependencyNode<T>>, IComparable<DependencyNode<T>>
    where T : IEquatable<T>
{
    #region Fields
    protected readonly HashSet<DependencyNode<T>> items = new();
    protected readonly HashSet<DependencyNode<T>> refs = new();
    #endregion

    #region Properties
    public T Value { get; private set; }
    public string Name { get; private set; }
    public IReadOnlyCollection<DependencyNode<T>> Referencers => refs;
    public IReadOnlyCollection<DependencyNode<T>> Dependencies => items;

    #endregion

    #region Accessors
    public int Count => ((ICollection<DependencyNode<T>>)items).Count;

    public bool IsReadOnly => ((ICollection<DependencyNode<T>>)items).IsReadOnly;
    #endregion

    public DependencyNode(string name, T value)
    {
        Name = name;
        Value = value;
    }

    #region IComparable
    public int CompareTo(DependencyNode<T> other)
    {
        if (Value.Equals(other.Value))
        {
            return 0;
        }

        return items.Contains(other) ? -1 : 1;
    }
    #endregion

    #region ISet
    public bool Add(DependencyNode<T> item)
    {
        var success = ((ISet<DependencyNode<T>>)items).Add(item);
        if (success)
        {
            item.refs.Add(this);
        }
        return success;
    }

    void ICollection<DependencyNode<T>>.Add(DependencyNode<T> item)
    {
        Add(item);
    }

    public void ExceptWith(IEnumerable<DependencyNode<T>> other)
    {
        ((ISet<DependencyNode<T>>)items).ExceptWith(other);
    }

    public void IntersectWith(IEnumerable<DependencyNode<T>> other)
    {
        ((ISet<DependencyNode<T>>)items).IntersectWith(other);
    }

    public bool IsProperSubsetOf(IEnumerable<DependencyNode<T>> other)
    {
        return ((ISet<DependencyNode<T>>)items).IsProperSubsetOf(other);
    }

    public bool IsProperSupersetOf(IEnumerable<DependencyNode<T>> other)
    {
        return ((ISet<DependencyNode<T>>)items).IsProperSupersetOf(other);
    }

    public bool IsSubsetOf(IEnumerable<DependencyNode<T>> other)
    {
        return ((ISet<DependencyNode<T>>)items).IsSubsetOf(other);
    }

    public bool IsSupersetOf(IEnumerable<DependencyNode<T>> other)
    {
        return ((ISet<DependencyNode<T>>)items).IsSupersetOf(other);
    }

    public bool Overlaps(IEnumerable<DependencyNode<T>> other)
    {
        return ((ISet<DependencyNode<T>>)items).Overlaps(other);
    }

    public bool SetEquals(IEnumerable<DependencyNode<T>> other)
    {
        return ((ISet<DependencyNode<T>>)items).SetEquals(other);
    }

    public void SymmetricExceptWith(IEnumerable<DependencyNode<T>> other)
    {
        ((ISet<DependencyNode<T>>)items).SymmetricExceptWith(other);
    }

    public void UnionWith(IEnumerable<DependencyNode<T>> other)
    {
        ((ISet<DependencyNode<T>>)items).UnionWith(other);
    }

    public void Clear()
    {
        foreach (var item in items)
        {
            item.refs.Remove(this);
        }

        items.Clear();
    }

    public bool Contains(DependencyNode<T> item)
    {
        return items.Contains(item);
    }

    public void CopyTo(DependencyNode<T>[] array, int arrayIndex)
    {
        ((ICollection<DependencyNode<T>>)items).CopyTo(array, arrayIndex);
    }

    public bool Remove(DependencyNode<T> item)
    {
        var success = items.Remove(item);
        if (success)
        {
            item.refs.Remove(this);
        }
        return success;
    }

    public IEnumerator<DependencyNode<T>> GetEnumerator()
    {
        return ((IEnumerable<DependencyNode<T>>)items).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)items).GetEnumerator();
    }
    #endregion
}



