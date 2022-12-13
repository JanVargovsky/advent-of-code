namespace AdventOfCode.Year2022.Day13;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]
""") == 140);
    }

    public int Solve(string input)
    {
        var pairs = input.Split(Environment.NewLine + Environment.NewLine);
        var packets = new List<Packet>();
        var divider1 = ParsePacket("[[2]]");
        packets.Add(divider1);
        var divider2 = ParsePacket("[[6]]");
        packets.Add(divider2);

        for (int i = 0; i < pairs.Length; i++)
        {
            var rows = pairs[i].Split(Environment.NewLine);
            packets.Add(ParsePacket(rows[0]));
            packets.Add(ParsePacket(rows[1]));
        }

        packets.Sort(Compare);

        var di1 = packets.IndexOf(divider1) + 1;
        var di2 = packets.IndexOf(divider2) + 1;
        var result = di1 * di2;
        return result;
    }

    private bool Compare(string left, string right)
    {
        var leftPacket = ParsePacket(left);
        var rightPacket = ParsePacket(right);
        var result = Compare(leftPacket, rightPacket);
        return result == -1;
    }

    private Packet ParsePacket(string packet)
    {
        var root = new ListPacket(new List<Packet>());
        var current = root;
        var parents = new Stack<ListPacket>();
        var i = 0;

        while (i < packet.Length)
        {
            var value = packet[i];
            if (value is '[')
            {
                parents.Push(current);
                var child = new ListPacket(new List<Packet>());
                current.Packets.Add(child);
                current = child;
            }
            else if (value is ']')
            {
                current = parents.Pop();
            }
            else if (value is ',')
            {
            }
            else
            {
                var start = i;
                var end = i + 1;
                while (char.IsDigit(packet[end]))
                {
                    end++;
                }

                var integer = int.Parse(packet[start..end]);
                current.Packets.Add(new IntegerPacket(integer));
            }

            i++;
        }

        return root.Packets.Single();
    }

    private int Compare(Packet a, Packet b)
    {
        if (a is IntegerPacket ia && b is IntegerPacket ib)
            return Compare(ia, ib);

        var la = ToListPacket(a);
        var lb = ToListPacket(b);
        return Compare(la, lb);
    }

    private int Compare(IntegerPacket a, IntegerPacket b)
    {
        return a.Value.CompareTo(b.Value);
    }

    private int Compare(ListPacket a, ListPacket b)
    {
        var ai = 0;
        var bi = 0;
        while (ai < a.Packets.Count && bi < b.Packets.Count)
        {
            var result = Compare(a.Packets[ai], b.Packets[bi]);

            // same order = keep going
            if (result == 0)
            {
                ai++;
                bi++;
            }
            else
            {
                return result;
            }
        }

        var aOut = ai >= a.Packets.Count;
        var bOut = bi >= b.Packets.Count;

        // same length
        if (aOut && bOut)
            return 0;

        // left ran out of items = right order
        if (aOut) return -1;
        // right ran out of order = not right order
        if (bOut) return 1;

        throw new Exception();

    }

    private ListPacket ToListPacket(Packet packet)
    {
        return packet switch
        {
            ListPacket listPacket => listPacket,
            IntegerPacket integerPacket => new ListPacket(new() { integerPacket }),
            _ => throw new ArgumentException(),
        };
    }
}

internal abstract record Packet();

internal record IntegerPacket(int Value) : Packet();

internal record ListPacket(List<Packet> Packets) : Packet();