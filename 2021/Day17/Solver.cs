namespace AdventOfCode.Year2021.Day17;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("target area: x=20..30, y=-10..-5") == "112");
    }

    public string Solve(string input)
    {
        var items = input.Split(new[] { "target area: x=", "..", ", y=" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();
        var (xMin, xMax, yMin, yMax) = (items[0], items[1], items[2], items[3]);

        const int limit = 1000;
        var velocities = 0;
        for (int x = -limit; x <= limit; x++)
            for (int y = -limit; y <= limit; y++)
            {
                if (Simulate(x, y, out var _))
                {
                    velocities++;
                }
            }

        var result = velocities;
        return result.ToString();

        bool Simulate(int velocityX, int velocityY, out int highestY)
        {
            var x = 0;
            var y = 0;
            highestY = y;

            while (true)
            {
                x += velocityX;
                y += velocityY;

                highestY = Math.Max(highestY, y);

                if (x >= xMin && x <= xMax && y >= yMin && y <= yMax)
                {
                    return true;
                }

                if (x > xMax && velocityX >= 0)
                    return false;
                else if (x < xMin && velocityX <= 0)
                    return false;
                else if (y < yMin)
                    return false;

                if (velocityX > 0)
                    velocityX--;
                else if (velocityX < 0)
                    velocityX++;
                velocityY--;
            }
        }
    }
}
