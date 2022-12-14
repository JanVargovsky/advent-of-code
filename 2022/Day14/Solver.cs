using System.Numerics;
using MoreLinq;

namespace AdventOfCode.Year2022.Day14;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9
""") == 93);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var cave = new Dictionary<Point, Tile>();
        var minRocks = new Dictionary<int, int>();
        LoadRocks();
        AddFloor();
        //Render(484, 513, 0, 11);
        LoadAbyssBounds();
        var start = new Point(500, 0);
        var directions = new[] {
            new Point(0, 1),
            new Point(-1, 1),
            new Point(1, 1)
        };

        var i = 1;
        while (DropSand())
        {
            Console.WriteLine(i);
            i++;
        }

        //Render(484, 513, 0, 11);
        var result = cave.Values.Count(t => t is Tile.Sand);
        return result;

        void AddFloor()
        {
            var maxY = cave.Max(t => t.Key.Y);
            var y = maxY + 2;

            for (int x = 500 - y; x <= 500 + y; x++)
            {
                cave[new Point(x, y)] = Tile.Rock;
            }
        }

        void Render(int minX, int maxX, int minY, int maxY)
        {
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    var c = '.';
                    if (cave.TryGetValue(new Point(x, y), out var tile))
                        c = tile switch
                        {
                            Tile.Sand => 'O',
                            Tile.Rock => '#',
                        };
                    Console.Write(c);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        bool IsAbyss(Point p)
        {
            if (minRocks.TryGetValue(p.X, out var lastRock))
                return p.Y > lastRock;
            return true;
        }

        void LoadAbyssBounds()
        {
            var minX = cave.Min(t => t.Key.X);
            var maxX = cave.Max(t => t.Key.X);

            for (int x = minX; x <= maxX; x++)
            {
                var rocks = cave.Where(t => t.Key.X == x).ToArray();
                if (rocks.Any())
                    minRocks[x] = rocks.Max(t => t.Key.Y);
            }
        }

        bool DropSand()
        {
            var sand = start;
            while (true)
            {
                var foundNext = false;
                foreach (var direction in directions)
                {
                    var newSand = sand + direction;
                    if (cave.TryGetValue(newSand, out var tile))
                    {
                        continue;
                    }
                    else
                    {
                        if (IsAbyss(newSand))
                            return false;
                        foundNext = true;
                        sand = newSand;
                        break;
                    }
                }

                if (foundNext)
                    continue;
                else
                    break;
            }

            cave[sand] = Tile.Sand;

            if (sand == start)
            {
                return false;
            }
            return true;
        }

        void LoadRocks()
        {
            foreach (var row in rows)
            {
                var points = row.Split(" -> ").Select(t =>
                {
                    var numbers = t.Split(',');
                    return new Point(int.Parse(numbers[0]), int.Parse(numbers[1]));
                });

                var pairs = points.Pairwise((start, end) => (start, end));

                foreach (var (start, end) in pairs)
                {
                    var direction = new Point(
                        Normalize(end.X - start.X, -1, 1),
                        Normalize(end.Y - start.Y, -1, 1));

                    var current = start;
                    while (true)
                    {
                        cave[current] = Tile.Rock;
                        if (current == end) break;
                        current += direction;
                    };
                }
            }
        }

        int Normalize(int x, int min, int max)
        {
            return Math.Max(Math.Min(max, x), min);
        }
    }
}

internal record struct Point(int X, int Y) : IAdditionOperators<Point, Point, Point>
{
    public static Point operator +(Point left, Point right)
    {
        return new(left.X + right.X, left.Y + right.Y);
    }
}

internal enum Tile { Rock, Sand, }