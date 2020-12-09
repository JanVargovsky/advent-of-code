using System;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2020.Day09
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Solve(@"35
20
15
25
47
40
62
55
65
95
102
117
150
182
127
219
299
277
309
576", 5) == "127");
        }

        public string Solve(string input, int preambleLength = 25)
        {
            var numbers = input.Split(Environment.NewLine)
                .Select(long.Parse)
                .ToArray();

            for (int i = preambleLength; i < numbers.Length; i++)
            {
                if(!IsValid(numbers[i], i))
                    return numbers[i].ToString();
            }

            throw new Exception();

            bool IsValid(long x, int offset)
            {
                for (int i = 0; i <= preambleLength; i++)
                {
                    for (int j = 1; j <= preambleLength; j++)
                    {
                        if (numbers[offset - i] + numbers[offset - j] == x)
                            return true;
                    }
                }
                return false;
            }
        }
    }
}
