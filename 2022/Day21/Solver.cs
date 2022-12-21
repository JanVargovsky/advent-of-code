namespace AdventOfCode.Year2022.Day21;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
root: pppw + sjmn
dbpl: 5
cczh: sllz + lgvd
zczc: 2
ptdq: humn - dvpt
dvpt: 3
lfqf: 4
humn: 5
ljgn: 2
sjmn: drzm * dbpl
sllz: 4
pppw: cczh / lfqf
lgvd: ljgn * ptdq
drzm: hmdt - zczc
hmdt: 32
""") == 152);
    }

    public long Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var results = new Dictionary<string, long>();
        var calculations = new Dictionary<string, Calculation>();
        foreach (var row in rows)
        {
            var name = row[..4];
            var tokens = row[6..].Split(' ');
            if (tokens.Length == 1)
            {
                results.Add(name, long.Parse(tokens[0]));
            }
            else
            {
                calculations.Add(name, new(tokens[0], tokens[2], tokens[1][0]));
            }
        }

        var expressionResults = results.ToDictionary(t => t.Key, t => (object)t.Value);
        expressionResults["humn"] = "x";
        var (a, b, _) = calculations["root"];
        var left = ToExpression(a);
        var right = ToExpression(b);

        Console.WriteLine($"{left} = {right}");

        var result = Calculate("root");
        return result;

        object ToExpression(string name)
        {
            if (expressionResults.TryGetValue(name, out var result))
                return result;

            var calculation = calculations[name];
            var aExpression = ToExpression(calculation.A);
            var bExpression = ToExpression(calculation.B);

            if (aExpression is long a && bExpression is long b)
                result = calculation.Operation switch
                {
                    '+' => a + b,
                    '-' => a - b,
                    '*' => a * b,
                    '/' => a / b,
                    _ => throw new InvalidOperationException(),
                };
            else
            {
                result = $"({aExpression}){calculation.Operation}({bExpression})";
            }
            expressionResults[name] = result;
            return result;
        }

        long Calculate(string name)
        {
            if (results.TryGetValue(name, out var result))
                return result;

            var calculation = calculations[name];
            var a = Calculate(calculation.A);
            var b = Calculate(calculation.B);
            result = calculation.Operation switch
            {
                '+' => a + b,
                '-' => a - b,
                '*' => a * b,
                '/' => a / b,
                _ => throw new InvalidOperationException(),
            };
            results[name] = result;
            return result;
        }
    }
}

internal record Calculation(string A, string B, char Operation);
