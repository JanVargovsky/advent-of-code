using System.Numerics;

namespace AdventOfCode.Year2024.Day15;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
########
#..O.O.#
##@.O..#
#...O..#
#.#.O..#
#...O..#
#......#
########

<^^>>>vv<v>>v<<
""") == 2028);

        Debug.Assert(Solve("""
##########
#..O..O.O#
#......O.#
#.OO..O.O#
#..O@..O.#
#O#..O...#
#O..O..O.#
#.OO.O.OO#
#....O...#
##########

<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^
""") == 10092);
    }

    public long Solve(string input)
    {
        var segments = input.Split(Environment.NewLine + Environment.NewLine);
        var map = segments[0].Split(Environment.NewLine);
        var movements = segments[1].Split(Environment.NewLine).SelectMany(t => t);
        var movementToDirection = new Dictionary<char, Point>
        {
            ['^'] = new Point(0, -1),
            ['v'] = new Point(0, 1),
            ['<'] = new Point(-1, 0),
            ['>'] = new Point(1, 0),
        };

        var points = new Dictionary<Point, char>();

        Point start = null!;
        for (var y = 0; y < map.Length; y++)
            for (var x = 0; x < map[y].Length; x++)
            {
                var c = map[y][x];
                var p = new Point(x, y);
                if (c is '@')
                    start = p;
                else if (c is '#' or 'O')
                    points[p] = c;
            }

        var current = start;
        foreach (var movement in movements)
        {
            var direction = movementToDirection[movement];
            var next = current + direction;

            if (!points.TryGetValue(next, out var c))
            {
                current = next;
            }
            else
            {
                if (c is '#')
                    continue;
                if (c is 'O')
                {
                    var nextSpot = next + direction;

                    // try find first empty spot
                    while (IsValid(nextSpot) && points.TryGetValue(nextSpot, out c))
                    {
                        if (c is '#')
                            break;

                        nextSpot += direction;
                    }

                    if (IsValid(nextSpot) && !points.ContainsKey(nextSpot))
                    {
                        current = next;
                        points.Remove(next);
                        points[nextSpot] = 'O';
                    }
                }
            }

            //PrintMove(movement);
        }

        var result = points.Where(t => t.Value == 'O').Sum(t => t.Key.X + t.Key.Y * 100);
        return result;

        void PrintMove(char move)
        {
            Console.WriteLine($"Move {move}:");
            for (var y = 0; y < map.Length; y++)
            {
                for (var x = 0; x < map[y].Length; x++)
                {
                    var p = new Point(x, y);
                    if (current == p)
                        Console.Write('@');
                    else if (points.TryGetValue(p, out var c))
                        Console.Write(c);
                    else
                        Console.Write('.');
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        bool IsValid(Point p) => p.X >= 0 && p.Y >= 0 && p.Y < map.Length && p.X < map[p.Y].Length;

        Point Find(char c)
        {
            for (var y = 0; y < map.Length; y++)
            {
                for (var x = 0; x < map[y].Length; x++)
                    if (map[y][x] == c)
                        return new Point(x, y);
            }

            throw new ItWontHappenException();
        }
    }

    record Point(int X, int Y) : IAdditionOperators<Point, Point, Point>
    {
        public static Point operator +(Point left, Point right) => new(left.X + right.X, left.Y + right.Y);
    }
}
