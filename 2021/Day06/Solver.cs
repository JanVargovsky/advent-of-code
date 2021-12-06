namespace AdventOfCode.Year2021.Day06;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"3,4,3,1,2", 18) == "26");
        Debug.Assert(Solve(@"3,4,3,1,2", 80) == "5934");
        Debug.Assert(Solve(@"3,4,3,1,2") == "26984457539");
    }

    public string Solve(string input, int days = 256)
    {
        var items = input.Split(',').Select(int.Parse);
        const int maxAge = 9;
        var lanternfishs = new long[maxAge];
        foreach (var item in items)
        {
            lanternfishs[item]++;
        }

        for (int d = 0; d < days; d++)
        {
            var newLanternfishs = new long[maxAge];
            for (int fish = 0; fish < maxAge; fish++)
            {
                var count = lanternfishs[fish];
                if (fish == 0)
                {
                    newLanternfishs[8] += count;
                    newLanternfishs[6] += count;
                }
                else
                {
                    newLanternfishs[fish - 1] += count;
                }
            }
            //Console.WriteLine($"Day {d,3} {string.Join(',', lanternfishs)} => {string.Join(',', newLanternfishs)}");
            lanternfishs = newLanternfishs;
        }

        var result = lanternfishs.Sum();
        return result.ToString();
    }
}