namespace AdventOfCode.Year2025.Day07;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
.......S.......
...............
.......^.......
...............
......^.^......
...............
.....^.^.^.....
...............
....^.^...^....
...............
...^.^...^.^...
...............
..^...^.....^..
...............
.^.^.^.^.^...^.
...............
""") == 21);
    }

    public long Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var beams = new HashSet<int>();
        beams.Add(rows[0].IndexOf('S'));
        var result = 0L;

        foreach (var row in rows[1..])
        {
            var newBeams = new HashSet<int>();
            foreach (var i in beams)
            {
                if (row[i] == '.')
                    newBeams.Add(i);
                else if (row[i] == '^')
                {
                    newBeams.Add(i - 1);
                    newBeams.Add(i + 1);
                    result++;
                }
            }
            beams = newBeams;
        }
        return result;
    }
}
