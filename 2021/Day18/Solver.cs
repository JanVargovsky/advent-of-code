namespace AdventOfCode.Year2021.Day18;

class Solver
{
    public Solver()
    {
        Debug.Assert(Parse("[1,2]") is { Left.Value: 1, Right.Value: 2 });
        Debug.Assert(Parse("[[1, 2], 3]") is { Left.Left.Value: 1, Left.Right.Value: 2, Right.Value: 3 });
        Debug.Assert(Parse("[9,[8, 7]]") is { Left.Value: 9, Right.Left.Value: 8, Right.Right.Value: 7 });
        Debug.Assert(Parse("[[1, 9],[8, 5]]") is { Left.Left.Value: 1, Left.Right.Value: 9, Right.Left.Value: 8, Right.Right.Value: 5 });

        Debug.Assert(Reduce("[[[[[9,8],1],2],3],4]").ToString() == "[[[[0,9],2],3],4]");
        Debug.Assert(Reduce("[7,[6,[5,[4,[3,2]]]]]").ToString() == "[7,[6,[5,[7,0]]]]");
        Debug.Assert(Reduce("[[6,[5,[4,[3,2]]]],1]").ToString() == "[[6,[5,[7,0]]],3]");
        Debug.Assert(Reduce("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]").ToString() == "[[3,[2,[8,0]]],[9,[5,[7,0]]]]");

        Debug.Assert(Reduce(@"[1,1]
[2,2]
[3,3]
[4,4]").ToString() == "[[[[1,1],[2,2]],[3,3]],[4,4]]");
        Debug.Assert(Reduce(@"[1,1]
[2,2]
[3,3]
[4,4]
[5,5]").ToString() == "[[[[3,0],[5,3]],[4,4]],[5,5]]");
        Debug.Assert(Reduce(@"[1,1]
[2,2]
[3,3]
[4,4]
[5,5]
[6,6]").ToString() == "[[[[5,0],[7,4]],[5,5]],[6,6]]");
        Debug.Assert(Reduce(@"[[[[4,3],4],4],[7,[[8,4],9]]]
[1,1]").ToString() == "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]");


        Debug.Assert(Solve("[[1,2],[[3,4],5]]") == "143");
        Debug.Assert(Solve("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]") == "1384");
        Debug.Assert(Solve("[[[[1,1],[2,2]],[3,3]],[4,4]]") == "445");
        Debug.Assert(Solve("[[[[3,0],[5,3]],[4,4]],[5,5]]") == "791");
        Debug.Assert(Solve("[[[[5,0],[7,4]],[5,5]],[6,6]]") == "1137");
        Debug.Assert(Solve("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]") == "3488");
        Debug.Assert(Solve("[[[[6,6],[7,6]],[[7,7],[7,0]]],[[[7,7],[7,7]],[[7,8],[9,9]]]]") == "4140");

        Debug.Assert(Reduce(@"[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]
[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]").ToString() == "[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]");
        Debug.Assert(Reduce(@"[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]
[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]").ToString() == "[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]");

        Debug.Assert(Reduce(@"[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]
[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]
[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]
[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]
[7,[5,[[3,8],[1,4]]]]
[[2,[2,2]],[8,[8,1]]]
[2,9]
[1,[[[9,3],9],[[9,0],[0,7]]]]
[[[5,[7,4]],7],1]
[[[[4,2],2],6],[8,7]]").ToString() == "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]");

        Debug.Assert(Reduce(@"[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]
[[[5,[2,8]],4],[5,[[9,9],0]]]
[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]
[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]
[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]
[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]
[[[[5,4],[7,7]],8],[[8,3],8]]
[[9,3],[[9,9],[6,[4,9]]]]
[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]
[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]").ToString() == "[[[[6,6],[7,6]],[[7,7],[7,0]]],[[[7,7],[7,7]],[[7,8],[9,9]]]]");
    }

    public string Solve(string input)
    {
        var number = Reduce(input);
        var result = Magnitude(number);
        return result.ToString();
    }

    Node Reduce(string input)
    {
        var numbers = input.Split(Environment.NewLine).Select(Parse).ToArray();
        if (numbers.Length == 1)
            return Reduce(numbers[0]);

        var number = numbers.Aggregate((a, b) =>
        {
            //Console.WriteLine("  " + a);
            //Console.WriteLine("+ " + b);
            var addition = Reduce(a + b);
            //Console.WriteLine("= " + addition);
            //Console.WriteLine();
            return addition;
        });
        return number;
    }

    Node Reduce(Node root)
    {
        var count = 0;
        //Console.WriteLine(root);

        while (Apply())
        {
            count++;
            //Console.WriteLine("applied " + root);
        }
        //Console.WriteLine(root);
        //Console.WriteLine();

        return root;

        bool Apply()
        {
            var exploded = Traverse(root, new(), (node, parents) =>
            {
                if (!node.Value.HasValue && parents.Count(t => !t.Value.HasValue) == 4)
                {
                    //Console.WriteLine($"explode {pair}");
                    var (parentLeft, parentRight) = Find(root, node);
                    if (parentLeft is null || parentRight is null)
                        node.Value = 0;
                    if (parentLeft is not null)
                        parentLeft.Value += node.Left.Value.Value;
                    if (parentRight is not null)
                        parentRight.Value += node.Right.Value.Value;

                    node.Value = 0;
                    node.Left = null;
                    node.Right = null;

                    parents.Pop();
                    return true;
                }

                return false;
            });

            //if (exploded)
            //    Console.WriteLine("after explode:  " + root);

            if (!exploded)
            {
                var split = Traverse(root, new(), (node, parents) =>
                {
                    if (node.Value.HasValue && node.Value.Value >= 10)
                    {
                        //Console.WriteLine($"split {node}");
                        var (value, remainder) = Math.DivRem(node.Value.Value, 2);
                        node.Value = null;
                        node.Left = new Node { Value = value };
                        node.Right = new Node { Value = value + (remainder > 0 ? 1 : 0) };
                        return true;
                    }

                    return false;
                });
                //if (split)
                //    Console.WriteLine("after split:    " + root);
                return split;

            }

            return exploded;
        }

        (Node?, Node?) Find(Node root, Node pair)
        {
            var nodes = Preorder(root).ToList();
            for (int i = 0; i < nodes.Count; i++)
            {
                // Warning: you can't use == because record overrides it
                if (ReferenceEquals(nodes[i], pair))
                {
                    var leftIndex = i - 2;
                    while (leftIndex >= 0)
                    {
                        if (nodes[leftIndex].Value.HasValue)
                            break;
                        leftIndex--;
                    }
                    var rightIndex = i + 2;
                    while (rightIndex < nodes.Count)
                    {
                        if (nodes[rightIndex].Value.HasValue)
                            break;
                        rightIndex++;
                    }

                    var left = leftIndex >= 0 ? nodes[leftIndex] : null;
                    var right = rightIndex < nodes.Count ? nodes[rightIndex] : null;
                    return (left, right);
                }
            }

            throw new InvalidOperationException();
        }

        bool Traverse(Node? node, Stack<Node> parents, Func<Node, Stack<Node>, bool> func)
        {
            if (node is null)
                return false;

            parents.Push(node);
            if (Traverse(node.Left, parents, func)) return true;
            parents.Pop();

            var result = func(node, parents);

            if (result) return true;

            parents.Push(node);
            if (Traverse(node.Right, parents, func)) return true;
            parents.Pop();
            return false;
        }

        IEnumerable<Node> Preorder(Node? node)
        {
            if (node is null)
                yield break;

            foreach (var item in Preorder(node.Left))
                yield return item;
            //if (node.Value.HasValue)
            yield return node;
            foreach (var item in Preorder(node.Right))
                yield return item;
        }
    }

    int Magnitude(Node node)
    {
        if (node.Value.HasValue)
            return node.Value.Value;

        return 3 * Magnitude(node.Left!) + 2 * Magnitude(node.Right!);
    }

    Node Parse(string input)
    {
        var root = new Node();
        var index = 0;
        var current = root;
        var parents = new Stack<Node>();

        while (index < input.Length)
        {
            var value = input[index++];
            if (value is '[')
            {
                parents.Push(current);
                if (current.Left is null)
                    current = current.Left = new Node();
                else
                    current = current.Right = new Node();
            }
            else if (value is ']')
            {
                current = parents.Pop();
            }
            else if (value is ',')
            {
            }
            else
            {
                var nodeValue = new Node { Value = value - '0' };
                if (current.Left is null)
                    current.Left = nodeValue;
                else
                    current.Right = nodeValue;
            }
        }

        return root.Left!;
    }

    record Node
    {
        public int? Value { get; set; }
        public Node? Left { get; set; }
        public Node? Right { get; set; }

        public override string ToString()
        {
            if (Value.HasValue) return Value.Value.ToString();
            return $"[{Left},{Right}]";
        }

        public static Node operator +(Node a, Node b) => new Node { Left = a, Right = b };
    }
}
