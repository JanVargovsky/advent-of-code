namespace AdventOfCode.Year2025.Day05;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
3-5
10-14
16-20
12-18

1
5
8
11
17
32
""") == 3);
    }

    public int Solve(string input)
    {
        var segments = input.Split(Environment.NewLine + Environment.NewLine);
        var ranges = segments[0]
            .Split(Environment.NewLine)
            .Select(ParseRange)
            .OrderBy(t => t.From)
            .ToArray();

        var ids = segments[1]
            .Split(Environment.NewLine)
            .Select(long.Parse);

        var result = ids.Count(id => ranges.Any(r => r.IsInRange(id)));

        return result;

        Range ParseRange(string s)
        {
            var tokens = s.Split('-');
            return new(long.Parse(tokens[0]), long.Parse(tokens[1]));
        }
    }

    record class Range(long From, long To)
    {
        public bool IsInRange(long x) => x >= From && x <= To;
    }
}
