namespace AdventOfCode.Year2025.Day03;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(MaximumJoltage("987654321111111") == 987654321111);
        Debug.Assert(MaximumJoltage("811111111111119") == 811111111119);
        Debug.Assert(MaximumJoltage("234234234234278") == 434234234278);
        Debug.Assert(MaximumJoltage("818181911112111") == 888911112111);

        Debug.Assert(Solve("""
987654321111111
811111111111119
234234234234278
818181911112111
""") == 3121910778619);
    }

    public long Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var sum = 0L;
        foreach (var row in rows)
        {
            var result = MaximumJoltage(row);
            sum += result;
        }

        return sum;
    }

    private long MaximumJoltage(string input)
    {
        const int Length = 12;
        const char CharOffset = '0';
        var result = 0L;
        var last = -1;

        for (var i = 0; i < Length; i++)
        {
            var tmp = input[(last + 1)..(input.Length - Length + i + 1)]
                .Select((c, i) => (Digit: c - CharOffset, Index: i + last + 1))
                .MaxBy(t => t.Digit);

            result *= 10;
            result += tmp.Digit;
            last = tmp.Index;
        }

        return result;
    }
}
