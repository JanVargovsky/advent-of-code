namespace AdventOfCode.Year2022.Day20;

internal class Solver
{
    private bool debugPrint = false;

    public Solver()
    {
        Debug.Assert(Solve("""
1
2
-3
3
-2
0
4
""") == 1623178306);
    }

    public long Solve(string input)
    {
        const long decryptionKey = 811589153;
        var items = new LinkedList<long>(input.Split(Environment.NewLine).Select(int.Parse).Select(t => t * decryptionKey));
        var nodes = GetNodes(items).ToArray();

        if (debugPrint)
        {
            DebugWrite("Initial arrangement: ");
            DebugWrite(string.Join(", ", items));
        }

        for (int i = 0; i < 10; i++)
        {
            foreach (var node in nodes)
            {
                var nextNode = node.Next ?? items.First!;
                items.Remove(node);

                nextNode = GetItem(nextNode, node.Value);

                items.AddBefore(nextNode, node);

                if (debugPrint)
                {
                    var left = (node.Previous ?? items.Last!).Value;
                    var right = (node.Next ?? items.First!).Value;

                    DebugWrite();
                    DebugWrite($"{node.Value} moves between {left} and {right}:");
                    DebugWrite(string.Join(", ", items));
                }
            }
        }

        var zero = items.Find(0)!;
        var resultItemValues = new[] { 1000, 2000, 3000 }.Select(t => GetItem(zero, t).Value);

        var result = resultItemValues.Sum();
        return result;

        LinkedListNode<long> GetItem(LinkedListNode<long> from, long offset)
        {
            var n = items.Count;
            offset %= n;

            var current = from;
            if (offset > 0)
            {
                while (offset-- > 0)
                {
                    current = current.Next ?? items.First!;
                }
            }
            else if (offset < 0)
            {
                while (offset++ < 0)
                {
                    current = current.Previous ?? items.Last!;
                }
            }

            return current;
        }

        IEnumerable<LinkedListNode<T>> GetNodes<T>(LinkedList<T> list)
        {
            var current = list.First;
            while (current is not null)
            {
                yield return current;
                current = current.Next;
            }
        }
    }

    private void DebugWrite(string? message = null)
    {
        if (!debugPrint) return;
        Console.WriteLine(message);
    }
}