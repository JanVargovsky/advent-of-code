namespace AdventOfCode.Year2024.Day11;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("125 17", 1) == 3);
        Debug.Assert(Solve("125 17", 6) == 22);
        Debug.Assert(Solve("125 17", 25) == 55312);
    }

    public long Solve(string input, int blink = 75)
    {
        var numbers = input.Split(' ').Select(long.Parse);
        var data = new Dictionary<long, long>(); // <number, count>
        foreach (var item in numbers)
            data[item] = data.GetValueOrDefault(item, 0) + 1;

        for (int i = 0; i < blink; i++)
            data = Blink(data);

        return data.Values.Sum();

        Dictionary<long, long> Blink(Dictionary<long, long> data)
        {
            var result = new Dictionary<long, long>();
            foreach (var (item, count) in data)
            {
                var stringItem = item.ToString();
                if (item == 0)
                {
                    result[1] = result.GetValueOrDefault(1, 0) + count;
                }
                else if (stringItem.Length % 2 == 0)
                {
                    var a = long.Parse(stringItem[..(stringItem.Length / 2)]);
                    var b = long.Parse(stringItem[(stringItem.Length / 2)..]);

                    result[a] = result.GetValueOrDefault(a, 0) + count;
                    result[b] = result.GetValueOrDefault(b, 0) + count;
                }
                else
                {
                    var a = item * 2024;
                    result[a] = result.GetValueOrDefault(a, 0) + count;
                }
            }
            return result;
        }
    }
}
