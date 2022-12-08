namespace AdventOfCode.Year2022.Day08;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
30373
25512
65332
33549
35390
""") == 21);
    }

    public int Solve(string input)
    {
        var grid = new Grid(input.Split(Environment.NewLine));
        var visible = new HashSet<Coords>();

        for (int x = 0; x < grid.MaxX; x++)
        {
            // left
            var row = grid.GetRow(x).ToArray();
            foreach (var item in GetVisible(row))
                visible.Add(item);
            // right
            Array.Reverse(row);
            foreach (var item in GetVisible(row))
                visible.Add(item);
        }

        for (int y = 0; y < grid.MaxY; y++)
        {
            // top
            var column = grid.GetColumn(y).ToArray();
            foreach (var item in GetVisible(column))
                visible.Add(item);
            // bottom
            Array.Reverse(column);
            foreach (var item in GetVisible(column))
                visible.Add(item);
        }

        var result = visible.Count;
        return result;

        IEnumerable<Coords> GetVisible(IList<Tree> trees)
        {
            var currentMax = trees[0];
            yield return currentMax.Coords;

            for (int i = 1; i < trees.Count - 1; i++)
            {
                if (trees[i].Height > currentMax.Height)
                {
                    currentMax = trees[i];
                    yield return trees[i].Coords;
                }
            }

            yield return trees[^1].Coords;
        }
    }
}

file record Grid(string[] Data)
{
    public int MaxX = Data.Length;
    public int MaxY = Data[0].Length;
    public char this[int x, int y] => Data[x][y];

    public IEnumerable<Tree> GetRow(int x)
    {
        for (int y = 0; y < MaxY; y++)
        {
            yield return new Tree(this[x, y], new Coords(x, y));
        }
    }

    public IEnumerable<Tree> GetColumn(int y)
    {
        for (int x = 0; x < MaxX; x++)
        {
            yield return new Tree(this[x, y], new Coords(x, y));
        }
    }
}

file record Coords(int x, int y);

file record Tree(char Height, Coords Coords);
