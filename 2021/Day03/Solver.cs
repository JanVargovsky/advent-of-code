namespace AdventOfCode.Year2021.Day03;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"00100
11110
10110
10111
10101
01111
00111
11100
10000
11001
00010
01010") == "198");
    }

    public string Solve(string input)
    {
        var numbers = input.Split(Environment.NewLine);
        var length = numbers[0].Length;
        var gammaRate = 0;
        var epsilon = 0;

        var ones = new int[length];
        for (int i = 0; i < length; i++)
        {
            foreach (var number in numbers)
            {
                if (number[i] == '1')
                {
                    ones[i]++;
                }
            }
        }

        for (int i = 0; i < length; i++)
        {
            var one = ones[i];
            var zero = numbers.Length - one;

            if (one > zero)
            {
                gammaRate |= (1 << length - i - 1);
            }
            else
            {
                epsilon |= (1 << length - i - 1);
            }
        }

        var result = (gammaRate * epsilon).ToString();
        return result;
    }
}
