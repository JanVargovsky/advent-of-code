using MoreLinq;

namespace AdventOfCode.Year2025.Day03;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
987654321111111
811111111111119
234234234234278
818181911112111
""") == 357);
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
        const char CharOffset = '0';
        return input.Subsets(2)
            .Select(items => items.Aggregate(0L, (a, c) => a * 10 + c - CharOffset))
            .Max();
    }
}
