using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MetaParser.DepsGraph
{
    [DebuggerDisplay(@"[In: {Incoming.Count}] [Out: {Outgoing.Count}]", Name = @"{Id}")]
    internal class Node : IEquatable<Node>, IComparable<Node>
    {
        #region Properties
        public readonly string Id;
        public WeakReference<ParentNode>? Parent { get; private set; }
        /// <summary> Nodes linking TO this one </summary>
        public readonly HashSet<Node> Incoming = new();
        /// <summary> Nodes linked to BY this one </summary>
        public readonly HashSet<Node> Outgoing = new();
        #endregion

        #region Constructors
        public Node(string id, ParentNode? parent = null)
        {
            Id = id;
            if (parent is not null)
            {
                Parent = new (parent);
                parent.Add(id, this);
            }
        }
        #endregion

        #region IComparable
        public int CompareTo(Node other)
        {
            if (Equals(other))
            {
                return 0;
            }

            return Outgoing.Contains(other) ? -1 : 1;
        }
        #endregion

        #region Links
        public bool Link(Node other)
        {
            bool success = Outgoing.Add(other);
            if (success)
            {
                if (Parent.TryGetTarget(out var parent))
                {
                    parent.Link(other);
                }

                other.Incoming.Add(this);
            }

            return success;
        }

        public bool Unlink(Node other)
        {
            bool success = Outgoing.Remove(other);
            if (success)
            {
                if (Parent.TryGetTarget(out var parent))
                {
                    parent.Unlink(other);
                }

                other.Incoming.Remove(this);
            }

            return success;
        }
        #endregion

        #region Equality
        public bool Equals(Node other)
        {
            return Id == other.Id;
        }
        #endregion

        #region Depth
        /// <summary>
        /// Returns the highest depth of links which this node is involved in
        /// </summary>
        /// <returns></returns>
        public int GetDepth()
        {
            if (Outgoing.Any())
            {
                return Outgoing.Max(n => n.GetDepth()) + 1;
            }

            return 0;
        }
        #endregion
    }
}
