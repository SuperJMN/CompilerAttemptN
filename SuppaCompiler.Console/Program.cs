using System;
using System.Collections.Generic;
using System.Linq;
using Superpower;
using SuppaCompiler.CodeAnalysis;
using SuppaCompiler.CodeAnalysis.Superpower;
using SuppaCompiler.CodeAnalysis.Syntax;

namespace SuppaCompiler.Console
{
    internal static class Program
    {
        private static void Main()
        {
            var showTree = false;
            var variables = new Dictionary<string, Symbol>();

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
                    var compilation = new Compilation(syntaxTree);
                    var result = compilation.Evaluate(variables);
                    
                    if (showTree)
                    {
                        System.Console.ForegroundColor = ConsoleColor.DarkGray;
                        PrettyPrint(syntaxTree.Root);
                        System.Console.ResetColor();
                    }

                    if (!result.Diagnostics.Any())
                    {
                        System.Console.WriteLine(result.Value);
                    }
                    else
                    {
                        System.Console.ForegroundColor = ConsoleColor.DarkRed;

                        foreach (var diagnostic in result.Diagnostics)
                        {
                            System.Console.WriteLine(diagnostic);
                        }

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

            indent += isLast ? "   " : "│  ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
            {
                PrettyPrint(child, indent, child == lastChild);
            }
        }
    }
}
