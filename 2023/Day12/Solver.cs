namespace AdventOfCode.Year2023.Day12;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(ReplaceAt("0123", 1, '.').Equals("0.23"));

        Debug.Assert(Solve("""
???.### 1,1,3
.??..??...?##. 1,1,3
?#?#?#?#?#?#?#? 1,3,1,6
????.#...#... 4,1,1
????.######..#####. 1,6,5
?###???????? 3,2,1
""") == 21);
    }

    public int Solve(string input)
    {
        var springs = input.Split(Environment.NewLine).Select(Parse).ToArray();
        var results = springs.Select(ArrangementsCount);
        var result = results.Sum();
        return result;
    }

    private Spring Parse(string row)
    {
        var tokens = row.Split(' ', ',');
        var numbers = tokens[1..].Select(int.Parse).ToArray();
        return new(tokens[0], numbers);
    }

    private int ArrangementsCount(Spring spring)
    {
        var s = new Stack<string>();
        s.Push(spring.Value);
        var visited = new HashSet<string>();
        var count = 0;

        while (s.TryPop(out var current))
        {
            if (!visited.Add(current))
                continue;

            var index = current.IndexOf('?');
            if (index == -1)
            {
                if (IsValid(current))
                    count++;
                continue;
            }

            s.Push(ReplaceAt(current, index, '.'));
            s.Push(ReplaceAt(current, index, '#'));
        }

        return count;

        bool IsValid(string value)
        {
            var groups = value.Split('.', StringSplitOptions.RemoveEmptyEntries);
            if (groups.Length != spring.GroupSizes.Length)
                return false;

            for (int i = 0; i < groups.Length; i++)
            {
                if (groups[i].Length != spring.GroupSizes[i])
                    return false;
            }

            return true;
        }
    }

    private string ReplaceAt(string s, int index, char c) => new([.. s[..index], c, .. s[(index + 1)..]]);

    private record Spring(string Value, int[] GroupSizes);
}
