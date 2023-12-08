using System.Numerics;
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

    public long Solve(string input)
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
        var result = FindLCM(lengths);
        return result;

        long CalculateLength(string node)
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

    private static TNumber GCD<TNumber>(TNumber a, TNumber b) where TNumber : INumber<TNumber>
    {
        var zero = TNumber.Zero;
        while (b != zero)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    private static TNumber LCM<TNumber>(TNumber a, TNumber b) where TNumber : INumber<TNumber>
    {
        return (a / GCD(a, b)) * b;
    }

    private static TNumber FindLCM<TNumber>(TNumber[] numbers) where TNumber : INumber<TNumber>
    {
        TNumber result = numbers[0];
        for (var i = 1; i < numbers.Length; i++)
        {
            result = LCM(result, numbers[i]);
        }
        return result;
    }

    private record Node(string From, string Left, string Right);
}
