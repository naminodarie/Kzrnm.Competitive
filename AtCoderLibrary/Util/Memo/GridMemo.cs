﻿using Kzrnm.Competitive.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoder.Util.Memo
{
    using static AtCoder.Global;

    internal class GridMemo
    {
        // https://atcoder.jp/contests/abc088/tasks/abc088_d
        readonly PropertyConsoleReader cr;
        public GridMemo(PropertyConsoleReader cr) { this.cr = cr; }

        public object Calc()
        {
            var H = cr.Int;
            var W = cr.Int;
            var grid = cr.Repeat(H).Ascii;
            var bfs = NewArray(H, W, 100000);
            bfs[0][0] = 1;
            var queue = new Queue<(int h, int w)>();
            queue.Enqueue((0, 0));
            while (queue.Count > 0)
            {
                var (h, w) = queue.Dequeue();
                if (h > 0 && grid[h - 1][w] == '.')
                    if (bfs[h - 1][w].UpdateMin(bfs[h][w] + 1))
                        queue.Enqueue((h - 1, w));
                if (h + 1 < H && grid[h + 1][w] == '.')
                    if (bfs[h + 1][w].UpdateMin(bfs[h][w] + 1))
                        queue.Enqueue((h + 1, w));
                if (w > 0 && grid[h][w - 1] == '.')
                    if (bfs[h][w - 1].UpdateMin(bfs[h][w] + 1))
                        queue.Enqueue((h, w - 1));
                if (w + 1 < W && grid[h][w + 1] == '.')
                    if (bfs[h][w + 1].UpdateMin(bfs[h][w] + 1))
                        queue.Enqueue((h, w + 1));
            }
            return Math.Max(grid.SelectMany(st => st).Count(c => c == '.') - bfs[H - 1][W - 1], -1);
        }
    }

}
