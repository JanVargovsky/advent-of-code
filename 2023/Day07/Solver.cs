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
        {
            var hand = new Hand("QJJQ2", 0);
            Debug.Assert(hand.HandResult is HandResult.FourOfKind);
        }
        {
            var hand1 = new Hand("JKKK2", 0);
            Debug.Assert(hand1.HandResult is HandResult.FourOfKind);
            var hand2 = new Hand("QQQQ2", 0);
            Debug.Assert(hand2.HandResult is HandResult.FourOfKind);
            Debug.Assert(comparer.Compare(hand1, hand2) < 0);
        }
        {
            var hand = new Hand("JJJ22", 0);
            Debug.Assert(hand.HandResult is HandResult.FiveOfKind);
        }
        {
            var hand = new Hand("JJ22X", 0);
            Debug.Assert(hand.HandResult is HandResult.FourOfKind);
        }
        {
            var hand1 = new Hand("2JJJJ", 0);
            Debug.Assert(hand1.HandResult is HandResult.FiveOfKind);
            var hand2 = new Hand("JJJJ2", 0);
            Debug.Assert(hand2.HandResult is HandResult.FiveOfKind);
            Debug.Assert(comparer.Compare(hand1, hand2) > 0);
        }

        Debug.Assert(Solve("""
32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483
""") == 5905);

        Debug.Assert(Solve("""
2345A 2
2345J 5
J345A 3
32T3K 7
T55J5 17
KK677 11
KTJJT 23
QQQJA 19
JJJJJ 29
JAAAA 37
AAAAJ 43
AAAAA 53
2AAAA 13
2JJJJ 41
JJJJ2 31
""") == 3667);
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
        private const string Cards = "AKQT98765432J";

        public int Compare(Hand? x, Hand? y)
        {
            if (x == y) return 0;

            var result = x.HandResult.CompareTo(y.HandResult);
            if (result is not 0)
                return result;

            for (int i = 0; i < x.Cards.Length; i++)
            {
                var indexX = Cards.IndexOf(x.Cards[i]);
                Debug.Assert(indexX >= 0);
                var indexY = Cards.IndexOf(y.Cards[i]);
                Debug.Assert(indexX >= 0);
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
            var hasJokers = groups.Remove('J', out var jokersCount);
            if (groups.Count == 0)
                return HandResult.FiveOfKind;

            var highestPair = groups.MaxBy(t => t.Value);
            var secondHighestCount = groups.Count >= 2 ? groups.Where(t => t.Key != highestPair.Key).Max(t => t.Value) : 0;
            var result = (highestPair.Value + jokersCount) switch
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
