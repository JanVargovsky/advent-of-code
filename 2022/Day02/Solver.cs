namespace AdventOfCode.Year2022.Day02;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
A Y
B X
C Z
""") == "12");
    }

    public string Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);

        var score = 0;

        foreach (var row in rows)
        {
            var opponent = ParsePlay(row[0]);
            var result = ParseResult(row[^1]);

            var me = Enum.GetValues<Plays>().First(me => GetResult(me, opponent) == result);

            score += (int)result;
            score += (int)me;
        }

        return score.ToString();

        Plays ParsePlay(char c)
        {
            return c switch
            {
                'A' => Plays.Rock,
                'B' => Plays.Paper,
                'C' => Plays.Scissors,
                _ => throw new ArgumentException()
            };
        }

        BattleResult ParseResult(char c)
        {
            return c switch
            {
                'X' => BattleResult.Lose,
                'Y' => BattleResult.Draw,
                'Z' => BattleResult.Win,
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
