using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Year2020.Day04
{
    class Solver
    {
        public string Solve(string input)
        {
            var passports = input.Split(Environment.NewLine + Environment.NewLine)
                .Select(line =>
                {
                    var fields = line.Split(new[] { Environment.NewLine, " " }, StringSplitOptions.None);

                    return fields.ToImmutableDictionary(
                        t => t[..3],
                        t => t[4..]);
                })
                .ToArray();

            return passports.Count(IsValid).ToString();

            bool IsValid(IDictionary<string, string> passport)
            {
                if (passport.Keys.Count == 8) return true;
                if (passport.Keys.Count == 7 && !passport.ContainsKey("cid")) return true;
                return false;
            }
        }
    }
}
