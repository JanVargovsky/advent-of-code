using System;
using System.Diagnostics;
using System.Linq;
using MoreLinq.Extensions;

namespace AdventOfCode.Year2020.Day11
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Solve(@"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL") == "26");
        }

        public string Solve(string input)
        {
            var seats = input.Split(Environment.NewLine)
                .Select(t => t.ToCharArray())
                .ToArray();

            var positions = new (int X, int Y)[]
            {
                (1, 0),
                (0, 1),
                (-1, 0),
                (0, -1),
                (-1, -1),
                (-1, 1),
                (1, -1),
                (1, 1)
            };

            var gen = 0;
            while (true)
            {
                var newSeats = seats.Select(t => t.ToArray()).ToArray();
                gen++;

                if (!Tick(seats, newSeats))
                    break;
                seats = newSeats;
                Console.WriteLine("Gen: " + gen);
                seats.ForEach(t => Console.WriteLine(string.Join("", t)));
            }

            return seats.Sum(t => t.Count(s => s == '#')).ToString();

            bool Tick(char[][] seats, char[][] newSeats)
            {
                var changed = false;
                for (int x = 0; x < seats.Length; x++)
                {
                    for (int y = 0; y < seats[0].Length; y++)
                    {
                        var adjacentSeats = positions
                            .Select((t) => GetVisible(seats, x, y, t.X, t.Y))
                            .ToArray();

                        if (seats[x][y] == 'L' && adjacentSeats.Count(t => t == '#') == 0)
                        {
                            newSeats[x][y] = '#';
                            changed = true;
                        }
                        else if (seats[x][y] == '#' && adjacentSeats.Count(t => t == '#') >= 5)
                        {
                            newSeats[x][y] = 'L';
                            changed = true;
                        }
                    }
                }
                return changed;
            }

            char? GetVisible(char[][] seats, int x, int y, int xi, int yi)
            {
                char? c = null;
                do
                {
                    x += xi;
                    y += yi;
                    c = Get(seats, x, y);
                } while (c != null && c == '.');
                return c;
            }

            char? Get(char[][] seats, int x, int y)
            {
                if (x >= 0 && x < seats.Length && y >= 0 && y < seats[0].Length)
                    return seats[x][y];
                return null;
            }
        }
    }
}
