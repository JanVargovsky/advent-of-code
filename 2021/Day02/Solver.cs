namespace AdventOfCode.Year2021.Day02;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"forward 5
down 5
forward 8
up 3
down 8
forward 2") == "900");
    }

    public string Solve(string input)
    {
        var commands = input.Split(Environment.NewLine);
        var depth = 0;
        var horizontal = 0;
        var aim = 0;

        foreach (var item in commands)
        {
            var tokens = item.Split(' ');
            var command = tokens[0];
            var number = int.Parse(tokens[1]);
            if (command == "forward")
            {
                horizontal += number;
                depth += (aim * number);
            }
            else if (command == "down")
            {
                aim += number;
            }
            else if (command == "up")
            {
                aim -= number;
            }
        }

        var result = (horizontal * depth).ToString();
        return result;
    }
}
