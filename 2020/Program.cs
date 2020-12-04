using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

var dayNumber = args.Length > 0 ? int.Parse(args[0]) : DateTime.UtcNow.Day;
var day = $"Day{dayNumber:D2}";
var solverType = Assembly.GetExecutingAssembly().GetType($"AdventOfCode.Year2020.{day}.Solver");
dynamic solver = Activator.CreateInstance(solverType);

var input = await GetOrDownloadInputAsync();
Console.WriteLine(solver.Solve(input));

async Task<string> GetOrDownloadInputAsync()
{
    var path = Path.Combine(day, "input.txt");
    if (!File.Exists(path))
    {
        const string AOCSESSION = "AOCSESSION";
        var session = Environment.GetEnvironmentVariable(AOCSESSION) ?? throw new NullReferenceException($"Env. variable {AOCSESSION} is missing.");
        using HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("Cookie", $"session={session}");
        var input = await httpClient.GetStringAsync($"https://adventofcode.com/2020/day/{dayNumber}/input");
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        // downloaded input is using Unix line ending, so we convert according to current OS
        await File.WriteAllTextAsync(path, string.Join(Environment.NewLine, input[..^1].Split("\n")));
    }

    return await File.ReadAllTextAsync(path);
}
