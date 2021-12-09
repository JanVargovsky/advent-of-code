namespace AdventOfCode.Year2021.Day09;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"2199943210
3987894921
9856789892
8767896789
9899965678") == "1134");
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

        var visited = new HashSet<Point>();
        var basinSizes = new List<int>();

        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                var basinSize = ScanBasin(x, y);
                if (basinSize > 0)
                    basinSizes.Add(basinSize);
            }
        }

        var result = basinSizes.OrderByDescending(t => t).Take(3).Aggregate(1, (t, x) => t * x);

        return result.ToString();

        int ScanBasin(int x, int y)
        {
            var size = 0;
            var queue = new Queue<Point>();
            queue.Enqueue(new Point(x, y));

            while (queue.Count > 0)
            {
                var p = queue.Dequeue();

                if (!IsValid(p.X, p.Y))
                    continue;

                if (!visited.Add(p))
                    continue;

                var value = map[p.Y][p.X];
                if (value == '9')
                    continue;

                size++;

                foreach (var direction in directions)
                {
                    queue.Enqueue(p + direction);
                }
            }

            return size;
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