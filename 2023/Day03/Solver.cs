namespace AdventOfCode.Year2023.Day03;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..
""") == 467835);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        List<Number> numbers = [];

        for (int r = 0; r < rows.Length; r++)
        {
            for (int c = 0; c < rows[r].Length; c++)
            {
                if (char.IsDigit(rows[r][c]))
                {
                    var start = new Point2d(r, c);
                    var length = 1;
                    while (c + length < rows[r].Length && char.IsDigit(rows[r][c + length]))
                    {
                        length++;
                    }
                    var value = int.Parse(rows[r][c..(c + length)]);
                    numbers.Add(new(start, length, value));
                    c += length;
                }
            }
        }

        var numberToGears = numbers.ToDictionary(t => t, GetAdjacentGearPoints);
        Dictionary<Point2d, List<Number>> gearToNumbers = [];
        foreach (var (number, gears) in numberToGears)
        {
            foreach (var gear in gears)
            {
                if (!gearToNumbers.TryGetValue(gear, out var gearNumbers))
                    gearToNumbers[gear] = gearNumbers = [];
                gearNumbers.Add(number);

            }
        }
        var result = gearToNumbers
            .Where(t => t.Value.Count == 2)
            .Sum(t => t.Value[0].Value * t.Value[1].Value);
        return result;

        Point2d[] GetAdjacentGearPoints(Number n)
        {
            return GetNeightborPoints(n).Where(IsValid).Where(p => rows[p.Row][p.Column] == '*').ToArray();
        }

        bool IsValid(Point2d p) =>
            p.Row >= 0 && p.Row < rows.Length &&
            p.Column >= 0 && p.Column < rows[p.Row].Length;

        IEnumerable<Point2d> GetNeightborPoints(Number n)
        {
            yield return new(n.Start.Row, n.Start.Column - 1);
            yield return new(n.Start.Row, n.Start.Column + n.Length);

            for (int i = -1; i <= n.Length; i++)
            {
                yield return new(n.Start.Row - 1, n.Start.Column + i);
                yield return new(n.Start.Row + 1, n.Start.Column + i);
            }
        }
    }

    private record Number(Point2d Start, int Length, int Value);

    private record Point2d(int Row, int Column);
}
