using System;
using System.Linq;
using MoreLinq;

namespace AdventOfCode.Year2020.Day01
{
    class Solver
    {
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
