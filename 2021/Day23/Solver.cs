namespace AdventOfCode.Year2021.Day23;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"#############
#...........#
###B#A#C#D###
  #A#B#C#D#
  #########") == "46");

        Debug.Assert(Solve(@"#############
#...........#
###B#C#B#D###
  #A#D#C#A#
  #########") == "12521");
    }

    public string Solve(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var (initialMap, finishMap) = GetStates(lines);
        const char space = '.';
        var fullGraph = CreateGraph(lines);
        var (graph, space2compactIndex) = PruneGraph(fullGraph);
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

                //Console.WriteLine($"Current state: {state} {totalEnergy}");

                var moves = AllNextMoves(state);
                foreach (var (from, to, distance) in moves)
                {
                    var newMap = state.ToCharArray();
                    var amphipod = newMap[from];
                    newMap[from] = space;
                    newMap[to] = amphipod;
                    var energy = Energy(amphipod) * distance;
                    var newMapString = new string(newMap);
                    var newTotalEnergy = totalEnergy + energy;
                    //Console.WriteLine($"{newMapString} {newTotalEnergy}");
                    queue.Enqueue(newMapString, newTotalEnergy);
                }
            }

            throw new InvalidOperationException();
        }

        IEnumerable<(int CompactFrom, int CompactTo, int Distance)> AllNextMoves(string state)
        {
            for (int i = 0; i < state.Length; i++)
            {
                var value = state[i];
                if (value is space) continue;
                var (mapSpace, edges) = graph[i];
                var nextMoves = NextMoves(i, state);

                // I can move anywhere
                if (mapSpace.SpaceType == SpaceType.Room)
                {
                    foreach (var (to, distance) in nextMoves)
                    {
                        yield return (i, to, distance);
                    }
                }
                // I can move directly to room only
                else if (mapSpace.SpaceType == SpaceType.Hall)
                {
                    foreach (var (to, distance) in nextMoves)
                    {
                        var (space, _) = graph[to];
                        if (space is Room room && room.DesiredState == value)
                        {
                            yield return (i, to, distance);
                        }
                    }
                }
            }
        }

        IEnumerable<(int CompactTo, int Distance)> NextMoves(int from, string state)
        {
            var queue = new Queue<(int, int)>();
            EnqueueNext(from, 0);
            var visited = new HashSet<int>();

            while (queue.Count > 0)
            {
                var (currentCompactIndex, distance) = queue.Dequeue();
                if (!visited.Add(currentCompactIndex))
                    continue;

                if (state[currentCompactIndex] is space)
                {
                    yield return (currentCompactIndex, distance);
                }
                else
                    continue;

                EnqueueNext(currentCompactIndex, distance);

            }

            void EnqueueNext(int compactIndex, int distance)
            {
                var (current, edges) = graph[compactIndex];
                foreach (var edge in edges)
                {
                    var toCompactIndex = space2compactIndex[edge.To];
                    queue.Enqueue((toCompactIndex, distance + edge.Distance));
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

        static (string, string) GetStates(string[] lines)
        {
            var initialState = new List<char>();
            var expectedState = new List<char>();
            for (int i = 3; i < lines[1].Length - 1; i += 2)
            {
                initialState.Add(lines[1][i]);
                expectedState.Add(space);
            }

            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 4; j++)
                {
                    {
                        initialState.Add(lines[2 + i][3 + j * 2]);
                        expectedState.Add((char)('A' + j));
                    }
                }

            return (new string(initialState.ToArray()), new string(expectedState.ToArray()));
        }

        static Dictionary<Space, HashSet<Space>> CreateGraph(string[] lines)
        {
            var graph = new Dictionary<Space, HashSet<Space>>();
            var id = 0;
            var halls = lines[1][1..^1].Select(_ => new Hall(id++)).ToArray();
            for (int i = 0; i < halls.Length; i++)
            {
                var hall = halls[i];

                var others = graph[hall] = new();
                if (i > 0)
                    others.Add(halls[i - 1]);
                if (i < halls.Length - 1)
                    others.Add(halls[i + 1]);
            }

            var rooms = new Room[2 * 4];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var desiredAmphipod = (char)('A' + j);
                    var room = rooms[i * 4 + j] = new Room(id++, desiredAmphipod);
                    var others = graph[room] = new();

                    Space top = (i == 0) ? halls[2 + j * 2] : rooms[(i - 1) * 4 + j];
                    others.Add(top);
                    graph[top].Add(room);
                }
            }

            return graph;
        }

        static (Dictionary<int, (Space, HashSet<Edge>)> Graph, Dictionary<Space, int> Space2CompactIndex) PruneGraph(Dictionary<Space, HashSet<Space>> graph)
        {
            var result = new Dictionary<int, (Space Space, HashSet<Edge> Neighbors)>();
            var space2CompactIndex = new Dictionary<Space, int>();
            var compactIndex2Id = new List<int>();
            var availableSpaces = new HashSet<Space>();
            var compactIndex = 0;
            foreach (var (space, neighbors) in graph)
            {
                if ((space.SpaceType is SpaceType.Hall && (neighbors.Count > 1 && !neighbors.Any(t => t.SpaceType == SpaceType.Room))) ||
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

    abstract record Space(int Id, SpaceType SpaceType);
    record Hall(int Id) : Space(Id, SpaceType.Hall);
    record Room(int Id, char DesiredState) : Space(Id, SpaceType.Room);

    record Edge(Space To, int Distance);
}
