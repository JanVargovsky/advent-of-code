﻿namespace AdventOfCode.Year2021.Day04;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1

22 13 17 11  0
 8  2 23  4 24
21  9 14 16  7
 6 10  3 18  5
 1 12 20 15 19

 3 15  0  2 22
 9 18 13 17  5
19  8  7 25 23
20 11 10 24  4
14 21 16 12  6

14 21 17 24  4
10 16 15  9 19
18  8 23 26 20
22 11 13  6  5
 2  0 12  3  7") == "1924");
    }

    public string Solve(string input)
    {
        var items = input.Split(Environment.NewLine + Environment.NewLine);
        var numbers = items[0].Split(',').Select(int.Parse).ToArray();
        const int boardSize = 5;
        var bingoBoards = items.Skip(1).Select(b =>
        {
            var numbers = b.Split(new[] { Environment.NewLine, " " }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).ToArray();
            return new BingoBoard(numbers, boardSize);
        }).ToList();

        var playedNumbers = new HashSet<int>(numbers.Take(boardSize - 1));
        foreach (var number in numbers.Skip(boardSize - 1))
        {
            playedNumbers.Add(number);

            foreach (var bingoBoard in bingoBoards.ToList())
            {
                if (bingoBoard.IsWinning(playedNumbers))
                {
                    bingoBoards.Remove(bingoBoard);
                    if (bingoBoards.Count == 0)
                        return bingoBoard.Score(playedNumbers, number).ToString();
                }
            }
        }

        throw new Exception();
    }

    record BingoBoard(int[] Board, int Length)
    {
        public bool IsWinning(HashSet<int> numbers)
        {
            for (int i = 0; i < Length; i++)
            {
                if (CheckRow(i) || CheckColumn(i))
                    return true;
            }
            return false;

            bool CheckRow(int row)
            {
                for (int i = 0; i < Length; i++)
                {
                    if (!numbers.Contains(Board[row * Length + i]))
                        return false;
                }
                return true;
            }

            bool CheckColumn(int column)
            {
                for (int i = 0; i < Length; i++)
                {
                    if (!numbers.Contains(Board[i * Length + column]))
                        return false;
                }
                return true;
            }
        }

        public int Score(HashSet<int> numbers, int lastNumber)
        {
            var remaining = Board.Where(t => !numbers.Contains(t)).Sum();
            return remaining * lastNumber;
        }
    }
}