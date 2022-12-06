namespace AdventOfCode.Year2022.Day06;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
mjqjpqmgbljsphdztnvjfqwrcgsmlb
""", 4) == 7);
        Debug.Assert(Solve("""
bvwbjplbgvbhsrlpgdmjqwftvncz
""", 4) == 5);
        Debug.Assert(Solve("""
nppdvjthqldpwncqszvftbrmjlhg
""", 4) == 6);
        Debug.Assert(Solve("""
nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg
""", 4) == 10);
        Debug.Assert(Solve("""
zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw
""", 4) == 11);

        Debug.Assert(Solve("""
mjqjpqmgbljsphdztnvjfqwrcgsmlb
""") == 19);
        Debug.Assert(Solve("""
bvwbjplbgvbhsrlpgdmjqwftvncz
""") == 23);
        Debug.Assert(Solve("""
nppdvjthqldpwncqszvftbrmjlhg
""") == 23);
        Debug.Assert(Solve("""
nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg
""") == 29);
        Debug.Assert(Solve("""
zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw
""") == 26);
    }

    public int Solve(string input, int markerLength = 14)
    {
        var i = markerLength;
        while (i < input.Length && input[(i - markerLength)..i].ToHashSet().Count != markerLength)
        {
            i++;
        }

        return i;
    }
}
