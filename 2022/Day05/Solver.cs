namespace AdventOfCode.Year2022.Day05;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
    [D]
[N] [C]
[Z] [M] [P]
 1   2   3

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2
""") == "MCD");
    }

    public string Solve(string input)
    {
        var segments = input.Split(Environment.NewLine + Environment.NewLine);
        var cargo = segments[0].Split(Environment.NewLine);
        var rearrangements = segments[1].Split(Environment.NewLine);

        var crates = GetItems(cargo[^1]).Select(_ => new Stack<char>()).ToArray();
        for (int i = cargo.Length - 2; i >= 0; i--)
        {
            var rowItems = GetItems(cargo[i]);
            for (int j = 0; j < rowItems.Count; j++)
            {
                var item = rowItems[j];
                if (item is not ' ')
                    crates[j].Push(item);
            }
        }

        foreach (var item in rearrangements)
        {
            var rearrangement = item.Split(new[] { " ", "move", "from", "to" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).ToArray();

            var stack = new Stack<char>();
            for (int i = 0; i < rearrangement[0]; i++)
            {
                var crate = crates[rearrangement[1] - 1].Pop();
                stack.Push(crate);
            }

            while (stack.Count > 0)
            {
                var create = stack.Pop();
                crates[rearrangement[2] - 1].Push(create);
            }
        }

        var result = new string(crates.Select(t => t.Peek()).ToArray());
        return result;

        List<char> GetItems(string row)
        {
            const int InitialOffset = 1;
            const int OffsetBetween = 4;

            var items = new List<char>();
            int i = InitialOffset;
            while (i < row.Length)
            {
                items.Add(row[i]);
                i += OffsetBetween;
            }

            return items;
        }
    }
}
