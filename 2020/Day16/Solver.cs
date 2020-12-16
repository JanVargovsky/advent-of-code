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
            Debug.Assert(Solve(@"class: 1-3 or 5-7
row: 6-11 or 33-44
seat: 13-40 or 45-50

your ticket:
7,1,14

nearby tickets:
7,3,47
40,4,50
55,2,20
38,6,12") == "71");
        }

        public string Solve(string input)
        {
            var data = input.Split(Environment.NewLine + Environment.NewLine);

            var fields = new List<Field>();
            foreach (var item in data[0].Split(Environment.NewLine))
            {
                var tokens = item.Split(' ', '-');
                fields.Add(new Field(
                    tokens[0],
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

            var result = nearbyTickets.Select(
                ticket => GetInvalidFields(ticket).Sum()
                ).Sum();
            return result.ToString();

            IEnumerable<int> GetInvalidFields(int[] ticket)
            {
                foreach (var value in ticket)
                {
                    if (fields.All(field =>
                            !field.Range1.IsValid(value) && !field.Range2.IsValid(value)))
                        yield return value;
                }
            }
        }

        record FieldRange(int From, int To)
        {
            public bool IsValid(int n) => From <= n && To >= n;
        }
        record Field(string Name, FieldRange Range1, FieldRange Range2);
    }
}
