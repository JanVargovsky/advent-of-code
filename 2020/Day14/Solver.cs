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
            Debug.Assert(Solve(@"mask = 000000000000000000000000000000X1001X
mem[42] = 100
mask = 00000000000000000000000000000000X0XX
mem[26] = 1") == "208");
        }

        public string Solve(string input)
        {
            var data = input.Split(Environment.NewLine);

            var mask = string.Empty;
            var mem = new Dictionary<long, long>();

            foreach (var instruction in data)
            {
                if (instruction.StartsWith("mask"))
                    mask = instruction[7..];
                else
                {
                    var index = long.Parse(instruction[4..instruction.IndexOf("]")]);
                    var value = long.Parse(instruction[(instruction.IndexOf("=") + 2)..]);

                    for (int i = 0; i < mask.Length; i++)
                        if (mask[^(i + 1)] == '1')
                            index |= (1L << i);

                    mem[index] = value;
                    var indices = new List<long>
                    {
                        index
                    };

                    for (int i = 0; i < mask.Length; i++)
                        if (mask[^(i + 1)] == 'X')
                        {
                            var len = indices.Count;
                            for (int j = 0; j < len; j++)
                            {
                                var copy = indices[j];

                                // 0
                                copy &= ~(1L << i);
                                mem[copy] = value;
                                indices[j] = copy;

                                // 1
                                copy |= (1L << i);
                                mem[copy] = value;
                                indices.Add(copy);
                            }
                        }
                }
            }
            var result = mem.Values.Sum();
            return result.ToString();
        }
    }
}
