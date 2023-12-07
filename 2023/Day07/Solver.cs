namespace AdventOfCode.Year2023.Day07;

internal class Solver
{
    public Solver()
    {
        var comparer = new HandComparer();
        {
            var hand1 = new Hand("33332", 0);
            Debug.Assert(hand1.HandResult is HandResult.FourOfKind);
            var hand2 = new Hand("2AAAA", 0);
            Debug.Assert(hand2.HandResult is HandResult.FourOfKind);
            Debug.Assert(comparer.Compare(hand1, hand2) > 0);
        }
        {
            var hand1 = new Hand("77888", 0);
            Debug.Assert(hand1.HandResult is HandResult.FullHouse);
            var hand2 = new Hand("77788", 0);
            Debug.Assert(hand2.HandResult is HandResult.FullHouse);
            Debug.Assert(comparer.Compare(hand1, hand2) > 0);
        }

        Debug.Assert(Solve("""
32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483
""") == 6440);
    }

    public long Solve(string input)
    {
        var hands = input.Split(Environment.NewLine).Select(Parse).ToArray();
        var sorted = hands.Order(new HandComparer());
        var result = sorted.Select((item, index) => item.Bid * (index + 1)).Sum();
        return result;

        Hand Parse(string row)
        {
            var items = row.Split(' ');
            return new(items[0], int.Parse(items[1]));
        }
    }

    private class HandComparer : IComparer<Hand>
    {
        private const string Cards = "AKQJT98765432";

        public int Compare(Hand? x, Hand? y)
        {
            if (x == y) return 0;

            var result = x.HandResult.CompareTo(y.HandResult);
            if (result is not 0)
                return result;

            for (int i = 0; i < x.Cards.Length; i++)
            {
                var indexX = Cards.IndexOf(x.Cards[i]);
                var indexY = Cards.IndexOf(y.Cards[i]);
                result = indexY.CompareTo(indexX); // inverse
                if (result is not 0)
                    return result;
            }

            throw new ItWontHappenException();
        }
    }

    private record Hand(string Cards, int Bid)
    {
        public HandResult HandResult { get; } = GetResult(Cards);

        private static HandResult GetResult(string cards)
        {
            var groups = cards.GroupBy(t => t, t => t).ToDictionary(t => t.Key, t => t.Count());

            var highestPair = groups.MaxBy(t => t.Value);
            var secondHighestCount = groups.Count >= 2 ? groups.Where(t => t.Key != highestPair.Key).Max(t => t.Value) : 0;
            var result = highestPair.Value switch
            {
                5 => HandResult.FiveOfKind,
                4 => HandResult.FourOfKind,
                3 when secondHighestCount is 2 => HandResult.FullHouse,
                3 => HandResult.ThreeOfKind,
                2 when secondHighestCount is 2 => HandResult.TwoPair,
                2 => HandResult.OnePair,
                _ => HandResult.HighCard,
            };

            return result;


        }
    }

    private enum HandResult
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfKind,
        FullHouse,
        FourOfKind,
        FiveOfKind,
    }
}
