using System;
using System.Diagnostics;
using System.Linq;
using MoreLinq;

namespace AdventOfCode.Year2020.Day13
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Solve(@"939
7,13,x,x,59,x,31,19") == "295");
        }

        public string Solve(string input)
        {
            var data = input.Split(new[] { Environment.NewLine, "," }, StringSplitOptions.None);
            var estimate = int.Parse(data[0]);
            var buses = data[1..].Where(id => id != "x").Select(int.Parse).ToArray();

            var departs = buses.Select(id => (ID: id, Depart: id - estimate % id));
            var nearestBus = departs.MinBy(t => t.Depart).First();
            var result = nearestBus.ID * nearestBus.Depart;
            return result.ToString();
        }
    }
}
