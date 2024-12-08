namespace AdventOfCode.Year2024.Day05;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13

75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47
""") == 123);
    }

    public long Solve(string input)
    {
        var segments = input.Split(Environment.NewLine + Environment.NewLine);
        var rules = segments[0].Split(Environment.NewLine).Select(ParseRow).ToArray();
        var updates = segments[1].Split(Environment.NewLine).Select(t => t.Split(',').Select(int.Parse).ToArray());

        var result = 0;
        foreach (var update in updates)
        {
            if (!Check(update, rules))
            {
                while (!Check(update, rules)) ;
                result += update[update.Length / 2];
            }
        }

        return result;

        bool Check(int[] update, PageOrderingRule[] rules)
        {
            for (int i = 0; i < update.Length; i++)
            {
                var relevantRules = rules.Where(t => t.Before == update[i] || t.After == update[i]).ToArray();

                for (var j = i + 1; j < update.Length; j++)
                {
                    foreach (var rule in relevantRules)
                    {
                        if (rule.Before == update[j] && rule.After == update[i])
                        {
                            (update[j], update[i]) = (update[i], update[j]);
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        PageOrderingRule ParseRow(string row)
        {
            var tokens = row.Split('|');
            return new PageOrderingRule(int.Parse(tokens[0]), int.Parse(tokens[1]));
        }
    }

    record PageOrderingRule(int Before, int After);
}
