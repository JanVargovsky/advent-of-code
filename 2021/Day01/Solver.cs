namespace AdventOfCode.Year2021.Day01;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"199
200
208
210
200
207
240
269
260
263") == "5");
    }

    public string Solve(string input)
    {
        var data = input.Split(Environment.NewLine).Select(int.Parse).ToArray();
        var increased = 0;
        const int windowSize = 3;

        for (int i = windowSize; i < data.Length; i++)
        {
            var window1 = data[(i - windowSize)..i].Sum();
            var window2 = data[(i - windowSize + 1)..(i + 1)].Sum();
            if (window1 < window2)
                increased++;
        }

        var result = increased.ToString();
        return result;
    }

    public string SolveOneLiner(string input)
    {
        return input
            .Split(Environment.NewLine)
            .Select(int.Parse)
            .Window(3)
            .Select(t => t.Sum())
            .Pairwise((a, b) => a < b)
            .Count(t => t)
            .ToString();
    }
}
