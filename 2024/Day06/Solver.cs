using System.Numerics;

namespace AdventOfCode.Year2024.Day06;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...
""") == 6);
    }

    public int Solve(string input)
    {
        var map = input.Split(Environment.NewLine);
        var loops = 0;

        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == '.')
                {
                    var backup = map[y];
                    var charArray = map[y].ToCharArray();
                    charArray[x] = '#';
                    map[y] = new string(charArray);

                    if (IsLoop())
                        loops++;

                    map[y] = backup;
                }
            }
        }

        return loops;

        bool IsLoop()
        {
            var directions = new[] { Point.Top, Point.Right, Point.Bottom, Point.Left };
            var directionIndex = 0;

            var visited = new HashSet<(Point, int)>();
            var current = Find('^');

            while (true)
            {
                var next = current + directions[directionIndex];

                // out of map
                if (!IsValid(next))
                    return false;

                if (map[next.Y][next.X] != '#')
                {
                    if (!visited.Add((current, directionIndex)))
                    {
                        //Print(current);
                        //Console.WriteLine();
                        return true;
                    }
                    current = next;
                }
                else
                {
                    directionIndex++;
                    directionIndex %= directions.Length;
                }
            }

            void Print(Point current)
            {
                for (var y = 0; y < map.Length; y++)
                {
                    for (var x = 0; x < map[y].Length; x++)
                    {
                        var p = new Point(x, y);
                        if (p == current)
                        {
                            if (directions[directionIndex] == Point.Top)
                                Console.Write('^');
                            else if (directions[directionIndex] == Point.Bottom)
                                Console.Write('v');
                            else if (directions[directionIndex] == Point.Left)
                                Console.Write('<');
                            else if (directions[directionIndex] == Point.Right)
                                Console.Write('>');
                            else
                                Console.Write('?');
                        }
                        else if (visited.Contains((p, directionIndex)))
                            Console.Write('X');
                        else
                            Console.Write(map[y][x]);
                    }
                    Console.WriteLine();
                }
            }
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
        public static readonly Point Top = new(0, -1);
        public static readonly Point Right = new(1, 0);
        public static readonly Point Bottom = new(0, 1);
        public static readonly Point Left = new(-1, 0);

        public static Point operator +(Point left, Point right) => new(left.X + right.X, left.Y + right.Y);
    }
}
