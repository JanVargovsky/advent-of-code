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
""") == 14);
    }

    public long Solve(string input)
    {
        var segments = input.Split(Environment.NewLine + Environment.NewLine);
        var ranges = segments[0]
            .Split(Environment.NewLine)
            .Select(ParseRange)
            .OrderBy(t => t.From)
            .ToList();

        var i = 0;
        while (i < ranges.Count - 1)
        {
            if (ranges[i].IsInRange(ranges[i + 1].From) || ranges[i + 1].IsInRange(ranges[i].To))
            {
                ranges[i] = new Range(
                    Math.Min(ranges[i].From, ranges[i + 1].From),
                    Math.Max(ranges[i].To, ranges[i + 1].To));
                ranges.RemoveAt(i + 1);
            }
            else
                i++;
        }

        var result = ranges.Sum(t => t.Count);

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
        public long Count => To - From + 1;
    }
}
