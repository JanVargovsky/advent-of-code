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
<{([{{}}[<[[[<>{}]]]>[]]") == "26397");
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

        var result = items.Sum(Validate);

        return result.ToString();

        int Validate(string line)
        {
            var s = new Stack<char>();

            foreach (var c in line)
            {
                if (mapping.TryGetValue(c, out var start))
                {
                    var lastStart = s.Pop();
                    if (start != lastStart)
                    {
                        return start switch
                        {
                            '(' => 3,
                            '[' => 57,
                            '{' => 1197,
                            '<' => 25137,
                        };
                    }
                }
                else
                {
                    s.Push(c);
                }
            }

            return 0;
        }
    }
}