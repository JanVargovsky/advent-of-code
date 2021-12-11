namespace AdventOfCode.Year2021.Day11;

class Solver
{
    public Solver()
    {
        Debug.Assert(Solve(@"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526") == "195");
    }

    public string Solve(string input)
    {
        var energies = input.Split(Environment.NewLine).Select(t => t.Select(c => c - '0').ToArray()).ToArray();
        var length = energies.Length * energies[0].Length;
        //Print("Before any steps:");

        var steps = 1;
        while (true)
        {
            var flashes = ProcessStep();
            //Print($"After step {steps}:");
            if (flashes == length)
                break;
            steps++;
        }

        var result = steps;
        return result.ToString();

        int ProcessStep()
        {
            var toFlash = new Queue<(int X, int Y)>();
            for (int y = 0; y < energies.Length; y++)
                for (int x = 0; x < energies[0].Length; x++)
                {
                    energies[y][x]++;
                    if (energies[y][x] > 9)
                    {
                        toFlash.Enqueue((x, y));
                    }
                }
            var flashed = new HashSet<(int X, int Y)>();

            while (toFlash.Count > 0)
            {
                var p = toFlash.Dequeue();
                if (!flashed.Add(p))
                    continue;

                for (int xi = -1; xi <= 1; xi++)
                    for (int yi = -1; yi <= 1; yi++)
                    {
                        var x = p.X + xi;
                        var y = p.Y + yi;
                        if (x >= 0 && y >= 0 && y < energies.Length && x < energies[0].Length)
                        {
                            energies[y][x]++;

                            if (energies[y][x] > 9)
                            {
                                toFlash.Enqueue((x, y));
                            }
                        }
                    }
            }

            for (int y = 0; y < energies.Length; y++)
                for (int x = 0; x < energies[0].Length; x++)
                    if (energies[y][x] > 9)
                        energies[y][x] = 0;

            return flashed.Count;
        }

        void Print(string title)
        {
            Console.WriteLine(title);
            for (int y = 0; y < energies.Length; y++)
                Console.WriteLine(string.Join(string.Empty, energies[y]));
            Console.WriteLine();
        }
    }
}