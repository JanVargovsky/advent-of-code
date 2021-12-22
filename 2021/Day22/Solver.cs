namespace AdventOfCode.Year2021.Day22;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"on x=10..12,y=10..12,z=10..12
on x=11..13,y=11..13,z=11..13
off x=9..11,y=9..11,z=9..11
on x=10..10,y=10..10,z=10..10") == "39");
        Debug.Assert(Solve(@"on x=-20..26,y=-36..17,z=-47..7
on x=-20..33,y=-21..23,z=-26..28
on x=-22..28,y=-29..23,z=-38..16
on x=-46..7,y=-6..46,z=-50..-1
on x=-49..1,y=-3..46,z=-24..28
on x=2..47,y=-22..22,z=-23..27
on x=-27..23,y=-28..26,z=-21..29
on x=-39..5,y=-6..47,z=-3..44
on x=-30..21,y=-8..43,z=-13..34
on x=-22..26,y=-27..20,z=-29..19
off x=-48..-32,y=26..41,z=-47..-37
on x=-12..35,y=6..50,z=-50..-2
off x=-48..-32,y=-32..-16,z=-15..-5
on x=-18..26,y=-33..15,z=-7..46
off x=-40..-22,y=-38..-28,z=23..41
on x=-16..35,y=-41..10,z=-47..6
off x=-32..-23,y=11..30,z=-14..3
on x=-49..-5,y=-3..45,z=-29..18
off x=18..30,y=-20..-8,z=-3..13
on x=-41..9,y=-7..43,z=-33..15
on x=-54112..-39298,y=-85059..-49293,z=-27449..7877
on x=967..23432,y=45373..81175,z=27513..53682") == "590784");
        Debug.Assert(Solve(@"on x=-20..26,y=-36..17,z=-47..7
on x=-20..33,y=-21..23,z=-26..28
on x=-22..28,y=-29..23,z=-38..16
on x=-46..7,y=-6..46,z=-50..-1
on x=-49..1,y=-3..46,z=-24..28
on x=2..47,y=-22..22,z=-23..27
on x=-27..23,y=-28..26,z=-21..29
on x=-39..5,y=-6..47,z=-3..44
on x=-30..21,y=-8..43,z=-13..34
on x=-22..26,y=-27..20,z=-29..19
off x=-48..-32,y=26..41,z=-47..-37
on x=-12..35,y=6..50,z=-50..-2
off x=-48..-32,y=-32..-16,z=-15..-5
on x=-18..26,y=-33..15,z=-7..46
off x=-40..-22,y=-38..-28,z=23..41
on x=-16..35,y=-41..10,z=-47..6
off x=-32..-23,y=11..30,z=-14..3
on x=-49..-5,y=-3..45,z=-29..18
off x=18..30,y=-20..-8,z=-3..13
on x=-41..9,y=-7..43,z=-33..15
on x=-54112..-39298,y=-85059..-49293,z=-27449..7877
on x=967..23432,y=45373..81175,z=27513..53682") == "590784");
    }

    public string Solve(string input)
    {
        var items = input.Split(Environment.NewLine).Select(l =>
        {
            var tokens = l.Split(new[] { " x=", "..", ",y=", ",z=" }, StringSplitOptions.None);
            var numbers = tokens[1..].Select(int.Parse).ToArray();
            return (tokens[0], new Cube(numbers[0], numbers[1], numbers[2], numbers[3], numbers[4], numbers[5]));
        }).ToList();

        var on = new HashSet<(int X, int Y, int Z)>();
        foreach (var (onOff, cube) in items)
        {
            if (cube.FromX < -50 || cube.ToX > 50 ||
                cube.FromY < -50 || cube.ToY > 50 ||
                cube.FromZ < -50 || cube.ToZ > 50)
            {
                continue;
            }

            if (onOff is "on")
            {
                foreach (var p in cube.GetPoints())
                {
                    on.Add(p);
                }
            }
            else
            {
                foreach (var p in cube.GetPoints())
                {
                    on.Remove(p);
                }
            }
        }
        var result = on.Count;
        return result.ToString();
    }

    record Cube(int FromX, int ToX, int FromY, int ToY, int FromZ, int ToZ)
    {
        public IEnumerable<(int X, int Y, int Z)> GetPoints()
        {
            for (int x = FromX; x <= ToX; x++)
                for (int y = FromY; y <= ToY; y++)
                    for (int z = FromZ; z <= ToZ; z++)
                        yield return (x, y, z);
        }
    }
}
