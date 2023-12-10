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
""") == 1);
        Debug.Assert(Solve("""
...........
.S-------7.
.|F-----7|.
.||.....||.
.||.....||.
.|L-7.F-J|.
.|..|.|..|.
.L--J.L--J.
...........
""") == 4);
        Debug.Assert(Solve("""
..........
.S------7.
.|F----7|.
.||....||.
.||....||.
.|L-7F-J|.
.|..||..|.
.L--JL--J.
..........
""") == 4);
        Debug.Assert(Solve("""
.F----7F7F7F7F-7....
.|F--7||||||||FJ....
.||.FJ||||||||L7....
FJL7L7LJLJ||LJ.L-7..
L--J.L7...LJS7F-7L7.
....F-J..F7FJ|L7L7L7
....L7.F7||L7|.L7L7|
.....|FJLJ|FJ|F7|.LJ
....FJL-7.||.||||...
....L---J.LJ.LJLJ...
""") == 8);
        Debug.Assert(Solve("""
FF7FSF7F7F7F7F7F---7
L|LJ||||||||||||F--J
FL-7LJLJ||||||LJL-77
F--JF--7||LJLJ7F7FJ-
L---JF-JLJ.||-FJLJJ7
|F|F-JF---7F7-L7L|7|
|FFJF7L7F-JF7|JL---7
7-L-JL7||F7|L7F-7F7|
L.L7LFJ|||||FJL7||LJ
L7JLJL-JLJLJL--JLJ.L
""") == 10);
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
        var (distance, farthestPoint, visitedDistances) = results.MaxBy(t => t.Distance);
        var points = GetLoopPoints(farthestPoint, visitedDistances);
        var result = CountInside(points);
        return result;

        int CountInside(HashSet<Point2d> loopPoints)
        {
            var count = 0;
            for (int row = 0; row < grid.Length; row++)
            {
                for (int col = 0; col < grid[row].Length; col++)
                {
                    var p = new Point2d(row, col);
                    if (!loopPoints.Contains(p) && IsInside(p))
                        count++;
                }
            }
            return count;

            bool IsInside(Point2d point)
            {
                var offset = new Point2d(1, 1);
                var current = point;
                var pipes = 0;
                while (IsValid(current))
                {
                    var c = grid[current.Row][current.Column];
                    if (loopPoints.Contains(current) && c != '7' && c != 'L')
                        pipes++;

                    current += offset;
                }
                return pipes % 2 == 1;
            }
        }

        HashSet<Point2d> GetLoopPoints(Point2d farthestLoopPoint, Dictionary<Point2d, int> distances)
        {
            var points = new HashSet<Point2d>();
            var toVisit = new Queue<Point2d>();
            toVisit.Enqueue(farthestLoopPoint);

            while (toVisit.TryDequeue(out var current))
            {
                if (!points.Add(current)) continue;

                var currentDistance = distances[current];

                foreach (var adjacent in GetAdjacent(current))
                {
                    if (distances.TryGetValue(adjacent, out var distance) && distance + 1 == currentDistance)
                        toVisit.Enqueue(adjacent);
                }
            }

            //PrintLoopPoints();

            void PrintLoopPoints()
            {
                for (int row = 0; row < grid.Length; row++)
                {
                    for (int col = 0; col < grid[row].Length; col++)
                    {
                        Console.Write(points.Contains(new(row, col)) ? 'X' : grid[row][col]);
                    }
                    Console.WriteLine();
                }
            }

            return points;
        }

        (int Distance, Point2d FarthestPoint, Dictionary<Point2d, int> VisitedDistances) Solve(char startPipe)
        {
            //Console.WriteLine($"Solving with {startPipe}");
            var visited = new Dictionary<Point2d, int>();
            var queue = new PriorityQueue<Point2d, int>();
            visited[start] = 0;
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
                    return (maxDistance, maxPoint, visited);
                }
            }

            return (-1, null, null);

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
