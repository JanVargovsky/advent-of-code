using System;
using System.Diagnostics;
using System.IO;
using AdventOfCode.Year2020.Day01;

var solver = new Solver();

//Debug.Assert(solver.Solve(@"1721
//979
//366
//299
//675
//1456") == "514579");

var day = "Day01";
var path = Path.Combine(day, "input.txt");
var input = await File.ReadAllTextAsync(path);
Console.WriteLine(solver.Solve(input));
