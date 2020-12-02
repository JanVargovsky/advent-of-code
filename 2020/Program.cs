using System;
using System.Diagnostics;
using System.IO;
using AdventOfCode.Year2020.Day02;

var solver = new Solver();

Debug.Assert(solver.Solve(@"1-3 a: abcde
1-3 b: cdefg
2-9 c: ccccccccc") == "1");

var day = "Day02";
var path = Path.Combine(day, "input.txt");
var input = await File.ReadAllTextAsync(path);
Console.WriteLine(solver.Solve(input));
