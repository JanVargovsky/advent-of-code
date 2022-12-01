namespace AdventOfCode.Year2022.Day01;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
1000
2000
3000

4000

5000
6000

7000
8000
9000

10000
""") == "24000");
    }

    public string Solve(string input)
    {
        var data = input.Split(Environment.NewLine + Environment.NewLine);
        var sums = data.Select(t => t.Split(Environment.NewLine).Select(long.Parse).Sum());

        var result = sums.Max();
        return result.ToString();
    }
}
