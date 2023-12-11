using MoreLinq;

namespace AdventOfCode.Year2023.Day11;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
...#......
.......#..
#.........
..........
......#...
.#........
.........#
..........
.......#..
#...#.....
""", 1) == 374);
        Debug.Assert(Solve("""
...#......
.......#..
#.........
..........
......#...
.#........
.........#
..........
.......#..
#...#.....
""", 10 - 1) == 1030);
        Debug.Assert(Solve("""
...#......
.......#..
#.........
..........
......#...
.#........
.........#
..........
.......#..
#...#.....
""", 100 - 1) == 8410);
    }

    public long Solve(string input, int expansion = 1000000 - 1)
    {
        var rows = input.Split(Environment.NewLine);
        var galaxies = new List<Point2d>();
        for (int r = 0; r < rows.Length; r++)
        {
            for (int c = 0; c < rows[r].Length; c++)
            {
                if (rows[r][c] == '#')
                    galaxies.Add(new(r, c));
            }
        }

        var galaxies2Shift = new Dictionary<Point2d, Point2d>(); // <position, shift>
        var rowShift = 0;
        for (int row = 0; row < rows.Length; row++)
        {
            var current = galaxies.Where(t => t.Row == row).ToArray();
            if (current.Length == 0)
            {
                rowShift += expansion;
            }
            else
            {
                foreach (var item in current)
                {
                    if (!galaxies2Shift.TryGetValue(item, out var shift))
                        shift = new Point2d(rowShift, 0);
                    else
                        shift = shift with { Row = shift.Row + rowShift };
                    galaxies2Shift[item] = shift;
                }
            }
        }
        var columnShift = 0;
        for (int column = 0; column < rows[0].Length; column++)
        {
            var current = galaxies.Where(t => t.Column == column).ToArray();
            if (current.Length == 0)
            {
                columnShift += expansion;
            }
            else
            {
                foreach (var item in current)
                {
                    if (!galaxies2Shift.TryGetValue(item, out var shift))
                        shift = new Point2d(0, columnShift);
                    else
                        shift = shift with { Column = shift.Column + columnShift };
                    galaxies2Shift[item] = shift;
                }
            }
        }

        var shiftedGalaxies = galaxies.Select(galaxy =>
            galaxies2Shift.TryGetValue(galaxy, out var shift) ? new(galaxy.Row + shift.Row, galaxy.Column + shift.Column) : galaxy
        );

        var result = shiftedGalaxies.Cartesian(shiftedGalaxies, Distance).Sum() / 2;
        return result;

        long Distance(Point2d x, Point2d y) => Math.Abs(y.Row - x.Row) + Math.Abs(y.Column - x.Column);
    }

    private record Point2d(int Row, int Column);
}
