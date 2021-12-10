namespace AdventOfCode.Year2021.Day10;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]") == "288957");
    }

    public string Solve(string input)
    {
        var items = input.Split(Environment.NewLine);
        var mapping = new Dictionary<char, char>()
        {
            [')'] = '(',
            [']'] = '[',
            ['}'] = '{',
            ['>'] = '<',
        };

        var results = items.Select(Validate).Where(t => t != 0).OrderBy(t => t).ToArray();
        var result = results[results.Length / 2];

        return result.ToString();

        long Validate(string line)
        {
            var s = new Stack<char>();

            foreach (var c in line)
            {
                if (mapping.TryGetValue(c, out var start))
                {
                    var lastStart = s.Pop();
                    if (start != lastStart)
                    {
                        return 0;
                    }
                }
                else
                {
                    s.Push(c);
                }
            }

            var score = 0L;

            foreach (var c in s)
            {
                var p = c switch
                {
                    '(' => 1,
                    '[' => 2,
                    '{' => 3,
                    '<' => 4,
                };
                score *= 5;
                score += p;
            }

            return score;
        }
    }
}