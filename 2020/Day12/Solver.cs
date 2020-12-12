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
F11") == "25");
        }

        public string Solve(string input)
        {
            var instructions = input.Split(Environment.NewLine)
                .Select(t => (Action: t[0], Value: int.Parse(t[1..])))
                .ToArray();

            //  N
            // W E
            //  S
            var direction = 1; // NESW
            var east = 0;
            var north = 0;

            foreach (var instruction in instructions)
            {
                var (action, value) = instruction;

                if (action == 'F')
                {
                    if (direction % 4 == 0) action = 'N';
                    else if (direction % 4 == 1) action = 'E';
                    else if (direction % 4 == 2) action = 'S';
                    else if (direction % 4 == 3) action = 'W';
                }

                if (action == 'N') north += value;
                else if (action == 'S') north -= value;
                else if (action == 'E') east += value;
                else if (action == 'W') east -= value;
                else if (action == 'L') direction -= (value / 90);
                else if (action == 'R') direction += (value / 90);

                Console.WriteLine($"east {east}, north {north}");
            }

            var result = Math.Abs(east) + Math.Abs(north);
            return result.ToString();
        }
    }
}
