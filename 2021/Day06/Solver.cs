namespace AdventOfCode.Year2021.Day06;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"3,4,3,1,2") == "5934");
    }

    public string Solve(string input)
    {
        var lanternfishs = input.Split(',').Select(int.Parse).ToList();

        for (int d = 0; d < 80; d++)
        {
            var l = lanternfishs.Count;
            for (int i = 0; i < l; i++)
            {
                if (lanternfishs[i] == 0)
                {
                    lanternfishs[i] = 6;
                    lanternfishs.Add(8);
                }
                else
                {
                    lanternfishs[i]--;
                }
            }
        }

        var result = lanternfishs.Count;
        return result.ToString();
    }
}