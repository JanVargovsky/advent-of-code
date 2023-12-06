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

        var result = Calculate2(time, distance);
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

    private long Calculate2(long time, long distance)
    {
        // quadratic formula
        var b = time;
        var a = distance;
        var d = Math.Sqrt(b * b - 4 * a);
        var x1 = -(b + d) / 2;
        var x2 = -(b - d) / 2;

        var result = Math.Abs(Math.Ceiling(x2) - Math.Floor(x1));
        return (long)result - 1;
    }
}
