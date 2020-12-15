using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2020.Day15
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Solve(@"0,3,6") == "436");
            Debug.Assert(Solve(@"1,3,2") == "1");
            Debug.Assert(Solve(@"2,1,3") == "10");
            Debug.Assert(Solve(@"1,2,3") == "27");
            Debug.Assert(Solve(@"2,3,1") == "78");
            Debug.Assert(Solve(@"3,2,1") == "438");
            Debug.Assert(Solve(@"3,1,2") == "1836");
        }

        public string Solve(string input)
        {
            var numbers = input.Split(',')
                .Select(int.Parse)
                .ToList();


            while (numbers.Count != 2020)
            {
                var turn = numbers[^1];
                var index = numbers.LastIndexOf(turn, numbers.Count - 2);

                if (index == -1)
                {
                    numbers.Add(0);
                }
                else
                {
                    var index2 = numbers.LastIndexOf(turn);
                    var x = index2 - index;
                    numbers.Add(index2 - index);
                }
                //Console.WriteLine($"Turn {numbers.Count} = {numbers[^1]}");
            }

            var result = numbers[^1];
            return result.ToString();
        }
    }
}
