namespace AdventOfCode.Year2024.Day07;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20
""") == 11387);
    }

    public long Solve(string input)
    {
        var equations = input.Split(Environment.NewLine).Select(Parse);
        var result = equations.Sum(eq => Evaluate(eq, 0, 0) > 0 ? eq.TestValue : 0);
        return result;

        long Evaluate(Equation equation, int index, long result)
        {
            if (result > equation.TestValue)
                return 0;

            if (index >= equation.Numbers.Length)
                return equation.TestValue == result ? 1 : 0;

            return Evaluate(equation, index + 1, result + equation.Numbers[index]) +
                Evaluate(equation, index + 1, result * equation.Numbers[index]) +
                Evaluate(equation, index + 1, long.Parse($"{result}{equation.Numbers[index]}"));
        }

        Equation Parse(string row)
        {
            var numbers = row.Split([' ', ':'], StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
            return new(numbers[0], numbers[1..]);
        }
    }

    record Equation(long TestValue, long[] Numbers);

    enum Operation
    {
        Add,
        Multiply,
    }
}
