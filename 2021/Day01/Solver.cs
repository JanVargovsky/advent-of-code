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
263") == "7");
    }

    public string Solve(string input)
    {
        var data = input.Split(Environment.NewLine).Select(int.Parse).ToArray();
        var increased = 0;

        for (int i = 1; i < data.Length; i++)
        {
            if (data[i] > data[i - 1])
                increased++;
        }

        var result = increased.ToString();
        return result;
    }
}
