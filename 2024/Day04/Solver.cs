namespace AdventOfCode.Year2024.Day04;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX
""") == 18);
    }

    public int Solve(string input)
    {
        var map = input.Split(Environment.NewLine);
        var count = 0;

        for (int y = 0; y < map.Length; y++)
            for (int x = 0; x < map[y].Length; x++)
                for (int dx = -1; dx <= 1; dx++)
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0)
                            continue;

                        if (Check(x, y, dx, dy))
                            count++;
                    }

        return count;

        bool Check(int x, int y, int dx, int dy)
        {
            if (!IsInRange(x + 3 * dx, y + 3 * dy))
                return false;

            var word = new char[4];
            for (int i = 0; i < word.Length; i++)
            {
                word[i] = map[y + i * dy][x + i * dx];
            }

            return word.SequenceEqual("XMAS");
        }

        bool IsInRange(int x, int y) => x >= 0 && y >= 0 && y < map.Length && x < map[y].Length;
    }
}
