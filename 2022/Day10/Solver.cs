namespace AdventOfCode.Year2022.Day10;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
noop
addx 3
addx -5
""") == 0);

        Debug.Assert(Solve("""
addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop
""") == 13140);
    }

    public int Solve(string input)
    {
        var instructions = input.Split(Environment.NewLine);

        var memory = new Memory();
        var result = 0;
        var cycles = 1;

        var pipeline = new LinkedList<Instruction>();

        for (int i = 0; i < instructions.Length; i++)
        {
            while (pipeline.Count > 0)
            {
                DoCycle();
            }

            var tokens = instructions[i].Split(' ');

            if (tokens is ["noop"])
            {
                pipeline.AddFirst(new NoopInstruction());
            }
            else if (tokens is ["addx", _])
            {
                pipeline.AddFirst(new AddXInstruction(int.Parse(tokens[1])));
            }
        }

        while (pipeline.Count > 0)
        {
            DoCycle();
        }

        return result;

        void CheckSignalStrength()
        {
            if (cycles % 40 == 20)
            {
                var signalStrength = cycles * memory.X;

                result += signalStrength;
            }
        }

        void DoCycle()
        {
            cycles++;
            var toRemove = new List<LinkedListNode<Instruction>>();

            for (var node = pipeline.First; node != null; node = node.Next)
            {
                var instruction = node.Value;
                var applied = instruction.DoCycle(cycles, memory);
                if (applied)
                {
                    toRemove.Add(node);
                }
            }

            toRemove.ForEach(pipeline.Remove);

            CheckSignalStrength();
        }
    }
}

file class Memory
{
    public int X { get; set; } = 1;
}

file abstract class Instruction
{
    public abstract int RequiredCycles { get; set; }

    public bool DoCycle(int cycle, Memory memory)
    {
        if (--RequiredCycles != 0)
        {
            return false;
        }

        Execute(cycle, memory);
        return true;
    }

    public abstract void Execute(int cycle, Memory memory);
}

file class NoopInstruction : Instruction
{
    public override int RequiredCycles { get; set; } = 1;

    public override void Execute(int cycle, Memory memory)
    {
    }
}

file class AddXInstruction : Instruction
{
    private readonly int _value;

    public override int RequiredCycles { get; set; } = 2;

    public AddXInstruction(int value)
    {
        _value = value;
    }

    public override void Execute(int cycle, Memory memory)
    {
        memory.X += _value;
    }
}