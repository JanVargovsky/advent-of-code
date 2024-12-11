namespace AdventOfCode.Year2024.Day11;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("125 17", 1) == 3);
        Debug.Assert(Solve("125 17", 6) == 22);
        Debug.Assert(Solve("125 17", 25) == 55312);
    }

    public int Solve(string input, int blink = 25)
    {
        var data = input.Split(' ').Select(long.Parse).ToList();

        for (int i = 0; i < blink; i++)
        {
            data = Blink(data);
        }

        return data.Count;

        List<long> Blink(List<long> data)
        {
            var result = new List<long>();
            foreach (var item in data)
            {
                var stringItem = item.ToString();
                if (item == 0)
                {
                    result.Add(1);
                }
                else if (stringItem.Length % 2 == 0)
                {
                    result.Add(long.Parse(stringItem[..(stringItem.Length / 2)]));
                    result.Add(long.Parse(stringItem[(stringItem.Length / 2)..]));
                }
                else
                {
                    result.Add(item * 2024);
                }
            }
            return result;
        }
    }
}
