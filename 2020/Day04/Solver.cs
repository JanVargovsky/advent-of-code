using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2020.Day04
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Solve(@"eyr:1972 cid:100
hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926

iyr:2019
hcl:#602927 eyr:1967 hgt:170cm
ecl:grn pid:012533040 byr:1946

hcl:dab227 iyr:2012
ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277

hgt:59cm ecl:zzz
eyr:2038 hcl:74454a iyr:2023
pid:3556412378 byr:2007") == "0");

            Debug.Assert(Solve(@"pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980
hcl:#623a2f

eyr:2029 ecl:blu cid:129 byr:1989
iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm

hcl:#888785
hgt:164cm byr:2001 iyr:2015 cid:88
pid:545766238 ecl:hzl
eyr:2022

iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719") == "4");
        }

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
