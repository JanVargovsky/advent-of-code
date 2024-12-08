using System.Numerics;

namespace AdventOfCode.Year2024.Day08;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
..........
..........
..........
....a.....
..........
.....a....
..........
..........
..........
..........
""") == 2);

        Debug.Assert(Solve("""
............
........0...
.....0......
.......0....
....0.......
......A.....
............
............
........A...
.........A..
............
............
""") == 14);
    }

    public int Solve(string input)
    {
        var map = input.Split(Environment.NewLine);
        Dictionary<char, List<Point>> groupedAnthenas = [];

        for (var y = 0; y < map.Length; y++)
            for (var x = 0; x < map[y].Length; x++)
            {
                var c = map[y][x];
                if (c != '.')
                {
                    if (!groupedAnthenas.TryGetValue(c, out var points))
                        groupedAnthenas[c] = points = [];
                    points.Add(new(x, y));
                }
            }

        var antinodes = new HashSet<Point>();

        foreach (var (name, points) in groupedAnthenas)
        {
            foreach (var antenaA in points)
                foreach (var antenaB in points)
                {
                    if (antenaA == antenaB) continue;

                    var vec = antenaB - antenaA;
                    var p = antenaB + vec;
                    if (IsValid(p))
                        antinodes.Add(p);
                }
        }

        //Print(antinodes);

        return antinodes.Count;

        bool IsValid(Point p) => p.X >= 0 && p.Y >= 0 && p.Y < map.Length && p.X < map[p.Y].Length;

        void Print()
        {
            for (var y = 0; y < map.Length; y++)
            {
                for (var x = 0; x < map[y].Length; x++)
                {
                    var p = new Point(x, y);
                    if (antinodes.Contains(p))
                        Console.Write('#');
                    else
                        Console.Write(map[y][x]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

    record Point(int X, int Y) : IAdditionOperators<Point, Point, Point>, ISubtractionOperators<Point, Point, Point>
    {
        public static Point operator +(Point left, Point right) => new(left.X + right.X, left.Y + right.Y);

        public static Point operator -(Point left, Point right) => new(left.X - right.X, left.Y - right.Y);
    }
}
