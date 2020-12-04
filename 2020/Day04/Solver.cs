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

            var result = passports.Count(IsValid);
            return result.ToString();

            bool IsValid(IDictionary<string, string> passport)
            {
                if (passport.Keys.Count == 8 || passport.Keys.Count == 7 && !passport.ContainsKey("cid"))
                {
                    if (!passport.TryGetValue("byr", out var byr) || !IsInRange(byr, 1920, 2002)) return false;
                    if (!passport.TryGetValue("iyr", out var iyr) || !IsInRange(iyr, 2010, 2020)) return false;
                    if (!passport.TryGetValue("eyr", out var eyr) || !IsInRange(eyr, 2020, 2030)) return false;
                    if (!passport.TryGetValue("hgt", out var hgt) || !IsValidHeight(hgt)) return false;
                    if (!passport.TryGetValue("hcl", out var hcl) || !IsValidHairColor(hcl)) return false;
                    if (!passport.TryGetValue("ecl", out var ecl) || !new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Contains(ecl)) return false;
                    if (!passport.TryGetValue("pid", out var pid) || !IsValidPassportID(pid)) return false;

                    return true;
                }
                return false;

                bool IsInRange(string value, int from, int to)
                {
                    var @int = int.Parse(value);
                    return @int >= from && @int <= to;
                }

                bool IsValidHeight(string value)
                {
                    if (value[^2..] == "cm")
                        return IsInRange(value[..^2], 150, 193);
                    else
                        return IsInRange(value[..^2], 59, 76);
                }

                bool IsValidHairColor(string value)
                {
                    if (value[0] != '#') return false;
                    if (value.Length != 7) return false;

                    return value[1..].All(c => char.IsDigit(c) || (c >= 'a' && c <= 'f'));
                }

                bool IsValidPassportID(string value)
                {
                    return value.Length == 9 && value.All(char.IsDigit);
                }
            }
        }
    }
}
