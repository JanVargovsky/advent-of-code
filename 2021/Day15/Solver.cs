namespace AdventOfCode.Year2021.Day15;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"1163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581") == "40");
    }

    public string Solve(string input)
    {
        var map = input.Split(Environment.NewLine);
        var length = map.Length;
        var distances = Calculate();
        var result = distances[length - 1, length - 1] - distances[0, 0];
        return result.ToString();

        int[,] Calculate()
        {
            var distances = new int[length, length];
            for (int x = 0; x < length; x++)
                for (int y = 0; y < length; y++)
                    distances[x, y] = int.MaxValue;

            var q = new Queue<(int, int, int)>();
            q.Enqueue((0, 0, 0));

            while (q.Count > 0)
            {
                var (x, y, pathCost) = q.Dequeue();
                if (x >= length || y >= length)
                    continue;

                var cost = map[y][x] - '0';
                pathCost += cost;
                if (distances[x, y] > pathCost)
                {
                    distances[x, y] = pathCost;
                    q.Enqueue((x + 1, y, pathCost));
                    q.Enqueue((x, y + 1, pathCost));
                }
            }

            return distances;
        }
    }
}
