using System.Reflection;
using TextCopy;

const int year = 2021;
var day = args.Length > 0 ? int.Parse(args[0]) : DateTime.UtcNow.Day;
var dayFolder = $"Day{day:D2}";
var solverType = Assembly.GetExecutingAssembly().GetType($"AdventOfCode.Year{year}.{dayFolder}.Solver");
dynamic solver = Activator.CreateInstance(solverType);

var input = await GetOrDownloadInputAsync();
var result = solver.Solve(input);
Console.WriteLine(result);
await ClipboardService.SetTextAsync(result);

async Task<string> GetOrDownloadInputAsync()
{
    var path = Path.Combine(dayFolder, "input.txt");
    if (!File.Exists(path))
    {
        const string AOCSESSION = "AOCSESSION";
        var session = Environment.GetEnvironmentVariable(AOCSESSION) ?? throw new ArgumentException($"Env. variable '{AOCSESSION}' is missing.");
        using HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("Cookie", $"session={session}");
        var input = await httpClient.GetStringAsync($"https://adventofcode.com/{year}/day/{day}/input");
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        // downloaded input is using Unix line ending, so we convert according to current OS
        await File.WriteAllTextAsync(path, string.Join(Environment.NewLine, input[..^1].Split("\n")));
    }

    return await File.ReadAllTextAsync(path);
}
