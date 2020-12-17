using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2020.Day17
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Solve(@".#.
..#
###") == "848");
        }

        public string Solve(string input)
        {
            var data = input.Split(Environment.NewLine);

            var grid = new Dictionary<Point, char>();

            for (int x = 0; x < data.Length; x++)
            {
                for (int y = 0; y < data[0].Length; y++)
                {
                    grid[new(x - 1, y - 1, 0, 0)] = data[x][y];
                }
            }

            var neighborOffsets = new List<Point>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        for (int w = -1; w <= 1; w++)
                        {
                            if (x == 0 && y == 0 && z == 0 && w == 0) continue;
                            neighborOffsets.Add(new(x, y, z, w));
                        }
                    }
                }
            }

            for (int i = 0; i < 6; i++)
            {
                Print(i);
                Expand();
                var newGrid = grid.ToDictionary(t => t.Key, t => t.Value);

                foreach (var (p, c) in grid)
                {
                    var activeNeighbors = 0;

                    foreach (var neighborOffset in neighborOffsets)
                    {
                        var p2 = p + neighborOffset;
                        if (grid.TryGetValue(p2, out var c2) && c2 == '#')
                            activeNeighbors++;
                    }

                    if (c == '#' && activeNeighbors is not (2 or 3))
                        newGrid[p] = '.';
                    else if (c == '.' && activeNeighbors is 3)
                        newGrid[p] = '#';
                }

                grid = newGrid;
            }

            var result = grid.Values.Count(t => t == '#');
            return result.ToString();

            void Expand()
            {
                var points = new HashSet<Point>(grid.Keys);

                foreach (var p in points)
                {
                    foreach (var neighborOffset in neighborOffsets)
                    {
                        var p2 = p + neighborOffset;
                        if (!points.Contains(p2))
                            grid[p2] = '.';
                    }
                }
            }

            void Print(int i)
            {
                //var z = grid.Keys.Min(t => t.Z);
                //var maxZ = grid.Keys.Max(t => t.Z);

                Console.WriteLine($"Cycle={i}");

                //while (z <= maxZ)
                //{
                //    Console.WriteLine($"z={z}");
                //    for (int x = -i - 1; x <= i + 1; x++)
                //    {
                //        for (int y = -i - 1; y <= i + 1; y++)
                //        {
                //            Console.Write(grid[new(x, y, z)]);
                //        }

                //        Console.WriteLine();
                //    }

                //    Console.WriteLine();
                //    z++;
                //}
            }
        }

        record Point(int X, int Y, int Z, int W)
        {
            public static Point operator +(Point a, Point b) =>
                new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
        }
    }
}