using System;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2020.Day13
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Solve(@"939
7,13,x,x,59,x,31,19") == "1068781");
            Debug.Assert(Solve(@"
17,x,13,19") == "3417");
            Debug.Assert(Solve(@"
67,7,59,61") == "754018");
            Debug.Assert(Solve(@"
67,x,7,59,61") == "779210");
            Debug.Assert(Solve(@"
67,7,x,59,61") == "1261476");
            Debug.Assert(Solve(@"
1789,37,47,1889") == "1202161486");
        }

        public string Solve(string input)
        {
            var buses = input.Split(new[] { Environment.NewLine, "," }, StringSplitOptions.None)[1..]
                .Select((id, i) => (id, i))
                .Where(t => t.id != "x")
                .Select(t => (Id: int.Parse(t.id), Index: t.i))
                .ToArray();

            long timestamp = buses[0].Id;
            long lcm = buses[0].Id;

            for (int i = 1; i < buses.Length; i++)
            {
                while ((timestamp + buses[i].Index) % buses[i].Id != 0)
                {
                    timestamp += lcm;
                }
                lcm *= buses[i].Id;
            }

            return timestamp.ToString();
        }
    }
}
