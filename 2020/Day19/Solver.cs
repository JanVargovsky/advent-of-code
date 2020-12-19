using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2020.Day19
{
    class Solver
    {
        public Solver()
        {
            var input = @"42: 9 14 | 10 1
9: 14 27 | 1 26
10: 23 14 | 28 1
1: ""a""
11: 42 31
5: 1 14 | 15 1
19: 14 1 | 14 14
12: 24 14 | 19 1
16: 15 1 | 14 14
31: 14 17 | 1 13
6: 14 14 | 1 14
2: 1 24 | 14 4
0: 8 11
13: 14 3 | 1 12
15: 1 | 14
17: 14 2 | 1 7
23: 25 1 | 22 14
28: 16 1
4: 1 1
20: 14 14 | 1 15
3: 5 14 | 16 1
27: 1 6 | 14 18
14: ""b""
21: 14 1 | 1 14
25: 1 1 | 1 14
22: 14 14
8: 42
26: 14 22 | 1 20
18: 15 15
7: 14 5 | 1 21
24: 14 1

abbbbbabbbaaaababbaabbbbabababbbabbbbbbabaaaa
bbabbbbaabaabba
babbbbaabbbbbabbbbbbaabaaabaaa
aaabbbbbbaaaabaababaabababbabaaabbababababaaa
bbbbbbbaaaabbbbaaabbabaaa
bbbababbbbaaaaaaaabbababaaababaabab
ababaaaaaabaaab
ababaaaaabbbaba
baabbaaaabbaaaababbaababb
abbbbabbbbaaaababbbbbbaaaababb
aaaaabbaabaaaaababaa
aaaabbaaaabbaaa
aaaabbaabbaaaaaaabbbabbbaaabbaabaaa
babaaabbbaaabaababbaabababaaab
aabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba";
            Debug.Assert(Solve(input, false) == "3");
            Debug.Assert(Solve(input) == "12");
        }

        public string Solve(string input, bool updateRules = true)
        {
            var data = input.Split(Environment.NewLine + Environment.NewLine);

            var rules = data[0].Split(Environment.NewLine)
                .Select(t =>
                {
                    var index = t.IndexOf(':');
                    var id = int.Parse(t[..index]);
                    var rawRule = t[(index + 2)..];

                    if (rawRule.StartsWith("\""))
                    {
                        return new Rule(id, rawRule[1..^1], null);
                    }
                    var subRules = rawRule.Split('|')
                        .Select(tt => tt.Trim(' ').Split(' ').Select(int.Parse).ToList())
                        .ToList();

                    return new Rule(id, null, subRules);
                })
                .ToDictionary(t => t.Id);
            var messages = data[1].Split(Environment.NewLine);

            if (updateRules)
            {
                rules[8] = new Rule(8, null, new()
                {
                    new() { 42 },
                    new() { 42, 8 }
                });
                rules[11] = new Rule(11, null, new()
                {
                    new() { 42, 31 },
                    new() { 42, 11, 31 }
                });
            }

            var matchedMessages = messages
                .Where(m => Matches(m, 0))
                .ToList();

            return matchedMessages.Count.ToString();

            bool Matches(string message, params int[] ruleIds)
            {
                if (ruleIds.Length == 0) return message.Length == 0;
                if (message.Length == 0) return false;

                var rule = rules[ruleIds[0]];
                if (rule.IsTerminal)
                {
                    return rule.Terminal[0] == message[0] && Matches(message[1..], ruleIds[1..]);
                }

                return rule.SubRules.Any(subRule => Matches(message, subRule.Concat(ruleIds.Skip(1)).ToArray()));
            }
        }

        record Rule(int Id, string Terminal, List<List<int>> SubRules)
        {
            public bool IsTerminal => Terminal is not null;
        }
    }
}
