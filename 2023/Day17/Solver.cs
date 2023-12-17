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
""") == 102);
    }

    public int Solve(string input)
    {
        var map = input.Split(Environment.NewLine);
        var start = new Point2d(0, 0);
        var end = new Point2d(map.Length - 1, map[^1].Length - 1);
        var distances = new Dictionary<Point2d, int>();
        distances[start] = 0;
        var q = new PriorityQueue<State, int>();
        q.Enqueue(new(start, new(0, 1), 0), 0);
        var path = new Dictionary<Point2d, Point2d>();
        var visited = new HashSet<State>();

        while (q.TryDequeue(out var state, out var totalDistance))
        {
            if (state.Position == end)
                break;
            if (!visited.Add(state))
                continue;

            var newStates = new List<State>();
            var left = new Point2d(-state.Direction.Column, state.Direction.Row);
            newStates.Add(new State(state.Position + left, left, 0));
            var right = new Point2d(state.Direction.Column, -state.Direction.Row);
            newStates.Add(new State(state.Position + right, right, 0));
            if (state.StraightCount < 2)
                newStates.Add(state with
                {
                    Position = state.Position + state.Direction,
                    StraightCount = state.StraightCount + 1,
                });

            foreach (var newState in newStates)
            {
                if (!IsValid(newState.Position))
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
            var points = new HashSet<Point2d>();
            var current = end;
            points.Add(current);
            while (path.TryGetValue(current, out var previous))
            {
                points.Add(previous);
                current = previous;
            }

            for (int row = 0; row < map.Length; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    var visited = points.Contains(new(row, col));
                    if (visited)
                        Console.ForegroundColor = ConsoleColor.Green;
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
