namespace AdventOfCode.Year2023.Day01;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen
""") == 281);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var result = 0;
        var numbers = new[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

        foreach (var row in rows)
        {
            var first = Find(row);
            Debug.Assert(first >= 0);
            var last = FindLast(row);
            Debug.Assert(last >= 0);

            var number = first * 10 + last;
            result += number;
        }
        return result;

        int FindLast(string row)
        {
            for (int i = row.Length - 1; i >= 0; i--)
            {
                if (char.IsDigit(row[i]))
                {
                    return row[i] - '0';
                }
                else
                {
                    for (int j = 0; j < numbers.Length; j++)
                    {
                        if (row[i..].StartsWith(numbers[j]))
                            return j + 1;
                    }
                }
            }
            return -1;
        }

        int Find(string row)
        {
            for (int i = 0; i < row.Length; i++)
            {
                if (char.IsDigit(row[i]))
                {
                    return row[i] - '0';
                }
                else
                {
                    for (int j = 0; j < numbers.Length; j++)
                    {
                        if (row[i..].StartsWith(numbers[j]))
                            return j + 1;
                    }
                }
            }
            return -1;
        }
    }
}
