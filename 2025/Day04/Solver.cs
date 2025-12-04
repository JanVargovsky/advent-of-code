using System.Text;

namespace AdventOfCode.Year2025.Day04;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
..@@.@@@@.
@@@.@.@.@@
@@@@@.@.@@
@.@@@@..@.
@@.@@@@.@@
.@@@@@@@.@
.@.@.@.@@@
@.@@@.@@@@
.@@@@@@@@.
@.@.@@@.@.
""") == 43);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var result = 0;
        var wasRemoved = false;

        do
        {
            wasRemoved = false;
            var newRows = rows.ToArray();

            for (int y = 0; y < rows.Length; y++)
            {
                for (int x = 0; x < rows[y].Length; x++)
                {
                    const char rollOfPaper = '@';
                    if (rows[y][x] != rollOfPaper) continue;

                    var adjacent = Adjacent(x, y);
                    var count = adjacent.Count(t => t == rollOfPaper);
                    if (count < 4)
                    {
                        wasRemoved = true;
                        result++;

                        const char removed = 'x';

                        newRows[y] = new StringBuilder(newRows[y].Length)
                            .Append(newRows[y][..x])
                            .Append(removed)
                            .Append(newRows[y][(x + 1)..])
                            .ToString();
                    }
                }
            }

            rows = newRows;
        } while (wasRemoved);

        return result;

        IEnumerable<char> Adjacent(int midX, int midY)
        {
            for (var yi = -1; yi <= 1; yi++)
            {
                var y = midY + yi;

                if (y < 0 || y >= rows.Length) continue;

                for (var xi = -1; xi <= 1; xi++)
                {
                    if (yi == 0 && xi == 0) continue;

                    var x = midX + xi;

                    if (x < 0 || x >= rows[y].Length) continue;

                    yield return rows[y][x];
                }
            }
        }
    }
}
