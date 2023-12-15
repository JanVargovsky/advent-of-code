namespace AdventOfCode.Year2023.Day15;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7
""") == 1320);
    }

    public int Solve(string input)
    {
        var values = input.Split(',');
        var results = values.Select(Hash);
        var result = results.Sum();
        return result;

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
}
