namespace AdventOfCode.Year2022.Day02;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
A Y
B X
C Z
""") == "15");
    }

    public string Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);

        var score = 0;

        foreach (var row in rows)
        {
            var opponent = Parse(row[0]);
            var me = Parse(row[^1]);

            var result = GetResult(me, opponent);

            score += (int)result;
            score += (int)me;
        }

        return score.ToString();

        Plays Parse(char c)
        {
            return c switch
            {
                'A' or 'X' => Plays.Rock,
                'B' or 'Y' => Plays.Paper,
                'C' or 'Z' => Plays.Scissors,
                _ => throw new ArgumentException()
            };
        }

        BattleResult GetResult(Plays p1, Plays p2)
        {
            return (p1, p2) switch
            {
                (Plays.Rock, Plays.Rock) => BattleResult.Draw,
                (Plays.Rock, Plays.Paper) => BattleResult.Lose,
                (Plays.Rock, Plays.Scissors) => BattleResult.Win,

                (Plays.Paper, Plays.Rock) => BattleResult.Win,
                (Plays.Paper, Plays.Paper) => BattleResult.Draw,
                (Plays.Paper, Plays.Scissors) => BattleResult.Lose,

                (Plays.Scissors, Plays.Rock) => BattleResult.Lose,
                (Plays.Scissors, Plays.Paper) => BattleResult.Win,
                (Plays.Scissors, Plays.Scissors) => BattleResult.Draw,

                _ => throw new ArgumentException()
            };
        }
    }
}

file enum Plays
{
    Rock = 1,
    Paper = 2,
    Scissors = 3,
}

file enum BattleResult
{
    Win = 6,
    Lose = 0,
    Draw = 3,
}
