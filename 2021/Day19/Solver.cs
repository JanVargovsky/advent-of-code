using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Year2021.Day19;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"--- scanner 0 ---
0,2
4,1
3,3

--- scanner 1 ---
-1,-1
-5,0
-2,1", 3) == "3");

        Debug.Assert(Solve(@"--- scanner 0 ---
0,0
1,1
10,10

--- scanner 1 ---
2,-2
3,-3
100,-100", 2) == "4");

        Debug.Assert(Solve(@"--- scanner 0 ---
-1,-1,1
-2,-2,2
-3,-3,3
-2,-3,1
5,6,-4
8,0,7

--- scanner 0 ---
1,-1,1
2,-2,2
3,-3,3
2,-1,3
-5,4,-6
-8,-7,0

--- scanner 0 ---
-1,-1,-1
-2,-2,-2
-3,-3,-3
-1,-3,-2
4,6,5
-7,0,8

--- scanner 0 ---
1,1,-1
2,2,-2
3,3,-3
1,3,-2
-4,-6,5
7,0,8

--- scanner 0 ---
1,1,1
2,2,2
3,3,3
3,1,2
-6,-4,-5
0,7,-8

--- scanner 0 shifted 1,1,1 ---
2,2,2
3,3,3
4,4,4
4,2,3
-5,-3,-4
1,8,-7", 6) == "6");

        Debug.Assert(Solve(@"--- scanner 0 ---
404,-588,-901
528,-643,409
-838,591,734
390,-675,-793
-537,-823,-458
-485,-357,347
-345,-311,381
-661,-816,-575
-876,649,763
-618,-824,-621
553,345,-567
474,580,667
-447,-329,318
-584,868,-557
544,-627,-890
564,392,-477
455,729,728
-892,524,684
-689,845,-530
423,-701,434
7,-33,-71
630,319,-379
443,580,662
-789,900,-551
459,-707,401

--- scanner 1 ---
686,422,578
605,423,415
515,917,-361
-336,658,858
95,138,22
-476,619,847
-340,-569,-846
567,-361,727
-460,603,-452
669,-402,600
729,430,532
-500,-761,534
-322,571,750
-466,-666,-811
-429,-592,574
-355,545,-477
703,-491,-529
-328,-685,520
413,935,-424
-391,539,-444
586,-435,557
-364,-763,-893
807,-499,-711
755,-354,-619
553,889,-390

--- scanner 2 ---
649,640,665
682,-795,504
-784,533,-524
-644,584,-595
-588,-843,648
-30,6,44
-674,560,763
500,723,-460
609,671,-379
-555,-800,653
-675,-892,-343
697,-426,-610
578,704,681
493,664,-388
-671,-858,530
-667,343,800
571,-461,-707
-138,-166,112
-889,563,-600
646,-828,498
640,759,510
-630,509,768
-681,-892,-333
673,-379,-804
-742,-814,-386
577,-820,562

--- scanner 3 ---
-589,542,597
605,-692,669
-500,565,-823
-660,373,557
-458,-679,-417
-488,449,543
-626,468,-788
338,-750,-386
528,-832,-391
562,-778,733
-938,-730,414
543,643,-506
-524,371,-870
407,773,750
-104,29,83
378,-903,-323
-778,-728,485
426,699,580
-438,-605,-362
-469,-447,-387
509,732,623
647,635,-688
-868,-804,481
614,-800,639
595,780,-596

--- scanner 4 ---
727,592,562
-293,-554,779
441,611,-461
-714,465,-776
-743,427,-804
-660,-479,-426
832,-632,460
927,-485,-438
408,393,-506
466,436,-512
110,16,151
-258,-428,682
-393,719,612
-211,-452,876
808,-476,-593
-575,615,604
-485,667,467
-680,325,-822
-627,-443,-432
872,-547,-609
833,512,582
807,604,487
839,-516,451
891,-625,532
-652,-548,-490
30,-46,-14") == "79");
    }

    public string Solve(string input, int minOverlap = 12)
    {
        var items = input.Split(Environment.NewLine);
        var scanners = ParseScanners(items);
        var dimensions = scanners[0].Points[0].Dimension;
        var rotations = Rotations().ToList();

        var overlaps = new Dictionary<Scanner, (Scanner Scanner, Vector Offset, Rotation Rotation)>();
        //overlaps[scanners[0]] = (null!, Vector.Zero(dimensions));
        var toVisit = new List<Scanner>(scanners.Skip(1));
        var visited = new List<Scanner>();
        visited.Add(scanners[0]);

        while (toVisit.Count > 0)
        {
            MapNext();
        }

        void MapNext()
        {
            foreach (var currentScanner in toVisit)
                foreach (var rotation in rotations)
                    foreach (var scanner in visited)
                    {
                        var rotatedScanner = Rotate(currentScanner, rotation);
                        if (Overlaps(scanner, rotatedScanner, out var offset))
                        {
                            overlaps[currentScanner] = (scanner, offset.Value, rotation);
                            Console.WriteLine($"{currentScanner.Id} => {scanner.Id} (rotation={rotation}, offset={offset.Value})");
                            Console.WriteLine($"Progress {visited.Count}/{scanners.Length}");
                            toVisit.Remove(currentScanner);
                            visited.Add(currentScanner);
                            return;
                        }
                    }
        }

        var points = new HashSet<Vector>();
        foreach (var scanner in scanners)
        {
            Console.WriteLine($"Adding {scanner.Id}");
            var offset = Vector.Zero(dimensions);
            var currentScanner = scanner;
            var transformed = scanner;

            while (currentScanner != null && overlaps.TryGetValue(currentScanner, out var value))
            {
                transformed = transformed with { Points = Rotate(transformed, value.Rotation).Points.Select(p => p + value.Offset).ToList() };
                currentScanner = value.Scanner;
            }
            if (currentScanner is null) currentScanner = scanner;

            Console.WriteLine($"offset {offset}");

            var movedPoints = transformed.Points;
            //foreach (var p in movedPoints)
            //{
            //    Console.WriteLine(string.Join(",", p));
            //}
            //Console.WriteLine();
            points.UnionWith(movedPoints);
        }

        var result = points.Count;
        return result.ToString();

        bool Overlaps(Scanner scannerA, Scanner scannerB, [NotNullWhen(true)] out Vector? offset)
        {
            foreach (var pa in scannerA.Points)
            {
                foreach (var pb in scannerB.Points)
                {
                    var o = pa - pb;

                    var intersectPoints = scannerB.AbsolutePoints(o).ToHashSet();
                    intersectPoints.IntersectWith(scannerA.Points);
                    if (intersectPoints.Count >= minOverlap)
                    {
                        offset = o;
                        //Console.WriteLine($"intersect points of {scannerA.Id} and {scannerB.Id} with offset {offset}");
                        //foreach (var p in intersectPoints)
                        //{
                        //    Console.WriteLine(string.Join(",", p));
                        //}
                        //Console.WriteLine();
                        return true;
                    }
                }
            }

            offset = null;
            return false;

        }

        IEnumerable<Rotation> Rotations()
        {
            if (dimensions == 2)
            {
                var allAxes = MoreLinq.MoreEnumerable.Permutations(new[] { 0, 1 });
                var flips = new[] { false, true };

                foreach (var axes in allAxes)
                    foreach (var flip0 in flips)
                        foreach (var flip1 in flips)
                            yield return new((axes[0], flip0), (axes[1], flip1));
            }
            else if (dimensions == 3)
            {
                var allAxes = MoreLinq.MoreEnumerable.Permutations(new[] { 0, 1, 2 });
                var flips = new[] { false, true };

                foreach (var axes in allAxes)
                    foreach (var flip0 in flips)
                        foreach (var flip1 in flips)
                            foreach (var flip2 in flips)
                                yield return new((axes[0], flip0), (axes[1], flip1), (axes[2], flip2));
            }
        }

        Scanner Rotate(Scanner scanner, Rotation rotation)
        {
            return scanner with { Points = scanner.Points.Select(p => p * rotation).ToList() };
        }

        Scanner[] ParseScanners(string[] items)
        {
            var scannerIndex = 0;
            var scanners = new List<List<Vector>>();
            for (int i = 1; i < items.Length; i++)
            {
                if (items[i] == string.Empty)
                {
                    scannerIndex++;
                    i++;
                    continue;
                }

                var p = items[i].Split(',').Select(int.Parse).ToArray();
                if (scannerIndex >= scanners.Count)
                    scanners.Add(new());
                scanners[scannerIndex].Add(new Vector(p));
            }

            return scanners.Select((p, id) => new Scanner(id, p)).ToArray();
        }
    }

    [DebuggerDisplay("Scanner {Id}")]
    record Scanner(int Id, List<Vector> Points)
    {
        public IEnumerable<Vector> AbsolutePoints(Vector offset)
        {
            return Points.Select(p => p + offset);
        }
    }

    struct Vector : IEquatable<Vector>
    {
        public int[] Values { get; }

        public int Dimension => Values.Length;

        public Vector(params int[] values)
        {
            Values = values;
        }

        public static Vector Zero(int dimensions) => new(Enumerable.Repeat(0, dimensions).ToArray());

        public static Vector operator -(Vector a)
        {
            return new(Enumerable.Range(0, a.Dimension).Select(i => -a.Values[i]).ToArray());
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new(Enumerable.Range(0, a.Dimension).Select(i => a.Values[i] - b.Values[i]).ToArray());
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new(Enumerable.Range(0, a.Dimension).Select(i => a.Values[i] + b.Values[i]).ToArray());
        }

        public static Vector operator *(Vector vector, Rotation rotation)
        {
            var values = new int[vector.Dimension];
            for (int i = 0; i < vector.Dimension; i++)
            {
                if (rotation.Axes[i].Flip)
                    values[i] = -vector.Values[rotation.Axes[i].Index];
                else
                    values[i] = vector.Values[rotation.Axes[i].Index];
            }
            return new(values);
        }

        public bool Equals(Vector other)
        {
            return Dimension.Equals(other.Dimension) && Values.SequenceEqual(other.Values);
        }

        public override bool Equals(object obj)
        {
            return obj is Vector && Equals((Vector)obj);
        }

        public override int GetHashCode()
        {
            var result = Dimension.GetHashCode();
            foreach (var item in Values)
            {
                result ^= item.GetHashCode();
            }
            return result;
        }

        public override string ToString()
        {
            return string.Join(',', Values);
        }
    }

    record Rotation(params (int Index, bool Flip)[] Axes)
    {
        public override string ToString()
        {
            return string.Join(" ", Axes.Select(t => $"{(t.Flip ? '-' : '+')}{t.Index}"));
        }
    }
}
