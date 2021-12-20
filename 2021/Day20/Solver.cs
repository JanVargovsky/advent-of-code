namespace AdventOfCode.Year2021.Day20;

class Solver
{
    public Solver()
    {
        var input = "..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..##" +
"#..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###" +
".######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#." +
".#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#....." +
".#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.." +
"...####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#....." +
@"..##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#

#..#.
#....
##..#
..#..
..###";
        Debug.Assert(Solve(input, 1) == "24");

        Debug.Assert(Solve(input, 2) == "35");

        Debug.Assert(Solve(input) == "3351");
    }

    public string Solve(string input, int time = 50)
    {
        var lines = input.Split(Environment.NewLine);
        var imageEnhancementAlgorithm = lines[0];
        var inputImage = lines[2..];
        const char light = '#';
        const char dark = '.';
        var image = new HashSet<(int X, int Y)>();
        for (int x = 0; x < inputImage.Length; x++)
        {
            for (int y = 0; y < inputImage[0].Length; y++)
            {
                if (inputImage[y][x] is light)
                    image.Add((x, y));
            }
        }

        var flips = imageEnhancementAlgorithm[0] is light & imageEnhancementAlgorithm[^1] is dark;

        for (int t = 0; t < time; t++)
        {
            var offset = 3;
            var minX = image.Min(p => p.X);
            var maxX = image.Max(p => p.X);
            var minY = image.Min(p => p.Y);
            var maxY = image.Max(p => p.Y);
            var newImage = new HashSet<(int X, int Y)>();

            for (int y = minY - offset; y <= maxY + offset; y++)
            {
                for (int x = minX - offset; x <= maxX + offset; x++)
                {
                    var isVoid = x <= minX || y <= minY || x >= maxX || y >= maxY;
                    var number = GetPixelsNumber(x, y, isVoid, t);
                    if (imageEnhancementAlgorithm[number] is light)
                        newImage.Add((x, y));
                }
            }

            image = newImage;
            //Print(minX, maxX, minY, maxY, offset);
        }

        var result = image.Count;
        return result.ToString();

        int GetPixelsNumber(int x, int y, bool isVoid, int time)
        {
            int result = 0;
            for (int yi = -1; yi <= 1; yi++)
            {
                for (int xi = -1; xi <= 1; xi++)
                {
                    var px = x + xi;
                    var py = y + yi;
                    result <<= 1;
                    if (image.Contains((px, py)) || (isVoid && flips && time % 2 == 1))
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        void Print(int minX, int maxX, int minY, int maxY, int offset)
        {
            for (int y = minY - offset; y <= maxY + offset; y++)
            {
                for (int x = minX - offset; x <= maxX + offset; x++)
                {
                    Console.Write(image.Contains((x, y)) ? light : dark);
                }
                Console.WriteLine();
            }
        }
    }
}