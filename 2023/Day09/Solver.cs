namespace AdventOfCode.Year2023.Day09;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45
""") == 114);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine).Select(Parse).ToArray();
        var predictions = rows.Select(PredictNext);
        var result = predictions.Sum();
        return result;

        int[] Parse(string row) => row.Split().Select(int.Parse).ToArray();
    }

    private int PredictNext(int[] values)
    {
        List<int[]> sequences = [values];

        while (!AllZero(sequences[^1]))
        {
            sequences.Add(GenerateNext(sequences[^1]));
        }

        var predicts = new int[sequences.Count];
        predicts[^1] = 0;
        for (int i = sequences.Count - 1 - 1; i >= 0; i--)
        {
            predicts[i] = sequences[i][^1] + predicts[i + 1];
        }

        return predicts[0];

        bool AllZero(int[] items) => items.All(t => t == 0);

        int[] GenerateNext(int[] items)
        {
            var result = new int[items.Length - 1];

            for (int i = 0; i < items.Length - 1; i++)
            {
                result[i] = items[i + 1] - items[i];
            }

            return result;
        }
    }
}
