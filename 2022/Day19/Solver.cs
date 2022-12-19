namespace AdventOfCode.Year2022.Day19;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
Blueprint 1:
  Each ore robot costs 4 ore.
  Each clay robot costs 2 ore.
  Each obsidian robot costs 3 ore and 14 clay.
  Each geode robot costs 2 ore and 7 obsidian.

Blueprint 2:
  Each ore robot costs 2 ore.
  Each clay robot costs 3 ore.
  Each obsidian robot costs 3 ore and 8 clay.
  Each geode robot costs 3 ore and 12 obsidian.
""") == 56 * 62);
    }

    public int Solve(string input)
    {
        var rows = input.Split(new string[] { Environment.NewLine, ":", "Each" },
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var blueprints = rows.Chunk(5).Select(Parse).Take(3).ToArray();
        var tasks = blueprints.Select(t => Task.Run(() => Calculate(t))).ToArray();
        Task.WaitAll(tasks);
        var results = tasks.Select(t => t.Result).ToArray();
        Console.WriteLine(string.Join(", ", results));
        var result = results.Aggregate(1, (a, b) => a * b);
        return result;

        int Calculate(Blueprint blueprint)
        {
            var robots = new[] { blueprint.OreRobot, blueprint.ClayRobot, blueprint.ObsidianRobot, blueprint.GeodeRobot };
            var maxOreNeeded = robots.Max(t => t.Ore);
            var maxClayNeeded = robots.Max(t => t.Clay);
            var maxObsidianNeeded = robots.Max(t => t.Obsidian);

            const int minutes = 32;
            var best = 0;
            var queue = new Stack<State>();
            var initial = new State(0, new(0, 1), new(0, 0), new(0, 0), new(0, 0));
            queue.Push(initial);
            var visited = new HashSet<State>();
            while (queue.TryPop(out var state))
            {
                best = Math.Max(best, state.Geode.Count);

                if (state.Minute == minutes)
                {
                    continue;
                }

                if (best > CalculatePotential(state))
                    continue;

                var oldState = state;
                state = Collect(state);
                if (!visited.Add(state))
                    continue;

                if (visited.Count % 1000000 == 0)
                {
                    Console.WriteLine($"Blueprint {blueprint.Number} tested {visited.Count}");
                }

                queue.Push(state);

                if (CanAfford(oldState, blueprint.GeodeRobot))
                {
                    var newState = BuyRobot(state, blueprint.GeodeRobot, isGeode: true);
                    queue.Push(newState);
                }
                else
                {
                    if (state.Ore.RobotCount < maxOreNeeded && CanAfford(oldState, blueprint.OreRobot))
                    {
                        var newState = BuyRobot(state, blueprint.OreRobot, isOre: true);
                        queue.Push(newState);
                    }
                    if (state.Clay.RobotCount < maxClayNeeded && CanAfford(oldState, blueprint.ClayRobot))
                    {
                        var newState = BuyRobot(state, blueprint.ClayRobot, isClay: true);
                        queue.Push(newState);
                    }
                    if (state.Obsidian.RobotCount < maxObsidianNeeded && CanAfford(oldState, blueprint.ObsidianRobot))
                    {
                        var newState = BuyRobot(state, blueprint.ObsidianRobot, isObsidian: true);
                        queue.Push(newState);
                    }
                }
            }

            return best;

            int CalculatePotential(State state)
            {
                var remaining = minutes - state.Minute;
                var currentTillEnd = state.Geode.Count + state.Geode.RobotCount * remaining;
                var newTillEnd = remaining * (remaining + 1) / 2;
                var result = currentTillEnd + newTillEnd;
                return result;
            }

            State Collect(State state)
            {
                return new(
                    state.Minute + 1,
                    new(state.Ore.Count + state.Ore.RobotCount, state.Ore.RobotCount),
                    new(state.Clay.Count + state.Clay.RobotCount, state.Clay.RobotCount),
                    new(state.Obsidian.Count + state.Obsidian.RobotCount, state.Obsidian.RobotCount),
                    new(state.Geode.Count + state.Geode.RobotCount, state.Geode.RobotCount)
                    );
            }

            State BuyRobot(State state, Robot robot, bool isOre = false, bool isClay = false, bool isObsidian = false, bool isGeode = false)
            {
                return new(
                    state.Minute,
                    Spend(state.Ore, robot.Ore, isOre),
                    Spend(state.Clay, robot.Clay, isClay),
                    Spend(state.Obsidian, robot.Obsidian, isObsidian),
                    Spend(state.Geode, 0, isGeode)
                    );
            }

            ResourceCount Spend(ResourceCount resourceCount, int count, bool bought)
            {
                return new ResourceCount(
                    resourceCount.Count - count,
                    resourceCount.RobotCount + (bought ? 1 : 0)
                    );
            }

            bool CanAfford(State state, Robot robot)
            {
                return state.Ore.Count >= robot.Ore &&
                    state.Clay.Count >= robot.Clay &&
                    state.Obsidian.Count >= robot.Obsidian;
            }
        }

        Blueprint Parse(string[] blueprint)
        {
            var name = blueprint[0].Split(' ');
            var number = int.Parse(name[^1]);

            var ore = blueprint[1].Split(' ');
            var oreRobot = new Robot(int.Parse(ore[^2]), 0, 0);

            var clay = blueprint[2].Split(' ');
            var clayRobot = new Robot(int.Parse(clay[^2]), 0, 0);

            var obsidian = blueprint[3].Split(' ');
            var obsidianRobot = new Robot(int.Parse(obsidian[^5]), int.Parse(obsidian[^2]), 0);

            var geode = blueprint[4].Split(' ');
            var geodeRobot = new Robot(int.Parse(geode[^5]), 0, int.Parse(geode[^2]));

            return new(number, oreRobot, clayRobot, obsidianRobot, geodeRobot);
        }
    }
}

internal record State(int Minute, ResourceCount Ore, ResourceCount Clay, ResourceCount Obsidian, ResourceCount Geode);
internal record ResourceCount(int Count, int RobotCount);

internal record Blueprint(int Number, Robot OreRobot, Robot ClayRobot, Robot ObsidianRobot, Robot GeodeRobot);
internal record Robot(int Ore, int Clay, int Obsidian);
