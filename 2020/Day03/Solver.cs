using System;

namespace AdventOfCode.Year2020.Day03
{
    class Solver
    {
        public string Solve(string input)
        {
            var array = input.Split(Environment.NewLine);

            var slopes = new[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) };
            var result = 1ul;
            foreach (var (rightSlope, downSlope) in slopes)
            {
                var right = rightSlope;
                var down = downSlope;
                var trees = 0ul;
                while (down < array.Length)
                {
                    trees += array[down][right] == '#' ? 1ul : 0ul;

                    right += rightSlope;
                    right %= array[0].Length;
                    down += downSlope;
                }

                result *= trees;
            }

            return result.ToString();
        }
    }
}
