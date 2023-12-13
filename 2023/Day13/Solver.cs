namespace AdventOfCode.Year2023.Day13;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Encode("..") == 0);
        Debug.Assert(Encode("##") == 3);
        Debug.Assert(Encode(".#.#") == 10);

        Debug.Assert(Solve("""
#.##..##.
..#.##.#.
##......#
##......#
..#.##.#.
..##..##.
#.#.##.#.

#...##..#
#....#..#
..##..###
#####.##.
#####.##.
..##..###
#....#..#
""") == 405);
    }

    public int Solve(string input)
    {
        var patterns = input.Split(Environment.NewLine + Environment.NewLine);
        var reflections = patterns.Select(Find);
        var result = reflections.Sum();
        return result;
    }

    private int Find(string pattern)
    {
        var rows = pattern.Split(Environment.NewLine);
        var encodedRows = rows.Select(Encode).ToArray();
        var encodedColumns = Enumerable.Range(0, rows[0].Length).Select(SelectColumn).Select(Encode).ToArray();

        for (int r = 0; r < encodedRows.Length - 1; r++)
        {
            if (Compare(encodedRows, r))
                return (r + 1) * 100;
        }

        for (int c = 0; c < encodedColumns.Length - 1; c++)
        {
            if (Compare(encodedColumns, c))
                return c + 1;
        }

        throw new ItWontHappenException();

        bool Compare(int[] values, int mid)
        {
            var left = values.Take(mid + 1).Reverse();
            var right = values.Skip(mid + 1);
            var same = left.Zip(right).All(t => t.First == t.Second);
            return same;
        }

        IEnumerable<char> SelectColumn(int i) => Enumerable.Range(0, rows.Length).Select(r => rows[r][i]);
    }

    private int Encode(IEnumerable<char> values)
    {
        int result = 0;
        var bit = 1;

        foreach (var item in values)
        {
            if (item == '#')
                result |= bit;
            bit <<= 1;
        }

        return result;
    }
}
