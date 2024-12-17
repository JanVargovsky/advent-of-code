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
""") == 7036);

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
""") == 11048);
    }

    public long Solve(string input)
    {
        var map = input.Split(Environment.NewLine);
        var start = Find('S');
        var end = Find('E');
        var q = new PriorityQueue<(Point Point, Point Direction), long>();
        q.Enqueue((start, new Point(1, 0)), 0);
        var visited = new Dictionary<(Point Point, Point Direction), long>();

        while (q.TryDequeue(out var items, out var distance))
        {
            var (position, direction) = items;
            if (position == end)
                return distance;

            if (!IsValid(position))
                continue;

            if (visited.GetValueOrDefault(items, int.MaxValue) < distance)
                continue;

            visited[items] = distance;

            if (map[position.Y][position.X] == '#')
                continue;

            var next = position + direction;
            var leftDirection = new Point(-direction.Y, direction.X);
            var nextLeft = position + leftDirection;
            var rightDirection = new Point(direction.Y, -direction.X);
            var nextRight = position + rightDirection;

            if (IsValid(next))
                q.Enqueue((next, direction), distance + 1);
            if (IsValid(nextLeft))
                q.Enqueue((nextLeft, leftDirection), distance + 1001);
            if (IsValid(nextRight))
                q.Enqueue((nextRight, rightDirection), distance + 1001);
        }

        throw new ItWontHappenException();

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
