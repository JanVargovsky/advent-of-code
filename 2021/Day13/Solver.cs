namespace AdventOfCode.Year2021.Day13;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0

fold along y=7
fold along x=5") == "17");
    }

    public string Solve(string input)
    {
        var items = input.Split(Environment.NewLine);
        var points = new HashSet<Point>();
        var i = 0;
        while (i < items.Length)
        {
            var separator = items[i].IndexOf(',');
            if (separator == -1)
                break;
            var x = int.Parse(items[i][..separator]);
            var y = int.Parse(items[i][(separator + 1)..]);
            points.Add(new Point(x, y));
            i++;
        }

        while (++i < items.Length)
        {
            var separator = items[i].IndexOf('=');
            var axis = items[i][separator - 1];
            var value = int.Parse(items[i][(separator + 1)..]);

            //Print(true, axis, value);
            Fold(axis, value);
            //Print(false, axis, value);
            break;
        }


        var result = points.Count;
        return result.ToString();

        void Fold(char axis, int value)
        {
            var newPoints = new HashSet<Point>();
            if (axis == 'y')
            {
                foreach (var point in points)
                {
                    if (point.Y <= value)
                    {
                        newPoints.Add(point);
                    }
                    else
                    {
                        newPoints.Add(point with { Y = 2 * value - point.Y });
                    }
                }
            }
            else if (axis == 'x')
            {
                foreach (var point in points)
                {
                    if (point.X <= value)
                    {
                        newPoints.Add(point);
                    }
                    else
                    {
                        newPoints.Add(point with { X = 2 * value - point.X });
                    }
                }
            }

            points = newPoints;
        }

        void Print(bool showFold, char axis, int value)
        {
            var topLeft = new Point(points.Min(t => t.X), points.Min(t => t.Y));
            var bottomRight = new Point(points.Max(t => t.X), points.Max(t => t.Y));
            for (int y = topLeft.Y; y <= bottomRight.Y; y++)
            {

                for (int x = topLeft.X; x <= bottomRight.X; x++)
                {
                    if (showFold && axis == 'y' && y == value)
                    {
                        Console.Write('-');
                    }
                    else if (showFold && axis == 'x' && x == value)
                        Console.Write('|');
                    else
                        Console.Write(points.Contains(new Point(x, y)) ? "#" : ".");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

    record Point(int X, int Y)
    {
        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }
    }
}
