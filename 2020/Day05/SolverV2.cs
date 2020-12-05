using System;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2020.Day05
{
    class SolverV2
    {
        public SolverV2()
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
                .ToHashSet();

            var mine = seatIDs.Single(id => !seatIDs.Contains(id + 1) && seatIDs.Contains(id + 2)) + 1;

            return mine.ToString();
        }

        private int GetSeatID(string value)
        {
            var row = GetIndex(value[..^3], 'B');
            var column = GetIndex(value[^3..], 'R');
            return GetSeatID(row, column);
        }

        private int GetSeatID(int row, int column)
        {
            return row * 8 + column;
        }

        private int GetIndex(string value, char one)
        {
            int result = 0;
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == one)
                    result |= (1 << value.Length - i - 1);
            }
            return result;
        }
    }
}
