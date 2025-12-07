namespace AdventOfCode.Year2025.Day06;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
123 328  51 64
 45 64  387 23
  6 98  215 314
*   +   *   +
""") == 4277556);
    }

    public long Solve(string input)
    {
        var rows = input.Split(Environment.NewLine).Select(t => t.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToArray();
        var numbers = rows[..^1].Select(t => t.Select(long.Parse).ToArray()).ToArray();
        var operators = rows[^1].ToArray();

        var total = 0L;
        for (int c = 0; c < operators.Length; c++)
        {
            var op = operators[c];
            var result = op == "*" ? 1L : 0L;

            for (int r = 0; r < numbers.Length; r++)
            {
                var value = numbers[r][c];
                if (op == "+")
                    result += value;
                else
                    result *= value;
            }

            total += result;
        }

        return total;
    }
}
