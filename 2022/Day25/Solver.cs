using System.Text;

namespace AdventOfCode.Year2022.Day25;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
1=-0-2
12111
2=0=
21
2=01
111
20012
112
1=-1=
1-12
12
1=
122
""") == "2=-1=0");
    }

    public string Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var sum = rows.Sum(FromSnafu);
        var result = ToSnafu(sum);
        return result;
    }

    private long FromSnafu(string x)
    {
        var result = 0L;

        for (int i = 0; i < x.Length; i++)
        {
            result *= 5;
            result += (x[i] switch
            {
                '2' => 2,
                '1' => 1,
                '0' => 0,
                '-' => -1,
                '=' => -2,
                _ => throw new ArgumentException()
            });
        }

        return result;
    }

    private string ToSnafu(long x)
    {
        StringBuilder sb = new();

        while (x != 0)
        {
            (x, var r) = Math.DivRem(x + 2, 5);
            sb.Insert(0, (r - 2) switch
            {
                -2 => '=',
                -1 => '-',
                0 => '0',
                1 => '1',
                2 => '2',
            });
        }

        return sb.ToString();
    }
}
