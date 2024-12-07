namespace AdventOfCode.Year2024.Day01;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
3   4
4   3
2   5
1   3
3   9
3   3
""") == 31);
    }

    public long Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var left = new List<long>();
        var right = new List<long>();
        foreach (var row in rows)
        {
            var tokens = row.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            left.Add(long.Parse(tokens[0]));
            right.Add(long.Parse(tokens[1]));
        }
        var grouped = right.GroupBy(t => t).ToDictionary(t => t.Key, t => t.Count());

        var result = left.Sum(t => t * grouped.GetValueOrDefault(t, 0));
        return result;
    }
}
