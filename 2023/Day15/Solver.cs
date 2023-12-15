namespace AdventOfCode.Year2023.Day15;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7
""") == 145);
    }

    public int Solve(string input)
    {
        var values = input.Split(',');
        var boxes = new List<Lens>[256];
        foreach (var str in values)
        {
            if (str[^1] == '-')
            {
                var name = str[..^1];
                var hash = Hash(name);
                var box = boxes[hash];
                if (box is null)
                    continue;
                var removed = box.RemoveAll(t => t.Name == name);
            }
            else
            {
                var tokens = str.Split('=');
                var name = tokens[0];
                var value = int.Parse(tokens[1]);
                var hash = Hash(name);
                var lens = new Lens(name, value);
                var box = boxes[hash] ??= [];
                var indexOfSame = box.FindIndex(t => t.Name == lens.Name);
                if (indexOfSame == -1)
                    box.Add(lens);
                else
                    box[indexOfSame] = lens;
            }
        }
        var result = boxes.Select((boxes, index) => FocusingPower(index + 1, boxes)).Sum();
        return result;

        int FocusingPower(int box, List<Lens> lenses)
        {
            if (lenses is null or { Count: 0 })
                return 0;

            var results = lenses.Select((lens, index) => box * (index + 1) * lens.Value);
            var result = results.Sum();
            return result;
        }

        int Hash(string str)
        {
            var value = 0;
            foreach (var c in str)
            {
                value += c;
                value *= 17;
                value %= 256;
            }
            return value;
        }
    }

    private record Lens(string Name, int Value);
}
