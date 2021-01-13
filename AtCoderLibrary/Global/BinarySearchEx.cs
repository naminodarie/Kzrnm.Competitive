﻿using AtCoder.Internal;
using System;

namespace AtCoder
{
#pragma warning disable IDE1006
    public static class __BinarySearchEx
#pragma warning restore IDE1006
    {
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static int BinarySearch<TOp>(int ok, int ng) where TOp : IOk<int>
            => BinarySearch(ok, ng, default(TOp));
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <paramref name="op"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="op"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<paramref name="op"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static int BinarySearch<TOp>(int ok, int ng, TOp op) where TOp : IOk<int>
        {
            while (Math.Abs(ok - ng) > 1)
            {
                var m = ((ok - ng) >> 1) + ng;
                if (op.Ok(m)) ok = m;
                else ng = m;
            }
            return ok;
        }

        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static long BinarySearch<TOp>(long ok, long ng) where TOp : IOk<long>
            => BinarySearch(ok, ng, default(TOp));
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <paramref name="op"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="op"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<paramref name="op"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static long BinarySearch<TOp>(long ok, long ng, TOp op) where TOp : IOk<long>
        {
            while (Math.Abs(ok - ng) > 1)
            {
                var m = ((ok - ng) >> 1) + ng;
                if (op.Ok(m)) ok = m;
                else ng = m;
            }
            return ok;
        }

        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static double BinarySearch<TOp>(double ok, double ng, double eps = 1e-7) where TOp : IOk<double>
            => BinarySearch(ok, ng, default(TOp), eps);
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <paramref name="op"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="op"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<paramref name="op"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static double BinarySearch<TOp>(double ok, double ng, TOp op, double eps = 1e-7) where TOp : IOk<double>
        {
            while (Math.Abs(ok - ng) > eps)
            {
                var m = (ok + ng) / 2;
                if (op.Ok(m)) ok = m;
                else ng = m;
            }
            return ok;
        }


        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static T BinarySearch<T, TOp>(T ok, T ng) where TOp : IBinaryOk<T>
            => BinarySearch(ok, ng, default(TOp));
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <paramref name="op"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="op"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<paramref name="op"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static T BinarySearch<T, TOp>(T ok, T ng, TOp op) where TOp : IBinaryOk<T>
        {
            while (op.Continue(ok, ng))
            {
                var m = op.Mid(ok, ng);
                if (op.Ok(m)) ok = m;
                else ng = m;
            }
            return ok;
        }

        //#pragma warning disable CA1031
        //        private static bool NgOrThrow<T>(IOk<T> op, T val) { try { return !op.Ok(val); } catch { return true; } }
        //#pragma warning restore CA1031
    }
    [IsOperator]
    public interface IOk<in T>
    {
        bool Ok(T value);
    }
    [IsOperator]
    public interface IBinaryOk<T> : IOk<T>
    {
        T Mid(T ok, T ng);
        bool Continue(T ok, T ng);
    }
}
