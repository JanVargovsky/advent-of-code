namespace AdventOfCode.Year2023.Day20;

internal class Solver
{
    public Solver()
    {
        //        Debug.Assert(Solve("""
        //broadcaster -> a, b, c
        //%a -> b
        //%b -> c
        //%c -> inv
        //&inv -> a
        //""") == 8000 * 4000);
        //        Debug.Assert(Solve("""
        //broadcaster -> a
        //%a -> inv, con
        //&inv -> b
        //%b -> con
        //&con -> output
        //""") == 4250 * 2750);
    }

    public long Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var edges = new Dictionary<string, string[]>();
        var edgesReversed = new Dictionary<string, string[]>();
        var modules = new Dictionary<string, Module>();
        foreach (var row in rows)
        {
            var values = row.Split([" -> ", ", "], StringSplitOptions.None);
            var type = values[0][0];
            var name = values[0][1..];
            Module from = type switch
            {
                '%' => new FlipFlopModule(name),
                '&' => new ConjunctionModule(name),
                _ => new BroadcastModule(values[0]),
            };
            modules.Add(from.Name, from);
            edges.Add(from.Name, values[1..]);
        }
        foreach (var (from, tos) in edges)
        {
            foreach (var to in tos)
            {
                if (modules.TryGetValue(to, out var module) && module is ConjunctionModule conjunction)
                    conjunction.Memory[from] = Pulse.Low;

                edgesReversed[to] = [.. edgesReversed.GetValueOrDefault(to, Array.Empty<string>()), from];
            }
        }
        edges["button"] = ["broadcaster"];
        modules["rx"] = new UntypedModule("rx");
        var inputsToRx = edgesReversed["rx"].Select(t => modules[t]); // 1 conjunction (nand)
        var inputsToInputToRx = inputsToRx.SelectMany(t => edgesReversed[t.Name]).Select(t => modules[t]); // 4 conjunctions (nand)
        var buttonPressesForRxInputs = inputsToInputToRx.ToDictionary(t => t.Name, _ => (int?)null);

        var pulseHistory = new List<Pulse>();
        var buttonPressed = 0;
        while (true)
        {
            buttonPressed++;
            var current = new List<State>();
            current.Add(new("button", Pulse.Low));

            while (current.Count > 0)
            {
                var next = new List<State>();
                foreach (var item in current)
                {
                    var tos = edges[item.From];
                    foreach (var to in tos)
                    {
                        pulseHistory.Add(item.Pulse);
                        var nextSignal = Send(item.From, item.Pulse, to);
                        if (nextSignal.HasValue)
                            next.Add(new(to, nextSignal.Value));
                    }
                }
                current = next;

                bool check = false;
                foreach (var item in next)
                {
                    if (item.Pulse is Pulse.High && buttonPressesForRxInputs.TryGetValue(item.From, out var value) && !value.HasValue)
                    {
                        buttonPressesForRxInputs[item.From] = buttonPressed;
                        check = true;
                    }
                }
                if (check && buttonPressesForRxInputs.Values.All(t => t.HasValue))
                {
                    return buttonPressesForRxInputs.Select(t => t.Value.Value).Aggregate(1L, (acc, t) => acc * t);
                }
            }
        }

        throw new ItWontHappenException();

        var low = pulseHistory.Count(t => t == Pulse.Low);
        var high = pulseHistory.Count(t => t == Pulse.High);
        var result = low * high;
        return result;

        Pulse? Send(string from, Pulse pulse, string to)
        {
            //Console.WriteLine($"{from} -{pulse.ToString().ToLower()}-> {to}");
            if (!modules.TryGetValue(to, out var module))
                return null;

            if (module is FlipFlopModule flipFlop)
            {
                if (pulse is Pulse.High)
                    return null;

                if (pulse is Pulse.Low)
                {
                    var valueBeforeFlip = flipFlop.IsOn;
                    flipFlop.IsOn = !flipFlop.IsOn;
                    return !valueBeforeFlip ? Pulse.High : Pulse.Low;
                }
            }
            else if (module is ConjunctionModule conjunction)
            {
                conjunction.Memory[from] = pulse;
                var allHigh = conjunction.Memory.Values.All(t => t == Pulse.High);
                return allHigh ? Pulse.Low : Pulse.High;
            }
            else if (module is BroadcastModule broadcast)
            {
                return pulse;
            }
            else if (module is UntypedModule untyped)
            {
                return null;
            }

            throw new ItWontHappenException();
        }
    }

    private record State(string From, Pulse Pulse);
    private record Module(string Name);
    private record BroadcastModule(string Name) : Module(Name);
    private record FlipFlopModule(string Name) : Module(Name)
    {
        public bool IsOn { get; set; } = false;
    }
    private record ConjunctionModule(string Name) : Module(Name)
    {
        public Dictionary<string, Pulse> Memory { get; set; } = new();
    }
    private record UntypedModule(string Name) : Module(Name);
    private enum Pulse
    {
        Low,
        High,
    }
}
