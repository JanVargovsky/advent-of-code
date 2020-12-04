using System;
using System.IO;
using AdventOfCode.Year2020.Day04;

var solver = new Solver();

var day = "Day04";
var path = Path.Combine(day, "input.txt");
var input = await File.ReadAllTextAsync(path);
Console.WriteLine(solver.Solve(input));
