using System.Numerics;

namespace AdventOfCode.Year2022.Day15;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
Sensor at x=8, y=7: closest beacon is at x=2, y=10
""", 10) == 12);

        Debug.Assert(Solve("""
Sensor at x=2, y=18: closest beacon is at x=-2, y=15
Sensor at x=9, y=16: closest beacon is at x=10, y=16
Sensor at x=13, y=2: closest beacon is at x=15, y=3
Sensor at x=12, y=14: closest beacon is at x=10, y=16
Sensor at x=10, y=20: closest beacon is at x=10, y=16
Sensor at x=14, y=17: closest beacon is at x=10, y=16
Sensor at x=8, y=7: closest beacon is at x=2, y=10
Sensor at x=2, y=0: closest beacon is at x=2, y=10
Sensor at x=0, y=11: closest beacon is at x=2, y=10
Sensor at x=20, y=14: closest beacon is at x=25, y=17
Sensor at x=17, y=20: closest beacon is at x=21, y=22
Sensor at x=16, y=7: closest beacon is at x=15, y=3
Sensor at x=14, y=3: closest beacon is at x=15, y=3
Sensor at x=20, y=1: closest beacon is at x=15, y=3
""", 10) == 26);
    }

    public int Solve(string input, int targetY = 2000000)
    {
        var rows = input.Split(Environment.NewLine);
        var sensors = new HashSet<Point>();
        var beacons = new HashSet<Point>();
        var targetRow = new HashSet<int>(); // X
        foreach (var row in rows)
        {
            var tokens = row.Split('=', ',', ':');
            var sensor = new Point(int.Parse(tokens[1]), int.Parse(tokens[3]));
            var beacon = new Point(int.Parse(tokens[5]), int.Parse(tokens[7]));
            sensors.Add(sensor);
            beacons.Add(beacon);

            var distance = ManhattanDistance(sensor, beacon);
            var distanceToTarget = ManhattanDistance(sensor, new Point(sensor.X, targetY));
            if (distanceToTarget < distance)
            {
                var diff = distance - distanceToTarget;
                var numbers = Enumerable.Range(sensor.X - diff, diff * 2 + 1);
                foreach (var item in numbers)
                {
                    targetRow.Add(item);
                }
            }
        }

        var beaconsOnTarget = beacons.Where(t => t.Y == targetY).Select(t => t.X);
        var result = targetRow.Except(beaconsOnTarget).Count();
        return result;
    }

    private int ManhattanDistance(Point a, Point b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }
}

internal record struct Point(int X, int Y) : IAdditionOperators<Point, Point, Point>
{
    public static Point operator +(Point left, Point right)
    {
        return new(left.X + right.X, left.Y + right.Y);
    }
}
