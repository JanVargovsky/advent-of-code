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
2311944581") == "315");
    }

    public string Solve(string input)
    {
        var tile = input.Split(Environment.NewLine);
        var length = tile.Length * 5;
        var map = new int[length, length];
        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < length; x++)
            {
                var (scaleX, xi) = Math.DivRem(x, tile.Length);
                var (scaleY, yi) = Math.DivRem(y, tile.Length);
                var value = tile[yi][xi] - '0';
                value += scaleX + scaleY;
                if (value > 9)
                    value %= 9;
                map[x, y] = value;
            }
        }
        //PrintMap();
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
                if (x < 0 || y < 0 || x >= length || y >= length)
                    continue;

                var cost = map[x, y];
                pathCost += cost;
                if (distances[x, y] > pathCost)
                {
                    distances[x, y] = pathCost;
                    q.Enqueue((x + 1, y, pathCost));
                    q.Enqueue((x - 1, y, pathCost));
                    q.Enqueue((x, y + 1, pathCost));
                    q.Enqueue((x, y - 1, pathCost));
                }
            }

            return distances;
        }

        void PrintMap()
        {
            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    Console.Write(map[x, y]);
                }
                Console.WriteLine();
            }
        }
    }
}
