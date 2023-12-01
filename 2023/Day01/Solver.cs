namespace AdventOfCode.Year2023.Day01;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet
""") == 142);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var result = 0;
        foreach (var row in rows)
        {
            var first = row.First(char.IsDigit);
            var last = row.Last(char.IsDigit);
            var number = Convert.ToInt32($"{first}{last}");
            result += number;
        }
        return result;
    }
}
