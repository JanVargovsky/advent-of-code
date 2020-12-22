using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2020.Day22
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Solve(@"Player 1:
9
2
6
3
1

Player 2:
5
8
4
7
10") == "306");
        }

        public string Solve(string input)
        {
            var decks = input.Split(Environment.NewLine + Environment.NewLine)
                .Select(t => new Queue<int>(t.Split(Environment.NewLine)[1..].Select(int.Parse)))
                .ToArray();
            var player1 = decks[0];
            var player2 = decks[1];

            while (player1.Count > 0 && player2.Count > 0)
            {
                var play1 = player1.Dequeue();
                var play2 = player2.Dequeue();

                if (play1 > play2)
                {
                    player1.Enqueue(play1);
                    player1.Enqueue(play2);
                }
                else
                {
                    player2.Enqueue(play2);
                    player2.Enqueue(play1);
                }
            }

            var winner = player1.Count > 0 ? player1 : player2;
            var result = 0L;
            while (winner.Count > 0)
            {
                result += (winner.Count * winner.Dequeue());
            }
            return result.ToString();
        }
    }
}
