namespace AdventOfCode.Year2022.Day12;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi
""") == 29);
    }

    public int Solve(string input)
    {
        var map = input.Split(Environment.NewLine);
        // a < b ... < z
        // S = a
        // E = z

        const char S = 'S';
        const char E = 'E';
        var end = Find(E);
        var result = FindAll('a').Append(Find(S)).Select(start => Search(start, end)).Min();

        return result;

        int Search(Point start, Point end)
        {
            var visited = new HashSet<Point>();
            var queue = new Queue<(Point, int)>();
            queue.Enqueue((start, 0));

            while (queue.Count > 0)
            {
                var (current, steps) = queue.Dequeue();

                if (current == end)
                    return steps;

                if (!visited.Add(current))
                    continue;

                var currentHeight = GetHeight(current.X, current.Y);

                foreach (var neighbor in GetNeighbors(current.X, current.Y))
                {
                    if (!IsValid(neighbor.X, neighbor.Y))
                        continue;

                    var height = GetHeight(neighbor.X, neighbor.Y);

                    if (currentHeight >= height || currentHeight == height - 1)
                    {
                        queue.Enqueue((neighbor, steps + 1));
                    }
                }
            }

            return int.MaxValue;
        }

        char GetHeight(int x, int y) => map[y][x] switch
        {
            E => 'z',
            S => 'a',
            _ => map[y][x]
        };

        bool IsValid(int x, int y) => x >= 0 && y >= 0 && x < map[0].Length && y < map.Length;

        IEnumerable<Point> GetNeighbors(int x, int y)
        {
            var neighborsOffsets = new[]
            {
                new Point(1, 0),
                new Point(-1, 0),
                new Point(0, 1),
                new Point(0, -1),
            };

            foreach (var offset in neighborsOffsets)
            {
                yield return new Point(x + offset.X, y + offset.Y);
            }
        }

        Point Find(char c)
        {
            for (int x = 0; x < map[0].Length; x++)
            {
                for (int y = 0; y < map.Length; y++)
                {
                    Debug.Assert(IsValid(x, y));
                    if (map[y][x] == c) return new(x, y);
                }
            }

            throw new Exception();
        }

        IEnumerable<Point> FindAll(char c)
        {
            for (int x = 0; x < map[0].Length; x++)
            {
                for (int y = 0; y < map.Length; y++)
                {
                    Debug.Assert(IsValid(x, y));
                    if (map[y][x] == c) yield return new(x, y);
                }
            }
        }
    }
}

file record Point(int X, int Y);
