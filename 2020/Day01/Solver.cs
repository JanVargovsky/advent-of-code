using System;
using System.Linq;

namespace AdventOfCode.Year2020.Day01
{
    class Solver
    {
        public string Solve(string input)
        {
            var array = input.Split(Environment.NewLine).Select(int.Parse).ToArray();

            foreach (var item1 in array)
            {
                foreach (var item2 in array)
                {
                    if (item1 + item2 == 2020)
                        return (item1 * item2).ToString();
                }
            }


            throw new NotImplementedException();
        }
    }
}
