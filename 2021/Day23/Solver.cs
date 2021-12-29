using System.Text;

namespace AdventOfCode.Year2021.Day23;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"#############
#...........#
###B#A#C#D###
  #A#B#C#D#
  #########", false) == "46");

        Debug.Assert(Solve(@"#############
#...........#
###B#A#C#D###
  #B#A#C#D#
  #########", false) == "114");

        var input = @"#############
#...........#
###B#C#B#D###
  #A#D#C#A#
  #########";
        Debug.Assert(Solve(input, false) == "12521");
        Debug.Assert(Solve(input) == "44169");
    }

    public string Solve(string input, bool extra = true)
    {
        var lines = input.Split(Environment.NewLine);
        var rows = 2;
        if (extra)
        {
            lines = lines[..3].Append("  #D#C#B#A#").Append("  #D#B#A#C#").Concat(lines[3..]).ToArray();
            rows = 4;
        }

        const char empty = '.';
        var fullGraph = CreateGraph(lines);
        var (graph, space2compactIndex) = PruneGraph(fullGraph);
        var hallCount = graph.Values.Count(t => t.Item1.SpaceType == SpaceType.Hall);
        var (initialMap, finishMap) = GetStates(lines);
        var result = Evaluate();
        return result.ToString();

        int Evaluate()
        {
            var visited = new HashSet<string>();
            var queue = new PriorityQueue<string, int>();
            queue.Enqueue(initialMap, 0);

            while (queue.TryDequeue(out var state, out var totalEnergy))
            {
                if (state == finishMap)
                    return totalEnergy;

                if (!visited.Add(state))
                    continue;

                //Console.WriteLine($"Current state: {StateToString(state)} {totalEnergy}");

                var moves = AllNextMoves(state);
                foreach (var (from, to, energy) in moves)
                {
                    var newMap = state.ToCharArray();
                    var amphipod = newMap[from];
                    newMap[from] = empty;
                    newMap[to] = amphipod;
                    var newMapString = new string(newMap);
                    var newTotalEnergy = totalEnergy + energy;
                    //Console.WriteLine($"{newMapString} {newTotalEnergy}");
                    queue.Enqueue(newMapString, newTotalEnergy);
                }
            }

            throw new InvalidOperationException();
        }

        string StateToString(string state)
        {
            var sb = new StringBuilder(hallCount + 4 * rows);
            sb.Append(state.AsSpan()[..hallCount]);
            for (int i = 0; i < 4; i++)
            {
                sb.Append('[')
                    .Append(state.AsSpan().Slice(hallCount + i * rows, rows))
                    .Append(']');
            }
            return sb.ToString();
        }

        IEnumerable<(int CompactFrom, int CompactTo, int Energy)> AllNextMoves(string state)
        {
            for (int i = 0; i < hallCount; i++)
            {
                var value = state[i];
                if (value is empty) continue;
                var (mapSpace, edges) = graph[i];
                Debug.Assert(mapSpace.SpaceType == SpaceType.Hall);
                // all possible
                var nextMoves = NextMoves(i, state);
                // filter only paths to room
                nextMoves = nextMoves.Where(m =>
                {
                    var (to, energy) = m;
                    var (space, _) = graph[to];
                    // I can move directly to room only
                    if (space.SpaceType == SpaceType.Room && space.DesiredState == value)
                    {
                        var roomIndex = (to - hallCount) / rows;
                        var roomValue = 'A' + (char)roomIndex;
                        var room = state.Substring(hallCount + roomIndex * rows, rows);
                        // its either empty or filled with right amphipods
                        return room.All(c => c == empty || c == roomValue);
                    }
                    return false;
                });
                // take the longest (depeest)
                if (nextMoves.Any())
                {
                    var (to, energy) = nextMoves.MaxBy(m => m.Energy);
                    yield return (i, to, energy);
                }
            }

            for (int room = 0; room < 4; room++)
            {
                var start = hallCount + room * rows;
                var sideRoom = state.Substring(start, rows);
                var roomValue = 'A' + (char)room;
                // complete room
                if (sideRoom.All(c => c == roomValue))
                    continue;

                // dirty room = invalid amphipod
                if (sideRoom.Any(c => c != roomValue && c != empty))
                {
                    for (int r = 0; r < rows; r++)
                    {
                        var value = sideRoom[r];
                        if (value == empty)
                            continue;
                        var nextMoves = NextMoves(start + r, state);
                        var nextMovesToRoom = nextMoves.Where(m =>
                        {
                            var (to, energy) = m;
                            var (space, _) = graph[to];
                            // I can move directly to my room only
                            if (space.SpaceType == SpaceType.Room && space.DesiredState == value)
                            {
                                var roomIndex = (to - hallCount) / rows;
                                var roomValue = 'A' + (char)roomIndex;
                                var room = state.Substring(hallCount + roomIndex * rows, rows);
                                // its either empty or filled with right amphipods
                                return room.All(c => c == empty || c == roomValue);
                            }
                            return false;
                        });

                        // is there a direct path to my room
                        if (nextMovesToRoom.Any())
                        {
                            var (to, energy) = nextMovesToRoom.MaxBy(m => m.Energy);
                            yield return (start + r, to, energy);
                        }
                        else
                            // go to hall only
                            foreach (var (to, energy) in nextMoves)
                            {
                                var (space, _) = graph[to];
                                if (space.SpaceType == SpaceType.Hall)
                                    yield return (start + r, to, energy);
                            }
                        break;
                    }
                }
            }
        }

        IEnumerable<(int CompactTo, int Energy)> NextMoves(int from, string state)
        {
            var queue = new Queue<(int, int)>();
            var multiplier = Energy(state[from]);
            EnqueueNext(from, 0);
            var visited = new HashSet<int>();

            while (queue.Count > 0)
            {
                var (currentCompactIndex, energy) = queue.Dequeue();
                if (!visited.Add(currentCompactIndex))
                    continue;

                if (state[currentCompactIndex] is empty)
                {
                    yield return (currentCompactIndex, energy);
                }
                else
                    continue;

                EnqueueNext(currentCompactIndex, energy);

            }

            void EnqueueNext(int compactIndex, int energy)
            {
                var (current, edges) = graph[compactIndex];
                foreach (var edge in edges)
                {
                    var toCompactIndex = space2compactIndex[edge.To];
                    queue.Enqueue((toCompactIndex, energy + edge.Distance * multiplier));
                }
            }
        }

        static int Energy(char c) => c switch
        {
            'A' => 1,
            'B' => 10,
            'C' => 100,
            'D' => 1000,
            _ => throw new InvalidOperationException(),
        };

        (string, string) GetStates(string[] lines)
        {
            var initialState = new List<char>();
            var expectedState = new List<char>();
            for (int i = 0; i < hallCount; i++)
            {
                initialState.Add(empty);
                expectedState.Add(empty);
            }

            for (int j = 0; j < 4; j++)
                for (int i = 0; i < rows; i++)
                {
                    initialState.Add(lines[2 + i][3 + j * 2]);
                    expectedState.Add((char)('A' + j));
                }

            return (new string(initialState.ToArray()), new string(expectedState.ToArray()));
        }

        Dictionary<Space, HashSet<Space>> CreateGraph(string[] lines)
        {
            var graph = new Dictionary<Space, HashSet<Space>>();
            var id = 0;
            var halls = lines[1][1..^1].Select(_ => new Space(id++, SpaceType.Hall, empty)).ToArray();
            for (int i = 0; i < halls.Length; i++)
            {
                var hall = halls[i];

                var others = graph[hall] = new();
                if (i > 0)
                    others.Add(halls[i - 1]);
                if (i < halls.Length - 1)
                    others.Add(halls[i + 1]);
            }

            var rooms = new Space[rows * 4];
            for (int j = 0; j < 4; j++)
                for (int i = 0; i < rows; i++)
                {
                    var desiredAmphipod = (char)('A' + j);
                    var room = rooms[i * 4 + j] = new Space(id++, SpaceType.Room, desiredAmphipod);
                    var others = graph[room] = new();

                    Space top = (i == 0) ? halls[2 + j * 2] : rooms[(i - 1) * 4 + j];
                    others.Add(top);
                    graph[top].Add(room);
                }

            return graph;
        }

        static (Dictionary<int, (Space, HashSet<Edge>)> Graph, Dictionary<Space, int> Space2CompactIndex) PruneGraph(Dictionary<Space, HashSet<Space>> graph)
        {
            //#############
            //#01.2.3.4.56#
            //###7#9#1#3###
            //  #8#0#2#4#
            //  #########

            var result = new Dictionary<int, (Space Space, HashSet<Edge> Neighbors)>();
            var space2CompactIndex = new Dictionary<Space, int>();
            var compactIndex2Id = new List<int>();
            var availableSpaces = new HashSet<Space>();
            var compactIndex = 0;
            foreach (var (space, neighbors) in graph)
            {
                if ((space.SpaceType is SpaceType.Hall && !neighbors.Any(t => t.SpaceType == SpaceType.Room)) ||
                    (space.SpaceType is SpaceType.Room))
                {
                    space2CompactIndex[space] = compactIndex;
                    result[compactIndex++] = (space, neighbors.Select(t => new Edge(t, 1)).ToHashSet());
                    availableSpaces.Add(space);
                }
            }

            foreach (var (removedSpace, neighbors) in graph)
            {
                if (availableSpaces.Contains(removedSpace))
                    continue;

                Console.WriteLine($"Removing {removedSpace.Id} ({removedSpace.SpaceType})");
                foreach (var neighbor in neighbors)
                {
                    var neighborCompactIndex = space2CompactIndex[neighbor];
                    var (neighborSpace, neighborNeighbors) = result[neighborCompactIndex];
                    var removed = neighborNeighbors.First(t => t.To == removedSpace);
                    neighborNeighbors.Remove(removed);

                    foreach (var neighbor2 in neighbors)
                    {
                        if (neighbor == neighbor2) continue;

                        neighborNeighbors.Add(new Edge(neighbor2, removed.Distance + 1));
                        Console.WriteLine($"Adding edge between {neighbor.Id} ({neighbor.SpaceType}) and {neighbor2.Id} ({neighbor2.SpaceType})");
                    }
                }
            }

            return (result, space2CompactIndex);
        }
    }

    enum SpaceType
    {
        Hall,
        Room
    }

    record Space(int Id, SpaceType SpaceType, char DesiredState);

    record Edge(Space To, int Distance);
}
