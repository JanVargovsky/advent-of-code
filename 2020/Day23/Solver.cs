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
            Debug.Assert(Solve(@"389125467") == "149245887792");
        }

        public string Solve(string input)
        {
            var firstCups = input.Select(c => c - '0').ToArray();
            var cups = new LinkedList<int>(
                firstCups
                .Concat(Enumerable.Range(firstCups.Max() + 1, 1_000_000 - firstCups.Length)));

            var currentCup = cups.First;
            var removedCups = new LinkedListNode<int>[3];
            var maxCup = cups.Max();

            var current = cups.First;
            var cupNodes = new Dictionary<int, LinkedListNode<int>>();
            while (current != null)
            {
                cupNodes[current.Value] = current;
                current = current.Next;
            }

            for (int move = 1; move <= 10_000_000; move++)
            {
                if (move % 1_000_000 == 0)
                    Console.WriteLine(move);

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

                //Console.WriteLine($"-- move {move} --");
                //Console.WriteLine("cups: " + string.Join("", cups.Select(c => c == currentCup.Value ? $"({c})" : $" {c} ")));
                //Console.WriteLine("pick up: " + string.Join(", ", removedCups.Select(c => c.Value)));
                //Console.WriteLine("destination: " + destinationCup);
                //Console.WriteLine();

                foreach (var cupToRemove in removedCups)
                {
                    cups.Remove(cupToRemove);
                }

                var destinationCupNode = cupNodes[destinationCup];
                cups.AddAfter(destinationCupNode, removedCups[0]);

                destinationCupNode = destinationCupNode.Next;
                cups.AddAfter(destinationCupNode, removedCups[1]);

                destinationCupNode = destinationCupNode.Next;
                cups.AddAfter(destinationCupNode, removedCups[2]);

                currentCup = currentCup.Next ?? cups.First;
            }

            var first = cups.Find(1).Next ?? cups.First;
            var second = first.Next ?? cups.First;
            var result = (long)first.Value * second.Value;

            return result.ToString();
        }
    }
}
