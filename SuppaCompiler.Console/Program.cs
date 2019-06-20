using System;
using System.Linq;
using Superpower;
using SuppaCompiler.Console.CodeAnalysis;
using SuppaCompiler.Console.CodeAnalysis.Binding;
using SuppaCompiler.Console.CodeAnalysis.Superpower;
using SuppaCompiler.Console.CodeAnalysis.Syntax;

namespace SuppaCompiler.Console
{
    internal static class Program
    {
        private static void Main()
        {
            var showTree = false;

            while (true)
            {
                System.Console.Write("> ");
                var line = System.Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    return;

                if (line == "#showTree")
                {
                    showTree = !showTree;
                    System.Console.WriteLine(showTree ? "Showing parse trees." : "Not showing parse trees");
                    continue;
                }

                if (line == "#cls")
                {
                    System.Console.Clear();
                    continue;
                }

                try
                {
                    var syntaxTree = Parsers.Tree.Parse(Tokenizer.Create().Tokenize(line));
                    var binder = new Binder();
                    var boundExpression = binder.BindExpression(syntaxTree.Root);
                    var diagnostics = binder.Diagnostics.ToArray();
                    
                    if (showTree)
                    {
                        System.Console.ForegroundColor = ConsoleColor.DarkGray;
                        PrettyPrint(syntaxTree.Root);
                        System.Console.ResetColor();
                    }

                    if (!diagnostics.Any())
                    {
                        var e = new Evaluator(boundExpression);
                        var result = e.Evaluate();
                        System.Console.WriteLine(result);
                    }
                    else
                    {
                        System.Console.ForegroundColor = ConsoleColor.DarkRed;

                        foreach (var diagnostic in diagnostics)
                            System.Console.WriteLine(diagnostic);

                        System.Console.ResetColor();
                    }
                }
                catch (Exception e)
                {
                    System.Console.ForegroundColor = ConsoleColor.DarkRed;
                    System.Console.WriteLine(e.Message);
                    System.Console.ResetColor();
                }
            }
        }

        static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
        {
            var marker = isLast ? "└──" : "├──";

            System.Console.Write(indent);
            System.Console.Write(marker);
            System.Console.Write(node.Kind);

            if (node is SyntaxToken t && t.Value != null)
            {
                System.Console.Write(" ");
                System.Console.Write(t.Value);
            }

            System.Console.WriteLine();

            indent += isLast ? "   " : "│   ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
            {
                PrettyPrint(child, indent, child == lastChild);
            }
        }
    }
}
