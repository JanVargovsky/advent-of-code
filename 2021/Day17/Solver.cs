namespace AdventOfCode.Year2021.Day17;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("target area: x=20..30, y=-10..-5") == "45");
    }

    public string Solve(string input)
    {
        var items = input.Split(new[] { "target area: x=", "..", ", y=" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();
        var (xMin, xMax, yMin, yMax) = (items[0], items[1], items[2], items[3]);

        var highestY = int.MinValue;
        for (int x = 0; x < 1000; x++)
            for (int y = 0; y < 1000; y++)
            {
                if (Simulate(x, y, out var currentHighestY))
                {
                    if (highestY < currentHighestY)
                        highestY = currentHighestY;
                }
            }

        var result = highestY;
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
