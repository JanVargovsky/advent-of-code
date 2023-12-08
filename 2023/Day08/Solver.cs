using MoreLinq;

namespace AdventOfCode.Year2023.Day08;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
RL

AAA = (BBB, CCC)
BBB = (DDD, EEE)
CCC = (ZZZ, GGG)
DDD = (DDD, DDD)
EEE = (EEE, EEE)
GGG = (GGG, GGG)
ZZZ = (ZZZ, ZZZ)
""") == 2);
        Debug.Assert(Solve("""
LLR

AAA = (BBB, BBB)
BBB = (AAA, ZZZ)
ZZZ = (ZZZ, ZZZ)
""") == 6);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var instructions = rows[0].Repeat();
        var nodes = rows[2..].Select(Parse).ToDictionary(t => t.From);
        const string Start = "AAA";
        const string End = "ZZZ";
        var i = 0;
        var current = Start;
        foreach (var instruction in instructions)
        {
            if (current == End)
                break;
            current = instruction switch
            {
                'L' => nodes[current].Left,
                'R' => nodes[current].Right
            };
            i++;
        }
        var result = i;
        return result;

        Node Parse(string row)
        {
            var tokens = row.Split([" = (", ", ", ")"], StringSplitOptions.None);
            return new(tokens[0], tokens[1], tokens[2]);
        }
    }

    private record Node(string From, string Left, string Right);
}
