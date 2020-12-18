using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2020.Day18
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Solve(@"1 + 2 * 3 + 4 * 5 + 6") == "231");
            Debug.Assert(Solve(@"1 + (2 * 3) + (4 * (5 + 6))") == "51");
            Debug.Assert(Solve(@"2 * 3 + (4 * 5)") == "46");
            Debug.Assert(Solve(@"5 + (8 * 3 + 9 + 3 * 4 * 3)") == "1445");
            Debug.Assert(Solve(@"5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))") == "669060");
            Debug.Assert(Solve(@"((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2") == "23340");
        }

        public string Solve(string input)
        {
            var expressions = input.Split(Environment.NewLine);

            var result = expressions.Sum(Evaluate);
            return result.ToString();

            long Evaluate(string expression)
            {
                var postfix = Parse(expression);
                Stack<long> values = new();

                while (postfix.Count > 0)
                {
                    var (token, type) = postfix.Dequeue();
                    if (type == TokenType.Number)
                        values.Push(long.Parse(token));
                    else if (type == TokenType.Operator)
                        ProcessOperation(token);
                    else Debug.Assert(false);
                }

                return values.Pop();

                void ProcessOperation(string operation)
                {
                    var a = values.Pop();
                    var b = values.Pop();
                    if (operation == "+") values.Push(a + b);
                    else if (operation == "*") values.Push(a * b);
                    else Debug.Assert(false);
                }
            }

            // Shunting-yard algorithm
            // infix to postfix
            Queue<(string, TokenType)> Parse(string expression)
            {
                var tokens = expression.Replace("(", "( ").Replace(")", " )").Split(' ');
                Queue<(string, TokenType)> postfix = new();
                Stack<(string Token, TokenType)> stack = new();
                Dictionary<string, int> operatorPrecedences = new()
                {
                    ["*"] = 1,
                    ["+"] = 2
                };

                foreach (var token in tokens)
                {
                    if (token is ("+" or "*"))
                    {
                        while (stack.Count > 0 && stack.Peek() is not (_, TokenType.LeftParenthesis) &&
                            operatorPrecedences[stack.Peek().Token] >= operatorPrecedences[token])
                        {
                            postfix.Enqueue(stack.Pop());
                        }
                        stack.Push((token, TokenType.Operator));
                    }
                    else if (token == "(")
                    {
                        stack.Push((token, TokenType.LeftParenthesis));
                    }
                    else if (token == ")")
                    {
                        while (stack.Count > 0 && stack.Peek() is not (_, TokenType.LeftParenthesis))
                        {
                            postfix.Enqueue(stack.Pop());
                        }

                        if (stack.Count > 0 && stack.Peek() is (_, TokenType.LeftParenthesis))
                            stack.Pop();
                        else Debug.Assert(false);
                    }
                    else
                    {
                        postfix.Enqueue((token, TokenType.Number));
                    }
                }

                while (stack.Count > 0)
                {
                    postfix.Enqueue(stack.Pop());
                }

                return postfix;
            }
        }

        enum TokenType
        {
            Number,
            Operator,
            LeftParenthesis,
            RightParenthesis,
        }
    }
}
