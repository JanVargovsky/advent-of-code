using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2020.Day08
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Solve(@"nop +0
acc +1
jmp +4
acc +3
jmp -3
acc -99
acc +1
jmp -4
acc +6") == "5");
        }

        public string Solve(string input)
        {
            var program = input.Split(Environment.NewLine)
                .Select(line => (line[..3], int.Parse(line[3..])))
                .ToArray();

            var acc = 0;
            var ip = 0;
            var executed = new HashSet<int>();
            while (executed.Add(ip))
            {
                var (instruction, argument) = program[ip];
                switch (instruction)
                {
                    case "acc":
                        acc += argument;
                        ip++;
                        break;
                    case "jmp":
                        ip += argument;
                        break;
                    case "nop":
                        ip++;
                        break;
                }
            }
            return acc.ToString();
        }
    }
}
