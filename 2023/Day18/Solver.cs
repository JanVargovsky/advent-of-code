using System.Numerics;

namespace AdventOfCode.Year2023.Day18;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
R 6 (#70c710)
D 5 (#0dc571)
L 2 (#5713f0)
D 2 (#d2c081)
R 2 (#59c680)
D 2 (#411b91)
L 5 (#8ceee2)
U 2 (#caa173)
L 1 (#1b58a2)
U 2 (#caa171)
R 2 (#7807d2)
U 3 (#a77fa3)
L 2 (#015232)
U 2 (#7a21e3)
""") == 952408144115);
    }

    public long Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var directions = new Dictionary<char, Point2d>
        {
            ['R'] = new(0, 1),
            ['L'] = new(0, -1),
            ['U'] = new(-1, 0),
            ['D'] = new(1, 0),
        };
        directions['0'] = directions['R'];
        directions['1'] = directions['D'];
        directions['2'] = directions['L'];
        directions['3'] = directions['U'];

        var current = new Point2d(0, 0);
        var length = 0;
        var points = new HashSet<Point2d>();
        points.Add(current);

        foreach (var row in rows)
        {
            var tokens = row.Split(' ');
            var hex = tokens[2][2..^1];
            var distance = Convert.ToInt32(hex[..^1], 16);
            var direction = directions[hex[^1]];
            current += direction * distance;
            length += distance;
            points.Add(current);
        }

        //Print(points);

        var result = CalculateCount();
        return result;

        long CalculateCount()
        {
            // https://en.wikipedia.org/wiki/Polygon#Simple_polygons => https://en.m.wikipedia.org/wiki/Shoelace_formula#Triangle_formula
            var area = Math.Abs(points.Zip(points.Skip(1)).Select(t => ShoelaceFormula(t.First, t.Second)).Sum()) / 2;

            var interiorArea = PicksTheorem(area, length);
            var result = interiorArea + length;
            return result;

            static long ShoelaceFormula(Point2d a, Point2d b) => (long)a.Row * b.Column - (long)b.Row * a.Column;
            static long PicksTheorem(long area, long boundaryPoints) => area - boundaryPoints / 2 + 1;
        }

        void Print(HashSet<Point2d> points)
        {
            var minX = points.Min(t => t.Row);
            var maxX = points.Max(t => t.Row);
            var minY = points.Min(t => t.Column);
            var maxY = points.Max(t => t.Column);
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    var contains = points.Contains(new(x, y));
                    Console.Write(contains ? '#' : '.');
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

    private record Point2d(int Row, int Column) : IAdditionOperators<Point2d, Point2d, Point2d>
    {
        public static Point2d operator +(Point2d left, Point2d right) => new(left.Row + right.Row, left.Column + right.Column);
        public static Point2d operator *(Point2d left, int value) => new(left.Row * value, left.Column * value);

    }
}
