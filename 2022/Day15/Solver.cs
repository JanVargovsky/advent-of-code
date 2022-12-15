using System.Numerics;

namespace AdventOfCode.Year2022.Day15;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Merge(new Interval[]
        {
            new(0, 10),
            new(5,15),
            new(2,5),
            new(20,30)
        }).SequenceEqual(new Interval[]
        {
            new(0, 15),
            new(20,30)
        }));

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
""", 20) == 56000011);
    }

    public long Solve(string input, int limit = 4000000)
    {
        var rows = input.Split(Environment.NewLine);
        var items = rows.Select(row =>
        {
            var tokens = row.Split('=', ',', ':');
            var sensor = new Point(int.Parse(tokens[1]), int.Parse(tokens[3]));
            var beacon = new Point(int.Parse(tokens[5]), int.Parse(tokens[7]));
            return (sensor, beacon);
        }).ToArray();

        for (int y = 0; y <= limit; y++)
        {
            var row = new List<Interval>();
            foreach (var (sensor, beacon) in items)
            {
                var distance = ManhattanDistance(sensor, beacon);
                var distanceToTarget = ManhattanDistance(sensor, new Point(sensor.X, y));
                if (distanceToTarget < distance)
                {
                    var diff = distance - distanceToTarget;
                    row.Add(new(sensor.X - diff, sensor.X + diff));
                }
            }

            var sortedRow = row.OrderBy(t => t.From).ThenBy(t => t.To).ToArray();
            var merged = Merge(sortedRow);

            if (merged.Count == 2)
            {
                var x = merged[0].To + 1;
                return x * 4000000L + y;
            }
        }

        throw new Exception();
    }

    private List<Interval> Merge(IList<Interval> items)
    {
        var result = new List<Interval>();
        var bounds = items[0];

        for (int i = 1; i < items.Count; i++)
        {
            var interval = items[i];
            if (interval.From <= bounds.To)
            {
                bounds.To = Math.Max(bounds.To, interval.To);
            }
            else
            {
                result.Add(bounds);
                bounds = interval;
            }
        }
        result.Add(bounds);

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

internal record struct Interval(int From, int To);
