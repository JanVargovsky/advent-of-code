namespace AdventOfCode.Year2022.Day09;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2
""") == 1);
    }

    public int Solve(string input)
    {
        var motions = input.Split(Environment.NewLine);
        var visited = new HashSet<Point>();
        var head = new Point(0, 0);
        var knots = new Point[9];
        visited.Add(knots[^1]);

        var directions = new Dictionary<string, Point>()
        {
            ["R"] = new Point(1, 0),
            ["L"] = new Point(-1, 0),
            ["D"] = new Point(0, 1),
            ["U"] = new Point(0, -1)
        };

        foreach (var motion in motions)
        {
            var tokens = motion.Split(' ');
            var nameOfDirection = tokens[0];
            var steps = int.Parse(tokens[1]);
            var direction = directions[nameOfDirection];

            for (int s = 0; s < steps; s++)
            {
                head.X += direction.X;
                head.Y += direction.Y;

                MoveIfNotAdjacent(head, ref knots[0]);

                for (int i = 0; i < knots.Length - 1; i++)
                {
                    MoveIfNotAdjacent(knots[i], ref knots[i + 1]);
                }

                visited.Add(knots[^1]);
            }

        }

        return visited.Count;

        void MoveIfNotAdjacent(Point a, ref Point b)
        {
            if (!AreAdjacent(a, b))
            {
                Move(a, ref b);
                Debug.Assert(AreAdjacent(a, b));
            }
        }

        void Move(Point a, ref Point b)
        {
            var x = a.X - b.X;
            if (x > 1) x = 1;
            else if (x < -1) x = -1;

            var y = a.Y - b.Y;
            if (y > 1) y = 1;
            else if (y < -1) y = -1;

            b.X += x;
            b.Y += y;
        }

        bool AreAdjacent(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) <= 1 && Math.Abs(a.Y - b.Y) <= 1;
        }
    }
}

file record struct Point(int X, int Y);