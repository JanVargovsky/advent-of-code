using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MoreLinq;

namespace AdventOfCode.Year2020.Day20
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Rotate(new[] { "123", "456" }).SequenceEqual(new[] { "41", "52", "63" }));

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
..#.###...") == "273");
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

            var n = (int)Math.Sqrt(tiles.Count);

            var corner = tiles[corners.First().Key];
            OrientedTile[,] grid = null;
            foreach (var orientedTile in GetOrientations(corner.Data))
            {
                grid = new OrientedTile[n, n];
                grid[0, 0] = new(corner.Id, orientedTile);
                if (CreateGrid(grid))
                    break;
            }

            var image = CreateImage(grid);
            //Console.WriteLine(string.Join(Environment.NewLine, image));
            var seaMonster = new[]
            {
"                  # ",
"#    ##    ##    ###",
" #  #  #  #  #  #   "
            };

            var seaMonsterIndices = new List<Point>();
            for (int x = 0; x < seaMonster.Length; x++)
            {
                for (int y = 0; y < seaMonster[0].Length; y++)
                {
                    if (seaMonster[x][y] == '#')
                        seaMonsterIndices.Add(new(x, y));
                }
            }

            var seaMonsters = new List<Point>();
            char[][] orientedImage = null;
            foreach (var im in GetOrientations(image))
            {
                for (int x = 0; x < im.Length - seaMonster.Length; x++)
                {
                    for (int y = 0; y < im[0].Length - seaMonster[0].Length; y++)
                    {
                        if (seaMonsterIndices.All(p => im[p.X + x][p.Y + y] == '#'))
                            seaMonsters.Add(new(x, y));
                    }
                }
                if (seaMonsters.Count > 0)
                {
                    orientedImage = im.Select(t => t.ToCharArray()).ToArray();
                    break;
                }
            }

            foreach (var seaMonsterPoint in seaMonsters)
            {
                seaMonsterIndices
                    .Select(p => seaMonsterPoint + p)
                    .ForEach(p => orientedImage[p.X][p.Y] = 'O');
            }

            for (int x = 0; x < orientedImage.Length; x++)
            {
                for (int y = 0; y < orientedImage[0].Length; y++)
                {
                    var c = orientedImage[x][y];
                    Console.ForegroundColor = c == 'O' ? ConsoleColor.Green : ConsoleColor.Gray;
                    Console.Write(c);
                }
                Console.WriteLine();
            }

            var roughness = orientedImage.Select(t => t.Count(c => c == '#')).Sum();
            return roughness.ToString();

            string[] CreateImage(OrientedTile[,] grid)
            {
                var tileLength = grid[0, 0].Data.Length - 2;
                var image = new string[tileLength * n];

                // 0 - 0 1
                // 1 - 0 2
                // 2 - 0 3
                // 3 - 0 4
                // 4 - 0 5
                // 5 - 0 6
                // 6 - 0 1
                // 7 - 0 2
                // 7 - 1 3

                for (int i = 0; i < image.Length; i++)
                {
                    var x = i / tileLength;
                    var gx = i % tileLength + 1;
                    //Console.WriteLine($"{i} - {x} {gx}");

                    var row = Enumerable.Range(0, n).Select(y => grid[y, x].Data[gx][1..^1]);
                    image[i] = string.Concat(row);
                }

                return image;
            }

            bool CreateGrid(OrientedTile[,] grid)
            {
                if (!TryGetFirstEmpty(grid, out var p))
                    return true;

                OrientedTile reference;
                Side referenceSide;
                Side referenceOppositeSide;
                if (p.X == 0)
                {
                    // get it from top tile
                    reference = grid[p.X, p.Y - 1];
                    referenceSide = Side.Bottom;
                    referenceOppositeSide = Side.Top;
                }
                else
                {
                    // get it from left tile
                    reference = grid[p.X - 1, p.Y];
                    referenceSide = Side.Right;
                    referenceOppositeSide = Side.Left;
                }

                var sideData = GetSide(reference.Data, referenceSide, false);
                var tileSide = sideToTile[sideData].FirstOrDefault(t => t.Id != reference.Id);
                if (tileSide is null)
                    return false;

                var tile = GetOrientations(tiles[tileSide.Id].Data).FirstOrDefault(t => sideData == GetSide(t, referenceOppositeSide, false));
                if (tile is null)
                    return false;

                grid[p.X, p.Y] = new(tileSide.Id, tile);

                return CreateGrid(grid);
            }

            IEnumerable<string[]> GetOrientations(string[] data)
            {
                yield return data;
                yield return data.Reverse().ToArray();
                for (int i = 0; i < 3; i++)
                {
                    data = Rotate(data);
                    yield return data;
                    yield return data.Reverse().ToArray();
                }
            }

            bool TryGetFirstEmpty(OrientedTile[,] grid, out Point p)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                    for (int y = 0; y < grid.GetLength(1); y++)
                    {
                        if (grid[x, y] is null)
                        {
                            p = new(x, y);
                            return true;
                        }
                    }

                p = null;
                return false;
            }

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

        private string[] Rotate(string[] array)
        {
            // 1 2 3
            // 4 5 6

            // 4 1
            // 5 2
            // 6 3

            var result = new string[array[0].Length];
            for (int x = array.Length - 1; x >= 0; x--)
            {
                for (int y = 0; y < array[0].Length; y++)
                {
                    result[y] += array[x][y];
                }
            }

            return result;
        }

        record Tile(long Id, string[] Data);
        record TileSide(long Id, Side Side, bool Flipped);
        record Point(int X, int Y)
        {
            public static Point operator +(Point a, Point b) =>
                new(a.X + b.X, a.Y + b.Y);
        }
        record OrientedTile(long Id, string[] Data);

        enum Side
        {
            Top,
            Bottom,
            Left,
            Right
        }
    }
}
