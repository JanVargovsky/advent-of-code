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
                int.Parse(coords[0]), int.Parse(coords[1]),
                int.Parse(coords[2]), int.Parse(coords[3]));
        });
        var diagram = new Dictionary<Point, int>();

        foreach (var line in lines)
        {
            if (line.IsHorizontal)
            {
                for (int y = Math.Min(line.Y1, line.Y2); y <= Math.Max(line.Y1, line.Y2); y++)
                {
                    var p = new Point(line.X1, y);
                    if (!diagram.TryAdd(p, 1))
                        diagram[p] += 1;
                }
            }
            else if (line.IsVertical)
            {
                for (int x = Math.Min(line.X1, line.X2); x <= Math.Max(line.X1, line.X2); x++)
                {
                    var p = new Point(x, line.Y1);
                    if (!diagram.TryAdd(p, 1))
                        diagram[p] += 1;
                }
            }
            else // is diagonal
            {
                var (p, end) = line.X1 < line.X2 ?
                    (new Point(line.X1, line.Y1), new Point(line.X2, line.Y2)) :
                    (new Point(line.X2, line.Y2), new Point(line.X1, line.Y1));

                var inc = p.Y < end.Y ? 1 : -1;
                while (true)
                {
                    if (!diagram.TryAdd(p, 1))
                        diagram[p] += 1;
                    if (p.Equals(end))
                        break;

                    p = p with
                    {
                        X = p.X + 1,
                        Y = p.Y + inc,
                    };
                }
            }

            //Print();
        }

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

    record Point(int X, int Y);

    record Line(int X1, int Y1, int X2, int Y2)
    {
        public bool IsHorizontal => X1 == X2;
        public bool IsVertical => Y1 == Y2;
    }
}