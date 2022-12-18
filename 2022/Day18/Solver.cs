using System.Numerics;

namespace AdventOfCode.Year2022.Day18;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
0,0,0
""") == 6);
        Debug.Assert(Solve("""
1,1,1
2,1,1
""") == 10);
        Debug.Assert(Solve("""
2,2,2
1,2,2
3,2,2
2,1,2
2,3,2
2,2,1
2,2,3
2,2,4
2,2,6
1,2,5
3,2,5
2,1,5
2,3,5
""") == 58);
    }

    public int Solve(string input)
    {
        var cubes = input.Split(Environment.NewLine).Select(t =>
        {
            var tokens = t.Split(',');
            return new Point(int.Parse(tokens[0]), int.Parse(tokens[1]), int.Parse(tokens[2]));
        }).ToHashSet();

        var queue = new Queue<Point>();
        var visited = new HashSet<Point>();
        var min = new Point(cubes.Min(t => t.X) - 1, cubes.Min(t => t.Y) - 1, cubes.Min(t => t.Z) - 1);
        var max = new Point(cubes.Max(t => t.X) + 1, cubes.Max(t => t.Y) + 1, cubes.Max(t => t.Z) + 1);
        var start = min;
        Debug.Assert(!cubes.Contains(start));
        queue.Enqueue(start);

        while (queue.TryDequeue(out var cube))
        {
            if (cube.X < min.X || cube.X > max.X ||
                    cube.Y < min.Y || cube.Y > max.Y ||
                    cube.Z < min.Z || cube.Z > max.Z)
                continue;

            if (cubes.Contains(cube))
                continue;

            if (!visited.Add(cube))
                continue;

            foreach (var neighbor in GetNeighbors(cube))
            {
                queue.Enqueue(neighbor);
            }
        }
        var outside = visited;
        var inside = new HashSet<Point>();
        for (int x = min.X; x <= max.X; x++)
        {
            for (int y = min.Y; y <= max.Y; y++)
            {
                for (int z = min.Z; z <= max.Z; z++)
                {
                    var p = new Point(x, y, z);
                    if (!outside.Contains(p))
                        inside.Add(p);
                }
            }
        }

        var result = CalculateSurface(inside);
        return result;

        int CalculateSurface(HashSet<Point> cubes)
        {
            int result = 0;
            foreach (var cube in cubes)
            {
                foreach (var neighbor in GetNeighbors(cube))
                {
                    if (!cubes.Contains(neighbor))
                        result++;
                }
            }
            return result;
        }

        IEnumerable<Point> GetNeighbors(Point a)
        {
            yield return a with { X = a.X + 1 };
            yield return a with { X = a.X - 1 };

            yield return a with { Y = a.Y + 1 };
            yield return a with { Y = a.Y - 1 };

            yield return a with { Z = a.Z + 1 };
            yield return a with { Z = a.Z - 1 };
        }
    }
}

internal record Point(int X, int Y, int Z) : IAdditionOperators<Point, Point, Point>
{
    public static Point operator +(Point left, Point right)
    {
        return new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }
}
