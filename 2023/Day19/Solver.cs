namespace AdventOfCode.Year2023.Day19;

internal class Solver
{
    public Solver()
    {
        Debug.Assert(Solve("""
px{a<2006:qkq,m>2090:A,rfg}
pv{a>1716:R,A}
lnx{m>1548:A,A}
rfg{s<537:gd,x>2440:R,A}
qs{s>3448:A,lnx}
qkq{x<1416:A,crn}
crn{x>2662:A,R}
in{s<1351:px,qqz}
qqz{s>2770:qs,m<1801:hdj,R}
gd{a>3333:R,R}
hdj{m>838:A,pv}

{x=787,m=2655,a=1222,s=2876}
{x=1679,m=44,a=2067,s=496}
{x=2036,m=264,a=79,s=2244}
{x=2461,m=1339,a=466,s=291}
{x=2127,m=1623,a=2188,s=1013}
""") == 19114);
    }

    public int Solve(string input)
    {
        var segments = input.Split(Environment.NewLine + Environment.NewLine);
        var workflows = ParseWorkflows(segments[0]).ToDictionary(t => t.Name);
        var ratings = ParseRatings(segments[1]).ToArray();
        var accepted = ratings.Where(Evaluate);
        var result = accepted.Sum(t => t.Values.Values.Sum());
        return result;

        bool Evaluate(Rating rating)
        {
            const string start = "in";
            const string accepted = "A";
            const string rejected = "R";
            var current = start;

            while (current != accepted && current != rejected)
            {
                var workflow = workflows[current];
                foreach (var rule in workflow.Rules)
                {
                    var (result, next) = rule.Evaluate(rating);
                    if (result)
                    {
                        current = next;
                        break;
                    }
                }
            }

            return current == accepted;
        }

        IEnumerable<Workflow> ParseWorkflows(string input) => input.Split(Environment.NewLine).Select(ParseWorkflow);

        Workflow ParseWorkflow(string input)
        {
            var tokens = input[..^1].Split('{', ',');
            var rules = tokens[1..].Select(ParseWorkflowRule).ToArray();
            return new Workflow(tokens[0], rules);
        }

        WorkflowRule ParseWorkflowRule(string input) => input.Contains(':') ? ParseWorkflowConditionalRule(input) : new WorkflowRule(input);
        WorkflowConditionalRule ParseWorkflowConditionalRule(string input)
        {
            var operatorIndex = input.IndexOfAny(['<', '>']);
            Debug.Assert(operatorIndex > 0);
            var valueIndex = input.IndexOf(':');
            var conditionValue = int.Parse(input[(operatorIndex + 1)..valueIndex]);
            var value = input[(valueIndex + 1)..];
            return new WorkflowConditionalRule(input[..operatorIndex][0], input[operatorIndex] == '<', conditionValue, value);
        };

        IEnumerable<Rating> ParseRatings(string input) => input.Split(Environment.NewLine).Select(ParseRating);

        Rating ParseRating(string input) => new Rating(input[1..^1].Split(',').Select(t => t.Split('=')).ToDictionary(t => t[0][0], t => int.Parse(t[1])));
    }

    private record Workflow(string Name, WorkflowRule[] Rules);

    private record WorkflowRule(string Value)
    {
        public virtual (bool, string) Evaluate(Rating rating) => (true, Value);
    }

    private record WorkflowConditionalRule(char Variable, bool IsLessThan, int ConditionValue, string Value) : WorkflowRule(Value)
    {
        public override (bool, string) Evaluate(Rating rating)
        {
            var variableValue = rating[Variable];
            var result = IsLessThan ?
                variableValue < ConditionValue :
                variableValue > ConditionValue;

            return (result, result ? Value : null);
        }
    }

    private record Rating(Dictionary<char, int> Values)
    {
        public int this[char value] => Values[value];
    }
}
