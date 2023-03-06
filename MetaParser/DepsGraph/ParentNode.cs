using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace MetaParser.DepsGraph
{
    [DebuggerDisplay(@"[Children: {children.Count}] [In: {Incoming.Count}] [Out: {Outgoing.Count}]", Name = @"{Id}")]
    internal class ParentNode : Node, IDictionary<string, Node>
    {
        #region Fields
        private readonly Dictionary<string, Node> children = new();
        #endregion

        #region Properties
        /// <summary></summary>
        public IReadOnlyDictionary<string, Node> Children => children;
        #endregion

        #region Constructors
        public ParentNode(string id) : base(id)
        {
        }
        #endregion

        public Node this[string key] { get => ((IDictionary<string, Node>)Children)[key]; set => ((IDictionary<string, Node>)Children)[key] = value; }

        public ICollection<string> Keys => ((IDictionary<string, Node>)Children).Keys;

        public ICollection<Node> Values => ((IDictionary<string, Node>)Children).Values;

        public int Count => ((ICollection<KeyValuePair<string, Node>>)Children).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<string, Node>>)Children).IsReadOnly;

        public void Add(string key, Node value)
        {
            ((IDictionary<string, Node>)Children).Add(key, value);
        }

        public void Add(KeyValuePair<string, Node> item)
        {
            ((ICollection<KeyValuePair<string, Node>>)Children).Add(item);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<string, Node>>)Children).Clear();
        }

        public bool Contains(KeyValuePair<string, Node> item)
        {
            return ((ICollection<KeyValuePair<string, Node>>)Children).Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return ((IDictionary<string, Node>)Children).ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, Node>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, Node>>)Children).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, Node>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, Node>>)Children).GetEnumerator();
        }

        public bool Remove(string key)
        {
            return ((IDictionary<string, Node>)Children).Remove(key);
        }

        public bool Remove(KeyValuePair<string, Node> item)
        {
            return ((ICollection<KeyValuePair<string, Node>>)Children).Remove(item);
        }

        public bool TryGetValue(string key, out Node value)
        {
            return ((IDictionary<string, Node>)Children).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Children).GetEnumerator();
        }
    }
}
