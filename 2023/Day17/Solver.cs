using System.Numerics;

namespace AdventOfCode.Year2023.Day17;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
2413432311323
3215453535623
3255245654254
3446585845452
4546657867536
1438598798454
4457876987766
3637877979653
4654967986887
4564679986453
1224686865563
2546548887735
4322674655533
""") == 94);
        Debug.Assert(Solve("""
111111111111
999999999991
999999999991
999999999991
999999999991
""") == 71);
    }

    public int Solve(string input)
    {
        var map = input.Split(Environment.NewLine);
        var start = new Point2d(0, 0);
        var end = new Point2d(map.Length - 1, map[^1].Length - 1);
        var distances = new Dictionary<Point2d, int>();
        distances[start] = 0;
        var q = new PriorityQueue<State, int>();
        q.Enqueue(new(start, new(0, 1), 1), 0);
        q.Enqueue(new(start, new(1, 0), 1), 0);
        var path = new Dictionary<Point2d, Point2d>();
        var visited = new HashSet<State>();

        while (q.TryDequeue(out var state, out var totalDistance))
        {
            if (state.Position == end)
                break;
            if (!visited.Add(state))
                continue;

            var newStates = new List<State>();
            if (state.StraightCount > 3)
            {
                var left = state.Direction.Left;
                newStates.Add(new State(state.Position + left, left, 1));
                var right = state.Direction.Right;
                newStates.Add(new State(state.Position + right, right, 1));
            }
            if (state.StraightCount < 10)
                newStates.Add(state with
                {
                    Position = state.Position + state.Direction,
                    StraightCount = state.StraightCount + 1,
                });

            foreach (var newState in newStates)
            {
                if (!IsValid(newState.Position))
                    continue;

                if (newState.Position == end && newState.StraightCount <= 3)
                    continue;

                var distance = totalDistance + GetDistance(newState.Position);
                if (distance < distances.GetValueOrDefault(newState.Position, int.MaxValue))
                {
                    path[newState.Position] = state.Position;
                    distances[newState.Position] = distance;
                }
                q.Enqueue(newState, distance);
            }
        }

        //Print();

        var result = distances[end];
        return result;

        bool IsValid(Point2d p) => p.Row >= 0 && p.Column >= 0 && p.Row < map.Length && p.Column < map[p.Row].Length;
        int GetDistance(Point2d p) => map[p.Row][p.Column] - '0';

        void Print()
        {
            var pathPoints = new HashSet<Point2d>();
            var current = end;
            pathPoints.Add(current);
            while (path.TryGetValue(current, out var previous))
            {
                pathPoints.Add(previous);
                current = previous;
            }
            var visitedPoints = visited.Select(t => t.Position).ToHashSet();

            for (int row = 0; row < map.Length; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    var p = new Point2d(row, col);
                    var isPath = pathPoints.Contains(p);
                    var isVisited = visitedPoints.Contains(p);
                    if (isPath)
                        Console.ForegroundColor = ConsoleColor.Green;
                    else if (isVisited)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else
                        Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(map[row][col]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.ResetColor();
        }
    }

    private record State(Point2d Position, Point2d Direction, int StraightCount);

    private record Point2d(int Row, int Column) : IAdditionOperators<Point2d, Point2d, Point2d>
    {
        public static Point2d operator +(Point2d left, Point2d right) => new(left.Row + right.Row, left.Column + right.Column);
        public Point2d Left => new Point2d(-Column, Row);
        public Point2d Right => new Point2d(Column, -Row);
    }
}
