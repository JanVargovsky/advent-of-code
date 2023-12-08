using MoreLinq;

namespace AdventOfCode.Year2023.Day08;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
LR

11A = (11B, XXX)
11B = (XXX, 11Z)
11Z = (11B, XXX)
22A = (22B, XXX)
22B = (22C, 22C)
22C = (22Z, 22Z)
22Z = (22B, 22B)
XXX = (XXX, XXX)
""") == 6);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var instructions = rows[0].Repeat();
        var nodes = rows[2..].Select(Parse).ToDictionary(t => t.From);
        const string Start = "A";
        const string End = "Z";
        var currents = nodes.Keys.Where(t => t.EndsWith(Start));
        var ends = nodes.Keys.Where(t => t.EndsWith(End)).ToHashSet();
        var lengths = currents.Select(CalculateLength).ToArray();
        var results = string.Join(" ", lengths);
        // Use e.g. https://www.calculatorsoup.com/calculators/math/lcm.php
        return 0;

        int CalculateLength(string node)
        {
            var i = 0;
            var current = node;
            foreach (var instruction in instructions)
            {
                if (ends.Contains(current))
                    return i;
                current = instruction switch
                {
                    'L' => nodes[current].Left,
                    'R' => nodes[current].Right
                };
                i++;
            }

            throw new ItWontHappenException();
        }

        Node Parse(string row)
        {
            var tokens = row.Split([" = (", ", ", ")"], StringSplitOptions.None);
            return new(tokens[0], tokens[1], tokens[2]);
        }
    }

    private record Node(string From, string Left, string Right);
}
