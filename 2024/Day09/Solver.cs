using System.Text;

namespace AdventOfCode.Year2024.Day09;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
2333133121414131402
""") == 1928);
    }

    public long Solve(string input)
    {
        var diskMap = Parse(input);
        Compact(diskMap);
        var result = Checksum(diskMap);
        return result;

        long Checksum(LinkedList<Block> diskMap)
        {
            var index = 0l;
            var result = 0l;

            foreach (var block in diskMap)
            {
                Debug.Assert(block.IsFile);
                for (var i = 0; i < block.Length; i++)
                {
                    result += (index++ * block.Id);
                }
            }

            return result;
        }

        void Compact(LinkedList<Block> diskMap)
        {
            var current = diskMap.First!;
            while (current != null && current != diskMap.Last)
            {
                if (current!.Value.IsFile)
                {
                    current = current.Next!;
                    continue;
                }

                if (!diskMap.Last!.Value.IsFile)
                {
                    diskMap.RemoveLast();
                    continue;
                }

                //Console.WriteLine(ToString(diskMap));

                var last = diskMap.Last.Value;

                if (current.Value.Length == last.Length)
                {
                    // perfect fit
                    diskMap.RemoveLast();
                    current.Value = last;
                }
                else if (current.Value.Length < last.Length)
                {
                    // empty block is not large enough
                    diskMap.Last.Value = last with
                    {
                        Length = last.Length - current.Value.Length,
                    };
                    current.Value = last with
                    {
                        Length = current.Value.Length
                    };
                }
                else if (current.Value.Length > last.Length)
                {
                    // empty block is larger
                    diskMap.AddBefore(current, last);
                    diskMap.RemoveLast();
                    current.Value = last with
                    {
                        IsFile = false,
                        Length = current.Value.Length - last.Length,
                    };
                }

                //Console.WriteLine(ToString(diskMap));
                //Console.WriteLine();
            }
        }

        string ToString(LinkedList<Block> diskMap)
        {
            var sb = new StringBuilder();
            foreach (var block in diskMap)
            {
                if (block.IsFile)
                    sb.Append(block.Id.ToString()[0], block.Length);
                else
                    sb.Append('.', block.Length);
            }
            return sb.ToString();
        }

        LinkedList<Block> Parse(string diskMap)
        {
            var result = new LinkedList<Block>();
            var isFile = true;
            var id = 0;
            foreach (var block in diskMap)
            {
                result.AddLast(new Block(isFile, block - '0', isFile ? id++ : 0));
                isFile = !isFile;
            }
            return result;
        }
    }

    record Block(bool IsFile, int Length, int Id);
}
