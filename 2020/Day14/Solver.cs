using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2020.Day14
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Solve(@"mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
mem[8] = 11
mem[7] = 101
mem[8] = 0") == "165");
        }

        public string Solve(string input)
        {
            var data = input.Split(Environment.NewLine);

            var mask = string.Empty;
            var mem = new Dictionary<long, long>();

            foreach (var instruction in data)
            {
                if (instruction.StartsWith("mask"))
                    mask = instruction;
                else
                {
                    var index = long.Parse(instruction[4..instruction.IndexOf("]")]);
                    var value = long.Parse(instruction[(instruction.IndexOf("=") + 2)..]);

                    for (int i = 0; i < mask.Length; i++)
                    {
                        switch (mask[^(i + 1)])
                        {
                            case '1':
                                value |= 1L << i;
                                break;
                            case '0':
                                value &= ~(1L << i);
                                break;
                        }
                    }
                    mem[index] = value;
                }
            }
            var result = mem.Values.Sum();
            return result.ToString();
        }
    }
}
