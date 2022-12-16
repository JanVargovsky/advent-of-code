namespace AdventOfCode.Year2022.Day16;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
Valve BB has flow rate=13; tunnels lead to valves CC, AA
Valve CC has flow rate=2; tunnels lead to valves DD, BB
Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
Valve EE has flow rate=3; tunnels lead to valves FF, DD
Valve FF has flow rate=0; tunnels lead to valves EE, GG
Valve GG has flow rate=0; tunnels lead to valves FF, HH
Valve HH has flow rate=22; tunnel leads to valve GG
Valve II has flow rate=0; tunnels lead to valves AA, JJ
Valve JJ has flow rate=21; tunnel leads to valve II
""") == 1707);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var nodes = new Dictionary<string, int>();
        var flowRates = new Dictionary<string, int>();
        var edges = new Dictionary<string, string[]>();
        var nodeIndex = 0;
        foreach (var row in rows)
        {
            var tokens = row.Split(new[] { ' ', '=', ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var name = tokens[1];
            var flowRate = int.Parse(tokens[5]);
            var nodeEdges = tokens[10..];

            flowRates[name] = flowRate;
            edges[name] = nodeEdges;
            nodes[name] = nodeIndex++;
        }

        var distances = FloydWarshall(nodes, edges);
        const string start = "AA";
        var removeUselessNodes = nodes.Where(t => t.Key != start && flowRates[t.Key] == 0).ToList();
        removeUselessNodes.ForEach(t => nodes.Remove(t.Key));
        const int minutes = 30;
        const int minutesWithElephant = 26;
        var startIndex = nodes[start];

        var result = Calculate(start, nodes, flowRates, distances, minutes);
        Console.WriteLine($"Starting with {result}");
        for (int i = 1; i <= nodes.Count / 2; i++)
        {
            Console.WriteLine($"Iterating subsets of size {i}");
            foreach (var elephantSubset in MoreLinq.MoreEnumerable.Subsets(nodes, i))
            {
                var myNodes = new Dictionary<string, int>(nodes.Except(elephantSubset));
                var elephantNodes = new Dictionary<string, int>(elephantSubset);
                myNodes[start] = startIndex;
                elephantNodes[start] = startIndex;
                var me = Calculate(start, myNodes, flowRates, distances, minutesWithElephant);
                var elephant = Calculate(start, elephantNodes, flowRates, distances, minutesWithElephant);

                var total = me + elephant;

                if (total > result)
                {
                    Console.WriteLine($"New better solution found, old={result}, new={total}");
                    result = total;
                }
            }
        }

        return result;
    }

    private int Calculate(string start, Dictionary<string, int> nodes, Dictionary<string, int> flowRates, int[,] distances, int minutes)
    {
        var queue = new PriorityQueue<State, int>(Comparer<int>.Create((a, b) => b.CompareTo(a)));
        queue.Enqueue(new State { Node = start, Minute = 0, OpenValves = new() }, 0);
        var memory = new Dictionary<State, int>();

        var best = 0;
        while (queue.TryDequeue(out var state, out var pressure))
        {
            var nexts = nodes
                .Where(t => !state.OpenValves.Contains(t.Key))
                //.Where(t => flowRates[t.Key] > 0)
                .Select(next =>
                {
                    var d = distances[nodes[state.Node], next.Value];
                    return (Node: next.Key, Distance: d);
                })
                .Where(t => t.Distance < minutes - state.Minute)
                .ToArray();

            if (nexts.Length == 0)
            {
                // no more moves, so just simulate how much pressure we can gather
                var totalPressure = pressure + (minutes - state.Minute - 1) * ReleasePressure(state.OpenValves);
                best = Math.Max(best, totalPressure);
                memory[state] = totalPressure;
                continue;
            }

            foreach (var next in nexts)
            {
                var newOpenValves = state.OpenValves.ToHashSet();
                newOpenValves.Add(next.Node);
                var newPressure = pressure + ReleasePressure(state.OpenValves) * next.Distance + ReleasePressure(newOpenValves);
                var newState = new State()
                {
                    Node = next.Node,
                    OpenValves = newOpenValves,
                    Minute = state.Minute + next.Distance + 1,
                };
                if (memory.TryAdd(newState, newPressure))
                    queue.Enqueue(newState, newPressure);
            }
        }

        return best;

        int ReleasePressure(HashSet<string> openValves)
        {
            return openValves.Select(t => flowRates[t]).Sum();
        }
    }

    private int[,] FloydWarshall(Dictionary<string, int> nodes, Dictionary<string, string[]> edges)
    {
        var n = nodes.Count;
        var distances = new int[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                distances[i, j] = n * 10;
            }
        }

        foreach (var (from, tos) in edges)
        {
            var fromIndex = nodes[from];
            foreach (var to in tos)
            {
                var toIndex = nodes[to];
                // distance is 1 minute
                distances[fromIndex, toIndex] = 1;
            }
        }

        foreach (var nodeIndex in nodes.Values)
        {
            // distance to itself is 0
            distances[nodeIndex, nodeIndex] = 0;
        }

        for (int k = 0; k < n; k++)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    var value = distances[i, k] + distances[k, j];
                    if (distances[i, j] > value)
                        distances[i, j] = value;
                }
            }
        }

        return distances;
    }
}

internal record Node(string Name, int FlowRate);

internal class State
{
    public string Node { get; init; }
    public int Minute { get; init; }
    public HashSet<string> OpenValves { get; init; }

    public override bool Equals(object? obj)
    {
        return Equals(obj, true);
    }

    public bool Equals(object? obj, bool comparePressure)
    {
        return obj is State state &&
               Node == state.Node &&
               Minute == state.Minute &&
               OpenValves.SetEquals(state.OpenValves);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Node, Minute, OpenValves);
    }

    public override string ToString()
    {
        return $"Node:{Node}, Minute={Minute}, OpenValves={string.Join(',', OpenValves)}";
    }
}