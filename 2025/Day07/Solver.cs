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
""") == 40);
    }

    public long Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var beams = new Dictionary<int, long>();
        beams[rows[0].IndexOf('S')] = 1;

        foreach (var row in rows[1..])
        {
            var newBeams = new Dictionary<int, long>();
            foreach (var (i, count) in beams)
            {
                if (row[i] == '.')
                    Add(newBeams, i, count);
                else if (row[i] == '^')
                {
                    Add(newBeams, i - 1, count);
                    Add(newBeams, i + 1, count);
                }
            }
            beams = newBeams;
        }
        return beams.Values.Sum();

        void Add(Dictionary<int, long> dic, int index, long value)
        {
            _ = dic.TryGetValue(index, out var existing);
            dic[index] = existing + value;
        }
    }
}
