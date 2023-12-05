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
""") == 46);
    }

    public long Solve(string input)
    {
        var segments = input.Split(Environment.NewLine + Environment.NewLine);
        var seedRanges = segments[0]["seeds: ".Length..].Split(' ').Select(long.Parse).ToArray();
        var maps = segments[1..].Select(ParseMap).ToArray();

        var ranges = new List<Range>();
        for (int i = 0; i < seedRanges.Length; i += 2)
        {
            ranges.Add(new(seedRanges[i], seedRanges[i] + seedRanges[i + 1] - 1));
        }

        var min = 0;
        while (true)
        {
            var destination = MapDestinationInReverse(min);
            if (ranges.Any(r => r.IsInRange(destination)))
            {
                break;
            }
            min++;
        }

        var result = min;
        return result;

        long MapDestinationInReverse(long value)
        {
            for (int i = maps.Length - 1; i >= 0; i--)
                value = maps[i].MapDestinationValue(value);
            return value;
        }
    }

    private Map ParseMap(string map)
    {
        var lines = map.Split(Environment.NewLine);
        var fromTo = lines[0].Split(["-to-", " "], StringSplitOptions.None);
        var ranges = lines[1..].Select(ParseRange)
            .OrderBy(t => t.SourceRangeStart)
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

        public long MapDestinationValue(long destination)
        {
            foreach (var range in Ranges)
            {
                if (range.TryGetSourceValue(destination, out var result))
                {
                    return result.Value;
                }
            }

            return destination;
        }
    }

    private record MappingRange(long DestinationRangeStart, long SourceRangeStart, long RangeLength)
    {
        public bool TryGetValue(long source, [NotNullWhen(true)] out long? destination)
        {
            var offset = source - SourceRangeStart;
            if (offset >= 0 && offset < RangeLength)
            {
                destination = DestinationRangeStart + offset;
                return true;
            }

            destination = null;
            return false;
        }

        public bool TryGetSourceValue(long destination, [NotNullWhen(true)] out long? source)
        {
            var offset = destination - DestinationRangeStart;
            if (offset >= 0 && offset < RangeLength)
            {
                source = SourceRangeStart + offset;
                return true;
            }

            source = null;
            return false;
        }
    }

    private record Range(long From, long To)
    {
        public bool IsInRange(long value) => From <= value && value <= To;
    }
}