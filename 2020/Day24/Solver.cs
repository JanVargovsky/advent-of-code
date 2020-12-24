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
wseweeenwnesenwwwswnew") == "10");
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
