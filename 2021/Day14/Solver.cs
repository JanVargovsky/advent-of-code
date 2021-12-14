namespace AdventOfCode.Year2021.Day14;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"NNCB

CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C") == "1588");
    }

    public string Solve(string input)
    {
        var items = input.Split(Environment.NewLine);
        var template = new LinkedList<char>(items[0]);
        var rules = new Dictionary<string, char>(items[2..].Select(rule =>
        {
            var index = rule.IndexOf(" -> ");
            return new KeyValuePair<string, char>(rule[..index], rule[^1]);
        }));

        for (int i = 0; i < 10; i++)
        {
            ApplyStep();
        }

        var counts = template.GroupBy(t => t).ToDictionary(t => t.Key, t => t.Count());
        var mostCommon = counts.MaxBy(t => t.Value);
        var leastCommon = counts.MinBy(t => t.Value);

        var result = mostCommon.Value - leastCommon.Value;
        return result.ToString();

        void ApplyStep()
        {
            var current = template.First;
            while (current != null && current.Next != null)
            {
                var value = $"{current.Value}{current.Next.Value}";
                var next = current.Next;
                var rule = rules[value];
                template.AddAfter(current, rule);
                current = next;
            }
        }
    }
}
