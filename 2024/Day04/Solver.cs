namespace AdventOfCode.Year2024.Day04;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
1.2
.A.
3.4
""") == 0);

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
""") == 9);
    }

    public int Solve(string input)
    {
        var map = input.Split(Environment.NewLine);
        var count = 0;

        for (int y = 0; y < map.Length; y++)
            for (int x = 0; x < map[y].Length; x++)
            {
                if (Check(x, y))
                    count++;
            }

        return count;

        bool Check(int x, int y)
        {
            if (map[y][x] != 'A')
                return false;

            if (!IsInRange(x - 1, y - 1) || !IsInRange(x - 1, y + 1) || !IsInRange(x + 1, y - 1) || !IsInRange(x + 1, y + 1))
                return false;

            var a = map[y - 1][x - 1];
            var b = map[y - 1][x + 1];
            var c = map[y + 1][x - 1];
            var d = map[y + 1][x + 1];

            return $"{a}{d}" is "MS" or "SM" && $"{b}{c}" is "MS" or "SM";
        }

        bool IsInRange(int x, int y) => x >= 0 && y >= 0 && y < map.Length && x < map[y].Length;
    }
}
