using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2020.Day24
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Solve(@"sesenwnenenewseeswwswswwnenewsewsw
neeenesenwnwwswnenewnwwsewnenwseswesw
seswneswswsenwwnwse
nwnwneseeswswnenewneswwnewseswneseene
swweswneswnenwsewnwneneseenw
eesenwseswswnenwswnwnwsewwnwsene
sewnenenenesenwsewnenwwwse
wenwwweseeeweswwwnwwe
wsweesenenewnwwnwsenewsenwwsesesenwne
neeswseenwwswnwswswnw
nenwswwsewswnenenewsenwsenwnesesenew
enewnwewneswsewnwswenweswnenwsenwsw
sweneswneswneneenwnewenewwneswswnese
swwesenesewenwneswnwwneseswwne
enesenwswwswneneswsenwnewswseenwsese
wnwnesenesenenwwnenwsewesewsesesew
nenewswnwewswnenesenwnesewesw
eneswnwswnwsenenwnwnwwseeswneewsenese
neswnwewnwnwseenwseesewsenwsweewe
wseweeenwnesenwwwswnew") == "2208");
        }

        public string Solve(string input)
        {
            var instructions = input.Split(Environment.NewLine);

            //  N
            // W E
            //  S

            // e, se, sw, w, nw, and ne
            //  NW NE
            // W     E
            //  SW SE

            var referenceTile = new Hex(0, 0, 0);
            var directionOffsets = new Dictionary<string, Hex>
            {
                ["w"] = new(-1, 1, 0),
                ["e"] = new(1, -1, 0),
                ["sw"] = new(-1, 0, 1),
                ["se"] = new(0, -1, 1),
                ["nw"] = new(0, 1, -1),
                ["ne"] = new(1, 0, -1)
            };
            var grid = new Dictionary<Hex, Tile>();

            foreach (var directions in instructions)
            {
                var current = referenceTile;

                for (int i = 0; i < directions.Length; i++)
                {
                    var direction = directions[i..(i + 1)];
                    if (direction is not ("w" or "e"))
                    {
                        direction = directions[i..(i + 2)];
                        i++;
                    }

                    current += directionOffsets[direction];
                }

                if (!grid.TryGetValue(current, out var tile))
                    tile = Tile.White;

                grid[current] = tile == Tile.White ? Tile.Black : Tile.White;
            }

            for (int day = 1; day <= 100; day++)
            {
                var hexToCheck = grid.Keys.SelectMany(p => directionOffsets.Values.Select(d => p + d).Append(p)).ToHashSet();
                var toFlip = new List<Hex>();

                foreach (var hex in hexToCheck)
                {
                    var black = 0;
                    foreach (var direction in directionOffsets.Values)
                    {
                        if (grid.TryGetValue(hex + direction, out var t) && t == Tile.Black)
                            black++;
                    }

                    if (!grid.TryGetValue(hex, out var tile))
                        tile = Tile.White;

                    if (tile == Tile.Black && black is 0 or > 2)
                        toFlip.Add(hex);
                    else if (tile == Tile.White && black is 2)
                        toFlip.Add(hex);
                }

                foreach (var hex in toFlip)
                {
                    if (!grid.TryGetValue(hex, out var tile))
                        tile = Tile.White;
                    grid[hex] = tile == Tile.White ? Tile.Black : Tile.White;
                }

                if (day < 10 || day % 10 == 0)
                    Console.WriteLine($"Day {day}: {grid.Values.Count(t => t == Tile.Black)}");
            }

            var result = grid.Values.Count(t => t == Tile.Black);
            return result.ToString();
        }

        record Hex(int X, int Y, int Z)
        {
            public static Hex operator +(Hex a, Hex b) =>
                new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        enum Tile
        {
            White,
            Black
        }
    }
}
