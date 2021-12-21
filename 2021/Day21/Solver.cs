namespace AdventOfCode.Year2021.Day21;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"Player 1 starting position: 4
Player 2 starting position: 8") == "444356092776315");
    }

    public string Solve(string input)
    {
        var players = input.Split(Environment.NewLine).Select(t => t[(t.IndexOf(": ") + 2)..]).Select(long.Parse).ToArray();
        var memory = new Dictionary<State, (long Wins1, long Wins2)>();
        var (wins1, wins2) = Play(new(new(players[0]), new(players[1])));
        var result = Math.Max(wins1, wins2);
        return result.ToString();

        (long, long) Play(State state)
        {
            if (memory.TryGetValue(state, out var wins))
                return wins;

            var (wins1, wins2) = (0L, 0L);
            foreach (var rollsSum in RollSums())
            {
                var player = state.CurrentPlayer;
                var space = player.Space + rollsSum;
                space = (space - 1) % 10 + 1;
                var score = player.Score + space;

                if (score >= 21)
                {
                    if (state.Player1Plays)
                        wins1++;
                    else
                        wins2++;
                }
                else
                {
                    var newPlayer = player with { Score = score, Space = space };
                    var newState = state.Player1Plays ?
                        state with { Player1 = newPlayer, Player1Plays = !state.Player1Plays } :
                        state with { Player2 = newPlayer, Player1Plays = !state.Player1Plays };
                    var subWins = Play(newState);
                    wins1 += subWins.Item1;
                    wins2 += subWins.Item2;
                }
            }

            return memory[state] = (wins1, wins2);
        }

        IEnumerable<int> RollSums()
        {
            for (int roll1 = 1; roll1 <= 3; roll1++)
                for (int roll2 = 1; roll2 <= 3; roll2++)
                    for (int roll3 = 1; roll3 <= 3; roll3++)
                        yield return roll1 + roll2 + roll3;
        }
    }

    record State(PlayerState Player1, PlayerState Player2, bool Player1Plays = true)
    {
        public PlayerState CurrentPlayer => Player1Plays ? Player1 : Player2;
    }
    record PlayerState(long Space, long Score = 0);
}