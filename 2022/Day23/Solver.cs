using System.Numerics;

namespace AdventOfCode.Year2022.Day23;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
.....
..##.
..#..
.....
..##.
.....
""") == 25);
        Debug.Assert(Solve("""
..............
..............
.......#......
.....###.#....
...#...#.#....
....#...##....
...#.###......
...##.#.##....
....#..#......
..............
..............
..............
""") == 110);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var points = new HashSet<Point>();
        for (int y = 0; y < rows.Length; y++)
            for (int x = 0; x < rows[y].Length; x++)
                if (rows[y][x] == '#')
                    points.Add(new(x, y));
        var initialMinX = points.Min(t => t.X);
        var initialMaxX = points.Max(t => t.X);
        var initialMinY = points.Min(t => t.Y);
        var initialMaxY = points.Max(t => t.Y);

        //  N
        // W E
        //  S
        var N = new Point(0, -1);
        var S = new Point(0, 1);
        var W = new Point(-1, 0);
        var E = new Point(1, 0);
        var NE = N + E;
        var NW = N + W;
        var SE = S + E;
        var SW = S + W;
        var rules = new List<Rule>
        {
            new(new[] { N, NE, NW}, N),
            new(new[] { S, SE, SW}, S),
            new(new[] { W, NW, SW}, W),
            new(new[] { E, NE, SE}, E),
        };
        var adjacent = new[] { N, S, W, E, NE, NW, SE, SW };
        var adjacentRule = new Rule(adjacent, new(0, 0));

        Console.WriteLine("Initial");
        Render();

        for (int i = 1; i <= 10; i++)
        {
            var newPoints = ApplyRound();
            var moved = false;
            foreach (var point in newPoints)
            {
                if (!points.Contains(point))
                {
                    moved = true;
                    break;
                }
            }

            if (moved)
                points = newPoints;
            else
                break;

            var currentFirstRule = rules[0];
            rules.RemoveAt(0);
            rules.Add(currentFirstRule);

            Console.WriteLine($"End of Round {i}");
            Render();
        }

        var totalSize = GetRectangleSize();
        var result = totalSize - points.Count;
        return result;

        int GetRectangleSize()
        {
            var minX = points.Min(t => t.X);
            var maxX = points.Max(t => t.X);
            var minY = points.Min(t => t.Y);
            var maxY = points.Max(t => t.Y);
            var diffX = maxX - minX + 1;
            var diffY = maxY - minY + 1;
            return diffX * diffY;
        }

        void Render()
        {
            var minX = Math.Min(initialMinX, points.Min(t => t.X));
            var maxX = Math.Max(initialMaxX, points.Max(t => t.X));
            var minY = Math.Min(initialMinY, points.Min(t => t.Y));
            var maxY = Math.Max(initialMaxY, points.Max(t => t.Y));

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (points.Contains(new(x, y)))
                        Console.Write('#');
                    else
                        Console.Write('.');
                }
                Console.WriteLine();
            }
        }

        HashSet<Point> ApplyRound()
        {
            var proposed = new List<(Point New, Point Old)>();

            foreach (var point in points)
            {
                var rule = rules.Prepend(adjacentRule).FirstOrDefault(rule => rule.Points.All(p => !points.Contains(point + p)));
                var newPoint = rule switch
                {
                    null => point,
                    _ => point + rule.Move,
                };
                proposed.Add((newPoint, point));
            }

            var result = new HashSet<Point>();
            var grouped = proposed.GroupBy(t => t.New);
            foreach (var group in grouped)
            {
                var items = group.ToList();
                if (items.Count == 1)
                    result.Add(items[0].New);
                else
                    foreach (var item in items)
                    {
                        result.Add(item.Old);
                    }
            }

            Debug.Assert(points.Count == result.Count);

            return result;
        }
    }
}

internal record Point(int X, int Y) : IAdditionOperators<Point, Point, Point>
{
    public static Point operator +(Point left, Point right)
    {
        return new(left.X + right.X, left.Y + right.Y);
    }
}

internal record Rule(Point[] Points, Point Move);
