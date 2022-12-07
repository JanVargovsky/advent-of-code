using MoreLinq;

namespace AdventOfCode.Year2022.Day07;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k
""") == 24933642);
    }

    public long Solve(string input)
    {
        var segments = input.Split('$', StringSplitOptions.RemoveEmptyEntries);
        var root = new File(0, "/", true, null);
        var currentDirectory = root;

        foreach (var segment in segments)
        {
            var rows = segment.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var command = rows[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var output = rows[1..];

            if (command is ["cd", "/"])
                SetToRoot();
            else if (command is ["cd", ".."])
                ParentDirectory();
            else if (command is ["cd", _])
                ChangeDirectory(command[1]);
            else if (command is ["dir", _])
                AddDirectory(command[1]);
            else if (command is ["ls"])
                List(output);
            else
                throw new NotSupportedException();
        }

        var directories = root.GetAllRecursiveFiles().Where(t => t.IsDirectory).ToList();

        foreach (var directory in directories)
        {
            directory.CalculateTotalSize();
        }

        var freeSpace = 70000000 - root.TotalSize;
        var requiredSpaceToFree = 30000000 - freeSpace;

        var candidates = directories
            .Where(t => t.TotalSize >= requiredSpaceToFree)
            .OrderBy(t => t.TotalSize);

        var result = candidates.First().TotalSize.Value;

        return result;

        void SetToRoot() => currentDirectory = root;
        void ParentDirectory() => currentDirectory = currentDirectory.Parent;
        void ChangeDirectory(string arg) => currentDirectory = currentDirectory.Files[arg]!;
        void AddDirectory(string arg) => currentDirectory.AddDirectory(arg);
        void List(string[] output)
        {
            foreach (var file in output)
            {
                var args = file.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (args is ["dir", _])
                    AddDirectory(args[1]);
                else
                {
                    var size = int.Parse(args[0]);
                    currentDirectory.AddFile(size, args[1]);
                }
            }
        }
    }
}

file record File(int Size, string Name, bool IsDirectory, File? Parent)
{
    public Dictionary<string, File>? Files { get; set; }
    public long? TotalSize { get; set; }

    public void AddDirectory(string name)
    {
        Files ??= new();
        Files[name] = new File(0, name, true, this);
    }

    public void AddFile(int size, string name)
    {
        if (!IsDirectory) throw new InvalidOperationException();

        Files ??= new();
        Files[name] = new File(size, name, false, this);
    }

    public IEnumerable<File> GetAllRecursiveFiles() =>
        MoreEnumerable.TraverseBreadthFirst(this, t => t.IsDirectory ? t.Files.Values : Enumerable.Empty<File>());

    public long CalculateTotalSize()
    {
        if (!IsDirectory) return Size;

        return TotalSize ??= GetAllRecursiveFiles()
            .Select(t => (long)t.Size)
            .Sum();
    }
}
