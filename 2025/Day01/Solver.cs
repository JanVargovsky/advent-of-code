namespace AdventOfCode.Year2025.Day01;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
L68
L30
R48
L5
R60
L55
L1
L99
R14
L82
""") == 3);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        int value = 50;
        const int range = 100; // 0 to 99
        var result = 0;
        foreach (var row in rows)
        {
            var n = int.Parse(row[1..]);
            if (row[0] == 'L')
                n = -n;
            var tmp = (value + n + range) % range;
            value = tmp;

            if (value == 0)
                result++;
        }
        return result;
    }
}
