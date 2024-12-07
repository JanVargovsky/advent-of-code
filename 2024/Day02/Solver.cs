namespace AdventOfCode.Year2024.Day02;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9
""") == 2);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var sequences = rows.Select(t => t.Split(' ').Select(int.Parse));
        var safeSequences = sequences.Where(s => IsSafe(s));
        var result = safeSequences.Count();
        return result;

        bool IsSafe(IEnumerable<int> sequence)
        {
            var first = sequence.First();
            var second = sequence.Skip(1).First();
            if (first > second)
                sequence = sequence.Reverse();

            var previous = sequence.First();
            foreach (var current in sequence.Skip(1))
            {
                var diff = current - previous;
                if (diff <= 0 || diff >= 4)
                    return false;

                previous = current;
            }
            return true;
        }
    }
}
