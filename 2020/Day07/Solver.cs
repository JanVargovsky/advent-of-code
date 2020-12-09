using System;
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
dotted black bags contain no other bags.") == "32");

            Debug.Assert(Solve(@"shiny gold bags contain 2 dark red bags.
dark red bags contain 2 dark orange bags.
dark orange bags contain 2 dark yellow bags.
dark yellow bags contain 2 dark green bags.
dark green bags contain 2 dark blue bags.
dark blue bags contain 2 dark violet bags.
dark violet bags contain no other bags.") == "126");
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
                        .Select(t => (BagCount: int.Parse(t[..1]), Name: t[2..t.IndexOf(" bag")]))
                        .ToArray();

                    return (bag, nestedBags);
                })
                .ToDictionary(t => t.bag, t => t.nestedBags);

            var result = Solve("shiny gold") - 1;
            return result.ToString();

            int Solve(string name)
            {
                return 1 + rules[name].Sum(t => t.BagCount * Solve(t.Name));
            }
        }
    }
}
