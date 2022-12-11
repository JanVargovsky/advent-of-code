namespace AdventOfCode.Year2022.Day11;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1
""") == 2713310158);
    }

    public long Solve(string input)
    {
        var segments = input.Split(Environment.NewLine + Environment.NewLine);
        var monkeys = segments.Select(Parse).ToArray();
        var mod = 1;
        foreach (var item in monkeys)
        {
            mod *= item.DivisibleTest;
        }

        for (int i = 0; i < 10000; i++)
        {
            for (int m = 0; m < monkeys.Length; m++)
            {
                var monkey = monkeys[m];
                foreach (var item in monkey.Items)
                {
                    var operationResult = EvaluateOperation(monkey.Operation, item);
                    var newItem = operationResult % mod;
                    var operationTestResult = newItem % monkey.DivisibleTest == 0;
                    monkeys[operationTestResult ? monkey.TestResultTrue : monkey.TestResultFalse].Items.Add(newItem);
                    monkey.Inspects++;
                }

                monkey.Items.Clear();
            }
        }

        var monkeysByInspects = monkeys.OrderByDescending(t => t.Inspects).ToArray();
        var result = monkeysByInspects[0].Inspects * monkeysByInspects[1].Inspects;
        return result;

        long EvaluateOperation(string operationString, long item)
        {
            var tokens = operationString.Split(' ');
            var a = tokens[^3] is "old" ? item : long.Parse(tokens[^3]);
            var operation = tokens[^2];
            var b = tokens[^1] is "old" ? item : long.Parse(tokens[^1]);

            var result = operation switch
            {
                "+" => a + b,
                "*" => a * b,
                _ => throw new ArgumentException()
            };
            return result;
        }
    }

    private Monkey Parse(string segment)
    {
        var rows = segment.Split(Environment.NewLine);
        var items = rows[1][(rows[1].IndexOf(':') + 2)..].Split(", ").Select(long.Parse).ToList();
        var operation = rows[2][(rows[2].LastIndexOf(':') + 2)..];
        var test = int.Parse(rows[3][(rows[3].LastIndexOf(' ') + 1)..]);
        var testTrue = int.Parse(rows[4][(rows[4].LastIndexOf(' ') + 1)..]);
        var testFalse = int.Parse(rows[5][(rows[5].LastIndexOf(' ') + 1)..]);

        return new(items, operation, test, testTrue, testFalse);
    }
}

public record class Monkey(List<long> Items, string Operation, int DivisibleTest, int TestResultTrue, int TestResultFalse)
{
    public long Inspects { get; set; }
}