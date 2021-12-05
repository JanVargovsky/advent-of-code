namespace AdventOfCode.Year2021.Day05;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"1,1 -> 3,3
9,7 -> 7,9") == "0");
        Debug.Assert(Solve(@"0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2") == "12");
    }

    public string Solve(string input)
    {
        var lines = input.Split(Environment.NewLine).Select(line =>
        {
            var coords = line.Split(new[] { ",", " -> " }, StringSplitOptions.None);
            return new Line(
                new(int.Parse(coords[0]), int.Parse(coords[1])),
                new(int.Parse(coords[2]), int.Parse(coords[3])));
        });
        var diagram = new Dictionary<Point, int>();

        foreach (var line in lines)
        {
            foreach (var point in line.Points)
            {
                if (!diagram.TryAdd(point, 1))
                    diagram[point] += 1;
            }

            //Print();
        }

        //var linqResult = lines.SelectMany(t => t.Points).GroupBy(t => t).Select(t => t.Count()).Count(t => t > 1);
        var result = diagram.Count(t => t.Value > 1);
        return result.ToString();

        void Print()
        {
            var topLeft = new Point(diagram.Keys.Min(t => t.X), diagram.Keys.Min(t => t.Y));
            var bottomRight = new Point(diagram.Keys.Max(t => t.X), diagram.Keys.Max(t => t.Y));
            for (int y = topLeft.Y; y <= bottomRight.Y; y++)
            {
                for (int x = topLeft.X; x <= bottomRight.X; x++)
                {
                    _ = diagram.TryGetValue(new Point(x, y), out var l);
                    Console.Write(l > 0 ? l.ToString() : ".");
                }
                Console.WriteLine();
            }
        }
    }

    record Point(int X, int Y)
    {
        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }
    }

    record Line(Point Start, Point End)
    {
        public IEnumerable<Point> Points => GetPoints();

        private IEnumerable<Point> GetPoints()
        {
            var increment = new Point(Math.Sign(End.X - Start.X), Math.Sign(End.Y - Start.Y));
            var current = Start;

            while (true)
            {
                yield return current;
                if (current == End)
                    break;
                current += increment;
            }
        }
    }
}