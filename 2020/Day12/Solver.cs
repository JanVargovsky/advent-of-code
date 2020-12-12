using System;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2020.Day12
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Solve(@"F10
N3
F7
R90
F11") == "286");
        }

        public string Solve(string input)
        {
            var instructions = input.Split(Environment.NewLine)
                .Select(t => (Action: t[0], Value: int.Parse(t[1..])))
                .ToArray();

            //  N
            // W E
            //  S
            var waypointEast = 10;
            var waypointNorth = 1;
            var east = 0;
            var north = 0;

            foreach (var instruction in instructions)
            {
                var (action, value) = instruction;

                if (action == 'F')
                {
                    east += waypointEast * value;
                    north += waypointNorth * value;
                }

                if (action == 'N') waypointNorth += value;
                else if (action == 'S') waypointNorth -= value;
                else if (action == 'E') waypointEast += value;
                else if (action == 'W') waypointEast -= value;
                else if (action == 'L')
                {
                    while ((value -= 90) >= 0)
                        (waypointEast, waypointNorth) = (-waypointNorth, waypointEast);
                }
                else if (action == 'R')
                {
                    // 10 units east and 4 units north
                    // R90
                    // 4 units east and 10 units south = 4 east and -10 north
                    while ((value -= 90) >= 0)
                        (waypointEast, waypointNorth) = (waypointNorth, -waypointEast);
                }

                Console.WriteLine($"east {east}, north {north}");
            }

            var result = Math.Abs(east) + Math.Abs(north);
            return result.ToString();
        }
    }
}
