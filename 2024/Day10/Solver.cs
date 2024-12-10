using System.Numerics;

namespace AdventOfCode.Year2024.Day10;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
...0...
...1...
...2...
6543456
7.....7
8.....8
9.....9
""") == 2);

        Debug.Assert(Solve("""
10..9..
2...8..
3...7..
4567654
...8..3
...9..2
.....01
""") == 3);

        Debug.Assert(Solve("""
89010123
78121874
87430965
96549874
45678903
32019012
01329801
10456732
""") == 81);
    }

    public long Solve(string input)
    {
        var map = input.Split(Environment.NewLine);
        var trailheadScores = FindAll('0').Select(Score);
        var result = trailheadScores.Sum();
        return result;

        long Score(Point start)
        {
            var paths = new HashSet<(Point Start, Point End)>();
            var score = ScoreInternal(start, 0, [start]);
            return score;

            long ScoreInternal(Point p, int expected, List<Point> path)
            {
                if (!IsValid(p))
                    return 0;

                if (map[p.Y][p.X] - '0' != expected)
                    return 0;

                if (expected == 9)
                {
                    //Print(path);
                    paths.Add((path[0], path[^1]));
                    return 1;
                }

                var result = 0L;
                foreach (var dp in Point.AllDirections)
                {
                    var next = p + dp;
                    result += ScoreInternal(next, expected + 1, [.. path, next]);
                }
                return result;
            }
        }

        void Print(List<Point> path)
        {
            for (var y = 0; y < map.Length; y++)
            {
                for (var x = 0; x < map[y].Length; x++)
                {
                    var p = new Point(x, y);
                    if (path.Contains(p))
                        Console.Write('#');
                    else
                        Console.Write(map[y][x]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        bool IsValid(Point p) => p.X >= 0 && p.Y >= 0 && p.Y < map.Length && p.X < map[p.Y].Length;

        IEnumerable<Point> FindAll(char c)
        {
            for (var y = 0; y < map.Length; y++)
            {
                for (var x = 0; x < map[y].Length; x++)
                    if (map[y][x] == c)
                        yield return new Point(x, y);
            }
        }
    }

    record Point(int X, int Y) : IAdditionOperators<Point, Point, Point>
    {
        public static readonly Point Up = new(0, -1);
        public static readonly Point Down = new(0, 1);
        public static readonly Point Left = new(-1, 0);
        public static readonly Point Right = new(1, 0);
        public static readonly Point[] AllDirections = [Up, Down, Right, Left];

        public static Point operator +(Point left, Point right) => new(left.X + right.X, left.Y + right.Y);
    }
}
