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
""") == 13);
    }

    public int Solve(string input)
    {
        var motions = input.Split(Environment.NewLine);
        var visited = new HashSet<Point>();
        var head = new Point(0, 0);
        var tail = new Point(0, 0);
        visited.Add(tail);

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

            for (int i = 0; i < steps; i++)
            {
                head.X += direction.X;
                head.Y += direction.Y;

                if (!AreAdjacent(head, tail))
                {
                    MoveTail();
                    Debug.Assert(AreAdjacent(head, tail));
                    visited.Add(tail);
                }
            }

        }

        return visited.Count;

        void MoveTail()
        {
            var x = head.X - tail.X;
            if (x > 1) x = 1;
            else if (x < -1) x = -1;

            var y = head.Y - tail.Y;
            if (y > 1) y = 1;
            else if (y < -1) y = -1;

            tail.X += x;
            tail.Y += y;
        }

        bool AreAdjacent(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) <= 1 && Math.Abs(a.Y - b.Y) <= 1;
        }
    }
}

file record struct Point(int X, int Y);