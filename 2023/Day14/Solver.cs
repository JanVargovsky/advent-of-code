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
""") == 136);
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

        var tiltDirection = new Point2d(0, -1);
        var afterTilt = Tilt(cubeRocks, roundRocks, tiltDirection);
        var heights = afterTilt.Select(t => rows.Length - t.Y);
        var result = heights.Sum();
        return result;
    }

    private HashSet<Point2d> Tilt(IReadOnlySet<Point2d> cubeRocks, IReadOnlySet<Point2d> roundRocks, Point2d direction)
    {
        var newRoundRocks = new HashSet<Point2d>();

        foreach (var rock in roundRocks.OrderBy(t => t.Y))
        {
            var current = rock;
            while (true)
            {
                var next = current + direction;
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

        bool IsInRange(Point2d p) => p.Y >= 0;
    }

    private record Point2d(int X, int Y) : IAdditionOperators<Point2d, Point2d, Point2d>
    {
        public static Point2d operator +(Point2d left, Point2d right) => new(left.X + right.X, left.Y + right.Y);
    }
}
