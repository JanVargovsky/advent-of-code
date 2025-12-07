using System.Text;

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
""") == 3263827);
    }

    public long Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var numbers = rows[..^1].ToArray();
        var operators = rows[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var columns = rows.Max(t => t.Length);
        var c = 0;
        var op = 'x';
        var values = new List<long>();
        var problems = new List<Problem>();

        while (c < columns)
        {
            var column = GetColumn(c);
            if (column[^1] != ' ')
            {
                if (values.Count > 0)
                    problems.Add(new Problem(op, values));
                op = column[^1];
                values = [];
            }

            var value = column[..^1].Trim();
            if (value != string.Empty)
                values.Add(long.Parse(value));
            c++;
        }
        if (values.Count > 0)
            problems.Add(new Problem(op, values));

        var total = 0L;
        foreach (var problem in problems)
        {
            var result = problem.Operator == '*' ?
                    problem.Values.Aggregate(1L, (acc, value) => acc *= value) :
                    problem.Values.Sum();
            total += result;
        }

        return total;

        string GetColumn(int c)
        {
            StringBuilder sb = new();
            foreach (var row in rows)
            {
                if (c < row.Length)
                    sb.Append(row[c]);
                else
                    sb.Append(' ');
            }

            return sb.ToString();
        }
    }

    record Problem(char Operator, List<long> Values);
}
