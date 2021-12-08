namespace AdventOfCode.Year2021.Day08;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab |
cdfeb fcadb cdfeb cdbaf") == "5353");
        Debug.Assert(Solve(@"be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb |
fdgacbe cefdb cefbgd gcbe
edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec |
fcgedb cgb dgebacf gc
fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef |
cg cg fdcagb cbg
fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega |
efabcd cedba gadfec cb
aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga |
gecf egdcabf bgf bfgea
fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf |
gebdcfa ecba ca fadegcb
dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf |
cefg dcbef fcge gbcadfe
bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd |
ed bcgafe cdgba cbgef
egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg |
gbdfcae bgc cg cgb
gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc |
fgae cfgab fg bagce") == "61229");
    }

    public string Solve(string input)
    {
        var items = input.Split(new[] { " | ", $"|{Environment.NewLine}", Environment.NewLine, "|" }, StringSplitOptions.None);
        var digits = new List<(string[] SignalPatterns, string[] OutputValues)>();
        var result = 0;

        for (int i = 0; i < items.Length; i += 2)
        {
            var uniqueSignalPatterns = items[i].Trim().Split(' ').Select(t => new string(t.OrderBy(c => c).ToArray())).ToArray();
            var digitOutputValues = items[i + 1].Trim().Split(' ').Select(t => new string(t.OrderBy(c => c).ToArray())).ToArray();
            digits.Add((uniqueSignalPatterns, digitOutputValues));
            var number = Decode(uniqueSignalPatterns, digitOutputValues);
            result += number;
        }

        return result.ToString();

        int Decode(string[] signalPatterns, string[] outputValues)
        {
            var decodedSignalPatterns = signalPatterns.ToDictionary(t => t, _ => -1);

            //_ = new[]
            //{
            //    "cf", // 1
            //    "acf", // 7
            //    "bcdf", // 4
            //    "abcdefg", // 8

            //    "abcdfg", // 9
            //    "abcefg", // 0
            //    "abdefg", // 6

            //    "acdfg", // 3
            //    "acdeg", // 2
            //    "abdfg", // 5
            //};

            for (int i = 0; i < signalPatterns.Length; i++)
            {
                if (signalPatterns[i].Length == 2)
                    decodedSignalPatterns[signalPatterns[i]] = 1;
                else if (signalPatterns[i].Length == 4)
                    decodedSignalPatterns[signalPatterns[i]] = 4;
                else if (signalPatterns[i].Length == 3)
                    decodedSignalPatterns[signalPatterns[i]] = 7;
                else if (signalPatterns[i].Length == 7)
                    decodedSignalPatterns[signalPatterns[i]] = 8;
            }

            //"cd" // 9
            //"ce" // 0
            //"de" // 6
            var patterns6 = signalPatterns.Where(t => t.Length == 6).ToArray();
            var digit4 = decodedSignalPatterns.First(t => t.Value == 4);
            // 4 => 9
            var patternDigit9 = patterns6.First(t => digit4.Key.All(c => t.Contains(c)));
            decodedSignalPatterns[patternDigit9] = 9;
            patterns6 = patterns6.Where(t => t != patternDigit9).ToArray();
            var diff06 = patterns6[0].Except(patterns6[1]).First();
            var digit1 = decodedSignalPatterns.First(t => t.Value == 1);
            // 1 => 0
            if (digit1.Key.Contains(diff06))
            {
                decodedSignalPatterns[patterns6[0]] = 0;
                decodedSignalPatterns[patterns6[1]] = 6;
            }
            else
            {
                decodedSignalPatterns[patterns6[1]] = 0;
                decodedSignalPatterns[patterns6[0]] = 6;
            }

            //"cf" // 3
            //"ce" // 2
            //"bf" // 5
            var patterns5 = signalPatterns.Where(t => t.Length == 5).ToArray();
            var join235 = string.Join(string.Empty, patterns5);
            var diff235 = new[]
            {
                join235.Except(patterns5[1]).Except(patterns5[2]).FirstOrDefault(),
                join235.Except(patterns5[0]).Except(patterns5[2]).FirstOrDefault(),
                join235.Except(patterns5[0]).Except(patterns5[1]).FirstOrDefault(),
            };
            // 2,5 => 3
            var digit3Index = Array.IndexOf(diff235, '\0');
            var pattern3 = patterns5[digit3Index];
            decodedSignalPatterns[pattern3] = 3;
            // 4 => 5, 2
            var diff25 = diff235.Where(t => t != '\0').ToArray();
            if (digit4.Key.Contains(diff25[0]))
            {
                var patternDigit5 = patterns5.First(t => t.Contains(diff25[0]));
                decodedSignalPatterns[patternDigit5] = 5;
                var patternDigit2 = patterns5.First(t => t.Contains(diff25[1]));
                decodedSignalPatterns[patternDigit2] = 2;
            }
            else
            {
                var patternDigit2 = patterns5.First(t => t.Contains(diff25[0]));
                decodedSignalPatterns[patternDigit2] = 2;
                var patternDigit5 = patterns5.First(t => t.Contains(diff25[1]));
                decodedSignalPatterns[patternDigit5] = 5;
            }


            var decoded = string.Join(string.Empty, outputValues.Select(t => decodedSignalPatterns[t]));
            var number = int.Parse(decoded);
            return number;
        }
    }
}