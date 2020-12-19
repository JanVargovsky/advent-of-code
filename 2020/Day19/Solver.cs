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
            Debug.Assert(Solve(@"0: 4 1 5
1: 2 3 | 3 2
2: 4 4 | 5 5
3: 4 5 | 5 4
4: ""a""
5: ""b""

ababbb
bababa
abbbab
aaabbb
aaaabbb") == "2");
        }

        public string Solve(string input)
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

            var result = messages.Count(m => Matches(m, 0, 0) >= m.Length);
            return result.ToString();

            int Matches(string message, int index, int ruleId)
            {
                if (index >= message.Length) return 1;

                var rule = rules[ruleId];
                if (rule.IsTerminal)
                {
                    if (rule.Terminal[0] == message[index])
                        return 1;
                    else
                        return -1;
                }

                foreach (var subRules in rule.SubRules)
                {
                    bool matched = true;
                    var shift = 0;
                    for (int i = 0; i < subRules.Count; i++)
                    {
                        var r = Matches(message, index + shift, subRules[i]);
                        if (r == -1)
                        {
                            matched = false;
                            break;
                        }
                        shift += r;
                    }

                    if (matched)
                        return shift;
                }

                return -1;
            }
        }

        record Rule(int Id, string Terminal, List<List<int>> SubRules)
        {
            public bool IsTerminal => Terminal is not null;
        }
    }
}
