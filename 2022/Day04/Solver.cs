namespace AdventOfCode.Year2022.Day04;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8
""") == "4");
    }

    public string Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);

        var result = 0;

        foreach (var row in rows)
        {
            var numbers = row.Split(',', '-').Select(int.Parse).ToArray();
            var a = new Interval(numbers[0], numbers[1]);
            var b = new Interval(numbers[2], numbers[3]);

            if (Overlap(a, b) || Overlap(b, a))
                result++;
        }

        return result.ToString();

        bool Overlap(Interval a, Interval b)
        {
            return b.Start >= a.Start && b.Start <= a.End;
        }
    }
}

file record Interval(int Start, int End);