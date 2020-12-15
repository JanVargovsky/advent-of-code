using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2020.Day15
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Solve(@"0,3,6", 2020) == "436");
            Debug.Assert(Solve(@"1,3,2", 2020) == "1");
            Debug.Assert(Solve(@"2,1,3", 2020) == "10");
            Debug.Assert(Solve(@"1,2,3", 2020) == "27");
            Debug.Assert(Solve(@"2,3,1", 2020) == "78");
            Debug.Assert(Solve(@"3,2,1", 2020) == "438");
            Debug.Assert(Solve(@"3,1,2", 2020) == "1836");

            Debug.Assert(Solve(@"0,3,6") == "175594");
            Debug.Assert(Solve(@"1,3,2") == "2578");
            Debug.Assert(Solve(@"2,1,3") == "3544142");
            Debug.Assert(Solve(@"1,2,3") == "261214");
            Debug.Assert(Solve(@"2,3,1") == "6895259");
            Debug.Assert(Solve(@"3,2,1") == "18");
            Debug.Assert(Solve(@"3,1,2") == "362");
        }

        public string Solve(string input, int n = 30000000)
        {
            var numbers = input.Split(',')
                .Select(int.Parse)
                .ToArray();

            Dictionary<int, int> numberToLastTurn = new();
            for (int i = 0; i < numbers.Length; i++)
            {
                numberToLastTurn[numbers[i]] = i + 1;
            }

            var last = numbers[^1];

            for (int turn = numbers.Length; turn < n; turn++)
            {
                var number = 0;
                if (numberToLastTurn.TryGetValue(last, out var lastTurn))
                    number = turn - lastTurn;

                numberToLastTurn[last] = turn;
                last = number;

                //Console.WriteLine($"Turn {turn + 1} = {last}");
            }

            return last.ToString();
        }
    }
}
