using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Year2023.Day05;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4
""") == 35);
    }

    public long Solve(string input)
    {
        var segments = input.Split(Environment.NewLine + Environment.NewLine);
        var seeds = segments[0]["seeds: ".Length..].Split(' ').Select(long.Parse).ToArray();
        var maps = segments[1..].Select(ParseMap).ToArray();

        var results = seeds.ToArray();
        foreach (var map in maps)
        {
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = map.MapValue(results[i]);
            }
        }

        var result = results.Min();
        return result;
    }

    private Map ParseMap(string map)
    {
        var lines = map.Split(Environment.NewLine);
        var fromTo = lines[0].Split(["-to-", " "], StringSplitOptions.None);
        var ranges = lines[1..].Select(ParseRange)
            //.OrderBy(t => t.SourceRangeStart) // we can sort it & search mapping using binary search
            .ToArray();
        return new(fromTo[0], fromTo[1], ranges);

        MappingRange ParseRange(string line)
        {
            var values = line.Split(' ').Select(long.Parse).ToArray();
            return new(values[0], values[1], values[2]);
        }
    }

    private record Map(string From, string To, MappingRange[] Ranges)
    {
        public long MapValue(long source)
        {
            foreach (var range in Ranges)
            {
                if (range.TryGetValue(source, out var result))
                {
                    return result.Value;
                }
            }

            return source;
        }
    }

    private record MappingRange(long DestinationRangeStart, long SourceRangeStart, long RangeLength)
    {
        public bool TryGetValue(long source, [NotNullWhen(true)] out long? destination)
        {
            var offset = source - SourceRangeStart;
            if (offset >= 0 && offset <= RangeLength)
            {
                destination = DestinationRangeStart + offset;
                return true;
            }

            destination = null;
            return false;
        }
    }
}