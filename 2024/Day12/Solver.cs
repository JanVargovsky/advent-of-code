using System.Numerics;

namespace AdventOfCode.Year2024.Day12;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
AAAA
BBCD
BBCC
EEEC
""") == 80);

        Debug.Assert(Solve("""
OOOOO
OXOXO
OOOOO
OXOXO
OOOOO
""") == 436);

        Debug.Assert(Solve("""
EEEEE
EXXXX
EEEEE
EXXXX
EEEEE
""") == 236);

        Debug.Assert(Solve("""
AAAAAA
AAABBA
AAABBA
ABBAAA
ABBAAA
AAAAAA
""") == 368);

        Debug.Assert(Solve("""
RRRRIICCFF
RRRRIICCCF
VVRRRCCFFF
VVRCCCJFFF
VVVVCJJCFE
VVIVCCJJEE
VVIIICJJEE
MIIIIIJJEE
MIIISIJEEE
MMMISSJEEE
""") == 1206);
    }

    public long Solve(string input)
    {
        var map = input.Split(Environment.NewLine);
        var regions = Parse(map);
        var prices = regions.Select(t => t.Area * t.Fences);
        var result = prices.Sum();
        return result;

        List<Region> Parse(string[] map)
        {
            var result = new List<Region>();
            var visited = new HashSet<Point>();

            for (var y = 0; y < map.Length; y++)
            {
                for (var x = 0; x < map[y].Length; x++)
                {
                    var p = new Point(x, y);
                    if (visited.Contains(p))
                        continue;

                    var region = MapRegion(p);
                    visited.UnionWith(region.Points);
                    result.Add(region);
                }
            }

            return result;

            bool IsValid(Point p) => p.X >= 0 && p.Y >= 0 && p.Y < map.Length && p.X < map[p.Y].Length;

            Region MapRegion(Point start)
            {
                var name = map[start.Y][start.X];
                var points = new HashSet<Point>();

                MapRegionInternal(start);

                return new Region(name, points);

                void MapRegionInternal(Point p)
                {
                    if (!IsValid(p))
                        return;

                    if (map[p.Y][p.X] != name)
                        return;

                    if (visited.Contains(p) || !points.Add(p))
                        return;

                    foreach (var dp in Point.AllDirections)
                    {
                        var next = p + dp;
                        MapRegionInternal(next);
                    }
                }
            }
        }
    }
}

record Region(char Name, HashSet<Point> Points)
{
    public long Area => Points.Count;
    public long Perimeter => CalculatePerimeter();
    public long Fences => CalculateFences();

    long CalculateFences()
    {
        var fences = new HashSet<(Point Point, FenceType Type)>();
        var directions = new (Point Direction, FenceType Type)[] {
            (Point.Up, FenceType.Up),
            (Point.Down, FenceType.Down),
            (Point.Left, FenceType.Left),
            (Point.Right, FenceType.Right)
        };

        foreach (var p in Points)
        {
            foreach (var (direction, type) in directions)
            {
                if (!Points.Contains(p + direction))
                    fences.Add((p, type));
            }
        }

        var groupedFences = fences.GroupBy(t => t.Type).ToDictionary(t => t.Key, t => t.Select(t => t.Point).ToHashSet());
        var fenceCount = 0L;

        foreach (var (type, points) in groupedFences)
        {
            var grouped = type switch
            {
                FenceType.Up or FenceType.Down => points.GroupBy(t => t.Y).ToDictionary(t => t.Key, t => t.Select(t => t.X).Order().ToArray()),
                FenceType.Left or FenceType.Right => points.GroupBy(t => t.X).ToDictionary(t => t.Key, t => t.Select(t => t.Y).Order().ToArray()),
                _ => throw new ItWontHappenException(),
            };

            foreach (var items in grouped.Values)
            {
                fenceCount++;
                // calculate gaps in increasing sequence
                for (int i = 1; i < items.Length; i++)
                {
                    if (items[i - 1] + 1 != items[i])
                        fenceCount++;
                }
            }
        }

        return fenceCount;
    }

    long CalculatePerimeter()
    {
        var result = 0L;
        foreach (var p in Points)
        {
            result += 4 - NeighborCount(p);
        }
        return result;


        int NeighborCount(Point p)
        {
            var result = 0;
            foreach (var dp in Point.AllDirections)
                if (Points.Contains(p + dp))
                    result++;
            return result;
        }
    }

    enum FenceType
    {
        Up,
        Down,
        Left,
        Right,
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
