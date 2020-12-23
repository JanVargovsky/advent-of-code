using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2020.Day23
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Solve(@"389125467", 10) == "92658374");
            Debug.Assert(Solve(@"389125467") == "67384529");
        }

        public string Solve(string input, int moves = 100)
        {
            var cups = new LinkedList<int>(input.Select(c => c - '0'));

            var currentCup = cups.First;
            var removedCups = new LinkedListNode<int>[3];
            var maxCup = cups.Max();

            for (int move = 1; move <= moves; move++)
            {
                removedCups[0] = currentCup.Next ?? cups.First;
                removedCups[1] = removedCups[0].Next ?? cups.First;
                removedCups[2] = removedCups[1].Next ?? cups.First;

                var destinationCup = currentCup.Value - 1;

                while (destinationCup == 0 || removedCups.Any(c => c.Value == destinationCup))
                {
                    if (destinationCup == 0)
                    {
                        destinationCup = maxCup;
                        continue;
                    }
                    destinationCup--;
                }

                Console.WriteLine($"-- move {move} --");
                Console.WriteLine("cups: " + string.Join("", cups.Select(c => c == currentCup.Value ? $"({c})" : $" {c} ")));
                Console.WriteLine("pick up: " + string.Join(", ", removedCups.Select(c => c.Value)));
                Console.WriteLine("destination: " + destinationCup);
                Console.WriteLine();

                foreach (var cupToRemove in removedCups)
                {
                    cups.Remove(cupToRemove);
                }

                var destinationCupNode = cups.Find(destinationCup);
                cups.AddAfter(destinationCupNode, removedCups[0]);

                destinationCupNode = destinationCupNode.Next;
                cups.AddAfter(destinationCupNode, removedCups[1]);

                destinationCupNode = destinationCupNode.Next;
                cups.AddAfter(destinationCupNode, removedCups[2]);

                currentCup = currentCup.Next ?? cups.First;
            }

            var first = cups.Find(1).Next ?? cups.First;
            string result = null;
            for (int i = 0; i < input.Length - 1; i++)
            {
                result += first.Value;
                first = first.Next ?? cups.First;
            }

            return result.ToString();
        }
    }
}
