using System.Text.RegularExpressions;

namespace AdventOfCode.Year2024.Day03;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))") == 161);
    }

    public long Solve(string input)
    {
        var regex = new Regex(@"mul\((\d+),(\d+)\)");
        var sum = 0l;
        foreach (Match match in regex.Matches(input))
        {
            var a = int.Parse(match.Groups[1].ValueSpan);
            var b = int.Parse(match.Groups[2].ValueSpan);

            sum += a * b;
        }
        return sum;
    }
}
