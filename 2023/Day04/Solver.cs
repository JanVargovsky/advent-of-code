using System.Collections.Immutable;

namespace AdventOfCode.Year2023.Day04;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11
""") == 30);
    }

    public int Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var scratchcards = rows.Select(Parse).ToList();
        List<int> points = [1, 1];
        while (points.Count <= scratchcards[0].Winning.Count)
            points.Add(points[^1] * 2);
        scratchcards.ForEach(card => (card.WinningCards, card.Points) = CalculatePoints(card));

        for (int i = 0; i < scratchcards.Count; i++)
        {
            for (int j = 1; j <= scratchcards[i].WinningCards; j++)
            {
                scratchcards[i + j].Count += (scratchcards[i].Count);
            }
        }

        var result = scratchcards.Sum(t => t.Count);
        return result;

        (int Cards, int Points) CalculatePoints(Scratchcard card)
        {
            var winning = card.Winning.Intersect(card.Owned);
            var winningPoints = winning.Select((_, index) => points[index]).Sum();
            return (winning.Count, winningPoints);
        }

        Scratchcard Parse(string row)
        {
            var parts = row.Split(':', '|');
            var id = int.Parse(parts[0].Split(' ')[^1]);
            var winning = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            var owned = parts[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            return new Scratchcard(id, ImmutableHashSet.Create(winning), ImmutableHashSet.Create(owned));
        }
    }

    private record Scratchcard(int Id, ImmutableHashSet<int> Winning, ImmutableHashSet<int> Owned)
    {
        public int WinningCards { get; set; }
        public int Points { get; set; }
        public int Count { get; set; } = 1;
    }
}
