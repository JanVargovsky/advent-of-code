using System.Numerics;

namespace AdventOfCode.Year2023.Day10;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
.....
.S-7.
.|.|.
.L-J.
.....
""") == 4);
        Debug.Assert(Solve("""
..F7.
.FJ|.
SJ.L7
|F--J
LJ...
""") == 8);
    }

    public int Solve(string input)
    {
        var grid = input.Split(Environment.NewLine);
        var north = new Point2d(-1, 0);
        var south = new Point2d(1, 0);
        var west = new Point2d(0, -1);
        var east = new Point2d(0, 1);
        var start = PointOf('S');
        var pipes = "F7JL-|";
        var results = pipes.Select(pipe => Solve(pipe));
        var result = results.Max();
        return result;

        int Solve(char startPipe)
        {
            //Console.WriteLine($"Solving with {startPipe}");
            var visited = new Dictionary<Point2d, int>();
            var queue = new PriorityQueue<Point2d, int>();
            queue.EnqueueRange(GetAdjacentFor(start, startPipe), 1);

            while (queue.TryDequeue(out var current, out var distance))
            {
                if (!IsValid(current)) continue;

                if (!visited.TryAdd(current, distance)) continue;

                queue.EnqueueRange(GetAdjacent(current), distance + 1);
            }

            var maxes = visited.OrderByDescending(t => t.Value);
            foreach (var (maxPoint, maxDistance) in maxes)
            {
                var adjacents = GetAdjacent(maxPoint).ToArray();
                if (adjacents.Length < 2) continue;
                if (visited.TryGetValue(adjacents[0], out var distance1) && visited.TryGetValue(adjacents[1], out var distance2) &&
                    maxDistance == distance1 + 1 && maxDistance == distance2 + 1)
                {
                    //Print();
                    return maxDistance;
                }
            }

            return -1;

            void Print()
            {
                for (int row = 0; row < grid.Length; row++)
                {
                    for (int col = 0; col < grid[row].Length; col++)
                    {
                        Console.Write(visited.TryGetValue(new(row, col), out var distance) ? distance.ToString() : grid[row][col]);
                    }
                    Console.WriteLine();
                }
            }
        }

        Point2d PointOf(char c)
        {
            for (int row = 0; row < grid.Length; row++)
            {
                for (int col = 0; col < grid[row].Length; col++)
                {
                    if (grid[row][col] == c)
                        return new(row, col);
                }
            }
            throw new ItWontHappenException();
        }

        IEnumerable<Point2d> GetAdjacent(Point2d point)
        {
            if (!IsValid(point)) return Enumerable.Empty<Point2d>();

            var c = grid[point.Row][point.Column];
            return GetAdjacentFor(point, c);
        }

        IEnumerable<Point2d> GetAdjacentFor(Point2d point, char c)
        {
            if (c is '|')
            {
                yield return point + north;
                yield return point + south;
            }
            else if (c is '-')
            {
                yield return point + east;
                yield return point + west;
            }
            else if (c is 'L')
            {
                yield return point + north;
                yield return point + east;
            }
            else if (c is 'J')
            {
                yield return point + north;
                yield return point + west;
            }
            else if (c is '7')
            {
                yield return point + south;
                yield return point + west;
            }
            else if (c is 'F')
            {
                yield return point + south;
                yield return point + east;
            }
            else if (c is '.')
            {
                yield break;
            }
            else if (c is 'S')
            {
                yield break;
            }
        }

        bool IsValid(Point2d p) =>
            p.Row >= 0 && p.Row < grid.Length &&
            p.Column >= 0 && p.Column < grid[p.Row].Length;
    }

    private record Point2d(int Row, int Column) : IAdditionOperators<Point2d, Point2d, Point2d>
    {
        public static Point2d operator +(Point2d left, Point2d right)
        {
            return new(left.Row + right.Row, left.Column + right.Column);
        }
    }
}
