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
4") == "35");

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
3") == "220");
        }

        public string Solve(string input)
        {
            var data = input.Split(Environment.NewLine)
                .Select(int.Parse)
                .OrderBy(t => t)
                .ToList();

            if (data[0] != 0)
                data.Insert(0, 0);
            data.Add(data[^1] + 3);

            var diff1 = 0;
            var diff3 = 0;

            for (int i = 0; i < data.Count - 1; i++)
            {
                var diff = data[i + 1] - data[i];

                if (diff == 1)
                {
                    diff1++;
                }
                else if (diff == 3)
                {
                    diff3++;
                }
            }


            return (diff1 * diff3).ToString();
        }
    }
}
