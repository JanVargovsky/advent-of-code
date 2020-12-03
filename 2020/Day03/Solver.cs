using System;
using System.Linq;

namespace AdventOfCode.Year2020.Day03
{
    class Solver
    {
        public string Solve(string input)
        {
            var array = input.Split(Environment.NewLine)
                .ToArray();

            var right = 3;
            var down = 1;
            var trees = 0;

            while (down < array.Length)
            {
                trees += array[down][right] == '#' ? 1 : 0;

                right += 3;
                right %= array[0].Length;
                down += 1;
            }

            return trees.ToString();
        }
    }
}
