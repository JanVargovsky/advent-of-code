namespace AdventOfCode.Year2021.Day09;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"2199943210
3987894921
9856789892
8767896789
9899965678") == "15");
    }

    public string Solve(string input)
    {
        var map = input.Split(Environment.NewLine);

        var directions = new Point[]
        {
            new(0, 1),
            new(0, -1),
            new(1, 0),
            new(-1, 0),
        };

        var result = 0;
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                if (IsLowPoint(x, y))
                {
                    var value = map[y][x] - '0';
                    var riskLevel = value + 1;
                    result += riskLevel;
                }
            }
        }

        return result.ToString();

        bool IsLowPoint(int x, int y)
        {
            var value = map[y][x];

            foreach (var direction in directions)
            {
                var xi = x + direction.X;
                var yi = y + direction.Y;
                if (!IsValid(xi, yi))
                    continue;

                var adjacentValue = map[yi][xi];
                if (value >= adjacentValue)
                    return false;
            }

            return true;
        }

        bool IsValid(int x, int y) => x >= 0 && y >= 0 && y < map.Length && x < map[y].Length;
    }

    record Point(int X, int Y)
    {
        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }
    }
}