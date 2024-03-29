﻿namespace AdventOfCode.Year2022.Day08;

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
""") == 8);
    }

    public int Solve(string input)
    {
        var grid = new Grid(input.Split(Environment.NewLine));

        var scenicScore = 0;
        for (int x = 1; x < grid.MaxX - 1; x++)
            for (int y = 1; y < grid.MaxY - 1; y++)
            {
                scenicScore = Math.Max(scenicScore, ScenicScore(x, y));
            }

        return scenicScore;

        int ScenicScore(int x, int y)
        {
            var row = grid.GetRow(x).ToArray();
            var column = grid.GetColumn(y).ToArray();

            var left = row[..(y + 1)].Reverse().ToArray();
            var right = row[y..];
            var top = column[..(x + 1)].Reverse().ToArray();
            var bottom = column[x..];

            var leftVisible = MeasureViewingDistance(left);
            var rightVisible = MeasureViewingDistance(right);
            var topVisible = MeasureViewingDistance(top);
            var bottomVisible = MeasureViewingDistance(bottom);

            return leftVisible * rightVisible * topVisible * bottomVisible;
        }

        int MeasureViewingDistance(IList<Tree> trees)
        {
            var me = trees[0];

            for (int i = 1; i < trees.Count; i++)
            {
                if (trees[i].Height >= me.Height)
                    return i;
            }

            return trees.Count - 1;
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
