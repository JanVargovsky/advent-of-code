namespace AdventOfCode.Year2021.Day07;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"16,1,2,0,4,2,7,1,2,14") == "37");
    }

    public string Solve(string input)
    {
        var positions = input.Split(',').Select(long.Parse).ToArray();

        var left = positions.Min();
        var right = positions.Max();
        var min = long.MaxValue;

        for (var i = left; i <= right; i++)
        {
            min = Math.Min(min, Calculate(i));
        }

        return min.ToString();

        long Calculate(long align) => positions.Sum(t => Math.Abs(align - t));
    }
}