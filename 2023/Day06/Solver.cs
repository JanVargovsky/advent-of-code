namespace AdventOfCode.Year2023.Day06;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
Time:      7  15   30
Distance:  9  40  200
""") == 71503);
    }

    public long Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var time = long.Parse(string.Join("", rows[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1)));
        var distance = long.Parse(string.Join("", rows[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1)));

        var result = Calculate(time, distance);
        return result;
    }

    private long Calculate(long time, long distance)
    {
        var wonCount = 0L;
        for (long i = 0; i <= time; i++)
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
