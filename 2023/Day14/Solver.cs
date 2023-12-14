using System.Numerics;

namespace AdventOfCode.Year2023.Day14;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
O....#....
O.OO#....#
.....##...
OO.#O....O
.O.....O#.
O.#..O.#.#
..O..#O..O
.......O..
#....###..
#OO..#....
""") == 64);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var cubeRocks = new HashSet<Point2d>();
        var roundRocks = new HashSet<Point2d>();
        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[y].Length; x++)
            {
                if (rows[y][x] is 'O')
                    roundRocks.Add(new(x, y));
                if (rows[y][x] is '#')
                    cubeRocks.Add(new(x, y));
            }
        }

        var north = new DirectionWithOrder(new(0, -1), p => p.Y);
        var west = new DirectionWithOrder(new(-1, 0), p => p.X);
        var south = new DirectionWithOrder(new(0, 1), p => -p.Y);
        var east = new DirectionWithOrder(new(1, 0), p => -p.X);
        var tilts = new DirectionWithOrder[] { north, west, south, east };
        var max = new Point2d(rows[0].Length, rows.Length);
        var cycle = 0;
        var wantedCycle = 1000000000;
        var history = new List<(HashSet<Point2d> RoundRocks, int Result)>();
        while (cycle++ <= wantedCycle)
        {
            foreach (var tilt in tilts)
                roundRocks = Tilt(cubeRocks, roundRocks, tilt, max);

            //Print(cubeRocks, roundRocks, max);
            //Console.WriteLine();re
            var heights = roundRocks.Select(t => rows.Length - t.Y);
            var result = heights.Sum();

            var index = history.FindIndex(t => t.RoundRocks.SetEquals(roundRocks));
            if (index >= 0)
            {
                var cycleStart = index;
                var cycleEnd = cycle - 1;
                var cycleLength = cycleEnd - cycleStart;
                var offsetBeforeCycle = cycleStart;
                var historyResultIndex = ((wantedCycle - offsetBeforeCycle) % cycleLength) + offsetBeforeCycle - 1;
                var historyResult = history[historyResultIndex];
                return historyResult.Result;
            }

            history.Add((roundRocks, result));
        }

        throw new ItWontHappenException();
    }

    private void Print(IReadOnlySet<Point2d> cubeRocks, IReadOnlySet<Point2d> roundRocks, Point2d max)
    {
        for (int y = 0; y < max.Y; y++)
        {
            for (int x = 0; x < max.X; x++)
            {
                var p = new Point2d(x, y);
                if (cubeRocks.Contains(p))
                    Console.Write('#');
                else if (roundRocks.Contains(p))
                    Console.Write('O');
                else
                    Console.Write('.');
            }
            Console.WriteLine();
        }
    }

    private HashSet<Point2d> Tilt(IReadOnlySet<Point2d> cubeRocks, IReadOnlySet<Point2d> roundRocks, DirectionWithOrder direction, Point2d max)
    {
        var newRoundRocks = new HashSet<Point2d>();

        foreach (var rock in roundRocks.OrderBy(direction.Order))
        {
            var current = rock;
            while (true)
            {
                var next = current + direction.Direction;
                if (IsInRange(next) && !cubeRocks.Contains(next) && !newRoundRocks.Contains(next))
                    current = next;
                else
                {
                    newRoundRocks.Add(current);
                    break;
                }
            }

        }

        return newRoundRocks;

        bool IsInRange(Point2d p) => p.X >= 0 && p.Y >= 0 && p.X < max.X && p.Y < max.Y;
    }

    private record DirectionWithOrder(Point2d Direction, Func<Point2d, int> Order);

    private record Point2d(int X, int Y) : IAdditionOperators<Point2d, Point2d, Point2d>
    {
        public static Point2d operator +(Point2d left, Point2d right) => new(left.X + right.X, left.Y + right.Y);
    }
}
