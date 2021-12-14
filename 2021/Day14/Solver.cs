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
CN -> C", 10) == "1588");
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
CN -> C") == "2188189693529");
    }

    public string Solve(string input, int steps = 40)
    {
        var items = input.Split(Environment.NewLine);
        var template = items[0];
        var rules = new Dictionary<string, char>(items[2..].Select(rule =>
        {
            var index = rule.IndexOf(" -> ");
            return new KeyValuePair<string, char>(rule[..index], rule[^1]);
        }));

        var counts = new Dictionary<string, ulong>();
        foreach (var (a, b) in template.Zip(template.Skip(1)))
        {
            var key = $"{a}{b}";
            counts[key] = counts.GetValueOrDefault(key) + 1;
        }

        for (int i = 0; i < steps; i++)
        {
            var newCounts = new Dictionary<string, ulong>();
            foreach (var item in counts)
            {
                var value = rules[item.Key];
                var count = item.Value;
                var a = $"{item.Key[0]}{value}";
                var b = $"{value}{item.Key[1]}";
                newCounts[a] = newCounts.GetValueOrDefault(a) + count;
                newCounts[b] = newCounts.GetValueOrDefault(b) + count;
            }
            counts = newCounts;
        }

        var resultCounts = new Dictionary<char, ulong>();
        foreach (var item in counts)
        {
            var c = item.Key[0];
            if (!resultCounts.TryAdd(c, item.Value))
                resultCounts[c] += item.Value;
        }
        resultCounts[template[^1]] += 1;
        var mostCommon = resultCounts.MaxBy(t => t.Value);
        var leastCommon = resultCounts.MinBy(t => t.Value);

        var result = mostCommon.Value - leastCommon.Value;
        return result.ToString();
    }
}
