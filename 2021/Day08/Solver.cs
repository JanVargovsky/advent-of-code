namespace AdventOfCode.Year2021.Day08;

class Solver
{
    public Solver()
    {
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
fgae cfgab fg bagce") == "26");
    }

    public string Solve(string input)
    {
        var items = input.Split(new[] { " | ", $"|{Environment.NewLine}", Environment.NewLine, "|" }, StringSplitOptions.None);
        var digits = new List<(string[] SignalPatterns, string[] OutputValues)>();

        var result = 0;
        // 1 = 2
        // 4 = 4
        // 7 = 3
        // 8 = 7
        var searchedDigitLengths = new[] { 2, 4, 3, 7 };

        for (int i = 0; i < items.Length; i += 2)
        {
            var uniqueSignalPatterns = items[i].Trim().Split(' ');
            var digitOutputValues = items[i + 1].Trim().Split(' ');
            digits.Add((uniqueSignalPatterns, digitOutputValues));
            foreach (var outputDigit in digitOutputValues)
            {
                if (searchedDigitLengths.Contains(outputDigit.Length))
                    result++;
            }
        }

        return result.ToString();
    }
}