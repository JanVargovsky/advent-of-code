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
""") == 140);

        Debug.Assert(Solve("""
OOOOO
OXOXO
OOOOO
OXOXO
OOOOO
""") == 772);

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
""") == 1930);
    }

    public long Solve(string input)
    {
        var map = input.Split(Environment.NewLine);
        var regions = Parse(map);
        var prices = regions.Select(t => t.Area * t.Perimeter);
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
