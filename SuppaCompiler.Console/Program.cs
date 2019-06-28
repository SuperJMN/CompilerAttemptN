using SuppaCompiler.CodeAnalysis;
using SuppaCompiler.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuppaCompiler.Console
{
    internal static class Program
    {
        private static void Main()
        {
            var dictionary = new Dictionary<string, Symbol>();

            while (true)
            {
                System.Console.Write("> ");
                var line = System.Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    return;

                var compiler = new Compiler();
                var compilationResult = compiler.Compile(line, dictionary);

                if (!compilationResult.Diagnostics.Any())
                {
                    var evaluation = new Evaluator(compilationResult.BoundExpression, dictionary).Evaluate();
                    System.Console.WriteLine(evaluation);
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
