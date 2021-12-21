namespace AdventOfCode.Year2021.Day21;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"Player 1 starting position: 4
Player 2 starting position: 8") == "739785");
    }

    public string Solve(string input)
    {
        var players = input.Split(Environment.NewLine).Select(t => t[(t.IndexOf(": ") + 2)..]).Select(long.Parse).ToArray();
        var (player1, player2) = (players[0], players[1]);
        var (score1, score2) = (0L, 0L);
        var roll = 1L;
        var player1Plays = true;
        var turns = 0;
        while (score1 < 1000 && score2 < 1000)
        {
            var rollsSum = 3 * roll + 3;
            if (player1Plays)
            {
                player1 += rollsSum;
                while (player1 > 10)
                    player1 -= 10;
                score1 += player1;
                Console.WriteLine($"player 1 total score of {score1}");
            }
            else
            {
                player2 += rollsSum;
                while (player2 > 10)
                    player2 -= 10;
                score2 += player2;
                Console.WriteLine($"player 2 total score of {score2}");
            }

            roll += 3;
            turns++;
            player1Plays = !player1Plays;
        }

        var result = (player1Plays ? score1 : score2) * (roll - 1);
        return result.ToString();
    }
}