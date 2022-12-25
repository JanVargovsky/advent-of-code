using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace AdventOfCode.Year2022.Day24;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
#.######
#>>.<^<#
#.<..<<#
#>v.><>#
#<^v^^>#
######.#
""") == 18);
    }

    public int Solve(string input)
    {
        const char wall = '#';

        var map = input.Split(Environment.NewLine);
        var blizzards = new List<Blizzard>();
        LoadBlizzards();
        var blizzardPoints = PrecalculateBlizzards();

        var start = new Point(map[0].IndexOf('.'), 0);
        var end = new Point(map[^1].IndexOf('.'), map.Length - 1);

        var result = CalculatePath(start, end);
        return result;

        int CalculatePath(Point start, Point end)
        {
            var q = new Queue<State>();
            var visited = new HashSet<State>();
            var initialState = new State(0, start);
            q.Enqueue((initialState));
            visited.Add(initialState);

            while (q.TryDequeue(out var state))
            {
                var (minute, me) = state;
                if (me == end)
                    return minute - 1;

                var newBlizzardPoints = blizzardPoints[minute % blizzardPoints.Count];

                foreach (var neighbor in Neighbors(me))
                {
                    if (TryGetMap(neighbor, out var c) && c is not wall)
                    {
                        if (!newBlizzardPoints.Contains(neighbor))
                        {
                            var newState = new State(minute + 1, neighbor);
                            if (visited.Add(newState))
                            {
                                q.Enqueue(newState);
                            }
                        }
                    }
                }
                if (!newBlizzardPoints.Contains(me))
                    q.Enqueue(new State(minute + 1, me));
            }

            return -1;
        }

        int[] MoveBlizzards(int[] blizzardPointIndices)
        {
            var result = new int[blizzardPointIndices.Length];
            for (int i = 0; i < blizzardPointIndices.Length; i++)
            {
                result[i] = (blizzardPointIndices[i] + 1) % blizzards[i].Points.Length;
            }
            return result;
        }

        IEnumerable<Point> Neighbors(Point p)
        {
            yield return new(p.X, p.Y + 1);
            yield return new(p.X, p.Y - 1);
            yield return new(p.X + 1, p.Y);
            yield return new(p.X - 1, p.Y);
        }

        List<HashSet<Point>> PrecalculateBlizzards()
        {
            var result = new List<HashSet<Point>>();
            var seen = new HashSet<int[]>(new ArrayEqualityComparer<int>());
            var indices = Enumerable.Repeat(0, blizzards.Count).ToArray();

            var i = 0;
            while (true)
            {
                if (!seen.Add(indices))
                    break;

                result.Add(indices.Select((t, i) => blizzards[i].Position(t)).ToHashSet());
                indices = MoveBlizzards(indices);
                i++;
            }

            return result;
        }

        void LoadBlizzards()
        {
            var blizzardDirection = new Dictionary<char, Point>()
            {
                ['^'] = new(0, -1),
                ['v'] = new(0, 1),
                ['<'] = new(-1, 0),
                ['>'] = new(1, 0),
            };

            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (blizzardDirection.TryGetValue(map[y][x], out var direction))
                    {
                        var initial = new Point(x, y);
                        var points = CalculatePoints(initial, direction);
                        blizzards.Add(new(points));
                    }
                }
            }

            Point[] CalculatePoints(Point initial, Point direction)
            {
                var forward = new List<Point>();
                var current = initial;
                while (Map(current) != wall)
                {
                    forward.Add(current);
                    current += direction;
                }

                var backward = new List<Point>();
                current = initial - direction;
                while (Map(current) != wall)
                {
                    backward.Add(current);
                    current -= direction;
                }
                backward.Reverse();

                return forward.Concat(backward).ToArray();
            }
        }

        bool TryGetMap(Point p, out char c)
        {
            var valid = p.X >= 0 && p.Y >= 0 && p.Y < map.Length && p.X < map[p.Y].Length;
            c = valid ? Map(p) : char.MinValue;
            return valid;
        }

        char Map(Point p) => map[p.Y][p.X];
    }
}

internal record Point(int X, int Y) :
    IAdditionOperators<Point, Point, Point>,
    ISubtractionOperators<Point, Point, Point>
{
    public static Point operator +(Point left, Point right)
    {
        return new(left.X + right.X, left.Y + right.Y);
    }
    public static Point operator -(Point left, Point right)
    {
        return new(left.X - right.X, left.Y - right.Y);
    }
}

internal record Blizzard(Point[] Points)
{
    public Point Position(int i) => Points[i % Points.Length];
}

internal record State(int Minute, Point Me);

internal class ArrayEqualityComparer<T> : IEqualityComparer<T[]>
{
    public bool Equals(T[]? x, T[]? y)
    {
        return x.SequenceEqual(y);
    }

    public int GetHashCode([DisallowNull] T[] obj)
    {
        if (obj.Length == 0) return 0;

        var hashCode = HashCode.Combine(obj[0]);
        for (int i = 1; i < obj.Length; i++)
        {
            hashCode = HashCode.Combine(hashCode, obj[i]);
        }
        return hashCode;
    }
}
