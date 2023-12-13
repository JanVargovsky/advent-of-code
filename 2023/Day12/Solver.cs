using System.Diagnostics.CodeAnalysis;
using MoreLinq;

namespace AdventOfCode.Year2023.Day12;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
???.### 1,1,3
.??..??...?##. 1,1,3
?#?#?#?#?#?#?#? 1,3,1,6
????.#...#... 4,1,1
????.######..#####. 1,6,5
?###???????? 3,2,1
""") == 525152);
    }

    public long Solve(string input)
    {
        var springs = input.Split(Environment.NewLine).Select(Parse).ToArray();
        var results = springs.Select(ArrangementsCount);
        var result = results.Sum();
        return result;
    }

    private Spring Parse(string row)
    {
        var tokens = row.Split(' ', ',');
        var unfold = 5;
        var value = string.Join('?', Enumerable.Range(0, unfold).Select(_ => tokens[0]));
        var numbers = tokens[1..].Select(int.Parse).ToArray().Repeat(5).ToArray();
        return new(value, numbers);
    }

    private long ArrangementsCount(Spring spring)
    {
        var dp = new Dictionary<Spring, long>(new SpringComparer());
        var result = ArrangementsCountInternalWithDpCache(spring);
        return result;

        long ArrangementsCountInternalWithDpCache(Spring spring)
        {
            if (dp.TryGetValue(spring, out var result))
                return result;
            result = ArrangementsCountInternal(spring);
            dp.Add(spring, result);
            return result;
        }

        long ArrangementsCountInternal(Spring spring)
        {
            if (spring.Value.Length == 0)
                return spring.GroupSizes.Length == 0 ? 1 : 0;

            if (spring.GroupSizes.Length == 0)
            {
                var invalid = spring.Value.Any(t => t == '#');
                return invalid ? 0 : 1;
            }

            if (spring.Value.Length < spring.GroupSizes.Sum())
                return 0;

            if (spring.Value[0] == '.')
            {
                return ArrangementsCountInternalWithDpCache(spring with
                {
                    Value = spring.Value[1..]
                });
            }
            else if (spring.Value[0] == '#')
            {
                var group = spring.GroupSizes[0];
                var invalid = spring.Value[..group].Any(t => t == '.');
                if (invalid)
                    return 0;

                if (group < spring.Value.Length && spring.Value[group] == '#')
                    return 0;

                return ArrangementsCountInternalWithDpCache(spring with
                {
                    Value = group + 1 < spring.Value.Length ? spring.Value[(group + 1)..] : string.Empty,
                    GroupSizes = spring.GroupSizes[1..]
                });
            }
            else if (spring.Value[0] == '?')
            {
                var a = spring with
                {
                    Value = '.' + spring.Value[1..],
                };
                var b = spring with
                {
                    Value = '#' + spring.Value[1..],
                };
                return ArrangementsCountInternalWithDpCache(a) + ArrangementsCountInternalWithDpCache(b);
            }
            else
                throw new ItWontHappenException();
        }
    }

    private record Spring(string Value, int[] GroupSizes);

    private class SpringComparer : IEqualityComparer<Spring>
    {
        public bool Equals(Spring? x, Spring? y)
        {
            return x.Value.Length == y.Value.Length &&
                x.GroupSizes.Length == y.GroupSizes.Length &&
                x.Value.Equals(y.Value) &&
                x.GroupSizes.SequenceEqual(y.GroupSizes);
        }

        public int GetHashCode([DisallowNull] Spring obj)
        {
            var result = obj.Value.GetHashCode();
            foreach (var item in obj.GroupSizes)
                result = HashCode.Combine(result, item);
            return result;
        }
    }
}
