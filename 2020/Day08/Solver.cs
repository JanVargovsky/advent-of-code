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
acc +6") == "8");
        }

        public string Solve(string input)
        {
            var originalProgram = input.Split(Environment.NewLine)
                .Select(line => (Instruction: line[..3], Argument: int.Parse(line[3..])))
                .ToArray();

            for (int i = 0; i < originalProgram.Length; i++)
            {
                foreach (var (from, to) in new[] { ("jmp", "nop"), ("nop", "jmp") })
                {
                    var program = originalProgram.ToArray();
                    if (program[i].Instruction == from)
                    {
                        program[i] = (to, program[i].Argument);
                    }

                    var acc = 0;
                    var ip = 0;
                    var executed = new HashSet<int>();
                    while (ip < program.Length && executed.Add(ip))
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

                    if (ip == program.Length)
                        return acc.ToString();
                }
            }

            throw new Exception();
        }
    }
}
