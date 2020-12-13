using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.Year2020.Day13
{
    class SolverV2
    {
        public SolverV2()
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

            var data = buses.Select(bus => (bus.Id - bus.Index % bus.Id, bus.Id)).ToArray();

            var timestamp = ChineseRemainderTheorem(data);
            return timestamp.ToString();

            long ChineseRemainderTheorem((int Remainder, int Modulo)[] data)
            {
                var M = 1L;
                foreach (var (_, modulo) in data)
                {
                    M *= modulo;
                }

                var result = 0L;
                foreach (var (remainder, modulo) in data)
                {
                    var mi = M / modulo;
                    var yi = (long)BigInteger.ModPow(mi, modulo - 2, modulo); // Modular multiplicative inverse
                    result += remainder * mi * yi;
                    result %= M;
                }
                return result;
            }
        }
    }
}
