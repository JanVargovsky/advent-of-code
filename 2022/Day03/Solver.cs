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
""") == "157");
    }

    public string Solve(string input)
    {
        var rucksacks = input.Split(Environment.NewLine);

        var result = 0;

        foreach (var item in rucksacks)
        {
            var mid = item.Length / 2;

            var first = item[..mid];
            var second = item[mid..];

            var shared = first.Intersect(second).First();

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
