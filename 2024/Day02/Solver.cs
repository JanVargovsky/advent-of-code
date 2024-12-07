namespace AdventOfCode.Year2024.Day02;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("10 2 3") == 1);
        Debug.Assert(Solve("1 1 2 3 4") == 1);
        Debug.Assert(Solve("2 5 4 3 2") == 1);

        Debug.Assert(Solve("""
7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9
""") == 4);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var sequences = rows.Select(t => t.Split(' ').Select(int.Parse));
        var safeSequences = sequences.Where(s => IsSafe(s) || IsSafe(s.Reverse()));
        var result = safeSequences.Count();
        return result;

        bool IsSafe(IEnumerable<int> sequence)
        {
            var isSafe = IsSafeStrict(sequence, out var i1);
            if (isSafe)
                return true;

            isSafe = IsSafeStrict(sequence.Where((_, index) => index != i1), out var _);
            if (isSafe)
                return true;

            isSafe = IsSafeStrict(sequence.Where((_, index) => index != i1 - 1), out var _);
            if (isSafe)
                return true;

            return false;
        }

        bool IsSafeStrict(IEnumerable<int> sequence, out int index)
        {
            var i = 1;
            var previous = sequence.First();
            foreach (var current in sequence.Skip(1))
            {
                var diff = current - previous;
                if (diff <= 0 || diff >= 4)
                {
                    index = i;
                    return false;
                }

                previous = current;
                i++;
            }

            index = -1;
            return true;
        }
    }
}
