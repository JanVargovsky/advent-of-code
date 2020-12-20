using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2020.Day20
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Solve(@"Tile 2311:
..##.#..#.
##..#.....
#...##..#.
####.#...#
##.##.###.
##...#.###
.#.#.#..##
..#....#..
###...#.#.
..###..###

Tile 1951:
#.##...##.
#.####...#
.....#..##
#...######
.##.#....#
.###.#####
###.##.##.
.###....#.
..#.#..#.#
#...##.#..

Tile 1171:
####...##.
#..##.#..#
##.#..#.#.
.###.####.
..###.####
.##....##.
.#...####.
#.##.####.
####..#...
.....##...

Tile 1427:
###.##.#..
.#..#.##..
.#.##.#..#
#.#.#.##.#
....#...##
...##..##.
...#.#####
.#.####.#.
..#..###.#
..##.#..#.

Tile 1489:
##.#.#....
..##...#..
.##..##...
..#...#...
#####...#.
#..#.#.#.#
...#.#.#..
##.#...##.
..##.##.##
###.##.#..

Tile 2473:
#....####.
#..#.##...
#.##..#...
######.#.#
.#...#.#.#
.#########
.###.#..#.
########.#
##...##.#.
..###.#.#.

Tile 2971:
..#.#....#
#...###...
#.#.###...
##.##..#..
.#####..##
.#..####.#
#..#.#..#.
..####.###
..#.#.###.
...#.#.#.#

Tile 2729:
...#.#.#.#
####.#....
..#.#.....
....#..#.#
.##..##.#.
.#.####...
####.#.#..
##.####...
##..#.##..
#.##...##.

Tile 3079:
#.#.#####.
.#..######
..#.......
######....
####.#..#.
.#...#.##.
#.#####.##
..#.###...
..#.......
..#.###...") == "20899048083289");
        }

        public string Solve(string input)
        {
            var tiles = input.Split(Environment.NewLine + Environment.NewLine)
                .Select(t =>
                {
                    var lines = t.Split(Environment.NewLine);
                    return new Tile(long.Parse(lines[0][5..^1]), lines[1..]);
                })
                .ToDictionary(t => t.Id);

            var sideToTile = new Dictionary<string, List<TileSide>>();

            foreach (var tile in tiles.Values)
                foreach (var side in Enum.GetValues<Side>())
                    foreach (var flip in new[] { false, true })
                    {
                        var sideData = GetSide(tile.Data, side, flip);
                        if (!sideToTile.TryGetValue(sideData, out var sideTiles))
                            sideToTile[sideData] = sideTiles = new();
                        sideTiles.Add(new(tile.Id, side, flip));
                    }

            var corners = sideToTile
                .Where(t => t.Value.Count == 1)
                .Select(t => t.Value[0])
                .GroupBy(t => t.Id)
                .Where(t => t.Count() == 4);

            var result = corners.Aggregate(1L, (a, b) => a * b.Key);
            return result.ToString();

            string GetSide(string[] tile, Side side, bool flip)
            {
                var sideData = side switch
                {
                    Side.Top => tile[0],
                    Side.Bottom => tile[^1],
                    Side.Left => string.Concat(tile.Select(t => t[0])),
                    Side.Right => string.Concat(tile.Select(t => t[^1])),
                    _ => throw new NotSupportedException(),
                };

                return flip ? string.Concat(sideData.Reverse()) : sideData;
            }
        }

        record Tile(long Id, string[] Data);
        record TileSide(long Id, Side Side, bool Flipped);

        enum Side
        {
            Top,
            Bottom,
            Left,
            Right
        }
    }
}
