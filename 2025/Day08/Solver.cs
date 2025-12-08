using MoreLinq;

namespace AdventOfCode.Year2025.Day08;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
162,817,812
57,618,57
906,360,560
592,479,940
352,342,300
466,668,158
542,29,236
431,825,988
739,650,466
52,470,668
216,146,977
819,987,18
117,168,530
805,96,715
346,949,466
970,615,88
941,993,340
862,61,35
984,92,344
425,690,689
""") == 25272);
    }

    public long Solve(string input)
    {
        var boxes = input.Split(Environment.NewLine)
            .Select(r => r.Split(',').Select(int.Parse).ToArray())
            .Select(p => new Box(p))
            .ToArray();
        var box2circuit = boxes.ToDictionary(t => t, t => new HashSet<Box>() { t });
        var connections = GetAllConnections(boxes)
            .OrderBy(t => t.Distance);

        foreach (var (a, b, distance) in connections)
        {
            if (box2circuit[a].Contains(b))
                continue;

            var ca = box2circuit[a];
            var cb = box2circuit[b];
            var newC = ca.Concat(cb).ToHashSet();

            foreach (var item in newC)
                box2circuit[item] = newC;

            if (box2circuit.Values.Distinct().Count() == 1)
            {
                return (long)a.Position[0] * b.Position[0];
            }
        }

        throw new ItWontHappenException();
    }

    private IEnumerable<(Box, Box, double Distance)> GetAllConnections(IList<Box> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            for (int j = i + 1; j < items.Count; j++)
            {
                yield return (items[i], items[j], Distance(items[i], items[j]));
            }
        }
    }

    private double Distance(Box a, Box b)
    {
        var sum = a.Position.Zip(b.Position).Select(p => Math.Pow(p.Second - p.First, 2)).Sum();
        return Math.Sqrt(sum);
    }

    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    record Box(int[] Position)
    {
        public override int GetHashCode()
        {
            return HashCode.Combine(Position[0], Position[1], Position[2]);
        }

        private string GetDebuggerDisplay()
        {
            return string.Join(',', Position);
        }
    }
}
