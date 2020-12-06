using System;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2020.Day06
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Solve(@"abc

a
b
c

ab
ac

a
a
a
a

b") == "11");
        }

        public string Solve(string input)
        {
            var answers = input.Split(Environment.NewLine + Environment.NewLine)
                .Select(group => group.Split(Environment.NewLine))
                .ToArray();

            return answers.Sum(group => group.SelectMany(t => t.ToCharArray()).Distinct().Count())
                .ToString();
        }
    }
}
