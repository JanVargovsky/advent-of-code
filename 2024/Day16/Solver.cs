using System.Numerics;

namespace AdventOfCode.Year2024.Day16;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
###############
#.......#....E#
#.#.###.#.###.#
#.....#.#...#.#
#.###.#####.#.#
#.#.#.......#.#
#.#.#####.###.#
#...........#.#
###.#.#####.#.#
#...#.....#.#.#
#.#.#.###.#.#.#
#.....#...#.#.#
#.###.#.#.#.#.#
#S..#.....#...#
###############
""") == 45);

        Debug.Assert(Solve("""
#################
#...#...#...#..E#
#.#.#.#.#.#.#.#.#
#.#.#.#...#...#.#
#.#.#.#.###.#.#.#
#...#.#.#.....#.#
#.#.#.#.#.#####.#
#.#...#.#.#.....#
#.#.#####.#.###.#
#.#.#.......#...#
#.#.###.#####.###
#.#.#...#.....#.#
#.#.#.#####.###.#
#.#.#.........#.#
#.#.#.#########.#
#S#.............#
#################
""") == 64);
    }

    public long Solve(string input)
    {
        var map = input.Split(Environment.NewLine);
        var start = Find('S');
        var end = Find('E');
        var q = new PriorityQueue<(Point Point, Point Direction, List<Point> Path), long>();
        q.Enqueue((start, new Point(1, 0), [start]), 0);
        var visited = new Dictionary<(Point Point, Point Direction), long>();

        var bestPaths = new List<List<Point>>();
        long? acceptedDistance = null;

        while (q.TryDequeue(out var items, out var distance))
        {
            var (position, direction, path) = items;
            if (position == end)
            {
                if (!acceptedDistance.HasValue)
                    acceptedDistance = distance;

                if ((acceptedDistance.HasValue && acceptedDistance.Value == distance))
                {
                    //Print(path.ToHashSet());
                    bestPaths.Add(path);
                }

                continue;
            }

            if (acceptedDistance.HasValue && distance > acceptedDistance)
                continue;

            if (!IsValid(position))
                continue;

            if (visited.GetValueOrDefault((position, direction), int.MaxValue) < distance)
                continue;

            visited[(position, direction)] = distance;

            if (map[position.Y][position.X] == '#')
                continue;

            var next = position + direction;
            var leftDirection = new Point(-direction.Y, direction.X);
            var nextLeft = position + leftDirection;
            var rightDirection = new Point(direction.Y, -direction.X);
            var nextRight = position + rightDirection;

            if (IsValid(next))
                q.Enqueue((next, direction, [.. path, next]), distance + 1);
            if (IsValid(nextLeft))
                q.Enqueue((nextLeft, leftDirection, [.. path, nextLeft]), distance + 1001);
            if (IsValid(nextRight))
                q.Enqueue((nextRight, rightDirection, [.. path, nextRight]), distance + 1001);
        }

        var bestPoints = bestPaths.SelectMany(t => t).ToHashSet();

        //Print(bestPoints);

        return bestPoints.Count;

        void Print(HashSet<Point> points)
        {
            for (var y = 0; y < map.Length; y++)
            {
                for (var x = 0; x < map[y].Length; x++)
                {
                    var p = new Point(x, y);

                    if (points.Contains(p))
                        Console.Write('O');
                    else
                        Console.Write(map[y][x]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        bool IsValid(Point p) => p.X >= 0 && p.Y >= 0 && p.Y < map.Length && p.X < map[p.Y].Length;

        Point Find(char c)
        {
            for (var y = 0; y < map.Length; y++)
            {
                for (var x = 0; x < map[y].Length; x++)
                    if (map[y][x] == c)
                        return new Point(x, y);
            }

            throw new ItWontHappenException();
        }
    }

    record Point(int X, int Y) : IAdditionOperators<Point, Point, Point>
    {
        public static Point operator +(Point left, Point right) => new(left.X + right.X, left.Y + right.Y);
    }
}
