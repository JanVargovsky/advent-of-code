using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2020.Day16
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Solve(@"class: 0-1 or 4-19
row: 0-5 or 8-19
seat: 0-13 or 16-19

your ticket:
11,12,13

nearby tickets:
3,9,18
15,1,5
5,14,9") == "1");
        }

        public string Solve(string input)
        {
            var data = input.Split(Environment.NewLine + Environment.NewLine);

            var fields = new List<Field>();
            foreach (var item in data[0].Split(Environment.NewLine))
            {
                var tokens = item.Split(' ', '-');
                fields.Add(new Field(
                    item[..item.IndexOf(':')],
                    new FieldRange(int.Parse(tokens[^5]), int.Parse(tokens[^4])),
                    new FieldRange(int.Parse(tokens[^2]), int.Parse(tokens[^1]))
                    ));
            }

            var myTicket = data[1]
                .Split(Environment.NewLine)
                .Skip(1)
                .First()
                .Split(',').Select(int.Parse).ToArray();
            var nearbyTickets = data[2].Split(Environment.NewLine)
                .Skip(1)
                .Select(t => t.Split(',').Select(int.Parse).ToArray())
                .ToArray();

            nearbyTickets = nearbyTickets.Where(IsValid).ToArray();

            var possibleMatches = new HashSet<Field>[myTicket.Length];

            for (int i = 0; i < possibleMatches.Length; i++)
            {
                possibleMatches[i] = fields.Where(field => nearbyTickets.All(ticket => field.IsValid(ticket[i]))).ToHashSet();
            }

            while (true)
            {
                bool removed = false;
                for (int i = 0; i < possibleMatches.Length; i++)
                {
                    if (possibleMatches[i].Count == 1) // final position of field
                    {
                        var fieldToClear = possibleMatches[i].First();
                        for (int j = 0; j < possibleMatches.Length; j++)
                        {
                            if (i == j) continue;
                            if (possibleMatches[j].Remove(fieldToClear))
                                removed = true;
                        }
                    }
                }
                if (!removed)
                    break;
            }

            var result = 1L;
            for (int i = 0; i < possibleMatches.Length; i++)
            {
                var field = possibleMatches[i].Single();
                Console.WriteLine($"{field.Name} is {myTicket[i]}");
                if (field.Name.StartsWith("departure"))
                    result *= myTicket[i];
            }
            return result.ToString();

            bool IsValid(int[] ticket)
            {
                foreach (var value in ticket)
                {
                    if (fields.All(field =>
                            !field.Range1.IsValid(value) && !field.Range2.IsValid(value)))
                        return false;
                }
                return true;
            }
        }

        record FieldRange(int From, int To)
        {
            public bool IsValid(int n) => From <= n && To >= n;
        }
        record Field(string Name, FieldRange Range1, FieldRange Range2)
        {
            public bool IsValid(int n) => Range1.IsValid(n) || Range2.IsValid(n);
        }
    }
}
