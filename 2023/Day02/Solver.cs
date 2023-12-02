namespace AdventOfCode.Year2023.Day02;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
""") == 2286);
    }

    public int Solve(string input)
    {
        var games = input.Split(Environment.NewLine).Select(Parse);
        var result = games.Select(t => t.GetFewestRequiredGameSet()).Sum(t => t.Data.Aggregate(1, (acc, v) => acc * v.Value));
        return result;

        Game Parse(string row)
        {
            var parts = row.Split([": ", "; "], StringSplitOptions.None);
            var gameId = int.Parse(parts[0]["Game ".Length..]);
            var sets = new GameSet[parts.Length - 1];
            for (int i = 0; i < sets.Length; i++)
            {
                var data = parts[i + 1]
                    .Split(", ")
                    .Select(t =>
                    {
                        var setTokens = t.Split(' ');
                        var count = int.Parse(setTokens[0]);
                        var color = setTokens[1];
                        return new KeyValuePair<string, int>(color, count);
                    }).ToDictionary();
                sets[i] = new(data);
            }

            return new Game(gameId, sets);
        }
    }

    private record Game(int Id, GameSet[] Sets)
    {
        public bool IsPossible(GameSet gameSet) =>
            Sets.All(t => t.IsPossible(gameSet));

        public GameSet GetFewestRequiredGameSet()
        {
            var data = Sets
                .SelectMany(t => t.Data)
                .GroupBy(t => t.Key, t => t)
                .Select(t => t.MaxBy(t => t.Value))
                .ToDictionary();
            return new(data);
        }
    }

    private record GameSet(Dictionary<string, int> Data)
    {
        public bool IsPossible(GameSet gameSet)
        {
            foreach (var (color, count) in Data)
                if (!gameSet.Data.TryGetValue(color, out var gameCount) || count > gameCount)
                    return false;

            return true;
        }
    }
}
