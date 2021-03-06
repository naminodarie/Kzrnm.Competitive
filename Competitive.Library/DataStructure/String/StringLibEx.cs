﻿using AtCoder.Internal;
using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive
{
    public static class StringLibEx
    {
        /// <summary>
        /// <paramref name="s"/> と <paramref name="t"/> の LCS(最長共通部分列)を求めます。
        /// </summary>
        public static char[] LCS(string s, string t) => LCS(s.AsSpan(), t);
        /// <summary>
        /// <paramref name="s"/> と <paramref name="t"/> の LCS(最長共通部分列)を求めます。
        /// </summary>
        public static T[] LCS<T>(T[] s, T[] t) => LCS((ReadOnlySpan<T>)s, t);
        /// <summary>
        /// <paramref name="s"/> と <paramref name="t"/> の LCS(最長共通部分列)を求めます。
        /// </summary>
        public static T[] LCS<T>(Span<T> s, Span<T> t) => LCS((ReadOnlySpan<T>)s, t);
        /// <summary>
        /// <paramref name="s"/> と <paramref name="t"/> の LCS(最長共通部分列)を求めます。
        /// </summary>
        public static T[] LCS<T>(ReadOnlySpan<T> s, ReadOnlySpan<T> t)
        {
            var dp = Global.NewArray(s.Length + 1, t.Length + 1, 0);
            int i, j;
            for (i = 0; i < s.Length; i++)
                for (j = 0; j < t.Length; j++)
                {
                    if (EqualityComparer<T>.Default.Equals(s[i], t[j]))
                        dp[i + 1][j + 1] = dp[i][j] + 1;
                    else
                        dp[i + 1][j + 1] = Math.Max(dp[i][j + 1], dp[i + 1][j]);
                }

            var list = LCSSearch(dp, s, s.Length, t.Length);
            list.Reverse();
            return list.ToArray();

            static SimpleList<T> LCSSearch(int[][] dp, ReadOnlySpan<T> s, int i, int j)
            {
                var list = new SimpleList<T>();
                while (i > 0 && j > 0)
                {
                    while (dp[i][j] == dp[i - 1][j])
                    {
                        if (--i <= 0) return list;
                    }
                    while (dp[i][j] == dp[i][j - 1])
                    {
                        if (--j <= 0) return list;
                    }

                    list.Add(s[i - 1]);
                    i--; j--;
                }
                return list;
            }
        }
    }
}