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
""") == 4361);
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
        var result = numbers.Where(HasAdjacentSymbol).Sum(t => t.Value);
        return result;

        bool HasAdjacentSymbol(Number n)
        {
            return GetNeightborPoints(n).Where(IsValid).Any(p =>
            {
                var c = rows[p.Row][p.Column];
                return !char.IsDigit(c) && c != '.';
            });
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
