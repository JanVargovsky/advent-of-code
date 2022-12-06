namespace AdventOfCode.Year2022.Day06;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
mjqjpqmgbljsphdztnvjfqwrcgsmlb
""") == 7);
        Debug.Assert(Solve("""
bvwbjplbgvbhsrlpgdmjqwftvncz
""") == 5);
        Debug.Assert(Solve("""
nppdvjthqldpwncqszvftbrmjlhg
""") == 6);
        Debug.Assert(Solve("""
nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg
""") == 10);
        Debug.Assert(Solve("""
zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw
""") == 11);
    }

    public int Solve(string input)
    {
        const int markerLength = 4;
        var i = markerLength;
        while (i < input.Length && input[(i - markerLength)..i].ToHashSet().Count != markerLength)
        {
            i++;
        }

        return i;
    }
}
