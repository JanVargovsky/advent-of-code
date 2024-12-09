using System.Text;

namespace AdventOfCode.Year2024.Day09;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
2333133121414131402
""") == 2858);
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
                for (var i = 0; i < block.Length; i++)
                {
                    result += (index++ * block.Id);
                }
            }

            return result;
        }

        void Compact(LinkedList<Block> diskMap)
        {
            var files = diskMap.Where(t => t.IsFile).Reverse().ToArray();

            foreach (var file in files)
            {
                var current = diskMap.First!;
                var fileNode = diskMap.Find(file)!;

                //Console.WriteLine($"Trying to move {file.Id}");
                //Console.WriteLine(ToString(diskMap));

                while (current != null && current != fileNode)
                {
                    if (current.Value.IsFile)
                    {
                        current = current.Next;
                        continue;
                    }

                    if (current.Value.Length == file.Length)
                    {
                        // perfect fit
                        (current.Value, fileNode.Value) = (fileNode.Value, current.Value);

                        //Console.WriteLine(ToString(diskMap));
                        break;
                    }
                    else if (current.Value.Length > file.Length)
                    {
                        // empty block is larger

                        var empty = current.Value;

                        diskMap.AddBefore(current, file);

                        current.Value = current.Value with
                        {
                            Length = current.Value.Length - file.Length,
                        };

                        fileNode.Value = file with
                        {
                            IsFile = false,
                            Id = 0,
                        };

                        //Console.WriteLine(ToString(diskMap));
                        break;
                    }
                    else
                        current = current.Next;
                }
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
