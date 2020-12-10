using System;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2020.Day10
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Solve(@"16
10
15
5
1
11
7
19
6
12
4") == "8");

            Debug.Assert(Solve(@"28
33
18
42
31
14
46
20
48
47
24
23
49
45
19
38
39
11
1
32
25
35
8
17
7
9
4
2
34
10
3") == "19208");
        }

        public string Solve(string input)
        {
            var data = input.Split(Environment.NewLine)
                .Select(int.Parse)
                .OrderBy(t => t)
                .ToList();

            data.Insert(0, 0);
            data.Add(data[^1] + 3);

            var counts = Enumerable.Repeat(0L, data.Count).ToList();
            counts[0] = 1;

            for (int i = 1; i < counts.Count; i++)
            {
                for (int j = Math.Max(i - 3, 0); j < i; j++)
                {
                    if (data[i] - data[j] <= 3)
                    {
                        counts[i] += counts[j];
                    }
                }
            }

            return counts[^1].ToString();
        }
    }
}
