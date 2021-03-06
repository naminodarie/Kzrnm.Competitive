﻿using AtCoder.Internal;
using System.Collections.Generic;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// BinaryTrie。整数値の集合を扱えるデータ構造 https://ei1333.github.io/library/structure/trie/binary-trie.cpp
    /// </summary>
    public class BinaryTrie
    {
        /// <summary>
        /// 1 &lt;&lt; <paramref name="maxDepth"/> 未満の数値を取り扱う BinaryTrie を作成する。
        /// </summary>
        public BinaryTrie(int maxDepth = 64) : this(new Node()) { }
        private BinaryTrie(Node root, int maxDepth = 64)
        {
            _root = root;
            MaxDepth = maxDepth - 1;
        }
        private Node _root;
        private readonly int MaxDepth;


        /// <summary>
        /// <para><paramref name="num"/> に <paramref name="delta"/> を追加する。</para>
        /// <para><paramref name="idx"/> に対して −1 以外を与えると accept にそのノードにマッチする全ての値のindexが格納される。</para>
        /// <para>すべての値に <paramref name="xorVal"/> と XOR を取った値で扱う。</para>
        /// </summary>
        public void Add(ulong num, int delta = 1, int idx = -1, ulong xorVal = 0)
            => _root = Add(_root, num, idx, MaxDepth, delta, xorVal);
        Node Add(Node t, ulong bit, int idx, int depth, int x, ulong xorVal)
        {
            if (depth == -1)
            {
                t.exist += x;
                if (idx >= 0) t.accept.Add(idx);
            }
            else
            {
                ref Node to = ref t.left;
                if ((((xorVal >> depth) ^ (bit >> depth)) & 1) != 0)
                    to = ref t.right;
                if (to == null)
                {
                    to = new Node();
                }
                to = Add(to, bit, idx, depth - 1, x, xorVal);
                t.exist += x;
            }
            return t;
        }

        /// <summary>
        /// <para><paramref name="num"/> をデクリメントする。</para>
        /// <para><paramref name="idx"/> に対して −1 以外を与えると accept にそのノードにマッチする全ての値のindexが格納される。</para>
        /// <para>すべての値に <paramref name="xorVal"/> と XOR を取った値で扱う。</para>
        /// </summary>
        public void Remove(ulong num, ulong xorVal = 0)
            => Add(num, -1, -1, xorVal);

        /// <summary>
        /// <para><paramref name="num"/> に対応するノードを返す。</para>
        /// <para>すべての値に <paramref name="xorVal"/> と XOR を取った値で扱う。</para>
        /// </summary>
        public Node Find(ulong num, ulong xorVal = 0)
            => Find(_root, num, MaxDepth, xorVal);
        Node Find(Node t, ulong bit, int depth, ulong xorVal)
        {
            if (depth == -1)
                return t;
            Node to;
            if ((((xorVal >> depth) ^ (bit >> depth)) & 1) != 0)
                to = t.right;
            else
                to = t.left;
            return to != null ? Find(to, bit, depth - 1, xorVal) : null;
        }

        /// <summary>
        /// <para><paramref name="num"/> に対応する個数を返す。</para>
        /// <para>すべての値に <paramref name="xorVal"/> と XOR を取った値で扱う。</para>
        /// </summary>
        public int Count(ulong num, ulong xorVal = 0) => Find(num, xorVal)?.exist ?? 0;

        /// <summary>
        /// <para>最小値と対応するノードを返す。</para>
        /// <para>すべての値に <paramref name="xorVal"/> と XOR を取った値で扱う。</para>
        /// </summary>
        public (ulong Num, Node Node) MinElement(ulong xorVal = 0)
        {
            Contract.Assert(_root.exist > 0);
            return KthElement(0, xorVal);
        }

        /// <summary>
        /// <para>最大値と対応するノードを返す。</para>
        /// <para>すべての値に <paramref name="xorVal"/> と XOR を取った値で扱う。</para>
        /// </summary>
        public (ulong Num, Node Node) MaxElement(ulong xorVal = 0)
        {
            Contract.Assert(_root.exist > 0);
            return KthElement(_root.exist - 1, xorVal);
        }

        /// <summary>
        /// <para><paramref name="k"/> 番目(0-indexed) に小さい値とそれに対応するノードを返す。</para>
        /// <para>すべての値に <paramref name="xorVal"/> と XOR を取った値で扱う。</para>
        /// </summary>
        public (ulong Num, Node Node) KthElement(int k, ulong xorVal = 0)
        {
            Contract.Assert(0 <= k && k < _root.exist);
            return KthElement(_root, k, MaxDepth, xorVal);
        }

        /// <summary>
        /// <para><paramref name="num"/> 未満の個数を返す。</para>
        /// <para>すべての値に <paramref name="xorVal"/> と XOR を取った値で扱う。</para>
        /// </summary>
        public int CountLess(ulong num, ulong xorVal = 0)
        {
            return CountLess(_root, num, MaxDepth, xorVal);
        }



        /// <summary>
        /// <para><paramref name="k"/> 番目(0-indexed) に小さい値とそれに対応するノードを返す。</para>
        /// <para>すべての値に <paramref name="xorVal"/> と XOR を取った値で扱う。</para>
        /// </summary>
        private (ulong Num, Node Node) KthElement(Node t, int k, int bitIndex, ulong xorVal)
        {
            if (bitIndex == -1)
                return (0, t);
            bool f = ((xorVal >> bitIndex) & 1) != 0;
            var tn = f ? t.right : t.left;
            var nb = tn?.exist ?? 0;
            if (nb <= k)
            {
                var (num, node) = KthElement((!f ? t.right : t.left), k - nb, bitIndex - 1, xorVal);
                num |= 1ul << bitIndex;
                return (num, node);
            }
            return KthElement(tn, k, bitIndex - 1, xorVal);
        }

        /// <summary>
        /// <para><paramref name="num"/> 未満の個数を返す。</para>
        /// <para>すべての値に <paramref name="xorVal"/> と XOR を取った値で扱う。</para>
        /// </summary>
        int CountLess(Node t, ulong num, int bitIndex, ulong xorVal)
        {
            if (bitIndex == -1) return 0;
            int ret = 0;
            bool f = ((xorVal >> bitIndex) & 1) != 0;
            var tn = f ? t.right : t.left;
            if ((num >> bitIndex & 1) != 0 && tn != null)
                ret += tn.exist;
            var nf = ((num >> bitIndex & 1) != 0) != f;
            var tf = nf ? t.right : t.left;
            if (tf != null)
                ret += CountLess(tf, num, bitIndex - 1, xorVal);
            return ret;
        }

        public class Node
        {
            public Node left, right;
            public int exist;
            public List<int> accept = new List<int>();
        }
    }
}
