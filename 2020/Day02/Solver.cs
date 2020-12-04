﻿using System;
using System.Linq;

namespace AdventOfCode.Year2020.Day02
{
    class Solver
    {
        public string Solve(string input)
        {
            var passwords = input.Split(Environment.NewLine)
                .Select(line =>
                {
                    var parts = line.Split(new[] { '-', ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);
                    return new PasswordWithPolicy(
                        int.Parse(parts[0]),
                        int.Parse(parts[1]),
                        parts[2][0],
                        parts[3]
                    );
                }).ToArray();

            return passwords.Count(p =>
            {
                return p.Password[p.From - 1] == p.Character ^ p.Password[p.To - 1] == p.Character;
            }).ToString();
        }

        record PasswordWithPolicy(int From, int To, char Character, string Password);
    }
}