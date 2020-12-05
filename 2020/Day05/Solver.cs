using System;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2020.Day05
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(GetSeatID(@"FBFBBFFRLR") == 357);
            Debug.Assert(GetSeatID(@"BFFFBBFRRR") == 567);
            Debug.Assert(GetSeatID(@"FFFBBBFRRR") == 119);
            Debug.Assert(GetSeatID(@"BBFFBBFRLL") == 820);
        }

        public string Solve(string input)
        {
            var seatIDs = input.Split(Environment.NewLine)
                .Select(GetSeatID)
                .ToArray();

            return seatIDs.Max().ToString();
        }

        private int GetSeatID(string value)
        {
            var row = GetIndex(value[..^3], 0, 127, 'F', 'B');
            var column = GetIndex(value[^3..], 0, 7, 'L', 'R');
            return row * 8 + column;
        }

        private int GetIndex(string value, int lower, int upper, char lowerHalf, char upperHalf)
        {
            foreach (var c in value)
            {
                if (c == lowerHalf)
                    upper = (lower + upper + 1) / 2;
                else
                    lower = (lower + upper + 1) / 2;
            }

            return lower;
        }
    }
}
