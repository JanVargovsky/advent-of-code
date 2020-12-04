using System;
using System.IO;
using System.Reflection;

var dayNumber = args.Length > 0 ? args[0] : DateTime.UtcNow.Day.ToString("D2");
var day = $"Day{dayNumber}";
var solverType = Assembly.GetExecutingAssembly().GetType($"AdventOfCode.Year2020.{day}.Solver");
dynamic solver = Activator.CreateInstance(solverType);

var path = Path.Combine(day, "input.txt");
var input = await File.ReadAllTextAsync(path);
Console.WriteLine(solver.Solve(input));
