using System.Numerics;

namespace AdventOfCode.Year2023.Day21;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
...........
.....###.#.
.###.##..#.
..#.#...#..
....#.#....
.##..S####.
.##..#...#.
.......##..
.##.#.####.
.##..##.##.
...........
""", 6) == 16);
    }

    public int Solve(string input, int maxSteps = 64)
    {
        var map = input.Split(Environment.NewLine);
        var north = new Point2d(-1, 0);
        var south = new Point2d(1, 0);
        var west = new Point2d(0, -1);
        var east = new Point2d(0, 1);
        var directions = new[] { north, south, west, east };
        var q = new Queue<(Point2d, int)>();
        q.Enqueue((FindStart(), 0));
        var visited = new HashSet<Point2d>();
        var visitedForResult = new HashSet<Point2d>();
        var lastStep = 0;

        while (q.TryDequeue(out var current))
        {
            var (position, step) = current;
            //Print(step);
            if (step >= maxSteps + 1)
                break;
            if (!(position.Row >= 0 && position.Column >= 0 && position.Row < map.Length && position.Column < map[position.Row].Length))
                continue;
            if (!(map[position.Row][position.Column] is '.' or 'S'))
                continue;
            if (step % 2 == 0)
                visitedForResult.Add(position);
            if (!visited.Add(position))
                continue;

            foreach (var direction in directions)
                q.Enqueue((position + direction, step + 1));

        }
        var result = visitedForResult.Count;
        return result;

        void Print(int step)
        {
            if (lastStep == step)
                return;

            lastStep = step;

            Console.WriteLine($"step={step - 1}");

            for (int row = 0; row < map.Length; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    var p = new Point2d(row, col);
                    var wasVisited = visitedForResult.Contains(p);
                    Console.Write(wasVisited ? 'O' : map[row][col]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        Point2d FindStart()
        {
            for (int row = 0; row < map.Length; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    if (map[row][col] == 'S')
                        return new(row, col);
                }
            }

            throw new ItWontHappenException();
        }
    }

    private record Point2d(int Row, int Column) : IAdditionOperators<Point2d, Point2d, Point2d>
    {
        public static Point2d operator +(Point2d left, Point2d right)
        {
            return new(left.Row + right.Row, left.Column + right.Column);
        }
    }
}
