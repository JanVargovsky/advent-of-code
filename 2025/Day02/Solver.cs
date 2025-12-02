namespace AdventOfCode.Year2025.Day02;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(IsInvalid("11"));
        Debug.Assert(!IsInvalid("12"));
        Debug.Assert(IsInvalid("22"));
        Debug.Assert(IsInvalid("999"));
        Debug.Assert(IsInvalid("1010"));
        Debug.Assert(Solve("""
11-22,95-115,998-1012,1188511880-1188511890,222220-222224,
1698522-1698528,446443-446449,38593856-38593862,565653-565659,
824824821-824824827,2121212118-2121212124
""") == 4174379265);
    }

    public long Solve(string input)
    {
        var rows = input.Split(',');
        var result = 0L;
        foreach (var row in rows)
        {
            var index = row.IndexOf('-');
            var start = long.Parse(row[..index]);
            var end = long.Parse(row[(index + 1)..]);
            for (var i = start; i <= end; i++)
            {
                if (IsInvalid(i.ToString()))
                    result += i;
            }
        }
        return result;
    }

    private bool IsInvalid(ReadOnlySpan<char> input)
    {
        for (int i = 1; i <= input.Length / 2; i++)
        {
            var (n, rem) = Math.DivRem(input.Length, i);
            if (rem != 0)
                continue;

            var left = input[..i];
            var invalid = false;
            for (var j = 1; j < n; j++)
            {
                var right = input[(i * j)..(i * (j + 1))];
                if (!left.SequenceEqual(right))
                {
                    invalid = true;
                    break;
                }
            }

            if (!invalid)
                return true;
        }

        return false;
    }
}
