using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2020.Day07
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Solve(@"light red bags contain 1 bright white bag, 2 muted yellow bags.
dark orange bags contain 3 bright white bags, 4 muted yellow bags.
bright white bags contain 1 shiny gold bag.
muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.
shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.
dark olive bags contain 3 faded blue bags, 4 dotted black bags.
vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.
faded blue bags contain no other bags.
dotted black bags contain no other bags.") == "4");
        }

        public string Solve(string input)
        {
            var rules = input.Split(Environment.NewLine)
                .Select(rule =>
                {
                    const string separator = " bags contain ";
                    var index = rule.IndexOf(separator);
                    var bag = rule[0..index];
                    var nestedBags = rule[(index + separator.Length)..]
                        .Split(", ", StringSplitOptions.RemoveEmptyEntries)
                        .Where(t => !t.StartsWith("no "))
                        .Select(t => t[2..t.IndexOf(" bag")])
                        .ToArray();

                    return (bag, nestedBags);
                })
                .ToDictionary(t => t.bag, t => t.nestedBags);

            var golds = rules.Where(rule => rule.Value.Contains("shiny gold")).Select(t => t.Key).ToHashSet();

            Solve(golds, rules);
            return golds.Count.ToString();

            void Solve(HashSet<string> bags, Dictionary<string, string[]> rules)
            {
                foreach (var rule in rules)
                {
                    if(rule.Value.Any(t => bags.Contains(t)) && bags.Add(rule.Key))
                    {
                        Solve(bags, rules);
                    }
                }
            }
        }
    }
}
