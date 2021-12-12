namespace AdventOfCode.Year2021.Day12;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"start-A
start-b
A-c
A-b
b-d
A-end
b-end") == "36");
        Debug.Assert(Solve(@"dc-end
HN-start
start-kj
dc-start
dc-HN
LN-dc
HN-end
kj-sa
kj-HN
kj-dc") == "103");
        Debug.Assert(Solve(@"fs-end
he-DX
fs-he
start-DX
pj-DX
end-zg
zg-sl
zg-pj
pj-he
RW-he
fs-DX
pj-RW
zg-RW
start-pj
he-WI
zg-he
pj-fs
start-RW") == "3509");
    }

    public string Solve(string input)
    {
        var allEdges = input.Split(Environment.NewLine).Select(t =>
        {
            var dash = t.IndexOf('-');
            var from = new Node(t[..dash]);
            var to = new Node(t[(dash + 1)..]);
            return new Edge(from, to);
        });
        var nodes = new Dictionary<Node, HashSet<Node>>();
        foreach (var edge in allEdges)
        {
            MapNode(edge.From, edge.To);
            MapNode(edge.To, edge.From);

            void MapNode(Node from, Node to)
            {
                if (!nodes.TryGetValue(from, out var edges))
                    nodes[from] = edges = new HashSet<Node>();
                edges.Add(to);
            }
        }

        var start = nodes.Keys.First(t => t.Name == "start");
        var end = nodes.Keys.First(t => t.Name == "end");
        var result = Search(start, new() { start }, false);
        return result.ToString();

        int Search(Node node, HashSet<Node> visited, bool visitedTwice)
        {
            if (node == end)
                return 1;

            var count = 0;
            foreach (var nextNode in nodes[node])
            {
                if (nextNode == start)
                    continue;

                if (nextNode.IsSmall)
                {
                    if (!visited.Contains(nextNode))
                    {
                        var newVisited = new HashSet<Node>(visited)
                        {
                            nextNode
                        };
                        count += Search(nextNode, newVisited, visitedTwice);
                    }
                    else if (!visitedTwice)
                    {
                        var newVisited = new HashSet<Node>(visited)
                        {
                            nextNode
                        };
                        count += Search(nextNode, newVisited, true);
                    }
                }
                else
                    count += Search(nextNode, visited, visitedTwice);
            }
            return count;
        }
    }

    record Edge(Node From, Node To);
    record Node(string Name)
    {
        public bool IsSmall => Name.All(c => c >= 'a' && c <= 'z');
    }
}
