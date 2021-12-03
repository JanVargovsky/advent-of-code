namespace AdventOfCode.Year2021.Day03;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"00100
11110
10110
10111
10101
01111
00111
11100
10000
11001
00010
01010") == "230");
    }

    public string Solve(string input)
    {
        var numbers = input.Split(Environment.NewLine);
        var generatorString = Determine(numbers, true);
        var scrubberString = Determine(numbers, false);

        var generator = Convert.ToInt32(generatorString, 2);
        var scrubber = Convert.ToInt32(scrubberString, 2);

        var result = (generator * scrubber).ToString();
        return result;

        static string Determine(IList<string> numbers, bool searchMostCommon)
        {
            var i = 0;
            while (numbers.Count != 1)
            {
                var one = numbers.Count(t => t[i] == '1');
                var zero = numbers.Count - one;
                var bit = searchMostCommon switch
                {
                    true => one > zero ? '1' : '0',
                    false => one > zero ? '0' : '1'
                };
                if (one == zero) bit = searchMostCommon ? '1' : '0';
                var filteredNumbers = numbers.Where(t => t[i] == bit).ToArray();
                numbers = filteredNumbers;
                i++;
            }

            return numbers[0];
        }
    }
}
