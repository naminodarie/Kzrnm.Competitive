﻿using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AtCoder.Internal;

namespace System
{
    using static MethodImplOptions;
    public static class __CollectionExtension
    {
        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
        {
            dic.TryGetValue(key, out var v);
            return v;
        }

        /// <summary>
        /// 連続する要素をひとまとめにした配列を返す。
        /// </summary>
        public static (T Value, int Count)[] CompressCount<T>(this IEnumerable<T> collection)
        {
            var e = collection.GetEnumerator();
            var list = new SimpleList<(T Value, int Count)>();
            if (!e.MoveNext()) return Array.Empty<(T, int)>();
            var cur = e.Current;
            list.Add((cur, 1));
            while (e.MoveNext())
            {
                if (EqualityComparer<T>.Default.Equals(cur, e.Current))
                    list[^1].Count++;
                else
                {
                    cur = e.Current;
                    list.Add((cur, 1));
                }
            }
            return list.ToArray();
        }
    }
}
