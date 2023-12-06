namespace AdventOfCode.Year2023.Day06;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
Time:      7  15   30
Distance:  9  40  200
""") == 288);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var times = rows[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToArray();
        var distances = rows[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToArray();
        Debug.Assert(times.Length == distances.Length);

        var result = Enumerable.Range(0, times.Length).Aggregate(1, (acc, index) => acc * Calculate(times[index], distances[index]));
        return result;
    }

    private int Calculate(int time, int distance)
    {
        var wonCount = 0;
        for (int i = 0; i <= time; i++)
        {
            var holdButton = i;
            var speed = i;
            var myDistance = (time - holdButton) * speed;
            if (myDistance > distance)
                wonCount++;
        }
        return wonCount;
    }
}
