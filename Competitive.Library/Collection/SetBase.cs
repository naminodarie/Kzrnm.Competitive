﻿using AtCoder.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive
{
    using static MethodImplOptions;
    using static SetNodeBase;

    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public abstract class SetBase<T, TCmp, Node, TOp> : ICollection, IReadOnlyCollection<T>
        where Node : SetNodeBase
        where TOp : struct, INodeOperator<T, Node>
    {
        private readonly TOp op;
        public bool IsMulti { get; }
        protected Node root;

        #region Constructor
        protected SetBase(bool isMulti, TOp op)
        {
            this.IsMulti = isMulti;
            this.op = op;
        }
        protected SetBase(bool isMulti, TOp op, IEnumerable<T> collection) : this(isMulti, op)
        {
            var (arr, count) = InitArray(collection);
            this.root = ConstructRootFromSortedArray(arr, 0, count - 1, null);
        }
        protected virtual (T[] array, int arrayCount) InitArray(IEnumerable<T> collection)
        {
            var comparer = Comparer<T>.Create((a, b) => op.Compare(a, b));
            T[] arr;
            int count;
            if (IsMulti)
            {
                arr = collection.ToArray();
                Array.Sort(arr, comparer);
                count = arr.Length;
            }
            else
            {
                arr = collection.ToArray();
                if (arr.Length == 0) return (arr, 0);
                count = 1;
                Array.Sort(arr, comparer);
                for (int i = 1; i < arr.Length; i++)
                {
                    if (comparer.Compare(arr[i], arr[i - 1]) != 0)
                    {
                        arr[count++] = arr[i];
                    }
                }
            }
            return (arr, count);
        }
        protected virtual Node ConstructRootFromSortedArray(T[] arr, int startIndex, int endIndex, Node redNode)
        {
            int size = endIndex - startIndex + 1;
            Node root;

            switch (size)
            {
                case 0:
                    return null;
                case 1:
                    root = op.Create(arr[startIndex], NodeColor.Black);
                    if (redNode != null)
                    {
                        root.Left = redNode;
                    }
                    break;
                case 2:
                    root = op.Create(arr[startIndex], NodeColor.Black);
                    root.Right = op.Create(arr[endIndex], NodeColor.Red);
                    if (redNode != null)
                    {
                        root.Left = redNode;
                    }
                    break;
                case 3:
                    root = op.Create(arr[startIndex + 1], NodeColor.Black);
                    root.Left = op.Create(arr[startIndex], NodeColor.Black);
                    root.Right = op.Create(arr[endIndex], NodeColor.Black);
                    if (redNode != null)
                    {
                        root.Left.Left = redNode;
                    }
                    break;
                default:
                    int midpt = ((startIndex + endIndex) / 2);
                    root = op.Create(arr[midpt], NodeColor.Black);
                    root.Left = ConstructRootFromSortedArray(arr, startIndex, midpt - 1, redNode);
                    root.Right = size % 2 == 0 ?
                        ConstructRootFromSortedArray(arr, midpt + 2, endIndex, op.Create(arr[midpt + 1], NodeColor.Red)) :
                        ConstructRootFromSortedArray(arr, midpt + 1, endIndex, null);
                    break;
            }
            return root;
        }
        #endregion Constructor

        internal Node MinNode()
        {
            if (root == null) return null;
            SetNodeBase cur = root;
            while (cur.Left != null) { cur = cur.Left; }
            return (Node)cur;
        }
        internal Node MaxNode()
        {
            if (root == null) return null;
            SetNodeBase cur = root;
            while (cur.Right != null) { cur = cur.Right; }
            return (Node)cur;
        }
        public T Min => MinNode() switch { { } n => op.GetValue(n), _ => default(T) };
        public T Max => MaxNode() switch { { } n => op.GetValue(n), _ => default(T) };

        #region Index
        public int Index(Node node)
        {
            SetNodeBase _node = node;
            var ret = NodeSize(_node.Left);
            SetNodeBase prev = _node;
            _node = _node.Parent;
            while (prev != root)
            {
                if (_node.Left != prev)
                {
                    ret += NodeSize(_node.Left) + 1;
                }
                prev = _node;
                _node = _node.Parent;
            }
            return ret;
        }

        public Node FindByIndex(int index)
        {
            SetNodeBase current = root;
            var currentIndex = current.Size - NodeSize(current.Right) - 1;
            while (currentIndex != index)
            {
                if (currentIndex > index)
                {
                    current = current.Left;
                    if (current == null) break;
                    currentIndex -= NodeSize(current.Right) + 1;
                }
                else
                {
                    current = current.Right;
                    if (current == null) break;
                    currentIndex += NodeSize(current.Left) + 1;
                }
            }
            return (Node)current;
        }
        #endregion Index

        #region Enumerate
        public IEnumerable<T> Reversed()
        {
            var e = new ValueEnumerator(this, true, null);
            while (e.MoveNext()) yield return e.Current;
        }

        /// <summary>
        /// <paramref name="from"/> 以上の要素を列挙する。<paramref name="from"/> がnullならばすべて列挙する。
        /// </summary>
        /// <param name="reverse">以上ではなく以下を列挙する</param>
        /// <returns></returns>
        public IEnumerable<T> EnumerateItem(Node from = null, bool reverse = false)
        {
            var e = new ValueEnumerator(this, reverse, from);
            while (e.MoveNext()) yield return e.Current;
        }
        /// <summary>
        /// <paramref name="from"/> 以上のノードを列挙する。<paramref name="from"/> がnullならばすべて列挙する。
        /// </summary>
        /// <param name="reverse">以上ではなく以下を列挙する</param>
        /// <returns></returns>
        public IEnumerable<Node> EnumerateNode(Node from = null, bool reverse = false)
        {
            var e = new Enumerator(this, reverse, from);
            while (e.MoveNext()) yield return e.Current;
        }
        #endregion Enumerate

        #region ICollection
        public void Clear()
        {
            root = null;
        }
        void ICollection.CopyTo(Array array, int index) => CopyTo((T[])array, index);
        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var item in this) array[arrayIndex++] = item;
        }
        #endregion ICollection


        /// <summary>
        /// 該当ノードを削除する。動作怪しいかも
        /// </summary>
        public void Remove(Node node)
            => RemoveMatch(
                match: node,
                parentOfMatch: (Node)node.Parent,
                current: node,
                parent: (Node)node.Parent,
                grandParent: (Node)node.Parent?.Parent);

        private void RemoveMatch(Node match, Node parentOfMatch, Node current, Node parent, Node grandParent)
        {
            while (current != null)
            {
                if (current.Is2Node)
                {
                    if (parent == null)
                    {
                        current.ColorRed();
                    }
                    else
                    {
                        Node sibling = (Node)parent.GetSibling(current);
                        if (sibling.IsRed)
                        {
                            Debug.Assert(parent.IsBlack);
                            if (parent.Right == sibling) parent.RotateLeft();
                            else parent.RotateRight();

                            parent.ColorRed();
                            sibling.ColorBlack();
                            ReplaceChildOrRoot(grandParent, parent, sibling);
                            grandParent = sibling;
                            if (parent == match) parentOfMatch = sibling;
                            sibling = (Node)parent.GetSibling(current);
                        }
                        Debug.Assert(IsNonNullBlack(sibling));
                        if (sibling.Is2Node)
                        {
                            parent.Merge2Nodes();
                        }
                        else
                        {
                            Node newGrandParent = (Node)parent.Rotate(parent.GetRotation(current, sibling));
                            newGrandParent.Color = parent.Color;
                            parent.ColorBlack();
                            current.ColorRed();
                            ReplaceChildOrRoot(grandParent, parent, newGrandParent);
                            if (parent == match)
                            {
                                parentOfMatch = newGrandParent;
                            }
                        }
                    }
                }
                grandParent = parent;
                parent = current;
                current = (Node)(current != match ? current.Left : current.Right);
            }
            if (match != null)
            {
                ReplaceNode(match, parentOfMatch, parent, grandParent);
            }
            root?.ColorBlack();
        }

        #region private
        protected void InsertionBalance(Node current, ref Node parent, Node grandParent, Node greatGrandParent)
        {
            Debug.Assert(parent != null);
            Debug.Assert(grandParent != null);
            bool parentIsOnRight = grandParent.Right == parent;
            bool currentIsOnRight = parent.Right == current;
            SetNodeBase newChildOfGreatGrandParent;
            if (parentIsOnRight == currentIsOnRight)
            {
                newChildOfGreatGrandParent = currentIsOnRight ? grandParent.RotateLeft() : grandParent.RotateRight();
            }
            else
            {
                newChildOfGreatGrandParent = currentIsOnRight ? grandParent.RotateLeftRight() : grandParent.RotateRightLeft();
                parent = greatGrandParent;
            }
            grandParent.ColorRed();
            newChildOfGreatGrandParent.ColorBlack();
            ReplaceChildOrRoot(greatGrandParent, grandParent, (Node)newChildOfGreatGrandParent);

        }
        protected void ReplaceChildOrRoot(Node parent, Node child, Node newChild)
        {
            if (parent != null)
                parent.ReplaceChild(child, newChild);
            else
                root = newChild;
        }
        protected void ReplaceNode(Node match, Node parentOfMatch, Node successor, Node parentOfSuccessor)
        {
            Debug.Assert(match != null);
            if (successor == match)
            {
                Debug.Assert(match.Right == null);
                successor = (Node)match.Left;
            }
            else
            {
                Debug.Assert(parentOfSuccessor != null);
                Debug.Assert(successor.Left == null);
                Debug.Assert((successor.Right == null && successor.IsRed) || (successor.Right.IsRed && successor.IsBlack));

                successor.Right?.ColorBlack();

                if (parentOfSuccessor != match)
                {
                    parentOfSuccessor.Left = successor.Right;
                    successor.Right = match.Right;
                }
                successor.Left = match.Left;
            }
            if (successor != null)
            {
                successor.Color = match.Color;
            }
            ReplaceChildOrRoot(parentOfMatch, match, successor);
        }
        #endregion private
        bool ICollection.IsSynchronized => false;
        object ICollection.SyncRoot => this;
        public int Count => NodeSize(root);

        [MethodImpl(AggressiveInlining)]
        internal static int NodeSize(SetNodeBase node) => node == null ? 0 : node.Size;


        public ValueEnumerator GetEnumerator() => new ValueEnumerator(this);
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => new ValueEnumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new ValueEnumerator(this);


        public struct Enumerator : IEnumerator<Node>
        {
            internal readonly SetBase<T, TCmp, Node, TOp> tree;
            readonly Stack<SetNodeBase> stack;
            Node current;

            readonly bool reverse;
            internal Enumerator(SetBase<T, TCmp, Node, TOp> set) : this(set, false, null) { }
            internal Enumerator(SetBase<T, TCmp, Node, TOp> set, bool reverse, Node startNode)
            {
                tree = set;
                stack = new Stack<SetNodeBase>(2 * Log2(tree.Count + 1));
                current = null;
                this.reverse = reverse;
                if (startNode == null) IntializeAll();
                else Intialize(startNode);

            }
            void IntializeAll()
            {
                SetNodeBase node = tree.root;
                while (node != null)
                {
                    var next = reverse ? node.Right : node.Left;
                    stack.Push(node);
                    node = next;
                }
            }
            void Intialize(Node startNode)
            {
                if (startNode == null)
                    throw new InvalidOperationException(nameof(startNode) + "is null");
                current = null;
                var list = reverse ? InitializeReverse(startNode) : InitializeNormal(startNode);

                list.Reverse();
                foreach (var n in list) stack.Push(n);
            }
            SimpleList<SetNodeBase> InitializeNormal(SetNodeBase node)
            {
                var list = new SimpleList<SetNodeBase>(2 * Log2(tree.Count + 1));

                while (node != null)
                {
                    while (node != null)
                    {
                        list.Add(node);
                        var parent = node.Parent;
                        if (parent == null || parent.Right == node) { node = parent; break; }
                        node = parent;
                    }
                    while (node != null)
                    {
                        var parent = node.Parent;
                        if (parent == null || parent.Left == node) { node = parent; break; }
                        node = parent;
                    }
                }
                return list;
            }
            SimpleList<SetNodeBase> InitializeReverse(SetNodeBase node)
            {
                var list = new SimpleList<SetNodeBase>(2 * Log2(tree.Count + 1));

                while (node != null)
                {
                    while (node != null)
                    {
                        list.Add(node);
                        var parent = node.Parent;
                        if (parent == null || parent.Left == node) { node = parent; break; }
                        node = parent;
                    }
                    while (node != null)
                    {
                        var parent = node.Parent;
                        if (parent == null || parent.Right == node) { node = parent; break; }
                        node = parent;
                    }
                }
                return list;
            }

            [MethodImpl(AggressiveInlining)]
            static int Log2(int num) => BitOperations.Log2((uint)num) + 1;
            public Node Current => current;
            [MethodImpl(AggressiveInlining)]
            internal T CurrentValue() => tree.op.GetValue(current);

            public bool MoveNext()
            {
                if (stack.Count == 0)
                {
                    current = null;
                    return false;
                }
                current = (Node)stack.Pop();
                var node = reverse ? current.Left : current.Right;
                while (node != null)
                {
                    var next = reverse ? node.Right : node.Left;
                    stack.Push(node);
                    node = next;
                }
                return true;
            }

            object IEnumerator.Current => Current;
            public void Dispose() { }
            public void Reset() => throw new NotSupportedException();
        }
        public struct ValueEnumerator : IEnumerator<T>
        {
            private Enumerator inner;
            internal ValueEnumerator(SetBase<T, TCmp, Node, TOp> set)
            {
                inner = new Enumerator(set);
            }
            internal ValueEnumerator(SetBase<T, TCmp, Node, TOp> set, bool reverse, Node startNode)
            {
                inner = new Enumerator(set, reverse, startNode);
            }

            public T Current => inner.CurrentValue();
            object IEnumerator.Current => Current;

            public void Dispose() { }
            public bool MoveNext() => inner.MoveNext();
            public void Reset() => throw new NotSupportedException();
        }
    }
    public interface INodeOperator<T, Node> : IComparer<T>
    {
        Node Create(T item, NodeColor color);
        T GetValue(Node node);
    }
    public class SetNodeBase
    {
        internal SetNodeBase(NodeColor color)
        {
            this.Color = color;
        }
        public SetNodeBase Parent { get; private set; }
        SetNodeBase _left;
        public SetNodeBase Left
        {
            get
            {
                return _left;
            }
            set
            {
                _left = value; if (value != null) value.Parent = this;
                for (var cur = this; cur != null; cur = cur.Parent)
                {
                    if (!cur.UpdateSize()) break;
                    if (cur.Parent != null && cur.Parent.Left != cur && cur.Parent.Right != cur)
                    {
                        cur.Parent = null;
                        break;
                    }
                }
            }
        }
        SetNodeBase _right;
        public SetNodeBase Right
        {
            get
            {
                return _right;
            }
            set
            {
                _right = value;
                if (value != null) value.Parent = this;
                for (var cur = this; cur != null; cur = cur.Parent)
                {
                    if (!cur.UpdateSize()) break;
                    if (cur.Parent != null && cur.Parent.Left != cur && cur.Parent.Right != cur)
                    {
                        cur.Parent = null; break;
                    }
                }
            }
        }

        public int Size
        {
            get; private set;
        } = 1;
        internal bool UpdateSize()
        {
            var oldsize = this.Size;
            var size = 1;
            if (Left != null) size += Left.Size;
            if (Right != null) size += Right.Size;
            this.Size = size;
            return oldsize != size;
        }
        [MethodImpl(AggressiveInlining)]
        internal static bool IsNonNullBlack(SetNodeBase node) => node != null && node.IsBlack;

        [MethodImpl(AggressiveInlining)]
        internal static bool IsNonNullRed(SetNodeBase node) => node != null && node.IsRed;

        [MethodImpl(AggressiveInlining)]
        internal static bool IsNullOrBlack(SetNodeBase node) => node == null || node.IsBlack;
        internal NodeColor Color { get; set; }
        internal bool IsBlack => Color == NodeColor.Black;
        internal bool IsRed => Color == NodeColor.Red;
        internal bool Is2Node => IsBlack && IsNullOrBlack(Left) && IsNullOrBlack(Right);
        internal bool Is4Node => IsNonNullRed(Left) && IsNonNullRed(Right);
        [MethodImpl(AggressiveInlining)]
        internal void ColorBlack() => Color = NodeColor.Black;
        [MethodImpl(AggressiveInlining)]
        internal void ColorRed() => Color = NodeColor.Red;

        [MethodImpl(AggressiveInlining)]
        internal TreeRotation GetRotation(SetNodeBase current, SetNodeBase sibling)
        {
            Debug.Assert(IsNonNullRed(sibling.Left) || IsNonNullRed(sibling.Right));
            bool currentIsLeftChild = Left == current;
            return IsNonNullRed(sibling.Left) ?
                (currentIsLeftChild ? TreeRotation.RightLeft : TreeRotation.Right) :
                (currentIsLeftChild ? TreeRotation.Left : TreeRotation.LeftRight);
        }

        [MethodImpl(AggressiveInlining)]
        internal SetNodeBase GetSibling(SetNodeBase node)
        {
            Debug.Assert(node != null);
            Debug.Assert(node == Left ^ node == Right);

            return node == Left ? Right : Left;
        }
        [MethodImpl(AggressiveInlining)]
        internal void Split4Node()
        {
            Debug.Assert(Left != null);
            Debug.Assert(Right != null);

            ColorRed();
            Left.ColorBlack();
            Right.ColorBlack();
        }
        static void ThrowInvalidOperationException() => throw new InvalidOperationException();
        [MethodImpl(AggressiveInlining)]
        internal SetNodeBase Rotate(TreeRotation rotation)
        {
            SetNodeBase removeRed;
            switch (rotation)
            {
                case TreeRotation.Right:
                    removeRed = Left.Left;
                    Debug.Assert(removeRed.IsRed);
                    removeRed.ColorBlack();
                    return RotateRight();
                case TreeRotation.Left:
                    removeRed = Right.Right;
                    Debug.Assert(removeRed.IsRed);
                    removeRed.ColorBlack();
                    return RotateLeft();
                case TreeRotation.RightLeft:
                    Debug.Assert(Right.Left.IsRed);
                    return RotateRightLeft();
                case TreeRotation.LeftRight:
                    Debug.Assert(Left.Right.IsRed);
                    return RotateLeftRight();
                default:
                    ThrowInvalidOperationException();
                    return this;
            }
        }
        [MethodImpl(AggressiveInlining)]
        internal SetNodeBase RotateLeft()
        {
            SetNodeBase child = Right;
            Right = child.Left;
            child.Left = this;
            return child;
        }
        [MethodImpl(AggressiveInlining)]
        internal SetNodeBase RotateLeftRight()
        {
            SetNodeBase child = Left;
            SetNodeBase grandChild = child.Right;

            Left = grandChild.Right;
            grandChild.Right = this;
            child.Right = grandChild.Left;
            grandChild.Left = child;
            return grandChild;
        }
        [MethodImpl(AggressiveInlining)]
        internal SetNodeBase RotateRight()
        {
            SetNodeBase child = Left;
            Left = child.Right;
            child.Right = this;
            return child;
        }
        [MethodImpl(AggressiveInlining)]
        internal SetNodeBase RotateRightLeft()
        {
            SetNodeBase child = Right;
            SetNodeBase grandChild = child.Left;

            Right = grandChild.Left;
            grandChild.Left = this;
            child.Left = grandChild.Right;
            grandChild.Right = child;
            return grandChild;
        }
        [MethodImpl(AggressiveInlining)]
        internal void Merge2Nodes()
        {
            Debug.Assert(IsRed);
            Debug.Assert(Left!.Is2Node);
            Debug.Assert(Right!.Is2Node);

            // Combine two 2-nodes into a 4-node.
            ColorBlack();
            Left.ColorRed();
            Right.ColorRed();
        }
        [MethodImpl(AggressiveInlining)]
        internal void ReplaceChild(SetNodeBase child, SetNodeBase newChild)
        {
            if (Left == child)
            {
                Left = newChild;
            }
            else
            {
                Right = newChild;
            }
        }
        public override string ToString() => $"Size = {Size}";
    }
    public enum NodeColor : byte
    {
        Black,
        Red
    }
    enum TreeRotation : byte
    {
        Left = 1,
        Right = 2,
        RightLeft = 3,
        LeftRight = 4,
    }
}
