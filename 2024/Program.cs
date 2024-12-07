using System.Reflection;
using TextCopy;

const int year = 2024;
var day = args.Length > 0 ? int.Parse(args[0]) : DateTime.UtcNow.Day;
var dayFolder = GetDayFolder(day);
var solverType = Assembly.GetExecutingAssembly().GetType($"AdventOfCode.Year{year}.{dayFolder}.Solver")
    ?? throw new ArgumentException("Solver not found.");
dynamic solver = Activator.CreateInstance(solverType)!;

var input = await GetOrDownloadInputAsync();
var stopwatch = Stopwatch.StartNew();
var result = Convert.ToString(solver.Solve(input));
stopwatch.Stop();
Console.WriteLine(result);
Console.WriteLine();
Console.WriteLine(stopwatch.Elapsed);
await ClipboardService.SetTextAsync(result);

async Task<string> GetOrDownloadInputAsync()
{
    var path = Path.Combine(dayFolder, "input.txt");
    if (!File.Exists(path))
    {
        const string AOCSESSION = "AOCSESSION";
        var session = Environment.GetEnvironmentVariable(AOCSESSION)
            ?? throw new ArgumentException($"Env. variable '{AOCSESSION}' is missing.");
        using HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("Cookie", $"session={session}");
        httpClient.DefaultRequestHeaders.UserAgent.Add(new("(github.com/JanVargovsky/advent-of-code by jan.vargovsky@gmail.com)"));
        var input = await httpClient.GetStringAsync($"https://adventofcode.com/{year}/day/{day}/input");
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        // downloaded input is using Unix line ending, so we convert it according to the current OS
        await File.WriteAllTextAsync(path, string.Join(Environment.NewLine, input[..^1].Split("\n")));
    }

    return await File.ReadAllTextAsync(path);
}

static string GetDayFolder(int day) => $"Day{day:D2}";

static async Task CreateSolverStructureAsync(string projectPath)
{
    const string dayFolderVariable = "<DayFolder>";
    string template = $$""""
namespace AdventOfCode.Year{{year}}.{{dayFolderVariable}};

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""

""") == "");
    }

    public string Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var result = string.Empty;
        return result;
    }
}

"""";
    const int from = 1;
    const int to = 25;
    for (int day = from; day <= to; day++)
    {
        var dayFolder = $"Day{day:D2}";
        var directoryPath = Path.Combine(projectPath, dayFolder);
        const string fileName = "Solver.cs";
        var filePath = Path.Combine(directoryPath, fileName);

        if (!File.Exists(filePath))
        {
            _ = Directory.CreateDirectory(directoryPath);
            var content = template.Replace(dayFolderVariable, dayFolder);
            Console.WriteLine($"Creating {day}");
            await File.WriteAllTextAsync(filePath, content);
        }
    }
}
