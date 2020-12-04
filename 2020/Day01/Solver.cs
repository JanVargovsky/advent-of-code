using System;
using System.Diagnostics;
using System.Linq;
using MoreLinq;

namespace AdventOfCode.Year2020.Day01
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Solve(@"1721
979
366
299
675
1456") == "241861950");
        }

        public string Solve(string input)
        {
            var array = input.Split(Environment.NewLine).Select(int.Parse).ToArray();

            return array
                .Subsets(3)
                .First(t => t.Sum() == 2020)
                .Aggregate((a, b) => a * b)
                .ToString();
        }
    }
}
