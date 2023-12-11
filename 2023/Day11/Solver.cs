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

        var rowShifts = CalculateShifts(rows.Length, p => p.Row);
        var columnShifts = CalculateShifts(rows[0].Length, p => p.Column);

        var shiftedGalaxies = galaxies.Select(galaxy => new Point2d(galaxy.Row + rowShifts.GetValueOrDefault(galaxy, 0), galaxy.Column + columnShifts.GetValueOrDefault(galaxy, 0)));

        var result = shiftedGalaxies.Cartesian(shiftedGalaxies, Distance).Sum() / 2;
        return result;

        long Distance(Point2d x, Point2d y) => Math.Abs(y.Row - x.Row) + Math.Abs(y.Column - x.Column);

        Dictionary<Point2d, int> CalculateShifts(int length, Func<Point2d, int> func)
        {
            var result = new Dictionary<Point2d, int>(); // <position, shift>
            var totalShift = 0;
            for (int i = 0; i < length; i++)
            {
                var current = galaxies.Where(t => func(t) == i).ToArray();
                if (current.Length == 0)
                {
                    totalShift += expansion;
                }
                else
                {
                    foreach (var item in current)
                        result[item] = totalShift;
                }
            }
            return result;
        }
    }

    private record Point2d(int Row, int Column);
}
