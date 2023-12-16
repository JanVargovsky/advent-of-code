using System.Numerics;

namespace AdventOfCode.Year2023.Day16;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
.|...\....
|.-.\.....
.....|-...
........|.
..........
.........\
..../.\\..
.-.-/..|..
.|....-|.\
..//.|....
""") == 46);
    }

    public int Solve(string input)
    {
        var grid = input.Split(Environment.NewLine);
        var up = new Point2d(-1, 0);
        var down = new Point2d(1, 0);
        var left = new Point2d(0, -1);
        var right = new Point2d(0, 1);
        var stateMachine = CreateStateMachine();
        var visited = new HashSet<State>();
        var states = new HashSet<State>();
        var initial = new State(new(0, 0), right);
        states.Add(initial);
        visited.Add(initial);

        while (states.Count > 0)
        {
            //Print();

            var newStates = new HashSet<State>();
            foreach (var state in states)
            {
                foreach (var newState in Next(state))
                {
                    if (IsInRange(newState.Point) && visited.Add(newState))
                        newStates.Add(newState);
                }
            }
            states = newStates;
        }

        var visitedPoints = visited.Select(t => t.Point).Distinct();
        var result = visitedPoints.Count();
        return result;

        void Print()
        {
            var points = visited.Select(t => t.Point).ToHashSet();
            for (int row = 0; row < grid.Length; row++)
            {
                for (int col = 0; col < grid[row].Length; col++)
                {
                    var visited = points.Contains(new(row, col));
                    Console.Write(visited ? '#' : grid[row][col]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        bool IsInRange(Point2d p) => p.Row >= 0 && p.Column >= 0 && p.Row < grid.Length && p.Column < grid[p.Row].Length;

        Dictionary<(char C, Point2d Direction), Point2d[]> CreateStateMachine() => new()
        {
            [('.', down)] = [down],
            [('.', up)] = [up],
            [('.', left)] = [left],
            [('.', right)] = [right],

            [('/', down)] = [left],
            [('/', up)] = [right],
            [('/', left)] = [down],
            [('/', right)] = [up],

            [('\\', down)] = [right],
            [('\\', up)] = [left],
            [('\\', left)] = [up],
            [('\\', right)] = [down],

            [('|', down)] = [down],
            [('|', up)] = [up],
            [('|', left)] = [up, down],
            [('|', right)] = [up, down],

            [('-', down)] = [left, right],
            [('-', up)] = [left, right],
            [('-', left)] = [left],
            [('-', right)] = [right],
        };

        IEnumerable<State> Next(State state)
        {
            var c = grid[state.Point.Row][state.Point.Column];
            return stateMachine[(c, state.Direction)].Select(direction => state with
            {
                Direction = direction,
                Point = state.Point + direction,
            });
        }
    }

    private record State(Point2d Point, Point2d Direction);

    private record Point2d(int Row, int Column) : IAdditionOperators<Point2d, Point2d, Point2d>
    {
        public static Point2d operator +(Point2d left, Point2d right) => new(left.Row + right.Row, left.Column + right.Column);
    }
}
