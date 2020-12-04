using System;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2020.Day02
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Solve(@"1-3 a: abcde
1-3 b: cdefg
2-9 c: ccccccccc") == "1");
        }

        public string Solve(string input)
        {
            var passwords = input.Split(Environment.NewLine)
                .Select(line =>
                {
                    var parts = line.Split(new[] { '-', ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);
                    return new PasswordWithPolicy(
                        int.Parse(parts[0]),
                        int.Parse(parts[1]),
                        parts[2][0],
                        parts[3]
                    );
                }).ToArray();

            return passwords.Count(p =>
            {
                return p.Password[p.From - 1] == p.Character ^ p.Password[p.To - 1] == p.Character;
            }).ToString();
        }

        record PasswordWithPolicy(int From, int To, char Character, string Password);
    }
}
