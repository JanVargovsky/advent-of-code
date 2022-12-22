using System.Numerics;

namespace AdventOfCode.Year2022.Day22;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
        ...#
        .#..
        #...
        ....
...#.......#
........#...
..#....#....
..........#.
        ...#....
        .....#..
        .#......
        ......#.

10R5L5R10L4R5L5
""") == 6032);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var map = rows[..^2];
        var instructions = rows[^1];

        var current = new Point(0, rows[0].IndexOf('.'));
        var facingIndex = 0;
        var facings = new Point[]
        {
            new(0, 1), // 0 = >
            new(1, 0), // 1 = v
            new(0, -1), // 2 = <
            new(-1, 0), // 3 = ^
        };
        var opposites = new Dictionary<Point, Point>();

        foreach (var (number, turn) in GetInstructions())
        {
            if (number.HasValue)
            {
                for (int i = 0; i < number; i++)
                {
                    var newCurrent = current + facings[facingIndex];

                    if (IsOutOfMap(newCurrent))
                    {
                        newCurrent = GetOpposite(current, facings[facingIndex]);
                    }

                    if (map[newCurrent.Row][newCurrent.Column] == '#')
                        break;

                    current = newCurrent;
                }
            }
            else if (turn.HasValue)
            {
                if (turn == 'L')
                {
                    facingIndex--;
                    facingIndex += facings.Length;
                }
                else if (turn == 'R')
                {
                    facingIndex++;
                }
                facingIndex %= facings.Length;
            }
        }

        var result = 1000 * (current.Row + 1) + 4 * (current.Column + 1) + facingIndex;
        return result;

        bool IsOutOfMap(Point p)
        {
            return p.Row < 0 || p.Column < 0 || p.Row >= map.Length || p.Column >= map[p.Row].Length ||
                    map[p.Row][p.Column] == ' ';
        }

        Point GetOpposite(Point p, Point facing)
        {
            if (!opposites.TryGetValue(p, out var result))
            {
                result = p;
                while (true)
                {
                    var tmp = result - facing;
                    if (!IsOutOfMap(tmp))
                        result = tmp;
                    else
                        break;
                }
                opposites[p] = result;
            }

            return result;
        }

        IEnumerable<(int?, char?)> GetInstructions()
        {
            var i = 0;
            var number = 0;
            while (i < instructions.Length)
            {
                if (char.IsDigit(instructions[i]))
                {
                    number *= 10;
                    number += instructions[i] - '0';
                }
                if (instructions[i] is 'R' or 'L')
                {
                    if (number > 0)
                    {
                        yield return (number, null);
                        number = 0;
                    }
                    yield return (null, instructions[i]);
                }
                i++;
            }

            if (number > 0)
            {
                yield return (number, null);
            }
        }
    }
}

internal record Point(int Row, int Column) :
    IAdditionOperators<Point, Point, Point>,
    ISubtractionOperators<Point, Point, Point>
{
    public static Point operator +(Point left, Point right)
    {
        return new(left.Row + right.Row, left.Column + right.Column);
    }

    public static Point operator -(Point left, Point right)
    {
        return new(left.Row - right.Row, left.Column - right.Column);
    }
}