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
10") == "291");
        }

        public string Solve(string input)
        {
            var decks = input.Split(Environment.NewLine + Environment.NewLine)
                .Select(t => new Queue<int>(t.Split(Environment.NewLine)[1..].Select(int.Parse)))
                .ToArray();
            var player1 = decks[0];
            var player2 = decks[1];
            bool isPlayer1Winner;
            (player1, player2, isPlayer1Winner) = PlayRecursiveCombat(player1, player2);

            var winner = isPlayer1Winner ? player1 : player2;
            var result = 0L;
            while (winner.Count > 0)
            {
                result += (winner.Count * winner.Dequeue());
            }
            return result.ToString();
        }

        private static (Queue<int>, Queue<int>, bool) PlayRecursiveCombat(Queue<int> player1, Queue<int> player2)
        {
            var prevs = new HashSet<(int, int)>();

            while (player1.Count > 0 && player2.Count > 0)
            {
                if (!prevs.Add((QueueToHash(player1), QueueToHash(player2))))
                {
                    return (player1, player2, true);
                }

                var play1 = player1.Dequeue();
                var play2 = player2.Dequeue();

                bool isPlayer1Winner;
                // is subgame needed?
                if (player1.Count >= play1 && player2.Count >= play2)
                {
                    (_, _, isPlayer1Winner) = PlayRecursiveCombat(
                        new Queue<int>(player1.Take(play1)),
                        new Queue<int>(player2.Take(play2)));
                }
                else
                {
                    isPlayer1Winner = play1 > play2;
                }

                if (isPlayer1Winner)
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

            return (player1, player2, player1.Count != 0);

            int QueueToHash(Queue<int> q)
            {
                int hash = q.Count;
                foreach (var item in q)
                {
                    hash = HashCode.Combine(hash, item);
                }
                return hash;
            }
        }
    }
}
