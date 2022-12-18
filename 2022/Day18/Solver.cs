using System.Numerics;

namespace AdventOfCode.Year2022.Day18;

internal class Solver
{
    public Solver()
    {
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
""") == 64);
    }

    public int Solve(string input)
    {
        var cubes = input.Split(Environment.NewLine).Select(t =>
        {
            var tokens = t.Split(',');
            return new Point(int.Parse(tokens[0]), int.Parse(tokens[1]), int.Parse(tokens[2]));
        }).ToHashSet();

        var result = 0;
        foreach (var cube in cubes)
        {
            foreach (var neighbor in GetNeighbors(cube))
            {
                if (!cubes.Contains(neighbor))
                    result++;
            }
        }
        return result;

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
