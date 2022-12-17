using System.Numerics;
using System.Text;

namespace AdventOfCode.Year2022.Day17;

internal class Solver
{
    private const int Wide = 7;
    private const bool DebugPrint = false;

    public Solver()
    {
        Debug.Assert(Solve("""
>>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>
""") == 3068);
    }

    public int Solve(string input)
    {
        var rocks = LoadRocks().ToArray();
        var left = new Point(-1, 0);
        var right = new Point(1, 0);
        var down = new Point(0, -1);
        //RenderRocks(rocks);

        var restRocks = Enumerable.Range(0, Wide).Select(x => new Point(x, 0)).ToHashSet();
        var rockIndex = 0;
        var moveIndex = 0;
        for (int i = 0; i < 2022; i++)
        {
            var rock = rocks[rockIndex];
            DropRock(rock);
            rockIndex++;
            rockIndex %= rocks.Length;
        }
        var result = restRocks.Max(t => t.Y);
        return result;

        void DropRock(Rock rock)
        {
            var p = new Point(2, 3 + restRocks.Max(t => t.Y) + 1);

            DebugWrite("begins falling:");
            Render(restRocks, (rock, p));

            while (true)
            {
                var move = input[moveIndex];
                moveIndex++;
                moveIndex %= input.Length;
                if (move is '<')
                {
                    var leftP = p + left;
                    if (leftP.X + rock.Left.X < 0 || CollidesWithRocks(leftP, rock, restRocks))
                    {
                        DebugWrite("Jet of gas pushes rock left, but nothing happens:");
                    }
                    else
                    {
                        p = leftP;
                        DebugWrite("Jet of gas pushes rock left:");
                    }
                    Render(restRocks, (rock, p));
                }
                else if (move is '>')
                {
                    var rightP = p + right;
                    if (rightP.X + rock.Right.X >= Wide || CollidesWithRocks(rightP, rock, restRocks))
                    {
                        DebugWrite("Jet of gas pushes rock right, but nothing happens:");
                    }
                    else
                    {
                        p = rightP;
                        DebugWrite("Jet of gas pushes rock right:");
                    }
                    Render(restRocks, (rock, p));

                }

                var downP = p + down;
                if (CollidesWithRocks(downP, rock, restRocks))
                {
                    DebugWrite("Rock falls 1 unit, causing it to come to rest:");
                    AddRockToRest(p, rock, restRocks);
                    Render(restRocks);
                    return;
                }
                else
                {
                    p = downP;
                    DebugWrite("Rock falls 1 unit:");
                    Render(restRocks, (rock, p));
                }
            }
        }

        void AddRockToRest(Point rockPosition, Rock rock, HashSet<Point> rocks)
        {
            var points = rock.GetAbsolutePoints(rockPosition);
            foreach (var point in points)
            {
                var added = rocks.Add(point);
                Debug.Assert(added);
            }
        }

        bool CollidesWithRocks(Point rockPosition, Rock rock, HashSet<Point> rocks)
        {
            return rock.GetAbsolutePoints(rockPosition).Any(rocks.Contains);
        }
    }

    private void DebugWrite(string message)
    {
        if (!DebugPrint) return;
        Console.WriteLine(message);
    }

    private void Render(HashSet<Point> restRocks, (Rock Rock, Point Point)? droppingRock = null)
    {
        if (!DebugPrint) return;
        var sb = new StringBuilder();
        if (droppingRock.HasValue)
        {
            var droppingRockPoints = droppingRock.Value.Rock.GetAbsolutePoints(droppingRock.Value.Point).ToHashSet();
            RenderInternal(restRocks, droppingRockPoints);
        }
        else
        {
            RenderInternal(restRocks, new());
        }

        sb
            .Append('+')
            .Append('-', Wide)
            .Append('+')
            .AppendLine();

        DebugWrite(sb.ToString());

        void RenderInternal(HashSet<Point> restRocks, HashSet<Point> droppingRockPoints)
        {
            var maxY = restRocks.Max(t => t.Y);
            if (droppingRockPoints.Count > 0) maxY = Math.Max(maxY, droppingRockPoints.Max(t => t.Y));
            for (int y = maxY; y >= 0; y--)
            {
                sb.Append('|');
                for (int x = 0; x < Wide; x++)
                {
                    if (restRocks.Contains(new(x, y)))
                    {
                        sb.Append('#');
                    }
                    else if (droppingRockPoints.Contains(new(x, y)))
                    {
                        sb.Append('@');
                    }
                    else
                    {
                        sb.Append('.');
                    }
                }
                sb
                    .Append('|')
                    .AppendLine();
            }
        }
    }

    private void RenderRocks(Rock[] rocks)
    {
        if (!DebugPrint) return;
        foreach (var rock in rocks)
        {
            DebugWrite("0");
            var ps = new HashSet<Point>() { new(0, 0) };
            var p = new Point(2, 3 + ps.Max(t => t.Y) + 1);
            Render(ps, (rock, p));

            DebugWrite("3");
            ps = new HashSet<Point>() { new(0, 3) };
            p = new Point(2, 3 + ps.Max(t => t.Y) + 1);
            Render(ps, (rock, p));
        }
    }

    private IEnumerable<Rock> LoadRocks()
    {
        const string input = """
####

.#.
###
.#.

..#
..#
###

#
#
#
#

##
##
""";
        var stringRocks = input.Split(Environment.NewLine + Environment.NewLine);
        foreach (var stringRock in stringRocks)
        {
            var rockRows = stringRock.Split(Environment.NewLine);
            var points = new List<Point>();
            for (int y = 0; y < rockRows.Length; y++)
                for (int x = 0; x < rockRows[y].Length; x++)
                {
                    if (rockRows[y][x] == '#')
                        points.Add(new(x, rockRows.Length - y - 1));
                }
            yield return new(points);
        }
    }
}

internal record Rock(IList<Point> Points)
{
    public Point Left { get; } = Points.MinBy(t => t.X);
    public Point Right { get; } = Points.MaxBy(t => t.X);
    public IList<Point> Bottom { get; } = Points.GroupBy(t => t.X).Select(t => t.MaxBy(p => p.Y)).ToArray();

    public IEnumerable<Point> GetAbsolutePoints(Point point)
    {
        return Points.Select(p => new Point(point.X + p.X, point.Y + p.Y));
    }
}

internal record Point(int X, int Y) : IAdditionOperators<Point, Point, Point>
{
    public static Point operator +(Point left, Point right)
    {
        return new(left.X + right.X, left.Y + right.Y);
    }
}