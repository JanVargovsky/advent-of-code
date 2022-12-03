namespace AdventOfCode.Year2022.Day03;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw
""") == "70");
    }

    public string Solve(string input)
    {
        var rucksacks = input.Split(Environment.NewLine).Chunk(3);

        var result = 0;

        foreach (var items in rucksacks)
        {
            var shared = items.Select(t => t.AsEnumerable()).Aggregate((a, b) => a.Intersect(b)).First();

            var score = 0;
            if (shared is >= 'a' and <= 'z')
                score = shared - 'a' + 1;
            else
                score = shared - 'A' + 27;

            result += score;
        }

        return result.ToString();
    }
}
